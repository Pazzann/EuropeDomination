using Godot;
using System;
using EuropeDominationDemo.Scripts;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;

public partial class GUI : Control
{

	private bool _isMapTypesClosed = false;
	private Sprite2D _mapTypes;
	
	private MapData _mapData; 
	public override void _Ready()
	{
		_mapData = ((MapHandler)GetNode("../../Map")).MapData;
		_mapTypes = GetChild(2).GetChild(1) as Sprite2D;
	}

	
	private void _on_terrain_types_pressed()
	{
		_mapData.CurrentMapMode = MapTypes.Terrain;
	}


	private void _on_political_types_pressed()
	{
		_mapData.CurrentMapMode = MapTypes.Political;
	}


	private void _on_goods_types_pressed()
	{
		_mapData.CurrentMapMode = MapTypes.Resources;
	}


	private void _on_trade_types_pressed()
	{
		_mapData.CurrentMapMode = MapTypes.Trade;
	}


	private void _on_development_types_pressed()
	{
		_mapData.CurrentMapMode = MapTypes.Development;
	}


	private void _on_factories_types_pressed()
	{
		_mapData.CurrentMapMode = MapTypes.Factories;
	}


	private void _on_close_types_pressed()
	{
		Tween tween = GetTree().CreateTween();
		if (_isMapTypesClosed)
		{
			tween.TweenProperty(_mapTypes, "position",new Vector2(310.0f, 20.0f) , 0.4f);
		}
		else
		{
			tween.TweenProperty(_mapTypes, "position",new Vector2(15.0f, 20.0f) , 0.4f);
		}

		_isMapTypesClosed = !_isMapTypesClosed;
		
		(_mapTypes.GetChild(0) as Button).Visible = !_isMapTypesClosed;
		(_mapTypes.GetChild(1) as Button).Visible = !_isMapTypesClosed;
		(_mapTypes.GetChild(2) as Button).Visible = !_isMapTypesClosed;
		(_mapTypes.GetChild(3) as Button).Visible = !_isMapTypesClosed;
		(_mapTypes.GetChild(4) as Button).Visible = !_isMapTypesClosed;
		(_mapTypes.GetChild(5) as Button).Visible = !_isMapTypesClosed;
	}
}
