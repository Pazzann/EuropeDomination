using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUITimeWindow : GUIHandler
{
	private Label _dateLabel;
	private bool _isPaused;
	private Sprite2D _pauseSprite;

	public override void Init()
	{
		_pauseSprite = GetNode<Sprite2D>("HBoxContainer2/GameDate/PauseSprite");
		_dateLabel = GetNode<Label>("HBoxContainer2/GameDate/DateLabel");
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUISetDateEvent b:
				_dateLabel.Text = b.DateTime.Day + "." + b.DateTime.Month + "." + b.DateTime.Year;
				return;
			case ToGUISetPause e:
				_pauseSprite.Visible = true;
				_isPaused = true;
				return;
			default:
				return;
		}
	}

	public override void InputHandle(InputEvent @event)
	{
		if (@event.IsActionPressed("pause_unpause")) _onPauseButtonPressed();
	}

	private void _onPauseButtonPressed()
	{
		_isPaused = !_isPaused;
		_pauseSprite.Visible = _isPaused;
		InvokeGUIEvent(new GUIPauseStateEvent(_isPaused));
	}
}
