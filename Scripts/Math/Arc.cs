 using Godot;
using static System.MathF;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Arc {
    public readonly Vector2 Center;
    public readonly float Radius, Point0, Point1, Point2;

    public Arc(Vector2 p0, Vector2 p1, Vector2 p2) {
        if (p0 == p1 || p1 == p2 || p0 == p2)
            throw new System.ArgumentException();
        
        var p01 = new Segment(p0, p1); 
        var p12 = new Segment(p1, p2);

        Center = p01.GetPerpendicularBisector().GetIntersection(p12.GetPerpendicularBisector());

        Point0 = getAngle(p0 - Center);
        Point1 = getAngle(p1 - Center);
        Point2 = getAngle(p2 - Center);

        Radius = (p0 - Center).Length();

        if (Point0 > Point2)
            (Point0, Point2) = (Point2, Point0);
    }

    public Vector2 GetPoint(float t) {
        if (t < 0 || t > 1)
            throw new System.ArgumentException();

        float p0 = Point0, p1 = Point2;

        if (getX(p0) > getX(p1))
           (p0, p1) = (p1, p0);
        
        float a = p0 * (1 - t) + p1 * t;

        return getPointFromAngle(a);
    }

    public Vector2 GetTangent(float t) {
        if (t < 0 || t > 1)
            throw new System.ArgumentException();

        float p0 = Point0, p1 = Point2;

        if (getX(p0) > getX(p1))
            (p0, p1) = (p1, p0);

        float a = p0 * (1 - t) + p1 * t;
        return (-p0 + p1) * Radius * new Vector2(-Sin(a), Cos(a));
    }

    public bool Intersects(Segment segment) {
        float a = (segment.Line.K * segment.Line.K + 1);
        float b = 2 * (segment.Line.B * segment.Line.K - Center.X - segment.Line.K * Center.Y);
        float c = segment.Line.B * segment.Line.B + Center.X * Center.X - 2 * segment.Line.B * Center.Y + Center.Y * Center.Y - Radius * Radius;

        float d = b * b - 4 * a * c;

        if (d < 0)
            return false;

        float x1 = (-b + System.MathF.Sqrt(d)) / (2 * a);
        float x2 = (-b - System.MathF.Sqrt(d)) / (2 * a);

        return (containsPoint(x1) && x1 >= segment.MinX && x1 <= segment.MaxX) || (containsPoint(x2) && x2 >= segment.MinX && x2 <= segment.MaxX);
    }

    private Vector2 getPointOnCircle(float x) {
        return Center + new Vector2((x - Center.X), System.MathF.Sqrt(Radius * Radius - x * x));
    }

    private float getX(float angle) {
        return Center.X + Radius * Cos(angle);
    }

    private Vector2 getPointFromAngle(float angle) {
        GD.Print($"Angle: {angle}; Point: {Center + Radius * new Vector2(Cos(angle), Sin(angle))}");
        return Center + Radius * new Vector2(Cos(angle), Sin(angle));
    }

    private static float getAngle(Vector2 point) {
        return Mathf.PosMod(System.MathF.Atan2(point.Y, point.X), 2 * PI);
    }

    private bool containsPoint(float x) {
        float a = getAngle(getPointOnCircle(x) - Center);

        if (Point1 > Point0 && Point1 < Point2)
            return a >= Point0 && a <= Point2;
        else
            return a <= Point0 && a >= Point2;
    }
}