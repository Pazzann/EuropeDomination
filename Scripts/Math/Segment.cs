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
        return Godot.Geometry2D.GetClosestPointToSegment(p, Point0, Point1).IsEqualApprox(p);
    }

    public bool Intersects(Segment other) {
        return Godot.Geometry2D.SegmentIntersectsSegment(Point0, Point1, other.Point0, other.Point1).VariantType != Godot.Variant.Type.Nil;
    }
}