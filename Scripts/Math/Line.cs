using System.Runtime.CompilerServices;
using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly record struct Line(Vector2 Point0, Vector2 Dir)
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public Vector2 GetIntersection(Line other)
    {
        var intersection = Geometry2D.LineIntersectsLine(Point0, Dir, other.Point0, other.Dir);
        return intersection.AsVector2();
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public Line GetPerpendicularAt(float t) {
        var p = Point0 + t * Dir;
        return new Line(p, Dir.GetPerpendicular());
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public bool IsParallelToApprox(Line other) {
        return Mathf.Abs(Dir.Cross(other.Dir)) < 0.01f;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public bool ContainsPoint(Vector2 point) {
        return Mathf.Abs((point - Point0).Cross(Dir)) < 0.00001f;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public Line Translated(in Vector2 delta)
    {
        return new Line(Point0 + delta, Dir);
    }
}