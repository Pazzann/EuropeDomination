using Godot;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Math;

public class ThickArc {
    public readonly Arc Lower, Upper;
    public Segment Side1, Side2;

    public ThickArc(Arc arc, float thickness) {
        Vector2 center = arc.GetPoint(0.5f);
        Line normal = new Line(center, center + arc.GetTangent(0.5f)).getPerpendicularAt(0.5f);
        Vector2 delta = normal.Dir.Normalized() * (thickness / 2);

        Lower = arc.offset(-delta);
        Upper = arc.offset(delta);

        Side1 = new Segment(Lower.GetPoint(0f), Upper.GetPoint(0f));
        Side2 = new Segment(Lower.GetPoint(1f), Upper.GetPoint(1f));
    }

    public Vector2 GetLetterCenter(float t) {
        return (Lower.GetPoint(t) + Upper.GetPoint(t)) / 2;
    }

    public Vector2 GetTangent(float t) {
        return Lower.GetTangent(t);
    }

    public static ThickArc fitText(Arc baseArc, int letterCount, Vector2 letterSize) {
        var letterWidthNormalized = letterSize.X / baseArc.Length();

        var leftMostLetterParam = 0.5f - letterWidthNormalized * (letterCount / 2 + 0.5f);
        var rightMostLetterParam = 0.5f + letterWidthNormalized * (letterCount / 2 + 0.5f);

        if (leftMostLetterParam < 0 || rightMostLetterParam > 1f)
            return null;

        leftMostLetterParam = 0.25f;
        rightMostLetterParam = 0.75f;

        var arc = new Arc(baseArc.GetPoint(leftMostLetterParam), baseArc.GetPoint(0.5f), baseArc.GetPoint(rightMostLetterParam));

        return new ThickArc(arc, letterSize.Y * 1.1f);
    }
}