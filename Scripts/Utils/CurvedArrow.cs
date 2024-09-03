using System.Collections.Generic;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

public partial class CurvedArrow : Node2D
{
    private Path2D _path2D;
    private PathFollow2D _pathFollow2D;
    private Line2D _line2D;

    public override void _Ready()
    {
        _path2D = GetChild(0) as Path2D;
        _pathFollow2D = _path2D.GetChild(0) as PathFollow2D;
        _line2D = GetChild(1) as Line2D;

        _path2D.Curve = new Curve2D();
    }

    public void Setup(List<Vector2> points)
    {
        _path2D.Curve.AddPoint(ToLocal(points[0]));
        for (int i = 1; i < points.Count - 1; i++)
        {
            _path2D.Curve.AddPoint(ToLocal(points[i]));
            // TODO: To make the line smooth, add parameters in and out
            // (https://docs.godotengine.org/en/stable/classes/class_curve2d.html#class-curve2d-method-add-point)
        }
        _path2D.Curve.AddPoint(ToLocal(points[^1]));
    }

    public void AddPoint(Vector2 point)
    {
        _path2D.Curve.AddPoint(point);
    }

    public void RemovePoint(int id)
    {
        _path2D.Curve.RemovePoint(id);
        if (_path2D.Curve.PointCount == 0)
            QueueFree();
        _line2D.ClearPoints();
        DrawLine();
    }

    public void DrawLine()
    {
        _line2D.Position = _path2D.Position;
        _line2D.DefaultColor = Color.Color8(255, 0, 0, 255);
        _line2D.Width = 100;
        _line2D.JointMode = Line2D.LineJointMode.Round;
        foreach (Vector2 point in _path2D.Curve.GetBakedPoints())
        {
            _line2D.AddPoint(point);
        }
    }

    public float Value // This is between 0 and 1. TODO: Make this a progress bar
    {
        get => _pathFollow2D.ProgressRatio;
        set => _pathFollow2D.ProgressRatio = value;
    }
}
