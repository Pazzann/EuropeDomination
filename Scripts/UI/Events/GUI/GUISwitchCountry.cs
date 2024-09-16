namespace EuropeDominationDemo.Scripts.UI.Events.GUI;

public struct GUISwitchCountry : GUIEvent
{
    public int Id;
    public GUISwitchCountry(int id) => Id = id;
}