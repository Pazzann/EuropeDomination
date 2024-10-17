using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public interface IPath
{
    public Vector2 GetPoint(float t);

    public Vector2 GetTangent(float t);

    public bool Intersects(in Segment segment);
}