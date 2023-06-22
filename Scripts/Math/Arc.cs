using Godot;
using static System.MathF;
using EuropeDominationDemo;

namespace EuropeDominationDemo.Scripts.Math;

public class Arc {
	private readonly Vector2 center;
	private readonly float radius, angle0, angle1;
	private readonly bool isCCW, isReversed;

	public static (Arc, Arc) withAngle(Vector2 p0, Vector2 p1, float angle) {
		Line bisector = new Segment(p0, p1).GetPerpendicularBisector();
		var p = bisector.Point0;
		var dir = bisector.Dir.Normalized();
		var radiusProjLen = 0.5f * (p1 - p0).Length() / Mathf.Tan(angle / 2);

		return (new Arc(p + dir * radiusProjLen, p0, p1), new Arc(p - dir * radiusProjLen, p0, p1));
	}

	public Arc(Vector2 center, Vector2 p0, Vector2 p1) {
		// if (p0 == p1 || p0 == center || p1 == center || Mathf.Abs((center - p0).LengthSquared() - (center - p1).LengthSquared()) > 0.01f)
		// 	throw new System.ArgumentException();

		this.center = center;

		if (p0.X < p1.X)
			(p0, p1) = (p1, p0);

		angle0 = getAngle(p0 - this.center);
		angle1 = getAngle(p1 - this.center);

		isCCW = Mathf.PosMod(angle1 - angle0, 2 * PI) <= PI;

		if (isCCW && angle0 > angle1)
			angle1 += 2 * PI;
		
		if (angle0 > angle1) {
			(angle0, angle1) = (angle1, angle0);
			isReversed = true;
		} else
			isReversed = false;

		if (isCCW)
			isReversed = !isReversed;

		radius = (p0 - this.center).Length();
	}

	private Arc(float radius, float angle0, float angle1, Vector2 center, bool isCCW) {
		this.radius = radius;
		this.angle0 = angle0;
		this.angle1 = angle1;
		this.center = center;
		this.isCCW = isCCW;
	}

	public Arc offset(Vector2 delta) {
		return new Arc(radius, angle0, angle1, center, isCCW);
	}

	public float Length() {
		return radius * GetAngle();
	}

	public Vector2 GetPoint(float t) {
		if (t < 0 || t > 1)
			throw new System.ArgumentException();
		
		GD.Print($"{angle0} {angle1} {Mathf.PosMod(angle1 - angle0, 2 * PI)} {isCCW} {isReversed}");

		if (isReversed)
			t = 1 - t;

		float a;

		if (isCCW)
			a = angle0 + GetAngle() * t;
		else 
			a = angle1 - GetAngle() * t;
		
		return getPointFromAngle(a);
	}

	public float GetAngle() {
		return Mathf.Min(angle1 - angle0, 2 * PI - (angle1 - angle0));
	}

	public Vector2 GetTangent(float t) {
		if (t < 0 || t > 1)
			throw new System.ArgumentException();

		if (isReversed)
			t = 1 - t;

		float a, d;

		if (isCCW) {
			a = angle0 + GetAngle() * t;
			d = GetAngle();
		} else {
			a = angle1 - GetAngle() * t;
			d = -GetAngle();
		}

		if (isReversed)
			d = -d;

		return d * radius * new Vector2(-Sin(a), Cos(a));
	}

	public bool Intersects(Segment segment) {
		if (Mathf.IsNaN(center.X) || Mathf.IsNaN(center.Y))
			return true;
		
		var p0 = segment.Point0 - center;
		var p1 = segment.Point1 - center;

		var delta = p1 - p0;
		var delta_len2 = delta.LengthSquared();
		var D = Utils.det(p0.X, p1.X, p0.Y, p1.Y);

		var discriminant = radius * radius * delta_len2 - D * D;

		if (discriminant < 0.001)
			return false;

		var sgn = Sign(delta.Y);

		var ip0 = new Vector2(D * delta.Y + sgn * delta.X * Sqrt(discriminant), -D * delta.X + Abs(delta.Y) * Sqrt(discriminant));
		var ip1 = new Vector2(D * delta.Y - sgn * delta.X * Sqrt(discriminant), -D * delta.X - Abs(delta.Y) * Sqrt(discriminant));

		return (containsPoint(ip0.X) && segment.ContainsPoint(ip0)) || (containsPoint(ip1.X) && segment.ContainsPoint(ip1));
	}

	private Vector2 getPointOnCircle(float x) {
		x -= center.X;
		return center + new Vector2(x, System.MathF.Sqrt(radius * radius - x * x));
	}

	private Vector2 getPointFromAngle(float angle) {
		return center + radius * new Vector2(Cos(angle), Sin(angle));
	}

	private bool containsPoint(float x) {
		float a = getAngle(getPointOnCircle(x) - center);
		float a0 = Mathf.PosMod(angle0, 2 * PI), a1 = Mathf.PosMod(angle1, 2 * PI);

		if (a0 > a1)
			(a0, a1) = (a1, a0);

		if (isCCW)
			return a >= a0 && a <= a1;
		else
			return a <= a0 || a >= a1;
	}

	private static float getAngle(Vector2 point) {
		return Mathf.PosMod(System.MathF.Atan2(point.Y, point.X), 2 * PI);
	}
}
