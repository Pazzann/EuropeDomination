using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIDockyard : Control
{
	[Signal]
	public delegate void DockyardTrasportationRouteMenuPressedEventHandler(int id);
	
	private HBoxContainer _transportationContainer;

	public void Init()
	{
		_transportationContainer =
			GetNode<HBoxContainer>("PanelContainer/MarginContainer/VBoxContainer/TransportationContatiner");

		var i = 0;
		foreach (var child in _transportationContainer.GetChildren())
		{
			var b = i;
			child.GetChild(1).GetChild<Button>(1).Pressed += (() => _onTransportButtonPressed(b));
			i++;
		}
	}

	public void ShowData(Dockyard dockyard)
	{
		for (int i = 0; i < dockyard.WaterTransportationRoutes.Length; i++)
		{
			
			var child = _transportationContainer.GetChild(i);
			if (dockyard.WaterTransportationRoutes[i] != null)
			{
				var route = dockyard.WaterTransportationRoutes[i];
				child.GetChild<Label>(0).Visible = true;
				child.GetChild<Label>(0).Text = "-" + route.Amount.ToString("N1") + "t/m";
				child.GetChild(1).GetChild<AnimatedTextureRect>(0).SetFrame((int)route.TransportationGood);
				child.GetChild<Label>(2).Text = EngineState.MapInfo.Scenario.Map[route.ProvinceIdTo].Name;
			}
			else
			{
				child.GetChild<Label>(0).Visible = false;
				child.GetChild(1).GetChild<AnimatedTextureRect>(0).Texture = null;
				child.GetChild<Label>(2).Text = "nowhere";
			}
			
		}
	} 
	
	private void _onTransportButtonPressed(int id)
	{
		EmitSignal(SignalName.DockyardTrasportationRouteMenuPressed, id);
	}
}
