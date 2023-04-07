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
	private MapData _mapData;

	private PackedScene _textScene;
	private Node2D _textSpawner;

	private async void _drawText()
	{
		var texts = _textSpawner.GetChildren();
		foreach (var text in texts)
			text.Free();

		foreach (var data in _mapData.Scenario.Countries)
		{
			var provinces = _mapData.Scenario.CountryProvinces(data.Value);
			var curve = GameMath.FindBezierCurve(provinces);
			if (curve.IsDefault)
			{
				var ids = GameMath.FindSquarePointsInsideState(provinces, mapMap, 10);
				curve = GameMath.FindBezierCurveFromPoints(ids);
			}
			
			TextBezierCurve obj = _textScene.Instantiate() as TextBezierCurve;
			obj.Curve = curve;
			obj.TextOnCurve = _mapData.Scenario.CountriesNames[data.Value];
			_textSpawner.AddChild(obj);
		
		}
	}
	
	
	
	
	public override void _Ready()
	{
		mapMap = this.Texture.GetImage();
		mapMaterial = this.Material as ShaderMaterial;
		_mapData = new MapData(new DemoScenario(mapMap));
		_textScene = (PackedScene)GD.Load("res://Prefabs/Text.tscn");
		_textSpawner = GetNode<Node2D>("../TextHandler");
		
		mapMaterial.SetShaderParameter("colors", _mapData.MapColors);

		_drawText();


	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		
		if (@event is not InputEventMouseButton mbe || mbe.ButtonIndex != MouseButton.Left || !mbe.Pressed) return;
		var mousePos = this.GetLocalMousePosition();
		var iMousePos = new Vector2I((int)(mousePos.X), (int)(mousePos.Y));
		GD.Print(iMousePos.X);
		if (mapMap.GetUsedRect().HasPoint(iMousePos))
		{
			var tileId = GameMath.GetProvinceID(mapMap.GetPixelv(iMousePos));
				
			if(tileId < 0  || tileId >= _mapData.Scenario.ProvinceCount)
				return;
			mapMaterial.SetShaderParameter("selectedID", tileId);
			_mapData.Scenario.Map[tileId].Owner = _mapData.Scenario.Countries["Green"];
			mapMaterial.SetShaderParameter("colors", _mapData.MapColors);
			_drawText();
		}
	}
}
