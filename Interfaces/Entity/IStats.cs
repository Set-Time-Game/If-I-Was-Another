using static Classes.Utils.Flags;

public interface IStats
{
    public float Armor { get; }
    public float Alt { get; }
    public float Health { get; }
    public float Stamina { get; }
    public float Damage { get; }

    public double Agility { get; }
    public double Strength { get; }
    public double Intelligence { get; }
}