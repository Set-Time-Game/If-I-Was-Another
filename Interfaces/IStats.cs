using System;
using UnityEngine;

public interface IStats
{
    public float Armor    { get; }
    public float Health   { get; }
    public float Damage   { get; }
    public float Stamina  { get; }

    public double Agility       { get; }
    public double Strength      { get; }
    public double Intelligence  { get; }
}
