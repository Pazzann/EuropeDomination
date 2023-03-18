using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;
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
			// var ids = GameMath.FindSquarePointsInsideState(provinces, mapMap, 30);
			// var curve = GameMath.FindBezierCurveFromPoints(ids);
			var curve = GameMath.FindBezierCurve(provinces);
			Node2D obj = _textScene.Instantiate() as Node2D;
			Node2D obj1 = _textScene.Instantiate() as Node2D;
			Node2D obj2 = _textScene.Instantiate() as Node2D;
			obj.Position = curve.Segment1;
			obj1.Position = curve.Segment2;
			obj2.Position = curve.Vertex;
			(obj.GetChild(0) as Label).Text = "1"; 
			(obj1.GetChild(0) as Label).Text = "2"; 
			(obj2.GetChild(0) as Label).Text = "3"; 
			_textSpawner.AddChild(obj);
			_textSpawner.AddChild(obj1);
			_textSpawner.AddChild(obj2);
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
