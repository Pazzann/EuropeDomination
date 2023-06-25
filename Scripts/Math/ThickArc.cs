using Godot;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Math;

public class ThickArc {
    public readonly Arc Lower, Upper;
    public readonly Segment Side1, Side2;

    public ThickArc(Arc arc, float thickness) {
        Vector2 center = arc.GetPoint(0.5f);
        Line normal = new Line(center, arc.GetTangent(0.5f)).getPerpendicularAt(0.5f);
        Vector2 delta = normal.Dir.Normalized() * (thickness / 2);

        Lower = arc.offset(-delta);
        Upper = arc.offset(delta);

        Side1 = new Segment(Lower.GetPoint(0f), Upper.GetPoint(0f));
        Side2 = new Segment(Lower.GetPoint(1f), Upper.GetPoint(1f));
    }

    public Vector2 GetPoint(float t) {
        return (Lower.GetPoint(t) + Upper.GetPoint(t)) / 2;
    }

    public Vector2 GetTangent(float t) {
        return Lower.GetTangent(t);
    }

    public float GetAngle() {
        return Lower.GetAngle();
    }

    public static ThickArc fitText(Arc baseArc, int letterCount, Vector2 letterSize) {
        var letterWidthNormalized = letterSize.X / baseArc.Length();

        var leftMostLetterParam = 0.5f - letterWidthNormalized * (letterCount / 2 + 1f);
        var rightMostLetterParam = 0.5f + letterWidthNormalized * (letterCount / 2 + 1f);

        if (leftMostLetterParam < 0.1f || rightMostLetterParam > 0.9f)
            return null;
        
        var arc = new Arc(baseArc.Center, baseArc.GetPoint(leftMostLetterParam), baseArc.GetPoint(rightMostLetterParam));

        return new ThickArc(arc, letterSize.Y);
    }
}