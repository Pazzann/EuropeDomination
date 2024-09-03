using Godot;
using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Utils;

public partial class GUIStockAndTrade : Control
{
	[Signal]
	public delegate void StockAndTradeTrasportationRouteMenuPressedEventHandler(int id);
	
	private GridContainer _transportationContainer;

	public void Init()
	{
		_transportationContainer =
			GetNode<GridContainer>("PanelContainer/MarginContainer/VBoxContainer/TransportContainer");

		var i = 0;
		foreach (var child in _transportationContainer.GetChildren())
		{
			var b = i;
			child.GetChild(1).GetChild<Button>(1).Pressed += (() => _onTransportButtonPressed(b));
			i++;
		}
	}

	public void ShowData(StockAndTrade stockAndTrade)
	{
		for (int i = 0; i < stockAndTrade.TransportationRoutes.Length; i++)
		{
			
			var child = _transportationContainer.GetChild(i);
			if (stockAndTrade.TransportationRoutes[i] != null)
			{
				var route = stockAndTrade.TransportationRoutes[i];
				child.GetChild<Label>(0).Visible = true;
				child.GetChild<Label>(0).Text = "-" + route.Amount.ToString("N1") + "t/m";
				child.GetChild(1).GetChild<AnimatedTextureRect>(0).SetFrame(route.TransportationGood.Id);
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
		EmitSignal(SignalName.StockAndTradeTrasportationRouteMenuPressed, id);
	}
	
}
