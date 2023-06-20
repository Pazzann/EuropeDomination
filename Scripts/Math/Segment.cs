using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Segment {
    public readonly Line Line;
    public readonly float MinX, MaxX;

    public Segment(Vector2 p0, Vector2 p1) {
        if (p0.X > p1.X)
            (p0, p1) = (p1, p0);
        
        Line = new Line(p0, p1);
        MinX = p0.X;
        MaxX = p1.X;
    }

    public Line GetPerpendicularBisector() {
        return Line.getPerpendicularAt((MinX + MaxX) / 2);
    }
}