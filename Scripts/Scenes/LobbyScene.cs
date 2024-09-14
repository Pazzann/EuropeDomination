using Godot;

namespace EuropeDominationDemo.Scripts.Scenes;

public partial class LobbyScene : Node2D
{
	private Camera _camera;
	public override void _Ready()
	{
		_camera = GetNode<Camera>("Camera");
		_camera.Reset(new Rect2(Vector2.Zero, GetNode<Sprite2D>("Map").Texture.GetSize()));
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		_camera.HandleInput(@event);
	}

	private void _onStartPressed()
	{
		GetTree().ChangeSceneToFile("res://Scenes/GameScene.tscn");
	}
}
