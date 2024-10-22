using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public partial class MultiplayerHandler : GameHandler
{
    //basic idea is that you have attribute over properties that you  want to sync
    //each day tick you send the data to the server that changed because you also store the previous days values and can compare
    //todo: implement the logic for the multiplayer handler
    
    public override void Init()
    {
        throw new System.NotImplementedException();
    }

    public override bool InputHandle(InputEvent @event, int tileId)
    {
        throw new System.NotImplementedException();
    }

    public override void ViewModUpdate(float zoom)
    {
        throw new System.NotImplementedException();
    }

    public override void GUIInteractionHandler(GUIEvent @event)
    {
        throw new System.NotImplementedException();
    }

    public override void DayTick()
    {
        throw new System.NotImplementedException();
    }

    public override void MonthTick()
    {
        throw new System.NotImplementedException();
    }

    public override void YearTick()
    {
        throw new System.NotImplementedException();
    }
}