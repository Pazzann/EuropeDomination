using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Line
{
    public readonly Vector2 Point0, Point1;

    public Line(Vector2 p0, Vector2 p1)
    {
        Point0 = p0;
        Point1 = p1;
    }

    public Vector2 GetDirection()
    {
        return Point1 - Point0;
    }

    public Vector2 GetIntersection(Line other)
    {
        float det1 = Utils.det(Point0.X, Point0.Y, Point1.X, Point1.Y);

        float det2 = Utils.det(other.Point0.X, other.Point0.Y, other.Point1.X, other.Point1.Y);

        float det3 = Utils.det(
            Point0.X - Point1.X, Point0.Y - Point1.Y,
            other.Point0.X - other.Point1.X, other.Point0.Y - other.Point1.Y
        );

        float det4 = Utils.det(
            det1, Point0.X - Point1.X,
            det2, other.Point0.X - other.Point1.X
        );

        float det5 = Utils.det(
            det1, Point0.Y  - Point1.Y,
            det2, other.Point0.Y - other.Point1.Y
        );

        return new Vector2(det4 / det3, det5 / det3);
    }

    public Line getPerpendicularAt(float t) {
        var p = Point0 * (1 - t) + Point1 * t;
        return new Line(p, p + getPerpendicular(Point1 - Point0));
    }

    static Vector2 getPerpendicular(Vector2 v) {
        return new Vector2(v.Y, -v.X);
    }
}