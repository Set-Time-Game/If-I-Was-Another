using System.Collections.Generic;
using Classes.World;
using static Classes.Utils.Structs;

public interface IGenerable : IEntity
{
    public Variable Variety { get; }
    public void Generate(out Variable variable);
}
