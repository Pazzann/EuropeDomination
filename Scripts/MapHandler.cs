using System.Collections.Generic;
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
	private ShaderMaterial mapMaterial;
	public MapData MapData { get; set; }

	private GUI _gui;
	private PackedScene _textScene;
	private Node2D _textSpawner;

	
	public override void _Ready()
	{
		mapMap = this.Texture.GetImage();
		mapMaterial = this.Material as ShaderMaterial;
		MapData = new MapData(new DemoScenario(mapMap));
		_textScene = (PackedScene)GD.Load("res://Prefabs/Text.tscn");
		_textSpawner = GetNode<Node2D>("../TextHandler");
		
		mapMaterial.SetShaderParameter("colors", MapData.MapColors);
		mapMaterial.SetShaderParameter("selectedID", -1);

		_drawText();

		_gui = GetNode<GUI>("../CanvasLayer/Control");
	}

	
	private async void _drawText()
	{
		var texts = _textSpawner.GetChildren();
		foreach (var text in texts)
			text.Free();

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
			mapMaterial.SetShaderParameter("selectedID", tileId);
			_gui.ShowProvinceData(MapData.Scenario.Map[tileId]);
			// mapMaterial.SetShaderParameter("colors", MapData.MapColors);
			// _drawText();
		}
	}
}
