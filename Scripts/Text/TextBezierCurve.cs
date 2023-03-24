using EuropeDominationDemo.Scripts.Math;
using Godot;

namespace EuropeDominationDemo.Scripts.Text;

public partial class TextBezierCurve : Node
{
	private PackedScene _textScene;
	public BezierCurve Curve;
	public string TextOnCurve;
	public override void _Ready()
	{
		_textScene = (PackedScene)GD.Load("res://Prefabs/Letter.tscn");
		TextOnCurve = "nnnnn";
		Curve = new BezierCurve();
		Curve.Segment1 = new Vector2(-70, 30);
		Curve.Segment2 = new Vector2(70, 30);
		Curve.Vertex = new Vector2(0, 0);
		DrawText();
	}

	public void DrawText()
	{
		int i = 0;
		foreach (var letter in TextOnCurve)
		{
			i *= -1;
			var x = i * 20;
			var obj = (Label)_textScene.Instantiate();
			GD.Print("t:", Curve.GetT(x));
			GD.Print("y:", Curve.YFromX(x));
			GD.Print("tan:", Curve.TgOnPoint(Curve.GetT(x)));
			//GD.Print("degree:", (float)((180 / System.Math.PI) * (System.Math.PI - System.Math.Atan(Curve.TgOnPoint(Curve.GetT(15.0f))))));
			GD.Print("degree:", (float)System.Math.Atan(Curve.TgOnPoint(Curve.GetT(x))));
			obj.Position = new Vector2(x, Curve.YOnCurveX(Curve.GetT(x)));
			obj.Rotation = (float)System.Math.Atan(Curve.TgOnPoint(Curve.GetT(x)));
			obj.Text = letter.ToString();
			AddChild(obj);
			if (i >= 0)
			{
				i++;
			}

		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
	
