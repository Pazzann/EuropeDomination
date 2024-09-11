using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using Godot;

public partial class GUIDiplomacyWindow : GUIHandler
{
    private CountryData _currentlyViewedCountry;
    private Label _currentlyViewedCountryNameLabel;
    private Button _currentlyViewedCountryTradeButton;
    private Label _currentlyViewedCountryTradeLabel;
    private Button _currentlyViewedCountryWarButton;
    private Label _currentlyViewedCountryWarLabel;

    public override void Init()
    {
        _currentlyViewedCountryNameLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/NameLabel");
        _currentlyViewedCountryWarLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/WarLabel");
        _currentlyViewedCountryWarButton = GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/WarButton");
        _currentlyViewedCountryTradeLabel = GetNode<Label>("PanelContainer/MarginContainer/VBoxContainer/TradeLabel");
        _currentlyViewedCountryTradeButton =
            GetNode<Button>("PanelContainer/MarginContainer/VBoxContainer/TradeButton");
    }

    public override void InputHandle(InputEvent @event)
    {
    }

    private void _showData()
    {
        _currentlyViewedCountryNameLabel.Text = _currentlyViewedCountry.Name;
        _currentlyViewedCountryWarLabel.Text =
            EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].DiplomacyAgreements
                .ContainsKey(_currentlyViewedCountry.Id) &&
            EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId]
                .DiplomacyAgreements[_currentlyViewedCountry.Id].OfType<War>().Any()
                ? " at war"
                : " not at war";
        _currentlyViewedCountryTradeLabel.Text =
            EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId]
                .DiplomacyAgreements
                .ContainsKey(_currentlyViewedCountry.Id) &&
            EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId]
                .DiplomacyAgreements[_currentlyViewedCountry.Id].OfType<TradeAgreement>()
                .Any()
                ? " at trade"
                : " not at trade";
    }

    public override void ToGUIHandleEvent(ToGUIEvent @event)
    {
        switch (@event)
        {
            case ToGUIShowDiplomacyWindow e:
            {
                _currentlyViewedCountry = e.CountryData;
                Visible = true;
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
            a.DiplomacyAgreements.Add(_currentlyViewedCountry.Id, new List<DiplomacyAgreement> { war });
            _currentlyViewedCountry.DiplomacyAgreements.Add(a.Id, new List<DiplomacyAgreement> { war });
        }

        _showData();
    }

    private void _onTradeAgreementPressed()
    {
        var a = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId];
        var trade = new TradeAgreement(a.Id, _currentlyViewedCountry.Id, EngineState.MapInfo.Scenario.Date);
        if (a.DiplomacyAgreements.ContainsKey(_currentlyViewedCountry.Id))
        {
            a.DiplomacyAgreements[_currentlyViewedCountry.Id].Add(trade);
            _currentlyViewedCountry.DiplomacyAgreements[a.Id].Add(trade);
        }
        else
        {
            a.DiplomacyAgreements.Add(_currentlyViewedCountry.Id, new List<DiplomacyAgreement> { trade });
            _currentlyViewedCountry.DiplomacyAgreements.Add(a.Id, new List<DiplomacyAgreement> { trade });
        }

        _showData();
    }
}