using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIMiniMapWindow : GUIHandler
{
	private Sprite2D _mapTypesSprite;
	private bool _isMapTypesVisible = true;
	
	public override void Init()
	{
		_mapTypesSprite = GetNode<Sprite2D>("MiniMapContainer/MapTypes");
		_mapTypesSprite.GetNode<Button>("./PoliticalType").Pressed += () => _onMapTypePressed(MapTypes.Political);
		_mapTypesSprite.GetNode<Button>("./TerrainType").Pressed += () => _onMapTypePressed(MapTypes.Terrain);
		_mapTypesSprite.GetNode<Button>("./GoodsType").Pressed += () => _onMapTypePressed(MapTypes.Goods);
		_mapTypesSprite.GetNode<Button>("./TradeType").Pressed += () => _onMapTypePressed(MapTypes.Trade);
		_mapTypesSprite.GetNode<Button>("./DevelopmentType").Pressed += () => _onMapTypePressed(MapTypes.Development);
		_mapTypesSprite.GetNode<Button>("./FactoriesType").Pressed += () => _onMapTypePressed(MapTypes.Factories);
		
	}
	public override void InputHandle(InputEvent @event)
	{
		
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		return;
	}
	
	private void _onMapTypePressed(MapTypes type)
	{
		InvokeGUIEvent(new GUIChangeMapType(type));
	}
	
	private void _onMapTypeSwitchState()
	{
		Tween tween = GetTree().CreateTween();
		if(_isMapTypesVisible)
			tween.TweenProperty(_mapTypesSprite, "position",_mapTypesSprite.Position + new Vector2(-200, 0), 0.4f);
		else
			tween.TweenProperty(_mapTypesSprite, "position",_mapTypesSprite.Position + new Vector2(200, 0), 0.4f);
	}

}
