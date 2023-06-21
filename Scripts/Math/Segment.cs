using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Segment {
    public readonly Line Line;

    public Vector2 Point0 {
        get => Line.Point0;
    }

    public Vector2 Point1 {
        get => Line.Point1;
    }

    public Segment(Vector2 p0, Vector2 p1) {
        if (p0.X > p1.X)
            (p0, p1) = (p1, p0);
        
        Line = new Line(p0, p1);
    }

    public Line GetPerpendicularBisector() {
        return Line.getPerpendicularAt(0.5f);
    }

    public bool ContainsPoint(Vector2 p) {
        return System.MathF.Abs(((p - Point0).Length() + (Point1 - p).Length()) - (Point1 - Point0).Length()) < 0.01;
    }
}