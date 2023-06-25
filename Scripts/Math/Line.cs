using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Line
{
    public readonly Vector2 Point0, Dir;

    public Line(Vector2 p0, Vector2 dir)
    {
        Point0 = p0;
        Dir = dir;
    }

    public Vector2 GetIntersection(Line other)
    {
        var intersection = Godot.Geometry2D.LineIntersectsLine(Point0, Dir, other.Point0, other.Dir);
        return intersection.AsVector2();
    }

    public Line getPerpendicularAt(float t) {
        var p = Point0 + t * Dir;
        return new Line(p, getPerpendicular(Dir));
    }

    public bool IsParallelToApprox(Line other) {
        return Mathf.Abs(Dir.Cross(other.Dir)) < 0.01f;
    }

    public bool ContainsPoint(Vector2 point) {
        return Mathf.Abs((point - Point0).Cross(Dir)) < 0.00001f;
    }

    static Vector2 getPerpendicular(Vector2 v) {
        return new Vector2(v.Y, -v.X);
    }
}