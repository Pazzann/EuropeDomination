using Godot;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EuropeDominationDemo.Scripts.Math;

public class Polygon {
	private readonly List<Vector2> _vertices = new();

	public IReadOnlyList<Vector2> Vertices => _vertices.AsReadOnly();
	
	public Polygon(IEnumerable<Vector2> vertices)
	{
		_vertices.AddRange(vertices);
		
		if (!Geometry2D.IsPolygonClockwise(_vertices.ToArray()))
			_vertices.Reverse();
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
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
}
