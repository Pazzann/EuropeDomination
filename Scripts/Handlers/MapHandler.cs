using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Text;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToEngine;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;

using Godot;
using Godot.Collections;
using ToGuiHideProvinceDataEvent = EuropeDominationDemo.Scripts.UI.Events.ToGUI.ToGuiHideProvinceDataEvent;

namespace EuropeDominationDemo.Scripts.Handlers;

public partial class MapHandler : GameHandler
{
	private Sprite2D _mapSprite;
	private ShaderMaterial _mapMaterial;


	private PackedScene _textScene;
	private Node2D _countryTextSpawner;
	private Node2D _provinceTextSpawner;

	private PackedScene _goodsScene;
	private Node2D _goodsSpawner;

	private PackedScene _devScene;
	private Node2D _devSpawner;

	private PackedScene _transportArrowScene;
	private Node2D _transportArrowSpawner;

	private Good _goodToTransport;
	private RouteAdressProvider _whereToAddRoute;
	private NewTransportationRouteReciever _uiReciever;
	private double _transportationAmount;
	
	private float _lastClickTimestamp = 0.0f;


	public override void Init()
	{
		_mapSprite = GetNode<Sprite2D>("./Map");
		_mapMaterial = _mapSprite.Material as ShaderMaterial;


		_textScene = (PackedScene)GD.Load("res://Prefabs/Text.tscn");
		_countryTextSpawner = GetNode<Node2D>("./CountryTextHandler");
		_provinceTextSpawner = GetNode<Node2D>("./ProvinceTextHandler");

		_goodsScene = (PackedScene)GD.Load("res://Prefabs/Good.tscn");
		_goodsSpawner = GetNode<Node2D>("./GoodsHandler");

		_devScene = (PackedScene)GD.Load("res://Prefabs/Development.tscn");
		_devSpawner = GetNode<Node2D>("./DevHandler");

		_transportArrowScene = (PackedScene)GD.Load("res://Prefabs/TransportArrow.tscn");
		_transportArrowSpawner = GetNode<Node2D>("TransportArrowHandler");


		_mapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
		_mapMaterial.SetShaderParameter("selectedID", -1);

		_updateText();
		_addGoods();
		_addDev();

		_goodsSpawner.Visible = false;
		_devSpawner.Visible = false;
		_transportArrowSpawner.Visible = false;
	}

	public override bool InputHandle(InputEvent @event, int tileId)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
		{
			_lastClickTimestamp = Time.GetTicksMsec() / 1000f;
			return false;
		}

		if (@event is not InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false }) return false;
		if (Time.GetTicksMsec() / 1000f - _lastClickTimestamp > 0.2f) return false;

		
		//todo: rewrite
		if (EngineState.MapInfo.CurrentMapMode == MapTypes.TransportationSelection && EngineState.MapInfo.MapColors[tileId] != MapDefaultColors.Unselectable)
		{
			if (EngineState.MapInfo.MapColors[tileId] ==  MapDefaultColors.Selectable)
			{
				var route = new TransportationRoute(tileId, EngineState.MapInfo.CurrentSelectedProvinceId, _goodToTransport,
					_transportationAmount);
				_whereToAddRoute.Invoke(route);
				_uiReciever.Invoke(route);
			}else if (EngineState.MapInfo.MapColors[tileId] ==  MapDefaultColors.OwnProvince)
			{
				_whereToAddRoute = null;
			}
			
			EngineState.MapInfo.CurrentMapMode = MapTypes.Political;
			InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] as LandProvinceData));
			InvokeToEngineEvent(new ToEngineViewModUpdate());
			return true;
		}

		if (EngineState.MapInfo.CurrentMapMode == MapTypes.SeaTransportationSelection &&
		    EngineState.MapInfo.MapColors[tileId] != MapDefaultColors.Unselectable)
		{
			if (EngineState.MapInfo.MapColors[tileId] ==  MapDefaultColors.Selectable)
			{
				var route = new WaterTransportationRoute(tileId, EngineState.MapInfo.CurrentSelectedProvinceId, _goodToTransport,
					_transportationAmount);
				_whereToAddRoute.Invoke(route);
				_uiReciever.Invoke(route);
			}else if (EngineState.MapInfo.MapColors[tileId] ==  MapDefaultColors.OwnProvince)
			{
				_whereToAddRoute = null;
			}
			
			EngineState.MapInfo.CurrentMapMode = MapTypes.Political;
			InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] as LandProvinceData));
			InvokeToEngineEvent(new ToEngineViewModUpdate());
			return true;
		}

		if (tileId == EngineState.MapInfo.CurrentSelectedProvinceId || tileId < 0)
		{
			DeselectProvince();
			EngineState.MapInfo.CurrentSelectedProvinceId = -2;
			return false;
		}

		EngineState.MapInfo.CurrentSelectedProvinceId = tileId;
		if (EngineState.MapInfo.Scenario.Map[tileId] is WastelandProvinceData)
			return false;
		_mapMaterial.SetShaderParameter("selectedID", tileId);
		if (EngineState.MapInfo.Scenario.Map[tileId] is LandProvinceData data)
			InvokeToGUIEvent(new ToGuiShowLandProvinceDataEvent(data));
		return false;
	}

	public override void ViewModUpdate(float zoom)
	{
		switch (EngineState.MapInfo.CurrentMapMode)
		{
			case MapTypes.Political:
			{
				_goodsSpawner.Visible = false;

				if (zoom < 3.0f)
				{
					_countryTextSpawner.Visible = true;
					_provinceTextSpawner.Visible = false;
					_devSpawner.Visible = false;
					_transportArrowSpawner.Visible = false;
				}
				else
				{
					_countryTextSpawner.Visible = false;
					_provinceTextSpawner.Visible = true;
					_devSpawner.Visible = true;
					_transportArrowSpawner.Visible = false;
					
					_addTransportArrows();
				}

				break;
			}
			case MapTypes.Goods:
			{
				_goodsSpawner.Visible = true;
				_devSpawner.Visible = false;
				_countryTextSpawner.Visible = false;
				_provinceTextSpawner.Visible = false;
				_transportArrowSpawner.Visible = false;
				break;
			}
			case MapTypes.Terrain:
			{
				_goodsSpawner.Visible = false;
				_countryTextSpawner.Visible = false;
				_provinceTextSpawner.Visible = false;
				_devSpawner.Visible = false;
				_transportArrowSpawner.Visible = false;
				break;
			}
			case MapTypes.Trade: //TODO: Finish
				_goodsSpawner.Visible = false;
				_countryTextSpawner.Visible = false;
				_provinceTextSpawner.Visible = false;
				_devSpawner.Visible = false;
				_transportArrowSpawner.Visible = true;
				break;
			case MapTypes.Development:
				break;
			case MapTypes.Factories:
				break;
			case MapTypes.TransportationSelection:
				_goodsSpawner.Visible = false;
				_countryTextSpawner.Visible = false;
				_provinceTextSpawner.Visible = false;
				_devSpawner.Visible = false;
				_transportArrowSpawner.Visible = false;
				break;
			case MapTypes.SeaTransportationSelection:
				_goodsSpawner.Visible = false;
				_countryTextSpawner.Visible = false;
				_provinceTextSpawner.Visible = false;
				_devSpawner.Visible = false;
				_transportArrowSpawner.Visible = false;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		_mapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
		_mapMaterial.SetShaderParameter("viewMod", zoom < 3.0f ? 1 : 0);
	}

	public override void GUIInteractionHandler(GUIEvent @event)
	{
		if (EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] is LandProvinceData data)
			switch (@event)
			{
				case GUIBuildBuildingEvent e:
					var province = (LandProvinceData)EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId];
					if (EngineState.PlayerCountryId == province.Owner &&
						EngineState.MapInfo.Scenario.Countries[province.Owner].Money - e.NewBuilding.Cost >= 0)
					{
						EngineState.MapInfo.Scenario.Countries[province.Owner].Money -= e.NewBuilding.Cost;
						data.Buildings.Add(e.NewBuilding);
						InvokeToGUIEvent(
							new ToGUIUpdateLandProvinceDataEvent(
								(LandProvinceData)EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId]));
						InvokeToGUIEvent(new ToGUIUpdateCountryInfo());
					}

					return;
				case GUIDestroyBuildingEvent e:
					var province2 = (LandProvinceData)EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId];
					if (EngineState.PlayerCountryId == province2.Owner)
					{
						data.Buildings.RemoveAt(e.DestroyedId);
						InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(province2));
					}

					return;
				case GUIGoodTransportChange e:
					_goodToTransport = e.GoodToTransport;
					_transportationAmount = e.Amount;
					_whereToAddRoute = e.RouteAdress;
					_uiReciever = e.NewTransportationRouteReciever;
					return;
				default:
					return;
			}
	}


	private void _clearText()
	{
		var texts = _countryTextSpawner.GetChildren();
		foreach (var text in texts)
			text.Free();
	}

	public void DeselectProvince()
	{
		InvokeToGUIEvent(new ToGuiHideProvinceDataEvent());
		EngineState.MapInfo.CurrentSelectedProvinceId = -1;
		_mapMaterial.SetShaderParameter("selectedID", EngineState.MapInfo.CurrentSelectedProvinceId);
	}

	private void _updateText()
	{
		_clearText();

        var mapContours = new MapContours(EngineState.MapInfo, EngineState.MapInfo.Scenario.MapTexture);
        var allLabels = new ConcurrentBag<CurvedText>();

        Parallel.ForEach(EngineState.MapInfo.Scenario.Countries.Values, country =>
        {
            var labels = new TextSolver2(mapContours.States, country.Id, country.Name, 0.5f).FitText();

            foreach (var label in labels)
                allLabels.Add(label);
        });
        
        foreach (var label in allLabels)
        {
            var node = _textScene.Instantiate() as CurvedLabel;
            node!.Text = label;
            _countryTextSpawner.AddChild(node);
        }
        
        allLabels.Clear();

        Parallel.ForEach(EngineState.MapInfo.Scenario.Map, province =>
        {
            var labels = new TextSolver2(mapContours.Provinces, province.Id, province.Name, 0.5f).FitText();

            foreach (var label in labels)
                allLabels.Add(label);
        });

        foreach (var label in allLabels)
        {
            var node = _textScene.Instantiate() as CurvedLabel;
            node!.Text = label;
            _provinceTextSpawner.AddChild(node);
        }
	}

	private void _addGoods()
	{
		foreach (var data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
		{
			AnimatedSprite2D obj = _goodsScene.Instantiate() as AnimatedSprite2D;
			obj.Frame = ((LandProvinceData)data).Good.Id;
			obj.Position = ((LandProvinceData)data).CenterOfWeight;
			_goodsSpawner.AddChild(obj);
		}
	}

	private void _addDev()
	{
		foreach (var data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
		{
			AnimatedSprite2D obj = _devScene.Instantiate() as AnimatedSprite2D;
			obj.Frame = ((LandProvinceData)data).Development - 1;
			obj.Position = ((LandProvinceData)data).CenterOfWeight;
			_devSpawner.AddChild(obj);
		}
	}


	private void _drawArrow(TransportationRoute route)
	{
		var arrow = _transportArrowScene.Instantiate() as Node2D;
		ProvinceData[] map = EngineState.MapInfo.Scenario.Map;
					
		arrow.GlobalPosition = Math.MathUtils.VectorCenter(map[route.ProvinceIdFrom].CenterOfWeight, map[route.ProvinceIdTo].CenterOfWeight);
					
		float arrowScale = (map[route.ProvinceIdTo].CenterOfWeight - map[route.ProvinceIdFrom].CenterOfWeight).Length() / arrow.GetChild<Sprite2D>(0).Texture.GetSize().X;

		arrow.Scale = new Vector2(arrowScale, arrowScale);
					
		arrow.LookAt(map[route.ProvinceIdFrom].CenterOfWeight);
					
		_transportArrowSpawner.AddChild(arrow);
	}
	
	
	private void _addTransportArrows()
	{
		var children = _transportArrowSpawner.GetChildren();
		foreach (var child in children)
		{
			_transportArrowSpawner.RemoveChild(child);
			child.QueueFree();
		}
		
		HashSet<Tuple<int, int>> drawnArrows = new HashSet<Tuple<int, int>>();
		foreach (LandProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
		{
			if (data.HarvestedTransport != null)
			{
				if (drawnArrows.Contains(new Tuple<int, int>(data.HarvestedTransport.ProvinceIdFrom,
					                                         data.HarvestedTransport.ProvinceIdTo)))
				{
					// TODO: Add [multiple] icons to transport arrow
				}
				else
				{
					_drawArrow(data.HarvestedTransport);
					drawnArrows.Add(new Tuple<int, int>(data.HarvestedTransport.ProvinceIdFrom,
						                                data.HarvestedTransport.ProvinceIdTo));
				}
			}

			foreach (SpecialBuilding building in data.SpecialBuildings.Where(b => b != null))
			{
				if (building is Factory factory && factory.TransportationRoute != null)
				{
					if (drawnArrows.Contains(new Tuple<int, int>(factory.TransportationRoute.ProvinceIdFrom,
						    factory.TransportationRoute.ProvinceIdTo)))
					{
						// TODO: Add [multiple] icons to transport arrow
					}
					else
					{
						_drawArrow(factory.TransportationRoute);
						drawnArrows.Add(new Tuple<int, int>(factory.TransportationRoute.ProvinceIdFrom,
							factory.TransportationRoute.ProvinceIdTo));
					}
				}

				if (building is StockAndTrade stockAndTrade)
				{
					foreach (var route in stockAndTrade.TransportationRoutes)
					{
						if (route != null)
						{
							if (drawnArrows.Contains(new Tuple<int, int>(route.ProvinceIdFrom,
								    route.ProvinceIdTo)))
							{
								// TODO: Add [multiple] icons to transport arrow
							}
							else
							{
								_drawArrow(route);
								drawnArrows.Add(new Tuple<int, int>(route.ProvinceIdFrom,
									route.ProvinceIdTo));
							}
						}
					}
				}
			}
			
			// If any new routes get added, put them here
		}
	}


	public override void DayTick()
	{
		foreach (LandProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
		{
			foreach (var building in data.Buildings.Where(building => !building.IsFinished))
			{
				building.BuildingTime++;
				if (building.BuildingTime == building.TimeToBuild)
				{
					building.IsFinished = true;
				}
			}
			foreach (var specialBuilding in data.SpecialBuildings.Where(d=>d !=null))
			{
				if (specialBuilding is MilitaryTrainingCamp camp)
				{
					if (camp.TrainingList.Count > 0 && camp.TrainingList.Peek().DayTick())
					{
						var a = camp.TrainingList.Dequeue();
						a.IsFinished = true;
						var b = new ArmyUnitData("test", data.Owner, data.Id, Modifiers.DefaultModifiers(),
							new List<ArmyRegiment>() { a }, null, new List<KeyValuePair<int, int>>(), 0,
							UnitStates.Standing);
						EngineState.MapInfo.Scenario.Countries[data.Owner].Units.Add(b);
						InvokeToEngineEvent(new ToEngineAddArmyUnitEvent(b));
					}
				}
			}
		}

		if (EngineState.MapInfo.CurrentSelectedProvinceId > -1 && EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] is LandProvinceData landData)
		{
			InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
		}
	}


	public override void MonthTick()
	{
		//goodgenerationandtransportation
		foreach (LandProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
		{
			data.Resources[data.Good.Id] += data.ProductionRate;
			if (data.HarvestedTransport != null)
			{
				var diff = data.Resources[data.Good.Id] - Mathf.Max(data.Resources[data.Good.Id] - data.HarvestedTransport.Amount, 0);
				data.Resources[data.Good.Id] -= diff;
				(EngineState.MapInfo.Scenario.Map[data.HarvestedTransport.ProvinceIdTo] as LandProvinceData).Resources[data.HarvestedTransport.TransportationGood.Id] += diff;
			}
		}
		//factorystage
		foreach (LandProvinceData data in EngineState.MapInfo.MapProvinces(ProvinceTypes.ColonizedProvinces))
		{
			foreach (SpecialBuilding building in data.SpecialBuildings.Where(b=> b!=null))
			{
				if (building is Factory factory && factory.Recipe != null)
				{
					if (factory.Recipe.Ingredients.Where(ingredient=> data.Resources[ingredient.Key.Id] - ingredient.Value * factory.ProductionRate < 0).ToArray().Length <= 0)
					{
						foreach (var ingredient in factory.Recipe.Ingredients)
						{
							data.Resources[ingredient.Key.Id] -= ingredient.Value * factory.ProductionRate;
						}

						data.Resources[factory.Recipe.Output.Id] += factory.ProductionRate;
						factory.ProductionRate = Mathf.Min(factory.ProductionRate + factory.ProductionGrowthRate, factory.MaxProductionRate);
					}
					else
					{
						factory.ProductionRate = Mathf.Max(factory.ProductionRate - factory.ProductionGrowthRate, 0.1f);
					}
					if (factory.TransportationRoute != null)
					{
						var diff = data.Resources[factory.Recipe.Output.Id] - Mathf.Max(data.Resources[factory.Recipe.Output.Id] - factory.TransportationRoute.Amount, 0);
						data.Resources[factory.Recipe.Output.Id] -= diff;
						(EngineState.MapInfo.Scenario.Map[factory.TransportationRoute.ProvinceIdTo] as LandProvinceData).Resources[factory.Recipe.Output.Id] += diff;
					}
				}

				if (building is StockAndTrade stockAndTrade)
				{
					foreach (var route in stockAndTrade.TransportationRoutes)
					{
						if (route != null)
						{
							var diff = data.Resources[route.TransportationGood.Id] - Mathf.Max(data.Resources[route.TransportationGood.Id] - route.Amount, 0);
							data.Resources[route.TransportationGood.Id] -= diff;
							(EngineState.MapInfo.Scenario.Map[route.ProvinceIdTo] as LandProvinceData).Resources[route.TransportationGood.Id] += diff;
						}
					}
				}
				if (building is Dockyard dockyard)
				{
					foreach (var route in dockyard.WaterTransportationRoutes)
					{
						if (route != null)
						{
							var diff = data.Resources[route.TransportationGood.Id] - Mathf.Max(data.Resources[route.TransportationGood.Id] - route.Amount, 0);
							data.Resources[route.TransportationGood.Id] -= diff;
							(EngineState.MapInfo.Scenario.Map[route.ProvinceIdTo] as LandProvinceData).Resources[route.TransportationGood.Id] += diff;
						}
					}
				}
			}
		}

		if (EngineState.MapInfo.CurrentSelectedProvinceId > -1 && EngineState.MapInfo.Scenario.Map[EngineState.MapInfo.CurrentSelectedProvinceId] is LandProvinceData landData)
		{
			InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
		}
	}

	public override void YearTick()
	{
	}
}
