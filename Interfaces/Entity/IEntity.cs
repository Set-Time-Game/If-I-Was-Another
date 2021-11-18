using UnityEngine;

public interface IEntity
{
    public SpriteRenderer SpriteRenderer { get; }
    public Transform Transform { get; }
    public GameObject GameObject { get; }
}