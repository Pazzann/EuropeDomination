using System;
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

    private PackedScene _armyScene;
    private Node2D _armySpawner;
    private int _currentMaxUnitId = 0;
    public List<ArmyUnit> CurrentSelectedUnits = new List<ArmyUnit> { };

    private CameraBehaviour _camera;
    private Timer _timer;
    private int _previousMonth;

    private bool _freezed = false;

    private int _selectedTileId = -2;
    private float _lastClickTimestamp = 0.0f;
    private bool _IsShiftPressed = false;

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

        _armyScene = (PackedScene)GD.Load("res://Prefabs/ArmyUnit.tscn");
        _armySpawner = GetNode<Node2D>("../ArmyHandler");

        _camera = (CameraBehaviour)GetNode<Camera2D>("../Camera");
        _timer = (Timer)GetChild(0);

        MapMaterial.SetShaderParameter("colors", MapData.MapColors);
        MapMaterial.SetShaderParameter("selectedID", -1);

        _drawText();
        _addGoods();
        _addDev();
        _addArmy();
        _goodsSpawner.Visible = false;
        _devSpawner.Visible = false;
        _armySpawner.Visible = false;

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
                _armySpawner.Visible = false;
                break;
            }
            case MapTypes.Terrain:
            {
                _clearText();
                _goodsSpawner.Visible = false;
                _devSpawner.Visible = false;
                _armySpawner.Visible = false;
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
            _armySpawner.Visible = false;
        }
    }

    public void ShowObjectsOnMap()
    {
        if (MapData.CurrentMapMode == MapTypes.Political)
        {
            _textSpawner.Visible = false;
            _devSpawner.Visible = true;
            _armySpawner.Visible = true;
        }
    }

    public void DeselectProvince()
    {
        _gui.DeselectProvinceBar();
        _selectedTileId = -1;
        MapMaterial.SetShaderParameter("selectedID", _selectedTileId);
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
            (obj.TextPath, obj.FontSize) = GameMath.FindSuitableTextPath(data.Value, stateMap, 0.5f,
                MapData.Scenario.CountriesNames[data.Value].Length, mapMap);
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
            if (MapData.Scenario.Map[_selectedTileId].Buildings.Count < 10 &&
                !MapData.Scenario.Map[_selectedTileId].Buildings.Exists(i => i.ID == 1) &&
                MapData.Scenario.Map[_selectedTileId].Buildings.Count < 4)
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

        var allUnits = GetTree().GetNodesInGroup("ArmyUnit");
        foreach (var unit in allUnits)
        {
            (unit as ArmyUnit).UpdateDayTick();
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

    private void _addArmy()
    {
        foreach (var data in MapData.Scenario.Map)
        {
            if (data.ArmyUnit != null)
            {
                ArmyUnit obj = _armyScene.Instantiate() as ArmyUnit;
                obj.SetupUnit(_currentMaxUnitId, data.ArmyUnit, this);
                _currentMaxUnitId++;

                //TODO: NORMAL CALCULATION OF ARMY POSITION
                obj.Position = new Vector2(data.CenterOfWeight.X + 5, data.CenterOfWeight.Y);
                _armySpawner.AddChild(obj);
            }
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

    public void HandleInput(InputEvent @event)
    {
        var tileId = _findTile();

        if (_freezed) return;

        if (@event is InputEventKey { KeyLabel: Key.Shift, Pressed: true })
            _IsShiftPressed = true;
        if (@event is InputEventKey { KeyLabel: Key.Shift, Pressed: false })
            _IsShiftPressed = false;


        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Right, Pressed: true })
        {
            if (tileId == -3)
                return;

            if (!_IsShiftPressed)
                foreach (var armyUnit in CurrentSelectedUnits)
                {
                    if (tileId == armyUnit.Data.CurrentProvince)
                    {
                        armyUnit.NewPath(Array.Empty<int>());
                        return;
                    }

                    armyUnit.NewPath(
                        PathFinder.FindWayFromAToB(armyUnit.Data.CurrentProvince, tileId, MapData.Scenario));
                }
            else
                foreach (var armyUnit in CurrentSelectedUnits)
                {
                    if (armyUnit.Path.Count == 0)
                    {
                        armyUnit.NewPath(PathFinder.FindWayFromAToB(armyUnit.Data.CurrentProvince, tileId, MapData.Scenario));
                        return;
                    }
                    armyUnit.AddPath(
                        PathFinder.FindWayFromAToB(armyUnit.Path[0], tileId, MapData.Scenario));
                }

            return;
        }

        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: true })
        {
            _lastClickTimestamp = Time.GetTicksMsec() / 1000f;
            return;
        }

        if (@event is not InputEventMouseButton { ButtonIndex: MouseButton.Left, Pressed: false }) return;
        if (Time.GetTicksMsec() / 1000f - _lastClickTimestamp > 0.2f) return;


        if (tileId == _selectedTileId || tileId == -3)
        {
            DeselectProvince();
            _selectedTileId = -2;
            return;
        }

        _selectedTileId = tileId;

        MapMaterial.SetShaderParameter("selectedID", tileId);
        _gui.ShowProvinceData(MapData.Scenario.Map[tileId]);
    }

    private int _findTile()
    {
        var mousePos = this.GetLocalMousePosition();
        var iMousePos = new Vector2I((int)(mousePos.X), (int)(mousePos.Y));
        if (mapMap.GetUsedRect().HasPoint(iMousePos))
        {
            var tileId = GameMath.GetProvinceID(mapMap.GetPixelv(iMousePos));

            if (tileId < 0 || tileId >= MapData.Scenario.ProvinceCount)
                return -3;

            return tileId;
        }

        return -3;
    }
}