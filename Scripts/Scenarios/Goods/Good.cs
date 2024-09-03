using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.Goods;

public abstract class Good
{
    public int Id { get;}
    public Vector3 Color { get; }
    public string Name { get; }

    public Good(int id, string name, Vector3 color)
    {
        Id = id;
        Name = name;
        Color = color;
    }
    
    
    public static double[] DefaultGoods()
    {
        return new double[] { 0, 0, 0, 0 };
    }
}