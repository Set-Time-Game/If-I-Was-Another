using System;
using System.Linq;
using Leopotam.Ecs;
using Types.Interfaces;
using Types.Structs;
using Random = UnityEngine.Random;

namespace Systems.General
{
    public class GeneratorSystem : IEcsPreInitSystem
    {
        public EcsWorld World = null;
        public MapData MapData = null;

        public void PreInit()
        {
            MapData.InitMap();
            GenerateBiomes();
        }

        private void GenerateBiomes()
        {
            for (byte biomeIndex = 0; biomeIndex < MapData.MapSize; biomeIndex++)
            {
                var biomeConfig = MapData.GetBiomeConfig(biomeIndex);
                var biomeEntity = World.NewEntity();

                ref var biome = ref biomeEntity.Get<Biome>();
                biome.Id = biomeIndex;
                biome.InitChunks(MapData.BiomeSize);

                GenerateChunks(ref biomeConfig, ref biome);
                MapData.SetBiome(biomeIndex, biomeEntity.Ref<Biome>());
            }
        }

        private void GenerateChunks(ref BiomeConfig biomeConfig, ref Biome biome)
        {
            for (uint chunkIndex = 0; chunkIndex < MapData.BiomeSize; chunkIndex++)
            {
                var chunkEntity = World.NewEntity();

                ref var chunk = ref chunkEntity.Get<Chunk>();
                chunk.Id = chunkIndex;
                chunk.BiomeId = biome.Id;
                chunk.Grounds = new EcsComponentRef<Ground>[MapData.ChunkSize];

                GenerateGrounds(ref biomeConfig, ref chunk);
                biome.Chunks[chunkIndex] = chunkEntity.Ref<Chunk>();
            }
        }

        private void GenerateGrounds(ref BiomeConfig biomeConfig, ref Chunk chunk)
        {
            for (byte groundIndex = 0; groundIndex < MapData.ChunkSize; groundIndex++)
            {
                var groundEntity = World.NewEntity();
                var groundConfig = GetBest(ref biomeConfig.groundConfigs);

                ref var ground = ref groundEntity.Get<Ground>();
                ground.Id = groundIndex;
                ground.ChunkId = chunk.Id;
                ground.BiomeId = chunk.BiomeId;
                ground.Parts = new GroundPart?[groundConfig.config.isPlaceable ? MapData.GroundSize : 0];
                ground.MainTexture = groundConfig.config.textureVariants[Random.Range(0, groundConfig.config.textureVariants.Length)];

                if (groundConfig.config.isPlaceable)
                    GenerateParts(ref biomeConfig, ref ground);

                chunk.Grounds[groundIndex] = groundEntity.Ref<Ground>();
            }
        }

        private void GenerateParts(ref BiomeConfig biomeConfig, ref Ground ground)
        {
            var parts = new GroundPartConfig[MapData.GroundSize];
            for (byte partIndex = 0; partIndex < MapData.GroundSize; partIndex++)
            {
                var cfg = GetBest(ref biomeConfig.groundPartConfigs);
                parts[partIndex] = cfg.Chance < Random.value
                    ? new GroundPartConfig { chance = -1}
                    : cfg;
            }

            if (parts.Any(x => x.chance != -1f && x.configAsset.Any(y => y.type is PartType.Big or PartType.Struct)))
                parts = new[] {parts.First(x => x.chance != -1f && x.configAsset.Any(y => y.type is PartType.Big or PartType.Struct))};
            else
                parts[0] = new GroundPartConfig {chance = -1};

            for (byte partIndex = 0; partIndex < parts.Length; partIndex++)
            {
                var part = parts[partIndex];
                ground.Parts[partIndex] = part.chance == -1 ? null : new GroundPart(parts[partIndex]);
            }
        }

        public static T GetBest<T>(ref T[] variants) where T: IVariant
        {
            T variant;

            do
                variant = variants[Random.Range(0, variants.Length)];
            while (variant.Chance < Random.value);
            
            return variant;
        }
    }
}