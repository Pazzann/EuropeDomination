using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI.Events.ToGUI;

public class ToGUIShowDiplomacyWindow : ToGUIEvent
{
    public CountryData CountryData;

    public ToGUIShowDiplomacyWindow(CountryData countryData)
    {
        CountryData = countryData;
    }
}