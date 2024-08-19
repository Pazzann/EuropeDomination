using System;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
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

        _mapMaterial.SetShaderParameter("selectedID", tileId);
        if (_mapData.Scenario.Map[tileId] is LandProvinceData data)
            InvokeToGUIEvent(new ToGuiShowLandProvinceDataEvent(data));
        return false;
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
        if (_mapData.Scenario.Map[_selectedTileId] is LandProvinceData data)
            switch (@event)
            {
                case GUIBuildBuildingEvent e:
                    data.Buildings.Add(e.NewBuilding);
                    InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent((LandProvinceData)_mapData.Scenario.Map[_selectedTileId]));
                    return;
                case GUIDestroyBuildingEvent e:
                    data.Buildings.RemoveAt(e.DestroyedId);
                    InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent((LandProvinceData)_mapData.Scenario.Map[_selectedTileId]));
                    return;
                default:
                    return;
            }
    }


    private void _clearText()
    {
        var texts = _textSpawner.GetChildren();
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

        var stateMap = new StateMap(_mapData, _mapData.Scenario.MapTexture);

		foreach (var data in _mapData.Scenario.Countries)
		{
			var provinces = _mapData.Scenario.CountryProvinces(data.Value.Id);

			if (provinces.Length == 0)
				continue;

            var texts = new TextSolver(stateMap, data.Value.Name, data.Value.Id, 0.5f).FitText();

            foreach (var text in texts)
            {
                var textNode = _textScene.Instantiate() as CurvedLabel;
                textNode.Text = text;
                _textSpawner.AddChild(textNode);
            }
            
			//
			//
			//
			// var f = Time.GetTicksMsec();
			//
			//
			//
			// GD.Print(Time.GetTicksMsec() - f);
			//
		}
	}

    private void _addGoods()
    {
        foreach (var data in _mapData.Scenario.Map.Where(data => data is LandProvinceData))
        {
            AnimatedSprite2D obj = _goodsScene.Instantiate() as AnimatedSprite2D;
            obj.Frame = (int)((LandProvinceData)data).Good;
            obj.Position = ((LandProvinceData)data).CenterOfWeight;
            _goodsSpawner.AddChild(obj);
        }
    }

    private void _addDev()
    {
        foreach (var data in _mapData.Scenario.Map.Where(data => data is LandProvinceData))
        {
            AnimatedSprite2D obj = _devScene.Instantiate() as AnimatedSprite2D;
            obj.Frame = ((LandProvinceData)data).Development - 1;
            obj.Position = ((LandProvinceData)data).CenterOfWeight;
            _devSpawner.AddChild(obj);
        }
    }


    public override void DayTick()
    {
        foreach (var data in _mapData.Scenario.Map.Where(data => data is LandProvinceData))
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

        if (_selectedTileId > -1 && _mapData.Scenario.Map[_selectedTileId] is LandProvinceData landData)
        {
            InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
        }
    }

    public override void MonthTick()
    {
        foreach (var data in _mapData.Scenario.Map.Where(data => data is LandProvinceData))
        {
            ((LandProvinceData)data).Resources[(int)((LandProvinceData)data).Good] +=
                ((LandProvinceData)data).ProductionRate;
        }

        if (_selectedTileId > -1 && _mapData.Scenario.Map[_selectedTileId] is LandProvinceData landData)
        {
            InvokeToGUIEvent(new ToGUIUpdateLandProvinceDataEvent(landData));
        }
    }

    public override void YearTick()
    {
    }
}
