using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using Pathfinding;
using Types.Classes;
using Types.Structs;
using UnityEngine;
using Chunk = Types.Structs.Chunk;
using Ground = Types.Structs.Ground;

namespace Systems.General
{
    public class WorldStateSystem : IEcsInitSystem
    {
        //public EcsWorld World = null;
        public Player PlayerInstance;
        public MapData MapData;

        private byte m_currentBiomeId;
        private uint m_currentChunkId;
        //private byte CurrentGroundId = 0;

        private uint m_pivot;
        private byte m_chunkRowSize = 27;
        private Vector2 m_vectors;

        private readonly Chunk[] m_currentChunks = new Chunk[9];
        private readonly uint[] m_indexes = new uint[9];

        private readonly WaitForFixedUpdate m_fixedUpdateTimer = new();
        private readonly WaitForSecondsRealtime m_waitTwoSeconds = new(2);

        public void Init()
        {
            m_vectors = new Vector2(10, 6);
            m_pivot = MapData.BiomeSize / 2;
            m_chunkRowSize = (byte) Mathf.RoundToInt(Mathf.Sqrt(MapData.BiomeSize));
            m_currentChunkId = m_pivot;
            
            //UpdateCluster();

            //var position = (Vector2) MapData.cluster.chunks[4].transform.position;
            //var offset = GetOffset(ref position);
            //var indexes = GetChunkIndexes(ref offset);
            
            UpdateChunkIndexes(ref m_pivot);
            UpdateCurrentChunks();
            UpdateCluster();

            MapData.StartCoroutine(StateUpdater());
            MapData.AstarPath.Scan();
        }

        private IEnumerator StateUpdater()
        {
            while (true)
            {
                /*yield return _fixedUpdateTimer;
                
                if (GetClosestChunk(out var closestChunk).Id == _currentChunkId &&
                    closestChunk.BiomeId == _currentBiomeId) continue;*/

                yield return m_waitTwoSeconds;
                
                if (GetClosestChunk(out var closestChunk).Id == m_currentChunkId &&
                    closestChunk.BiomeId == m_currentBiomeId) continue;

                UpdateState(closestChunk);
            }
        }

        private void UpdateState(Chunk closestChunk)
        {
            //TODO: кривое отображение чанков
            m_currentBiomeId = closestChunk.BiomeId;
            m_currentChunkId = closestChunk.Id;
            UpdateChunkIndexes(ref m_currentChunkId);
            UpdateCurrentChunks();
            UpdateCluster();

            ((MapData.AstarPath.graphs[0] as GridGraph)!).center = MapData.cluster.Transform.position;
            MapData.AstarPath.Scan();
        }

        private Chunk GetClosestChunk(out Chunk chunk)
        {
            Chunk? result = null;
            
            var chunks = MapData.cluster.chunks;
            var playerPosition = (Vector2) PlayerInstance.transform.position;
            var lastPosition = (Vector2) chunks[4].transform.position;
            var closestPosition = lastPosition;

            {
                var closestDistance = Vector2.Distance(lastPosition, playerPosition);

                foreach (var c in chunks)
                {
                    var chunkPosition = (Vector2) c.transform.position;
                    var distance = Vector2.Distance(chunkPosition, playerPosition);

                    if (!(distance < closestDistance)) continue;

                    closestPosition = chunkPosition;
                    closestDistance = distance;
                }
            }
            
            if (closestPosition != lastPosition)
            {
                var closest = (closestPosition - lastPosition) / m_vectors;

                MapData.cluster.transform.position += (Vector3) (closestPosition - lastPosition);
                result = m_currentChunks[GetOffset(ref closest)];
            }

            result ??= m_currentChunks[4];
            
            chunk = (Chunk) result;
            return chunk;
        }

        private void UpdateCurrentChunks()
        {
            var biome = MapData.GetBiome(m_currentBiomeId).Unref();

            for (byte i = 0; i < 9; i++)
                m_currentChunks[i] = biome.Chunks[m_indexes[i]].Unref();
        }

        private void UpdateChunkIndexes(ref uint position)
        {
            m_indexes[4] = position;
            m_indexes[3] = position - 1;
            m_indexes[5] = position + 1;

            m_indexes[1] = GetOffset(0, 1, position, m_chunkRowSize);
            m_indexes[0] = m_indexes[1] - 1;
            m_indexes[2] = m_indexes[1] + 1;

            m_indexes[7] = GetOffset(0, -1, position, m_chunkRowSize);
            m_indexes[6] = m_indexes[7] - 1;
            m_indexes[8] = m_indexes[7] + 1;
        }

        private static uint GetOffset(ref Vector2 position, uint pivot = 4, uint rowSize = 3) 
            => GetOffset((int) position.x, (int) position.y, pivot, rowSize);
        private static uint GetOffset(int x, int y, uint pivot = 4, uint rowSize = 3) 
            => (uint) ((x + pivot) - (rowSize * y));

        private void UpdateCluster()
        {
            //ref var biomeData = ref MapData.Map[_currentBiomeId].Unref();
            
            for (byte i = 0; i < MapData.cluster.chunks.Length; i++)
            {
                //SetChunk(ref biomeData, ref i, ref _indexes[i]);
                var chunkInstance = MapData.cluster.chunks[i];
                
                for (byte j = 0; j < chunkInstance.grounds.Length; j++)
                {
                    SetGround(ref m_currentChunks[i], ref j, ref chunkInstance);
                }
            }
        }

        private void SetChunk(ref Biome biomeData, ref byte instanceIndex, ref uint chunkIndex)
        {
            ref var chunkData = ref biomeData.Chunks[chunkIndex].Unref();
            var chunkInstance = MapData.cluster.chunks[instanceIndex];

            for (byte j = 0; j < chunkInstance.grounds.Length; j++)
                SetGround(ref chunkData, ref j, ref chunkInstance);
        }

        private void SetGround(ref Chunk chunkData, ref byte index, ref Types.Classes.Chunk chunkInstance)
        {
            ref var groundData = ref chunkData.Grounds[index].Unref();
            var groundInstance = chunkInstance.grounds[index];

            groundInstance.spriteRenderer.sprite = groundData.MainTexture;
            
            if (groundData.Parts.Length > 0)
            {
                SetParts(ref groundData, ref groundInstance);
                return;
            }

            for (byte partIndex = 0; partIndex < MapData.groundSize; partIndex++)
            {
                var part = groundInstance.parts[partIndex];
                part.spriteRenderer.sprite = null;
                part.collectCollider.enabled = false;
                part.collisionCollider.enabled = false;
            }
        }

        private static void SetParts(ref Ground groundData, ref Types.Classes.Ground groundInstance)
        {
            for (byte partIndex = 0; partIndex < groundData.Parts.Length; partIndex++)
            {
                var partInstance = groundInstance.parts[partIndex];
                var partData = groundData.Parts[partIndex];

                partInstance.spriteRenderer.sprite = partData.MainTexture;
                partInstance.collectCollider.enabled = partData.HasCollector;
                partInstance.collisionCollider.enabled = partData.HasCollision;

                if (partData.HasCollision)
                    partInstance.gameObject.layer = 8;
            }
        }
    }
}