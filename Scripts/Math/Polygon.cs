using Godot;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Math;

public class Polygon {
	private List<Vector2> vertices;

	public IList<Vector2> Vertices {
		get => vertices.AsReadOnly();
	}

	public Polygon() {
		vertices = new List<Vector2>();
	}

	public void AddVertex(Vector2 vertex) {
		vertices.Add(vertex);
	}

	public void SortVertices() {
		var sum = new Vector2();

		foreach (var vertex in vertices)
			sum += vertex;

		var center = sum / vertices.Count;

		vertices.Sort((lhs, rhs) => -(lhs - center).Angle().CompareTo((rhs - center).Angle()));

		Debug.Assert(Geometry2D.IsPolygonClockwise(vertices.ToArray()));
	}

	public bool Intersects(ThickArc arc) {
		return Intersects(arc.Lower) || Intersects(arc.Upper) || Intersects(arc.Side1) || Intersects(arc.Side2);
	}

	public bool Intersects(Arc arc) {
		for (int i = 0; i < vertices.Count - 1; ++i) {
			if (arc.Intersects(new Segment(vertices[i], vertices[i + 1])))
				return true;
		}

		if (arc.Intersects(new Segment(vertices[0], vertices[Vertices.Count - 1])))
			return true;

		return false;
	}

	public bool Intersects(Segment segment) {
		for (int i = 0; i < vertices.Count - 1; ++i) {
			if (segment.Intersects(new Segment(vertices[i], vertices[i + 1])))
				return true;
		}

		if (segment.Intersects(new Segment(vertices[0], vertices[Vertices.Count - 1])))
			return true;

		return false;
	}
}
