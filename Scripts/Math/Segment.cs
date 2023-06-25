using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Segment {
    public readonly Line Line;

    public Vector2 Point0 {
        get => Line.Point0;
    }

    public Vector2 Point1 {
        get => Line.Point0 + Line.Dir;
    }

    public Segment(Vector2 p0, Vector2 p1) {
        if (p0.X > p1.X)
            (p0, p1) = (p1, p0);
        
        Line = new Line(p0, p1 - p0);
    }

    public Line GetPerpendicularBisector() {
        return Line.getPerpendicularAt(0.5f);
    }

    public bool ContainsPoint(Vector2 p) {
        // const float EPS = 0.1f;

        // if (!Line.ContainsPoint(p))
        //     return false;

        // var dot = (p - Point0).Dot(Line.Dir.Normalized());
        // return dot > EPS && dot < Line.Dir.LengthSquared() - EPS;

        // if (Mathf.Abs(((p - Point0).Length() + (p - Point1).Length() - (Point1 - Point0).Length())) < 0.01f) {
        //     GD.Print("77");
        // }

        return Mathf.Abs(((p - Point0).Length() + (p - Point1).Length() - (Point1 - Point0).Length())) < 0.0001f;
    }

    public bool Intersects(Segment other) {
        return Godot.Geometry2D.SegmentIntersectsSegment(Point0, Point1, other.Point0, other.Point1).VariantType != Godot.Variant.Type.Nil;
    }
}