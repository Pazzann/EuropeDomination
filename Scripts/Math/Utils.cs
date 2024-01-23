using Godot;

namespace EuropeDominationDemo.Scripts.Math;

class Utils {
    private Utils() {}

    public static float det(float a, float b, float c, float d) {
        return a * d - b * c;
    }

    public static Vector2 VectorCenter(Vector2 posA, Vector2 posB)
    {
        return (posA + posB) / 2;
    }
}