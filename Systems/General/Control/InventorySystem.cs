using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using Types.Classes;
using Types.Structs;
using UnityEngine;
using UnityEngine.UI;
using GroundPart = Types.Classes.GroundPart;

namespace Systems.General.Control
{
    public class InventorySystem : MonoBehaviour, IEcsInitSystem
    {
        public Button CollectButton;
        public PlayerCollector Collector;
        
        public Player Player;
        public MapData MapData;
        public WorldStateSystem WorldState;

        public Types.Structs.GroundPart m_closest;

        public InventorySystem(WorldStateSystem worldState)
        {
            WorldState = worldState;
        }

        public void OnCollect()
        {
            if (m_closest.State != PartState.Highlighted) return;
            
            m_closest.Collect();
            ref var biome = ref MapData.GetBiome(WorldState.m_currentBiomeId).Unref();
            ref var chunk = ref biome.Chunks[WorldState.m_currentChunkId].Unref();
            ref var ground = ref chunk.Grounds[m_closest.Instance.GroundId].Unref();
            ground.Parts[m_closest.Instance.Id] = m_closest;
            m_closest.Instance.collectCollider.enabled = false;
        }
        
        public void OnCollectableAdd(GroundPart part, List<GroundPart> list)
        {
            ref var biome = ref MapData.GetBiome(WorldState.m_currentBiomeId).Unref();
            ref var chunk = ref biome.Chunks[WorldState.m_currentChunkId].Unref();
            ref var ground = ref chunk.Grounds[part.GroundId].Unref();
            if (ground.Parts.Length < part.Id)
            {
                return;
            }
            var groundPartRaw = ground.Parts[part.Id];
            
            if (groundPartRaw == null) return;
            var groundPart = groundPartRaw.Value;
            
            var pPos = Player.Transform.position;
            var cPos = m_closest.Instance.transform.position;
            var gPos = groundPart.Instance.transform.position;
            var distance = Vector2.Distance(pPos, gPos);

            if (distance > Vector2.Distance(pPos, cPos)) return;

            m_closest = groundPart;
            groundPart.SetHighlight(true);
            ground.Parts[part.Id] = groundPart;
            
            CollectButton.gameObject.SetActive(true);
        }
        
        public void OnCollectableRemove(GroundPart part, List<GroundPart> list)
        {
            ref var biome = ref MapData.GetBiome(WorldState.m_currentBiomeId).Unref();
            ref var chunk = ref biome.Chunks[WorldState.m_currentChunkId].Unref();
            var ground = chunk.Grounds[part.GroundId].Unref();
            if (ground.Parts.Length < part.Id)
            {
                return;
            }
            var groundPartRaw = ground.Parts[part.Id];
            if (groundPartRaw == null) return;

            var groundPart = groundPartRaw.Value;
            groundPart.SetHighlight(false);

            if (!list.Any())
            {
                CollectButton.gameObject.SetActive(false);
                m_closest.SetHighlight(false);
                return;
            }

            foreach (var gpart in list)
            {
                var last_dist = Vector2.Distance(Player.Transform.position, groundPart.Instance.transform.position);
                if (last_dist > Vector2.Distance(Player.Transform.position, m_closest.Instance.transform.position)) continue;
                
                m_closest = chunk.Grounds[gpart.GroundId].Unref().Parts[gpart.Id].Value;
            }

            m_closest.SetHighlight(true);
        }

        public void Init()
        {
            m_closest = MapData
                .GetBiome(WorldState.m_currentBiomeId).Unref()
                .Chunks[WorldState.m_currentChunkId].Unref()
                .Grounds.First(x => x.Unref().Parts.Any(y => y.HasValue))
                .Unref().Parts.First(x => x.HasValue).Value;
        }
    }
}