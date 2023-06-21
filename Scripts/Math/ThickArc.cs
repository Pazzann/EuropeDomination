using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct ThickArc {
    public readonly Arc Lower, Upper;

    public ThickArc(Arc arc, float thickness) {
        Vector2 center = arc.GetPoint(0.5f);
        Line normal = new Line(center, center + arc.GetTangent(0.5f)).getPerpendicularAt(center.X);
        Vector2 delta = normal.GetDirection() * (thickness / 2);

        Lower = arc.offset(-delta);
        Upper = arc.offset(delta);
    }
}