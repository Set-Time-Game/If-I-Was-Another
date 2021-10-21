using System;
using System.Collections;
using System.Collections.Generic;
using Classes.World;
using UnityEngine;

public interface IGenerable : IEntity
{
    public Variable Variety { get; }
    public void Generate(out Variable variable);
    public void SetHighlight(bool enable);
}
