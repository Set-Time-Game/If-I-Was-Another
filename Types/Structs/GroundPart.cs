using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Types.Structs
{
    public struct GroundPart
    {
        public PartState m_state;
        
        public string Name;
        
        public bool HasCollision;
        public bool HasCollector;
        
        public PartType Type;

        public PartState State
        {
            get => m_state;
            set
            {
                m_state = value;
                
                if (Instance is not null)
                    Instance.spriteRenderer.sprite = GetSprite();
            }
        }
        
        public Sprite MainTexture;
        public Sprite HighlightedTexture;
        public Sprite CollectedTexture;

        public Classes.GroundPart Instance;

        public GroundPart(GroundPartConfig config)
        {
            if (config.chance == -1f)
            {
                Type = PartType.None;
                m_state = PartState.None;
                HasCollision = false;
                HasCollector = false;
                MainTexture = null;
                HighlightedTexture = null;
                CollectedTexture = null;
                Instance = null;
                Name = "GroundPart Null";
                return;
            }

            this = config.configAsset[Random.Range(0, config.configAsset.Length)].GeneratePart();
        }

        public void SetHighlight(bool state)
        {
            if (State == PartState.Collected) return;
            State = state ? PartState.Highlighted : PartState.Default;
        }

        public void Collect()
        {
            State = PartState.Collected;
        }

        public Sprite GetSprite()
        {
            return State switch
            {
                PartState.None => null,
                PartState.Default => MainTexture,
                PartState.Highlighted => HighlightedTexture,
                PartState.Collected => CollectedTexture,
                PartState.Destroyed => null,
                _ => throw new ArgumentOutOfRangeException(nameof(State), State, null)
            };
        }
    }
}