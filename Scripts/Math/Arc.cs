using Godot;
using static System.MathF;
using EuropeDominationDemo;

namespace EuropeDominationDemo.Scripts.Math;

public class Arc {
	// Circle center
	private readonly Vector2 center;

	// Circle radius
	private readonly float radius;

	// Polar angle of the curve's start
	private readonly float angle0;

	// Polar angle of the curve's end
	private readonly float angle1;

	// If the curve should be traversed counter clockwise
	private readonly bool isCCW;

	public Vector2 Center {
		get => center;
	}

	public static (Arc, Arc) withAngle(Vector2 p0, Vector2 p1, float angle) {
		Line bisector = new Segment(p0, p1).GetPerpendicularBisector();
		var p = bisector.Point0;
		var dir = bisector.Dir.Normalized();
		var radiusProjLen = 0.5f * (p1 - p0).Length() / Mathf.Tan(angle / 2);

		return (new Arc(p + dir * radiusProjLen, p0, p1), new Arc(p - dir * radiusProjLen, p0, p1));
	}

	public Arc(Vector2 center, Vector2 p0, Vector2 p1) {
		this.center = center;
		this.radius = (p0 - this.center).Length();

		// Adjust curve's endpoint arrangement

		if (p0.X > p1.X)
			(p0, p1) = (p1, p0);

		// Transition to polar coordinates

		this.angle0 = getAngleFromPoint(p0 - this.center);
		this.angle1 = getAngleFromPoint(p1 - this.center);

		// Find out if the curve should be interpolated counter clockwise or clockwise

		this.isCCW = getAngleDiffCCW(angle0, angle1) <= PI;
	}

	private Arc(float radius, float angle0, float angle1, Vector2 center, bool isCCW) {
		this.radius = radius;
		this.angle0 = angle0;
		this.angle1 = angle1;
		this.center = center;
		this.isCCW = isCCW;
	}

	public Arc offset(Vector2 delta) {
		return new Arc(radius, angle0, angle1, center + delta, isCCW);
	}

	public float Length() {
		return radius * GetAngle();
	}

	public Vector2 GetPoint(float t) {
		return getPointFromAngle(interpolateAngle(t));
	}

	public float GetAngle() {
		if (isCCW)
			return getAngleDiffCCW(angle0, angle1);
		else
			return getAngleDiffCCW(angle1, angle0);
	}

	public Vector2 GetTangent(float t) {
		if (t < 0 || t > 1)
			throw new System.ArgumentException();

		float a = interpolateAngle(t);
		return interpolateAngleDerivative(t) * radius * new Vector2(-Sin(a), Cos(a));
	}

	public bool Intersects(Segment segment) {
		var p = segment.Point0;
		var v = segment.Line.Dir.Normalized();
		var u = Center - p;
		var u1 = u.Dot(v) * v;
		var u2 = u - u1;
		var d2 = u2.LengthSquared();

		if (d2 > radius * radius)
			return false;

		var m = Mathf.Sqrt(radius * radius - d2);
		var p1 = p + u1 + m * v;
		var p2 = p + u1 - m * v;

		// if ((containsPoint(p1) && segment.ContainsPoint(p1)) || (containsPoint(p2) && segment.ContainsPoint(p2)))
		// 	GD.Print("Found intersection");

		return (containsPoint(p1) && segment.ContainsPoint(p1)) || (containsPoint(p2) && segment.ContainsPoint(p2));
	}

	private Vector2 getPointFromAngle(float angle) {
		return center + radius * new Vector2(Cos(angle), Sin(angle));
	}

	private bool containsPoint(Vector2 point) {
		const float EPS = 0.0001f;

		float a = getAngleFromPoint(point - Center);
		float diff;

		if (isCCW)
			diff = getAngleDiffCCW(angle0, a + EPS) + getAngleDiffCCW(a - EPS, angle1);
		else
			diff = getAngleDiffCCW(a - EPS, angle0) + getAngleDiffCCW(angle1, a + EPS);

		return Mathf.Abs(diff - GetAngle()) < 2.3f * EPS;
	}

	private float interpolateAngle(float t) {
		return isCCW ? (angle0 + t * GetAngle()) : (angle0 - t * GetAngle());
	}

	private float interpolateAngleDerivative(float t) {
		return isCCW ? GetAngle() : -GetAngle();
	}

	private static float getAngleFromPoint(Vector2 point) {
		return Mathf.PosMod(System.MathF.Atan2(point.Y, point.X), 2 * PI);
	}

	private static float getAngleDiffCCW(float a1, float a2) {
		return Mathf.PosMod(a2 - a1, 2 * PI);
	}
}
