using Godot;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.Math;

public class Polygon {
    private List<Vector2> vertices;

    public Polygon() {
        vertices = new List<Vector2>();
    }

    public void AddVertex(Vector2 vertex) {
        vertices.Add(vertex);
    }

    public void SortVertices() {
        // Sort vertices in the CCW order
        vertices.Sort((lhs, rhs) => -lhs.Cross(rhs).CompareTo(0));
    }

    public bool Intersects(ThickArc arc) {
        // TODO: add checks for "sides" as well
        return Intersects(arc.Lower) || Intersects(arc.Upper);
    }

    public bool Intersects(Arc arc) {
        for (int i = 0; i < vertices.Count - 1; ++i) {
            if (!arc.Intersects(new Segment(vertices[i], vertices[i + 1])))
                return false;
        }

        return true;
    }
}