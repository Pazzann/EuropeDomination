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
		TextOnCurve = "GODOTGODOT";
		Curve = new BezierCurve();
		Curve.Segment1 = new Vector2(-80, 50);
		Curve.Segment2 = new Vector2(80, 50);
		Curve.Vertex = new Vector2(0, -10);
		DrawText();
	}

	public void DrawText()
	{
		var labelSettings = new LabelSettings();
		labelSettings.FontSize = 20;
		
		var prevCenter = new Vector2(0f, 0f);
		
		for (int i = 0; i < TextOnCurve.Length; i++) {
			var t = (float)i / ((float) (TextOnCurve.Length - 1));
			var obj = (Label)_textScene.Instantiate();
			
			obj.LabelSettings = labelSettings;
			obj.Size = new Vector2((float)labelSettings.FontSize / obj.Size.Y * obj.Size.X, (float)labelSettings.FontSize);
			
			obj.Position = Curve.GetPoint(t) - obj.Size / 2;
			obj.PivotOffset = obj.Size / 2;
			obj.Rotation = -Curve.GetTangent(t).AngleTo(new Vector2(1, 0));
			obj.Text = TextOnCurve[i].ToString();
			
			GD.Print("dst: ", (obj.Position - prevCenter).Length());
			prevCenter = obj.Position;
			
			GD.Print("pos: ", obj.Position);
			
			AddChild(obj);
		}
		
		/*int i = 0;
		foreach (var letter in TextOnCurve)
		{
			i *= -1;
			var x = i * 20;
			var obj = (Label)_textScene.Instantiate();
			GD.Print("t:", Curve.GetT(x));
			GD.Print("y:", Curve.YFromX(x));
			GD.Print("tan:", Curve.TgOnPoint(Curve.GetT(x)));*/
			//GD.Print("degree:", (float)((180 / System.Math.PI) * (System.Math.PI - System.Math.Atan(Curve.TgOnPoint(Curve.GetT(15.0f))))));
			/*GD.Print("degree:", (float)System.Math.Atan(Curve.TgOnPoint(Curve.GetT(x))));
			obj.Position = new Vector2(x, Curve.YOnCurveX(Curve.GetT(x)));*/
			//obj.Rotation = (float)System.Math.Atan(Curve.TgOnPoint(Curve.GetT(x)));
			/*obj.Text = letter.ToString();
			AddChild(obj);
			if (i >= 0)
			{
				i++;
			}

		}*/
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
	
