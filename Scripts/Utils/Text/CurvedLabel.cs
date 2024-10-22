using EuropeDominationDemo.Scripts.Math;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils.Text;

public readonly record struct CurvedText(string Text, float FontSize, IPath Path);

public partial class CurvedLabel : Node
{
	private CurvedText _text;
	private PackedScene _textScene;

	public CurvedText Text
	{
		get => _text;
		set
		{
			_text = value;

			if (IsInsideTree())
				Update();
		}
	}

	public override void _Ready()
	{
		_textScene = (PackedScene)GD.Load("res://Prefabs/Letter.tscn");
		Update();
	}

	private void Update()
	{
		var (text, fontSize, path) = Text;

		for (var i = 0; i < Text.Text.Length; i++)
		{
			var t = text.Length == 1 ? 0.5f : i / (float)(text.Length - 1);
			var angle = path.GetTangent(t).Angle();

			var obj = (Label)_textScene.Instantiate();
			obj.Size = new Vector2(obj.LabelSettings.FontSize / obj.Size.Y * obj.Size.X, obj.LabelSettings.FontSize);
			obj.Scale = new Vector2(fontSize / obj.LabelSettings.FontSize, fontSize / obj.LabelSettings.FontSize);
			obj.Position = path.GetPoint(t) - obj.Size * 0.5f;
			obj.PivotOffset = obj.Size * 0.5f;
			obj.Rotation = angle;
			obj.Text = text[i].ToString();

			AddChild(obj);
		}
	}
}
