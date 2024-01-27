using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Text;
using Godot;

namespace EuropeDominationDemo.Scripts.Handlers;

public partial class MapHandler : GameHandler
{
    private Sprite2D _mapSprite;
    private ShaderMaterial _mapMaterial;
    private MapData _mapData { get; set; }
    
    private PackedScene _textScene;
    private Node2D _textSpawner;

    private PackedScene _goodsScene;
    private Node2D _goodsSpawner;

    private PackedScene _devScene;
    private Node2D _devSpawner;

    private int _selectedTileId = -2;
    private float _lastClickTimestamp = 0.0f;
    
    
    public override void Init(MapData mapData)
    {
        _mapSprite = GetNode<Sprite2D>("./Map");
        _mapMaterial = _mapSprite.Material as ShaderMaterial;
        _mapData = mapData;
        
        _textScene = (PackedScene)GD.Load("res://Prefabs/Text.tscn");
        _textSpawner = GetNode<Node2D>("./TextHandler");

        _goodsScene = (PackedScene)GD.Load("res://Prefabs/Good.tscn");
        _goodsSpawner = GetNode<Node2D>("./GoodsHandler");

        _devScene = (PackedScene)GD.Load("res://Prefabs/Development.tscn");
        _devSpawner = GetNode<Node2D>("./DevHandler");
        


        _mapMaterial.SetShaderParameter("colors", _mapData.MapColors);
        _mapMaterial.SetShaderParameter("selectedID", -1);

        _drawText();
        _addGoods();
        _addDev();
  
        _goodsSpawner.Visible = false;
        _devSpawner.Visible = false;
        
    }

    public override void InputHandle(InputEvent @event, int tileId)
    {
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

        _mapMaterial.SetShaderParameter("selectedID", tileId);
        _gui.ShowProvinceData(_mapData.Scenario.Map[tileId]);
    }

    public override void ViewModUpdate(float zoom)
    {
        switch (_mapData.CurrentMapMode)
        {
            case MapTypes.Political:
            {
                _goodsSpawner.Visible = false;

                if (zoom < 3.0f)
                {
                    _textSpawner.Visible = true;
                    _devSpawner.Visible = false;
                }
                else
                {
                    _textSpawner.Visible = false;
                    _devSpawner.Visible = true;
                }

                break;
            }
            case MapTypes.Goods:
            {
                _goodsSpawner.Visible = true;
                _devSpawner.Visible = false;
                _textSpawner.Visible = false;
                break;
            }
            case MapTypes.Terrain:
            {
                _goodsSpawner.Visible = false;
                _textSpawner.Visible = false;
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

        _mapMaterial.SetShaderParameter("colors", _mapData.MapColors);
        _mapMaterial.SetShaderParameter("viewMod", zoom < 3.0f ? 1 : 0);
    }

    public override void GUIInteractionHandler(GUIEvent @event)
    {
        throw new NotImplementedException();
    }

    // Called when the node enters the scene tree for the first time.
    

    private void _clearText()
    {
        var texts = _textSpawner.GetChildren();
        foreach (var text in texts)
            text.Free();
    }

    public void DeselectProvince()
    {
        _gui.DeselectProvinceBar();
        _selectedTileId = -1;
        _mapMaterial.SetShaderParameter("selectedID", _selectedTileId);
    }

    private void _drawText()
    {
        _clearText();

        var stateMap = new StateMap(_mapData, _mapData.Scenario.MapTexture);

        foreach (var data in _mapData.Scenario.Countries)
        {
            var provinces = _mapData.Scenario.CountryProvinces(data.Value.Id);
            if (provinces.Length == 0)
                continue;
            var curve = GameMath.FindBezierCurve(provinces);
            if (curve.IsDefault)
            {
                var ids = GameMath.FindSquarePointsInsideState(provinces, _mapData.Scenario.MapTexture, 10);
                curve = GameMath.FindBezierCurveFromPoints(ids);
                curve.Sort();
            }

            //var arc = Arc.withAngle(curve.Segment1, curve.Segment2, Mathf.Pi / 6f).Item2;

            TextBezierCurve obj = _textScene.Instantiate() as TextBezierCurve;
            obj.Curve = curve;
            (obj.TextPath, obj.FontSize) = GameMath.FindSuitableTextPath(data.Value.Id, stateMap, 0.5f,
                data.Value.Name.Length, _mapData.Scenario.MapTexture);
            //obj.TextPath = new ThickArc(arc, 30f);
            //obj.TextPath = Arc.withAngle(curve.Segment1, curve.Segment2, Mathf.Pi / 6f).Item1;
            obj.TextOnCurve = data.Value.Name;
            _textSpawner.AddChild(obj);
        }
    }

    //TODO: add buildings slots variable
    public void AddBuilding(Building building)
    {
        if (_selectedTileId >= 0)
        {
            if (_mapData.Scenario.Map[_selectedTileId].Buildings.Count < 10 &&
                !_mapData.Scenario.Map[_selectedTileId].Buildings.Exists(i => i.ID == 1) &&
                _mapData.Scenario.Map[_selectedTileId].Buildings.Count < 4)
            {
                _mapData.Scenario.Map[_selectedTileId].Buildings.Add(building);
                _gui.ShowProvinceData(_mapData.Scenario.Map[_selectedTileId]);
            }
        }
    }

    public void RemoveBuilding(List<Building> buildings)
    {
        _mapData.Scenario.Map[_selectedTileId].Buildings = buildings;
        _gui.ShowProvinceData(_mapData.Scenario.Map[_selectedTileId]);
    }

    private void _dayTick()
    {
        foreach (var data in _mapData.Scenario.Map)
        {
            foreach (var building in data.Buildings.Where(building => !building.IsFinished))
            {
                building.BuildingTime++;
                if (building.BuildingTime == building.TimeToBuild)
                {
                    building.IsFinished = true;
                }
            }
        }

        if (_gui != null)
            _gui.SetTime(_mapData.Scenario.Date);
    }

    private void _monthTick()
    {
        foreach (var data in _mapData.Scenario.Map)
        {
            data.Resources[(int)data.Good] += data.ProductionRate;
        }

        if (_selectedTileId > -1)
        {
            _gui.ShowProvinceData(_mapData.Scenario.Map[_selectedTileId]);
        }
    }


    private void _addGoods()
    {
        foreach (var data in _mapData.Scenario.Map)
        {
            AnimatedSprite2D obj = _goodsScene.Instantiate() as AnimatedSprite2D;
            obj.Frame = (int)data.Good;
            obj.Position = data.CenterOfWeight;
            _goodsSpawner.AddChild(obj);
        }
    }

    private void _addDev()
    {
        foreach (var data in _mapData.Scenario.Map)
        {
            AnimatedSprite2D obj = _devScene.Instantiate() as AnimatedSprite2D;
            obj.Frame = data.Development - 1;
            obj.Position = data.CenterOfWeight;
            _devSpawner.AddChild(obj);
        }
    }
}