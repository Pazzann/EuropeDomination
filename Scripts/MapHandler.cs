using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
using EuropeDominationDemo.Scripts.Text;
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
		
		MapMaterial.SetShaderParameter("colors", MapData.MapColors);
		MapMaterial.SetShaderParameter("selectedID", -1);

		_drawText();
		_addGoods();
		_goodsSpawner.Visible = false;

		_gui = GetNode<GUI>("../CanvasLayer/Control");
	}

	public void MapUpdate()
	{
		switch (MapData.CurrentMapMode)
		{
			case MapTypes.Political:
			{
				_drawText();
				_goodsSpawner.Visible = false;
				break;
			}
			case MapTypes.Goods:
			{
				_clearText();
				_goodsSpawner.Visible = true;
				break;
			}
			case MapTypes.Terrain:
			{
				_clearText();
				_goodsSpawner.Visible = false;
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

	public void HideText()
	{
		_textSpawner.Visible = false;
	}

	public void ShowText()
	{
		_textSpawner.Visible = true;
	}

	public void DeselectProvince()
	{
		_gui.DeselectProvinceBar();
		MapMaterial.SetShaderParameter("selectedID", -1);
	}
	
	
	
	private void _drawText()
	{
		_clearText();
		foreach (var data in MapData.Scenario.Countries)
		{
			var provinces = MapData.Scenario.CountryProvinces(data.Value);
			if(provinces.Length == 0)
				continue;
			var curve = GameMath.FindBezierCurve(provinces);
			if (curve.IsDefault)
			{
				var ids = GameMath.FindSquarePointsInsideState(provinces, mapMap, 10);
				curve = GameMath.FindBezierCurveFromPoints(ids);
				curve.Sort();
			}
			
			TextBezierCurve obj = _textScene.Instantiate() as TextBezierCurve;
			obj.Curve = curve;
			obj.TextOnCurve = MapData.Scenario.CountriesNames[data.Value];
			_textSpawner.AddChild(obj);
		
		}
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
	
	
	
	
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _Input(InputEvent @event)
	{
		
		if (@event is not InputEventMouseButton mbe || mbe.ButtonIndex != MouseButton.Left || !mbe.Pressed) return;
		var mousePos = this.GetLocalMousePosition();
		var iMousePos = new Vector2I((int)(mousePos.X), (int)(mousePos.Y));
		if (mapMap.GetUsedRect().HasPoint(iMousePos))
		{
			var tileId = GameMath.GetProvinceID(mapMap.GetPixelv(iMousePos));
				
			if(tileId < 0  || tileId >= MapData.Scenario.ProvinceCount)
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
