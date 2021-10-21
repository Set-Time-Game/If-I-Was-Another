using System;
using Classes.World;
using UnityEngine;

public interface IEntity
{
    public Transform Transform { get; }
    public GameObject GameObject { get; }
}
