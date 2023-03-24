using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public class BezierCurve
{
    public Vector2 Vertex = new Vector2(0.0f, 0.0f);
    public Vector2 Segment1 = new Vector2(0.0f, 0.0f);
    public Vector2 Segment2 = new Vector2(0.0f, 0.0f);

    public bool IsDefault => (Vertex.IsZeroApprox() && Segment1.IsZeroApprox() && Segment2.IsZeroApprox());

    public float TgOnPoint(float t)
    {
        var a = 2.0f * (1 - t) * (Vertex - Segment1) + 2 * t * (Segment2 - Vertex);
        return a.Y / a.X;
    } 
    public float GetT(float x) => (new Vector2(x, YFromX(x)) - Segment1).Length()/(Segment2-Segment1).Length();
    
    public float YFromX(float x)
    {
        var a = (Segment2 - Segment1);
        return Segment1.Y + (a.Y/a.X) * (x - Segment1.X);
    }
    public float YOnCurveX(float t) => ((1 - t) * (1 - t) * Segment1 + 2 * (1 - t) * t * Vertex + t * t * Segment2).Y;
    
}