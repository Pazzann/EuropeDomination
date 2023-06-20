using System;
using Godot;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI;
public partial class GUI : Control
{

	private bool _isMapTypesClosed = false;
	private Sprite2D _mapTypes;
	private Sprite2D _provinceData;
	private MapHandler _mapHandler;
	private Label _dateLabel;
	private Sprite2D _pauseSprite;

	private bool _pause = false;

	
	
	public override void _Ready()
	{
		_mapHandler = GetParent().GetParent().GetNode<MapHandler>("Map");
		_mapTypes = GetChild(2).GetChild(1) as Sprite2D;
		_provinceData = GetChild(3).GetChild(0) as Sprite2D;
		_dateLabel = GetChild(0).GetChild(0).GetChild(0) as Label;
		_pauseSprite = GetChild(0).GetChild(0).GetChild(2) as Sprite2D;
	}

	public void SetTime(DateTime date)
	{
		_dateLabel.Text = date.Day + "." + date.Month + "." + date.Year;
	}

	

	private void _on_terrain_types_pressed()
	{
		_mapHandler.MapData.CurrentMapMode = MapTypes.Terrain;
		_mapHandler.MapUpdate();
	}
	private void _on_political_types_pressed()
	{
		_mapHandler.MapData.CurrentMapMode = MapTypes.Political;
		_mapHandler.MapUpdate();
	}
	private void _on_goods_types_pressed()
	{
		_mapHandler.MapData.CurrentMapMode = MapTypes.Goods;
		_mapHandler.MapUpdate();
	}
	private void _on_trade_types_pressed()
	{
		_mapHandler.MapData.CurrentMapMode = MapTypes.Trade;
		_mapHandler.MapUpdate();
	}
	private void _on_development_types_pressed()
	{
		_mapHandler.MapData.CurrentMapMode = MapTypes.Development;
		_mapHandler.MapUpdate();
	}
	private void _on_factories_types_pressed()
	{
		_mapHandler.MapData.CurrentMapMode = MapTypes.Factories;
		_mapHandler.MapUpdate();
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
	private void _on_close_button_pressed()
	 {
		 _mapHandler.DeselectProvince();
	 }
	
	private void _on_h_box_container_4_mouse_entered()
	{
		_mapHandler.FreezeZoom();
	}
	
	private void _on_control_mouse_entered()
	{
		_mapHandler.FreezeZoom();
	}


	private void _on_province_type_selection_mouse_entered()
	{
		_mapHandler.FreezeZoom();
	}
	
	private void _on_h_box_container_4_mouse_exited()
	{
		_mapHandler.UnFreezeZoom();
	}
	
	private void _on_control_mouse_exited()
	{
		_mapHandler.UnFreezeZoom();
	}


	private void _on_province_type_selection_mouse_exited()
	{
		_mapHandler.UnFreezeZoom();
	}

	private void _on_pause_button_pressed()
	{
		GD.Print(1);
		_pause = !_pause;
		if (_pause)
		{
			_pauseSprite.Visible = true;
			_mapHandler.Pause();
		}
		else
		{
			_pauseSprite.Visible = false;
			_mapHandler.UnPause();
		}
	}


	
	
	
	public void DeselectProvinceBar()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(_provinceData.GetParent(), "position",new Vector2(-550.0f, 200.0f) , 0.4f);
	}
	public void ShowProvinceData(ProvinceData data){
		if ((_provinceData.GetParent() as HBoxContainer).Position != new Vector2(20.0f, 200.0f))
		{
			Tween tween = GetTree().CreateTween(); 
			tween.TweenProperty(_provinceData.GetParent(), "position",new Vector2(20.0f, 200.0f) , 0.4f);
		}
		(_provinceData.GetChild(1) as Label).Text = data.Name;
		(_provinceData.GetChild(2) as AnimatedSprite2D).Frame = (int)data.Good;
		(_provinceData.GetChild(3) as AnimatedSprite2D).Frame = data.Owner;
		//terrain here GetChild(4);
		(_provinceData.GetChild(5).GetChild(0) as GUIResources).DrawResources(data.Resources);
	}

}
