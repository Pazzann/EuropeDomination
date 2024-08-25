using System;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Text;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;
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

	private int _selectedTileId = -2;
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


		_mapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
		_mapMaterial.SetShaderParameter("selectedID", -1);

		_updateText();
		_addGoods();
		_addDev();

		_goodsSpawner.Visible = false;
		_devSpawner.Visible = false;
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


		if (tileId == _selectedTileId || tileId < 0)
		{
			DeselectProvince();
			_selectedTileId = -2;
			return false;
		}

		_selectedTileId = tileId;
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
				}
				else
				{
					_countryTextSpawner.Visible = false;
					_provinceTextSpawner.Visible = true;
					_devSpawner.Visible = true;
				}

				break;
			}
			case MapTypes.Goods:
			{
				_goodsSpawner.Visible = true;
				_devSpawner.Visible = false;
				_countryTextSpawner.Visible = false;
				_provinceTextSpawner.Visible = false;
				break;
			}
			case MapTypes.Terrain:
			{
				_goodsSpawner.Visible = false;
				_countryTextSpawner.Visible = false;
				_provinceTextSpawner.Visible = false;
				_devSpawner.Visible = false;
				break;
			}
			case MapTypes.Trade:
				break;
			case MapTypes.Development:
				break;
			case MapTypes.Factories:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		_mapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
		_mapMaterial.SetShaderParameter("viewMod", zoom < 3.0f ? 1 : 0);
	}

	public override void GUIInteractionHandler(GUIEvent @event)
	{
		if (EngineState.MapInfo.Scenario.Map[_selectedTileId] is LandProvinceData data)
			switch (@event)
			{
				case GUIBuildBuildingEvent e:
					var province = (LandProvinceData)EngineState.MapInfo.Scenario.Map[_selectedTileId];
					if (EngineState.PlayerCountryId == province.Owner &&
						EngineState.MapInfo.Scenario.Countries[province.Owner].Money - e.NewBuilding.Cost >= 0)
					{
						EngineState.MapInfo.Scenario.Countries[province.Owner].Money -= e.NewBuilding.Cost;
						data.Buildings.Add(e.NewBuilding);
						InvokeToGUIEvent(
							new ToGUIUpdateLandProvinceDataEvent(
								(LandProvinceData)EngineState.MapInfo.Scenario.Map[_selectedTileId]));
						InvokeToGUIEvent(new ToGUIUpdateCountryInfo());
					}

					return;
				case GUIDestroyBuildingEvent e:
					var province2 = (LandProvinceData)EngineState.MapInfo.Scenario.Map[_selectedTileId];
					if (EngineState.PlayerCountryId == province2.Owner)
					{
						data.Buildings.RemoveAt(e.DestroyedId);
						InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(province2));
					}

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
		_selectedTileId = -1;
		_mapMaterial.SetShaderParameter("selectedID", _selectedTileId);
	}

	private void _updateText()
	{
		_clearText();

		var mapContours = new MapContours(EngineState.MapInfo, EngineState.MapInfo.Scenario.MapTexture);

		foreach (var country in EngineState.MapInfo.Scenario.Countries.Values)
		{
			var labels = new TextSolver(mapContours.States, country.Id, country.Name, 0.5f).FitText();

			foreach (var label in labels)
			{
				var node = _textScene.Instantiate() as CurvedLabel;
				node!.Text = label;
				_countryTextSpawner.AddChild(node);
			}
		}

		foreach (var province in EngineState.MapInfo.Scenario.Map)
		{
			var labels = new TextSolver(mapContours.Provinces, province.Id, province.Name, 0.5f).FitText();
		
			foreach (var label in labels)
			{
				var node = _textScene.Instantiate() as CurvedLabel;
				node!.Text = label;
				_provinceTextSpawner.AddChild(node);
			}
		}

		// foreach (var data in EngineState.MapInfo.Scenario.Countries)
		// {
		// 	var provinces = EngineState.MapInfo.Scenario.CountryProvinces(data.Value.Id);
		//
		// 	if (provinces.Length == 0)
		// 		continue;
		//
		// 	var texts = new TextSolver(mapContours, data.Value.Name, data.Value.Id, 0.5f).FitText();
		//
		// 	foreach (var text in texts)
		// 	{
		// 		var textNode = _textScene.Instantiate() as CurvedLabel;
		// 		textNode.Text = text;
		// 		_textSpawner.AddChild(textNode);
		// 	}
		// 	
		// 	//
		// 	//
		// 	//
		// 	// var f = Time.GetTicksMsec();
		// 	//
		// 	//
		// 	//
		// 	// GD.Print(Time.GetTicksMsec() - f);
		// 	//
		// }
	}

	private void _addGoods()
	{
		foreach (var data in EngineState.MapInfo.Scenario.Map.Where(data => data is LandProvinceData))
		{
			AnimatedSprite2D obj = _goodsScene.Instantiate() as AnimatedSprite2D;
			obj.Frame = (int)((LandProvinceData)data).Good;
			obj.Position = ((LandProvinceData)data).CenterOfWeight;
			_goodsSpawner.AddChild(obj);
		}
	}

	private void _addDev()
	{
		foreach (var data in EngineState.MapInfo.Scenario.Map.Where(data => data is LandProvinceData))
		{
			AnimatedSprite2D obj = _devScene.Instantiate() as AnimatedSprite2D;
			obj.Frame = ((LandProvinceData)data).Development - 1;
			obj.Position = ((LandProvinceData)data).CenterOfWeight;
			_devSpawner.AddChild(obj);
		}
	}


	public override void DayTick()
	{
		foreach (var data in EngineState.MapInfo.Scenario.Map.Where(data => data is LandProvinceData))
		{
			foreach (var building in ((LandProvinceData)data).Buildings.Where(building => !building.IsFinished))
			{
				building.BuildingTime++;
				if (building.BuildingTime == building.TimeToBuild)
				{
					building.IsFinished = true;
				}
			}
		}

		if (_selectedTileId > -1 && EngineState.MapInfo.Scenario.Map[_selectedTileId] is LandProvinceData landData)
		{
			InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
		}
	}

	public override void MonthTick()
	{
		foreach (var data in EngineState.MapInfo.Scenario.Map.Where(data => data is LandProvinceData))
		{
			((LandProvinceData)data).Resources[(int)((LandProvinceData)data).Good] +=
				((LandProvinceData)data).ProductionRate;
		}

		if (_selectedTileId > -1 && EngineState.MapInfo.Scenario.Map[_selectedTileId] is LandProvinceData landData)
		{
			InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
		}
	}

	public override void YearTick()
	{
	}
}
