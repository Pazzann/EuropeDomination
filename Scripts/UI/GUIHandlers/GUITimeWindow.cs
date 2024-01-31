using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUITimeWindow : GUIHandler
{
	private Sprite2D _pauseSprite;
	private Label _dateLabel;
	private bool _isPaused = false;
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
			default:
				return;
		}
	}
	
	private void _onPauseButtonPressed()
	{
		_isPaused = !_isPaused;
		_pauseSprite.Visible = _isPaused;
		InvokeGUIEvent(new GUIPauseStateEvent(_isPaused));
	}

}


