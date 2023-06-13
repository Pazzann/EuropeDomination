using Godot;
using EuropeDominationDemo.Scripts;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;

public partial class GUI : Control
{

	private bool _isMapTypesClosed = false;
	private Sprite2D _mapTypes;
	private Sprite2D _provinceData;
	private MapHandler _mapHandler;

	public override void _Ready()
	{
		_mapHandler = GetParent().GetParent().GetNode<MapHandler>("Map");
		_mapTypes = GetChild(2).GetChild(1) as Sprite2D;
		_provinceData = GetChild(3).GetChild(0) as Sprite2D;
	}

	public void DeselectProvinceBar()
	{
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(_provinceData, "position",new Vector2(-280.0f, 20.0f) , 0.4f);
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
	public void ShowProvinceData(ProvinceData data){
		if (_provinceData.Position != new Vector2(200.0f, 0.0f))
		{
			Tween tween = GetTree().CreateTween(); 
			tween.TweenProperty(_provinceData, "position",new Vector2(280.0f, 20.0f) , 0.4f);
			(_provinceData.GetChild(1) as Label).Text = data.Name;
			(_provinceData.GetChild(2) as AnimatedSprite2D).Frame = (int)data.Good;
			(_provinceData.GetChild(3) as AnimatedSprite2D).Frame = data.Owner;
		}
		
	}

}


