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
		DrawText();
	}

	public void DrawText()
	{
		var labelSettings = new LabelSettings();
		labelSettings.FontSize = 20;
		
		if (Curve.Segment1.X > Curve.Segment2.X)
			(Curve.Segment1, Curve.Segment2) = (Curve.Segment2, Curve.Segment1);

		for (int i = 0; i < TextOnCurve.Length; i++)
		{
			var t = (float)i / ((float)(TextOnCurve.Length - 1));
			var obj = (Label)_textScene.Instantiate();

			obj.LabelSettings = labelSettings;
			obj.Size = new Vector2((float)labelSettings.FontSize / obj.Size.Y * obj.Size.X,
				(float)labelSettings.FontSize);

			obj.Position = Curve.GetPoint(t) - obj.Size / 2;
			obj.PivotOffset = obj.Size / 2;
			var angle = -Curve.GetTangent(t).AngleTo(new Vector2(1, 0));
			GD.Print(angle);
			// if (Curve.Vertex.Y < Curve.Segment1.Y && Curve.Vertex.Y < Curve.Segment2.Y)
			// {
			// 	angle -= (float)System.Math.PI;
			// }
			obj.Rotation = angle;
			obj.Text = TextOnCurve[i].ToString();
			
			AddChild(obj);
		}
	}
}
