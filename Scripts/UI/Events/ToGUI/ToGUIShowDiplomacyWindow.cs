using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public struct ToGUIShowDiplomacyWindow : ToGUIEvent
{
    public CountryData CountryData;

    public ToGUIShowDiplomacyWindow(CountryData countryData)
    {
        CountryData = countryData;
    }
}