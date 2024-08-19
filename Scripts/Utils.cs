using Godot;

namespace EuropeDominationDemo.Scripts;

public static class Utils
{
    public static Vector2I RoundToInt(this Vector2 v)
    {
        var (x, y) = v;
        return new Vector2I(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
}