using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public class SolidPath<T> : IPath where T : IPath
{
    private T _path;
    private float _thickness;

    public T Path
    {
        set => _path = value;
    }

    public float Thickness
    {
        set => _thickness = value;
    }

    public SolidPath(T path, float thickness)
    {
        Path = path;
        Thickness = thickness;
    }

    public Vector2 GetPoint(float t)
    {
        return _path.GetPoint(t);
    }

    public Vector2 GetTangent(float t)
    {
        return _path.GetTangent(t);
    }

    public bool Intersects(in Segment segment)
    {
        var offset = ComputeNormal(_path).Normalized() * (_thickness * 0.5f);

        var start = GetPoint(0f);
        var end = GetPoint(1f);

        var leftSide = new Segment(start - offset, start + offset);
        var rightSide = new Segment(end - offset, end + offset);

        return _path.Intersects(segment.Translated(offset)) || _path.Intersects(segment.Translated(-offset)) ||
               leftSide.Intersects(segment) || rightSide.Intersects(segment);
    }

    private static Vector2 ComputeNormal(in T path)
    {
        var normal = path.GetTangent(0.5f).GetPerpendicular();
        return normal.Normalized();
    }
}