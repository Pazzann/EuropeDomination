using System.Runtime.CompilerServices;
using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly record struct Segment
{
    public readonly Line Line;

    public Vector2 Point0 => Line.Point0;
    public Vector2 Point1 => Line.Point0 + Line.Dir;

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public Segment(Vector2 p0, Vector2 p1)
    {
        if (p0.X > p1.X)
            (p0, p1) = (p1, p0);

        Line = new Line(p0, p1 - p0);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    private Segment(Line line)
    {
        Line = line;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public Line GetPerpendicularBisector()
    {
        return Line.GetPerpendicularAt(0.5f);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public bool ContainsPoint(Vector2 p, float eps = 0.0001f)
    {
        return Mathf.Abs((p - Point0).Length() + (p - Point1).Length() - (Point1 - Point0).Length()) < eps;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public bool Intersects(Segment other)
    {
        return Geometry2D.SegmentIntersectsSegment(Point0, Point1, other.Point0, other.Point1).VariantType !=
               Variant.Type.Nil;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public Segment Translated(in Vector2 delta)
    {
        return new Segment(Line.Translated(delta));
    }
}