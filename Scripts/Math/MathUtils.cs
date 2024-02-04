using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public static class MathUtils {
    public static Vector2 VectorCenter(Vector2 posA, Vector2 posB)
    {
        return (posA + posB) / 2;
    }
    
    public static Vector2 GetPerpendicular(this Vector2 v) {
        return new Vector2(v.Y, -v.X);
    }
}