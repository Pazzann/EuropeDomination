using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUICountryInfo : GUIHandler
{
	private AnimatedSprite2D _flag;
	private Label _moneyLabel;
	private Label _manpowerLabel;
	
	public override void Init()
	{
		_flag = GetNode<AnimatedSprite2D>("./CountryFlag");
		_moneyLabel = GetNode<Label>("./MainUi/MoneyCount");
		_manpowerLabel = GetNode<Label>("MainUi/ManpowerCount");
		return;
	}
	public override void InputHandle(InputEvent @event)
	{
		
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIUpdateCountryInfo:
				_flag.Frame = EngineState.PlayerCountryId;
				_moneyLabel.Text = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].Money.ToString("N0");
				_manpowerLabel.Text = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].Manpower.ToString();
				return;
			default:
				return;
		}
	}

	private void _onOpenCountryUIPressed()
	{
		InvokeGUIEvent(new GUIShowCountryWindowEvent());
	}
}
