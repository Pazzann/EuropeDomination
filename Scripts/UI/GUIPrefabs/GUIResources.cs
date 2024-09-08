using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIPrefabs;

public partial class GUIResources : VBoxContainer
{
	private PackedScene GUIResource;

	public void Init()
	{
		var resourceCount = EngineState.MapInfo.Scenario.Goods.Count;
		GUIResource = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIResource.tscn");
		(GetParent() as ScrollContainer).CustomMinimumSize = new Vector2(450.0f, 40.0f * resourceCount);
		CustomMinimumSize = new Vector2(450.0f, 40.0f * resourceCount);
		
		for (int i = 0; i < resourceCount; i++)
		{
			var a = GUIResource.Instantiate();
			(a.GetChild(0).GetChild(0).GetChild(0) as AnimatedTextureRect).SetFrame(i);
			AddChild(a);
		}
	}
	
	private void _clearInfo(double[] resources)
	{
		for (int i = 0; i < resources.Length; i++)
		{
			(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).Text = "+0t/m";
			(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).SelfModulate = new Color(1.0f, 1.0f, 1.0f);
		}
	}

	public void DrawResources(LandColonizedProvinceData data)
	{
		var AllResourcesChange = new double[EngineState.MapInfo.Scenario.Goods.Count];

		for (int i = 0; i < data.Resources.Length; i++)
		{
			AllResourcesChange[i] = 0;
		}

		_clearInfo(data.Resources);

		foreach (LandColonizedProvinceData provinceData in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
		{
			if (provinceData.HarvestedTransport != null && provinceData.HarvestedTransport.ProvinceIdTo == data.Id)
				AllResourcesChange[provinceData.HarvestedTransport.TransportationGood.Id] +=
					provinceData.HarvestedTransport.Amount;
			foreach (var building in provinceData.SpecialBuildings)
			{
				if (building is Factory factory && factory.TransportationRoute != null && factory.TransportationRoute.ProvinceIdTo == data.Id)
					AllResourcesChange[factory.TransportationRoute.TransportationGood.Id] +=
						factory.TransportationRoute.Amount;
				if (building is StockAndTrade stockAndTrade)
					foreach (var route in stockAndTrade.TransportationRoutes.Where(a => a!=null && a.ProvinceIdTo == data.Id))
							AllResourcesChange[route.TransportationGood.Id] += route.Amount;
				if (building is Dockyard dockyard)
					foreach (var route in dockyard.WaterTransportationRoutes.Where(a => a!=null && a.ProvinceIdTo == data.Id))
						AllResourcesChange[route.TransportationGood.Id] += route.Amount;
			}
		}
		

		foreach (var building in data.SpecialBuildings)
		{
			if (building is Factory factory && factory.Recipe != null)
			{
				foreach (var ingredient in factory.Recipe.Ingredients)
				{
					AllResourcesChange[ingredient.Key.Id] -= ingredient.Value * factory.ProductionRate;
				}

				AllResourcesChange[factory.Recipe.Output.Id] += factory.ProductionRate;
				if (factory.TransportationRoute != null)
				{
					AllResourcesChange[factory.Recipe.Output.Id] -= factory.TransportationRoute.Amount;
				}
			}

			if (building is StockAndTrade stockAndTrade)
			{
				foreach (var route in stockAndTrade.TransportationRoutes)
				{
					if (route != null)
					{
						AllResourcesChange[route.TransportationGood.Id] -= route.Amount;
					}
				}
			}
			if (building is Dockyard dockyard)
			{
				foreach (var route in dockyard.WaterTransportationRoutes)
				{
					if (route != null)
					{
						AllResourcesChange[route.TransportationGood.Id] -= route.Amount;
					}
				}
			}
		}
		

		AllResourcesChange[data.Good.Id] += data.ProductionRate;
		if (data.HarvestedTransport != null)
			AllResourcesChange[data.HarvestedTransport.TransportationGood.Id] -= data.HarvestedTransport.Amount;

		for (int i = 0; i < data.Resources.Length; i++)
		{
			(GetChild(i).GetChild(0).GetChild(0).GetChild(1) as Label).Text = data.Resources[i].ToString("N1");
			(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).Text = ((AllResourcesChange[i] >= 0) ? "+" : "") +
																			  AllResourcesChange[i].ToString("N1") + "t/m";
			if (AllResourcesChange[i] >= 0)
				(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).SelfModulate = MapDefaultColors.ResourceIncrease;
			else
				(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).SelfModulate = MapDefaultColors.ResourceDecrease;
		}
	}
}
