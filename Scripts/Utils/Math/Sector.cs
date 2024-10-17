using System;
using System.Runtime.CompilerServices;
using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Sector : IPath
{
    private readonly float _radius;
    private readonly float _angle0;
    private readonly float _angle1;
    private readonly bool _isCcw;

    public Vector2 Center { get; }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static (Sector, Sector) WithAngle(Vector2 p0, Vector2 p1, float angle)
    {
        var (p, dir) = new Segment(p0, p1).GetPerpendicularBisector();
        dir = dir.Normalized();

        var radiusProjLen = 0.5f * (p1 - p0).Length() / Mathf.Tan(angle / 2);

        return (new Sector(p + dir * radiusProjLen, p0, p1), new Sector(p - dir * radiusProjLen, p0, p1));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Sector(Vector2 center, Vector2 p0, Vector2 p1)
    {
        Center = center;
        _radius = (p0 - Center).Length();

        // Adjust endpoint arrangement

        if (p0.X > p1.X)
            (p0, p1) = (p1, p0);

        // Convert to polar coordinates

        _angle0 = GetAngleFromPoint(p0 - Center);
        _angle1 = GetAngleFromPoint(p1 - Center);

        // If the curve should be interpolated counterclockwise or clockwise (always choose the smaller arc)
        _isCcw = GetAngleDiffCcw(_angle0, _angle1) <= Mathf.Pi;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private Sector(float radius, float angle0, float angle1, Vector2 center, bool isCcw)
    {
        _radius = radius;
        _angle0 = angle0;
        _angle1 = angle1;
        Center = center;
        _isCcw = isCcw;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Sector Translated(Vector2 delta)
    {
        return new Sector(_radius, _angle0, _angle1, Center + delta, _isCcw);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public float ArcLength()
    {
        return _radius * GetAngle();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Vector2 GetPoint(float t)
    {
        return GetPointFromAngle(InterpolateAngle(t));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Vector2 GetTangent(float t)
    {
        if (t is < 0 or > 1)
            throw new ArgumentException($"Invalid interpolation parameter: {t}");

        var a = InterpolateAngle(t);
        return InterpolateAngleDerivative(t) * _radius * new Vector2(-Mathf.Sin(a), Mathf.Cos(a));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Intersects(in Segment segment)
    {
        var p = segment.Point0;
        var v = segment.Line.Dir.Normalized();
        var u = Center - p;
        var u1 = u.Dot(v) * v;
        var u2 = u - u1;
        var d2 = u2.LengthSquared();

        if (d2 > _radius * _radius)
            return false;

        var m = Mathf.Sqrt(_radius * _radius - d2);
        var p1 = p + u1 + m * v;
        var p2 = p + u1 - m * v;

        return (ContainsPoint(p1) && segment.ContainsPoint(p1)) || (ContainsPoint(p2) && segment.ContainsPoint(p2));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private float GetAngle()
    {
        return _isCcw ? GetAngleDiffCcw(_angle0, _angle1) : GetAngleDiffCcw(_angle1, _angle0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private Vector2 GetPointFromAngle(float angle)
    {
        return Center + _radius * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private bool ContainsPoint(Vector2 point)
    {
        const float eps = 0.0001f;

        var a = GetAngleFromPoint(point - Center);
        float diff;

        if (_isCcw)
            diff = GetAngleDiffCcw(_angle0, a + eps) + GetAngleDiffCcw(a - eps, _angle1);
        else
            diff = GetAngleDiffCcw(a - eps, _angle0) + GetAngleDiffCcw(_angle1, a + eps);

        return Mathf.Abs(diff - GetAngle()) < 2.3f * eps;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private float InterpolateAngle(float t)
    {
        return _isCcw ? _angle0 + t * GetAngle() : _angle0 - t * GetAngle();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private float InterpolateAngleDerivative(float t)
    {
        return _isCcw ? GetAngle() : -GetAngle();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static float GetAngleFromPoint(Vector2 point)
    {
        return Mathf.PosMod(Mathf.Atan2(point.Y, point.X), Mathf.Tau);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private static float GetAngleDiffCcw(float a1, float a2)
    {
        return Mathf.PosMod(a2 - a1, Mathf.Tau);
    }
}