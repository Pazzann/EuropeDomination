using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIDiplomacyWindow : GUIHandler
{
	
	private CountryData _currentlyViewedCountry;
	private Label _currentlyViewedCountryNameLabel;
	public override void Init()
	{
		_currentlyViewedCountryNameLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/Label");	
	}

	public override void InputHandle(InputEvent @event)
	{
		
	}

	private void _showData()
	{
		
		_currentlyViewedCountryNameLabel.Text = _currentlyViewedCountry.Name + (EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].DiplomacyAgreements.ContainsKey(_currentlyViewedCountry.Id) && EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].DiplomacyAgreements[_currentlyViewedCountry.Id].OfType<War>().Any()? " at war" :" not at war");

	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowDiplomacyWindow e:
			{
				_currentlyViewedCountry = e.CountryData;Visible = true;
				_showData();
				return;
			}
			case ToGUIShowCountryWindowEvent:
			case ToGUIShowArmyViewerEvent:
			case ToGuiShowLandProvinceDataEvent:
			{
				Visible = false;
				return;
			}
		}
	}
	

	private void _onDeclareWarButtonPressed()
	{
		var a = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId];
		var war = new War(a.Id, _currentlyViewedCountry.Id, EngineState.MapInfo.Scenario.Date);
		if (a.DiplomacyAgreements.ContainsKey(_currentlyViewedCountry.Id))
		{
			a.DiplomacyAgreements[_currentlyViewedCountry.Id].Add(war);
			_currentlyViewedCountry.DiplomacyAgreements[a.Id].Add(war);
		}
		else
		{
			a.DiplomacyAgreements.Add(_currentlyViewedCountry.Id, new List<DiplomacyAgreement>() { war });
			_currentlyViewedCountry.DiplomacyAgreements.Add(a.Id, new List<DiplomacyAgreement>() { war });
		}
		_showData();
	}
}
