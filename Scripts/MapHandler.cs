using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using EuropeDominationDemo.Scripts.Text;
using EuropeDominationDemo.Scripts.UI;
using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class MapHandler : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	private Image mapMap;
	public ShaderMaterial MapMaterial;
	public MapData MapData { get; set; }

	private GUI _gui;
	private PackedScene _textScene;
	private Node2D _textSpawner;

	private PackedScene _goodsScene;
	private Node2D _goodsSpawner;

	private PackedScene _devScene;
	private Node2D _devSpawner;

	private CameraBehaviour _camera;
	private Timer _timer;
	private int _previousMonth;
	
	private bool _freezed = false;
	
	private int _selectedTileId = -2;


	public override void _Ready()
	{
		mapMap = this.Texture.GetImage();
		MapMaterial = this.Material as ShaderMaterial;
		MapData = new MapData(new DemoScenario(mapMap));
		_textScene = (PackedScene)GD.Load("res://Prefabs/Text.tscn");
		_textSpawner = GetNode<Node2D>("../TextHandler");

		_goodsScene = (PackedScene)GD.Load("res://Prefabs/Good.tscn");
		_goodsSpawner = GetNode<Node2D>("../GoodsHandler");

		_devScene = (PackedScene)GD.Load("res://Prefabs/Development.tscn");
		_devSpawner = GetNode<Node2D>("../DevHandler");

		_camera = (CameraBehaviour)GetNode<Camera2D>("../Camera");
		_timer = (Timer)GetChild(0);

		MapMaterial.SetShaderParameter("colors", MapData.MapColors);
		MapMaterial.SetShaderParameter("selectedID", -1);

		_drawText();
		_addGoods();
		_addDev();
		_goodsSpawner.Visible = false;
		_devSpawner.Visible = false;

		_gui = GetNode<GUI>("../CanvasLayer/Control");
		_gui.SetTime(MapData.Scenario.Date);
		_previousMonth = MapData.Scenario.Date.Month;
	}

	public void MapUpdate()
	{
		switch (MapData.CurrentMapMode)
		{
			case MapTypes.Political:
			{
				_drawText();
				_goodsSpawner.Visible = false;

				if (_camera.IsZoomed())
				{
					HideObjectsOnMap();
				}
				else
				{
					ShowObjectsOnMap();
				}

				break;
			}
			case MapTypes.Goods:
			{
				_clearText();
				_goodsSpawner.Visible = true;
				_devSpawner.Visible = false;
				break;
			}
			case MapTypes.Terrain:
			{
				_clearText();
				_goodsSpawner.Visible = false;
				_devSpawner.Visible = false;
				break;
			}
		}

		MapMaterial.SetShaderParameter("colors", MapData.MapColors);
	}

	private void _clearText()
	{
		var texts = _textSpawner.GetChildren();
		foreach (var text in texts)
			text.Free();
	}

	public void HideObjectsOnMap()
	{
		if (MapData.CurrentMapMode == MapTypes.Political)
		{
			_textSpawner.Visible = true;
			_devSpawner.Visible = false;
		}
	}

	public void ShowObjectsOnMap()
	{
		if (MapData.CurrentMapMode == MapTypes.Political)
		{
			_textSpawner.Visible = false;
			_devSpawner.Visible = true;
		}
	}

	public void DeselectProvince()
	{
		_gui.DeselectProvinceBar();
		MapMaterial.SetShaderParameter("selectedID", -1);
	}

	private void _drawText()
	{
		_clearText();

		var stateMap = new StateMap(MapData, mapMap);

		foreach (var data in MapData.Scenario.Countries)
		{
			var provinces = MapData.Scenario.CountryProvinces(data.Value);
			if (provinces.Length == 0)
				continue;
			var curve = GameMath.FindBezierCurve(provinces);
			if (curve.IsDefault)
			{
				var ids = GameMath.FindSquarePointsInsideState(provinces, mapMap, 10);
				curve = GameMath.FindBezierCurveFromPoints(ids);
				curve.Sort();
			}

			//var arc = Arc.withAngle(curve.Segment1, curve.Segment2, Mathf.Pi / 6f).Item2;

			TextBezierCurve obj = _textScene.Instantiate() as TextBezierCurve;
			obj.Curve = curve;
			(obj.TextPath, obj.FontSize) = GameMath.FindSuitableTextPath(data.Value, stateMap, 0.5f, MapData.Scenario.CountriesNames[data.Value].Length, mapMap);
			//obj.TextPath = new ThickArc(arc, 30f);
			//obj.TextPath = Arc.withAngle(curve.Segment1, curve.Segment2, Mathf.Pi / 6f).Item1;
			obj.TextOnCurve = MapData.Scenario.CountriesNames[data.Value];
			_textSpawner.AddChild(obj);
		}
	}
	//TODO: add buildings slots variable
	public void AddBuilding(Building building)
	{
		if (_selectedTileId >= 0)
		{
			if (MapData.Scenario.Map[_selectedTileId].Buildings.Count < 10 && !MapData.Scenario.Map[_selectedTileId].Buildings.Exists(i => i.ID == 1) && MapData.Scenario.Map[_selectedTileId].Buildings.Count < 4)
			{
				MapData.Scenario.Map[_selectedTileId].Buildings.Add(building);
				_gui.ShowProvinceData(MapData.Scenario.Map[_selectedTileId]);
			}
		}
	}

	public void RemoveBuilding(List<Building> buildings)
	{
		MapData.Scenario.Map[_selectedTileId].Buildings = buildings;
		_gui.ShowProvinceData(MapData.Scenario.Map[_selectedTileId]);
	}

	private void _dayTick()
	{
		MapData.Scenario.Date = MapData.Scenario.Date.Add(MapData.Scenario.Ts);

		foreach (var data in MapData.Scenario.Map)
		{
			foreach (var building in data.Buildings)
			{
				if (!building.IsFinished)
				{
					building.BuildingTime++;
					if (building.BuildingTime == building.TimeToBuild)
					{
						building.IsFinished = true;
					}
				}
			}
		}
		
		


		if (_previousMonth != MapData.Scenario.Date.Month)
		{
			_monthTick();
			_previousMonth = MapData.Scenario.Date.Month;
		}
		else if (_selectedTileId > -1)
		{
			_gui.ShowProvinceData(MapData.Scenario.Map[_selectedTileId]);
		}


		//calcs before

		if (_gui != null)
			_gui.SetTime(MapData.Scenario.Date);
	}

	private void _monthTick()
	{
		foreach (var data in MapData.Scenario.Map)
		{
			data.Resources[(int)data.Good] += data.ProductionRate;
		}

		if (_selectedTileId > -1)
		{
			_gui.ShowProvinceData(MapData.Scenario.Map[_selectedTileId]);
		}
	}

	private void _on_timer_timeout()
	{
		_dayTick();
		_timer.Start();
	}


	private void _addGoods()
	{
		foreach (var data in MapData.Scenario.Map)
		{
			AnimatedSprite2D obj = _goodsScene.Instantiate() as AnimatedSprite2D;
			obj.Frame = (int)data.Good;
			obj.Position = data.CenterOfWeight;
			_goodsSpawner.AddChild(obj);
		}
	}

	private void _addDev()
	{
		foreach (var data in MapData.Scenario.Map)
		{
			AnimatedSprite2D obj = _devScene.Instantiate() as AnimatedSprite2D;
			obj.Frame = data.Development - 1;
			obj.Position = data.CenterOfWeight;
			_devSpawner.AddChild(obj);
		}
	}

	public void FreezeMap()
	{
		_freezed = true;
		_camera.SetZoomable(false);
	}

	public void UnFreezeMap()
	{
		_freezed = false;
		_camera.SetZoomable(true);
	}

	public void Pause()
	{
		_timer.Stop();
	}

	public void UnPause()
	{
		_timer.Start();
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is not InputEventMouseButton mbe || mbe.ButtonIndex != MouseButton.Left || !mbe.Pressed || _freezed) return;
		var mousePos = this.GetLocalMousePosition();
		var iMousePos = new Vector2I((int)(mousePos.X), (int)(mousePos.Y));
		if (mapMap.GetUsedRect().HasPoint(iMousePos))
		{
			var tileId = GameMath.GetProvinceID(mapMap.GetPixelv(iMousePos));

			if (tileId < 0 || tileId >= MapData.Scenario.ProvinceCount)
				return;

			if (tileId == _selectedTileId)
			{
				DeselectProvince();
				_selectedTileId = -2;
				return;
			}

			_selectedTileId = tileId;

			MapMaterial.SetShaderParameter("selectedID", tileId);
			_gui.ShowProvinceData(MapData.Scenario.Map[tileId]);
			// mapMaterial.SetShaderParameter("colors", MapData.MapColors);
			// _drawText();
		}
	}
}
