using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct Line {
    public readonly float K, B;

    public Line(Vector2 p0, Vector2 p1) {
        var p01 = p1 - p0;

        K = p01.Y / p01.X;
        B = p0.Y - K * p0.X;
    }

    public Line(float k, float b) {
        K = k;
        B = b;
    }

    public Vector2 GetIntersection(Line other) {
        var iX = (other.B - B) / (K - other.K);
        var iY = iX * K + B;

        return new Vector2(iX, iY);
    }

    public Line getPerpendicularAt(float x) {
        return new Line(-1.0f / K, x * K + x / K + B);
    }
}