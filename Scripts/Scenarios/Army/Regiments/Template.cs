namespace EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;

public abstract class Template
{
    public string Name;
    public int Id;

    public Template(string name, int id)
    {
        Name = name;
        Id = id;
    }

    public abstract int TrainingTime { get; }
    public abstract float Cost { get; }
}