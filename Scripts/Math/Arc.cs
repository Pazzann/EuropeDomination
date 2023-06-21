using Godot;
using static System.MathF;

namespace EuropeDominationDemo.Scripts.Math;

public class Arc {
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

    private Arc(float radius, float point0, float point1, float point2, Vector2 center) {
        Radius = radius;
        Point0 = point0;
        Point1 = point1;
        Point2 = point2;
        Center = center;
    }

    public Arc offset(Vector2 delta) {
        return new Arc(Radius, Point0, Point1, Point2, Center + delta);
    }

    public Vector2 GetPoint(float t) {
        if (t < 0 || t > 1)
            throw new System.ArgumentException();

        float p0 = Point0, p1 = Point2;

        if (getX(p0) > getX(p1))
           (p0, p1) = (p1, p0);

        if (Point1 < Point0 && Point1 > Point2)
            t = 1 - t;
        
        float a = p0 * (1 - t) + p1 * t;

        if (Point1 < Point0 && Point1 > Point2)
            a += PI;

        return getPointFromAngle(a);
    }

    public Vector2 GetTangent(float t) {
        if (t < 0 || t > 1)
            throw new System.ArgumentException();

        float p0 = Point0, p1 = Point2;

        if (getX(p0) > getX(p1))
            (p0, p1) = (p1, p0);

        if (Point1 < Point0 && Point1 > Point2)
            t = 1 - t;

        float a = p0 * (1 - t) + p1 * t;

        if (Point1 < Point0 && Point1 > Point2)
            a += PI;

        return (-p0 + p1) * Radius * new Vector2(-Sin(a), Cos(a));
    }

    public bool Intersects(Segment segment) {
        var p0 = segment.Point0 - Center;
        var p1 = segment.Point1 - Center;

        var delta = p1 - p0;
        var delta_len2 = delta.LengthSquared();
        var D = Utils.det(p0.X, p1.X, p0.Y, p1.Y);

        var discriminant = Radius * Radius * delta_len2 - D * D;

        if (discriminant < 0.001)
            return false;

        var sgn = Sign(delta.Y);

        var ip0 = new Vector2(D * delta.Y + sgn * delta.X * Sqrt(discriminant), -D * delta.X + Abs(delta.Y) * Sqrt(discriminant));
        var ip1 = new Vector2(D * delta.Y - sgn * delta.X * Sqrt(discriminant), -D * delta.X - Abs(delta.Y) * Sqrt(discriminant));

        return (containsPoint(ip0.X) && segment.ContainsPoint(ip0)) || (containsPoint(ip1.X) && segment.ContainsPoint(ip1));
    }

    private Vector2 getPointOnCircle(float x) {
        return Center + new Vector2((x - Center.X), System.MathF.Sqrt(Radius * Radius - x * x));
    }

    private float getX(float angle) {
        return Center.X + Radius * Cos(angle);
    }

    private Vector2 getPointFromAngle(float angle) {
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