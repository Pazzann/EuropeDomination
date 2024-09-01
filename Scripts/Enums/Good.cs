using Godot;

namespace EuropeDominationDemo.Scripts.Enums;

public enum Good
{
    Iron = 0,
    Wheat = 1,
    IronSword = 2,
    Wood = 3
}

public static class GoodsColors
{
    public static Vector3[] Colors = new Vector3[]
    {
        new Vector3(0.5f, 0.3f, 0.0f),
        new Vector3(0.7f, 0.8f, 0.0f),
        new Vector3(1.0f, 0.2f, 0.3f),
        new Vector3(0.0f, 0.7f, 0.4f)
    };

    public static double[] DefaultGoods()
    {
        return new double[] { 0, 0, 0, 0 };
    }
}