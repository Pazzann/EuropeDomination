using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Math;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

public partial class CurvedArrow : Node2D
{
	private Path2D _path2D;
	private PathFollow2D _pathFollow2D;
	private Line2D _line2D;
	private Line2D _progressLine2D;
	private Sprite2D _arrowHead;

	public override void _Ready()
	{
		_path2D = GetChild(0) as Path2D;
		_pathFollow2D = _path2D.GetChild(0) as PathFollow2D;
		_line2D = GetChild(1) as Line2D;
		_progressLine2D = GetChild(2) as Line2D;
		_arrowHead = GetChild(3) as Sprite2D;
		
		_path2D.Curve = new Curve2D();
		MaxValue = 1;
		Value = 0;
	}

	// Sets up the path2d to follow points in the list.
	// Doesn't display anything: for that see DrawLine.
	public void Setup(List<Vector2> points)
	{
		_path2D.Curve.AddPoint(ToLocal(points[0]));
		for (int i = 1; i < points.Count - 1; i++)
		{
			Vector2 p = points[i].ProjectOntoLine(points[i - 1], points[i + 1]);
			_path2D.Curve.AddPoint(ToLocal(points[i]), 50f*(points[i-1]-p), 50f*(points[i+1]-p));
		}
		_path2D.Curve.AddPoint(ToLocal(points[^1]));
		_arrowHead.Position = ToLocal(points[^1]);
		_arrowHead.Rotation = _path2D.Curve.SampleBakedWithRotation(_path2D.Curve.GetBakedLength()).Rotation;
	}

	public bool RemovePoint(int id)
	{
		_path2D.Curve.RemovePoint(id);
		if (_path2D.Curve.PointCount <= 1)
		{
			QueueFree();
			return false;
		}
		_line2D.ClearPoints();
		DrawLine();
		return true;
	}

	// Draws a line on the screen that follows the path set up in Setup.
	public void DrawLine()
	{
		_line2D.ClearPoints();
		_line2D.Position = _path2D.Position;
		_line2D.DefaultColor = Color.Color8(255, 0, 0, 255);
		_line2D.Width = 100;
		_line2D.JointMode = Line2D.LineJointMode.Round;
		foreach (Vector2 point in _path2D.Curve.GetBakedPoints())
		{
			_line2D.AddPoint(point);
		}
	}

	private int IndexOfClosestBakedPoint(Vector2 point)
	{
		float minDistance = float.PositiveInfinity;
		int res = -1;
		Vector2[] bakedPoints = _path2D.Curve.GetBakedPoints();
		for (int i = 0; i < bakedPoints.Length; i++)
		{
			if ((point - bakedPoints[i]).Length() < minDistance)
			{
				minDistance = (point - bakedPoints[i]).Length();
				res = i;
			}
		}

		return res;
	}
	
	private void DrawProgressLine()
	{
		_progressLine2D.ClearPoints();
		_progressLine2D.Position = _path2D.Position;
		_progressLine2D.DefaultColor = Color.Color8(0, 255, 0, 255);
		_progressLine2D.Width = 100;
		_progressLine2D.JointMode = Line2D.LineJointMode.Round;
		_progressLine2D.EndCapMode = Line2D.LineCapMode.Round;
		
		Vector2[] bakedPoints = _path2D.Curve.GetBakedPoints();
		int index = IndexOfClosestBakedPoint(_pathFollow2D.Position);
		for (int i = 0; i <= index; i++)
		{
			_progressLine2D.AddPoint(bakedPoints[i]);
		}
		_progressLine2D.AddPoint(_pathFollow2D.Position);
	}

	public float MaxValue;
	private float _trueValue;
	public float Value
	{
		get => _trueValue;
		set
		{
			_trueValue = value;
			_pathFollow2D.ProgressRatio = value/MaxValue;
			DrawProgressLine();
		}
	}
}
