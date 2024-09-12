using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using Godot;

public partial class GUIColonizeProvinceInfo : GUIHandler
{
	private UncolonizedProvinceData _currentlyShownProvince;
	private Button _colonizeButton;
	private Label _colonizeProvinceName;
	private Label _colonizeProvinceLabel;
	private ProgressBar _colonizeProgressBar;

	public override void Init()
	{
		_colonizeProvinceName = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Label");
		_colonizeButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/Button");
		_colonizeProvinceLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Label2");
		_colonizeProgressBar = GetNode<ProgressBar>("PanelContainer/MarginContainer/VBoxContainer/ProgressBar");
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
				_showData(e.CanBeColonized);
				return;
			}
		}
	}

	private void _showData(bool canBeColonized )
	{
		Visible = true;
		_colonizeButton.Visible = canBeColonized;
		if (_currentlyShownProvince.CurrentlyColonizedByCountry != null)
		{
			_colonizeProgressBar.Visible = true;
			_colonizeProvinceLabel.Visible = true;
			_colonizeProgressBar.MaxValue = _currentlyShownProvince.SettlersNeeded;
			_colonizeProgressBar.Value = _currentlyShownProvince.SettlersCombined;
			_colonizeProvinceLabel.Text = $"{_currentlyShownProvince.SettlersCombined}/{_currentlyShownProvince.SettlersNeeded}";
		}
		else
		{
					
			_colonizeProgressBar.Visible = false;
			_colonizeProvinceLabel.Visible = false;
		}
	}

	private void _onColonizeButtonPressed()
	{
		InvokeGUIEvent(new GUIColonizeProvince(_currentlyShownProvince.Id));
	}
}
