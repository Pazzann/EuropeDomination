using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using Godot;

public partial class GUIColonizeProvinceInfo : GUIHandler
{
	private UncolonizedProvinceData _currentlyShownProvince;
	private Button _colonizeButton;

	public override void Init()
	{
		_colonizeButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/Button");
	}

	public override void InputHandle(InputEvent @event)
	{
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGuiShowLandProvinceDataEvent:
			case ToGUIShowArmyViewerEvent:
			case ToGUIShowDiplomacyWindow:
			case ToGUIShowCountryWindowEvent:
				Visible = false;
				return;
			case ToGUIShowUncolonizedProvinceData e:
			{
				_currentlyShownProvince = e.UncolonizedProvinceData;
				Visible = true;
				_colonizeButton.Visible = e.CanBeColonized;
				return;
			}
		}
	}
}
