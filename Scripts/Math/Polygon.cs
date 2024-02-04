using Godot;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Math;

public class Polygon {
	private readonly List<Vector2> _vertices = new();

	public IList<Vector2> Vertices => _vertices.AsReadOnly();

	public void AddVertex(Vector2 vertex) {
		_vertices.Add(vertex);
	}

	public void SortVertices() {
		if (!Geometry2D.IsPolygonClockwise(_vertices.ToArray()))
			_vertices.Reverse();
	}

	// public bool Intersects(in Segment segment) {
	// 	for (var i = 0; i < _vertices.Count - 1; ++i) {
	// 		if (segment.Intersects(new Segment(_vertices[i], _vertices[i + 1])))
	// 			return true;
	// 	}
	//
	// 	return segment.Intersects(new Segment(_vertices[0], _vertices[Vertices.Count - 1]));
	// }

	public bool Intersects<T>(in T path) where T : IPath
	{
		for (var i = 0; i < _vertices.Count - 1; ++i)
		{
			var edge = new Segment(_vertices[i], _vertices[i + 1]);
			
			if (path.Intersects(edge))
				return true;
		}

		return false;
	}

	// public bool IsPointInsidePolygon(Vector2 point) {
	// 	var segment = new Segment(point, point + new Vector2(10000f, 10000f));
	// 	var intersectionCount = 0;
	//
	// 	for (int i = 0; i < _vertices.Count - 1; ++i) {
	// 		if (segment.Intersects(new Segment(_vertices[i], _vertices[i + 1])))
	// 			intersectionCount++;
	// 	}
	//
	// 	if (segment.Intersects(new Segment(_vertices[0], _vertices[Vertices.Count - 1])))
	// 		intersectionCount++;
	//
	// 	return intersectionCount % 2 == 1;
	// }
}
