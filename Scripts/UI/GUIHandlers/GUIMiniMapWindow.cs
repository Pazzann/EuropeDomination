using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIMiniMapWindow : GUIHandler
{
	private Camera _camera;
	private Panel _cameraBox;
	private bool _isMapTypesVisible = true;
	private Sprite2D _mapTypesSprite;
	private ShaderMaterial _minimapMaterial;
	private TextureRect _minimapSprite;
	private Viewport _viewport;

	public override void Init()
	{
		_minimapSprite = GetNode<TextureRect>("MiniMapContainer/MapScreen");
		_minimapMaterial = _minimapSprite.Material as ShaderMaterial;
		_minimapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
		_minimapMaterial.SetShaderParameter("selectedID", -1);

		_cameraBox = GetNode<Panel>("MiniMapContainer/MapScreen/CameraBox");

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
		switch (@event)
		{
			case ToGUISetCamera e:
				_camera = e.Camera;
				_viewport = e.Viewport;
				return;
			case ToGUIUpdateMinimap:
				_minimapMaterial.SetShaderParameter("colors", EngineState.MapInfo.MapColors);
				return;
			default:
				return;
		}
	}

	public override void _Process(double delta)
	{
		Vector2 mapSize = GlobalResources.MapTexture.GetSize();

		if (Input.IsMouseButtonPressed(MouseButton.Left))
			if (GetLocalMousePosition().X < _minimapSprite.Size.X && GetLocalMousePosition().Y < _minimapSprite.Size.Y)
				_camera.GoTo(GetLocalMousePosition() / _minimapSprite.Size * mapSize, 0.1f);

		_cameraBox.PivotOffset = _cameraBox.Size / 2;
		_cameraBox.Position = _camera.GetScreenCenterPosition() * (_minimapSprite.Size / mapSize) - _cameraBox.Size / 2;

		_cameraBox.Size = _viewport.GetVisibleRect().Size / mapSize * _minimapSprite.Size / _camera.Zoom;
	}

	private void _onMapTypePressed(MapTypes type)
	{
		InvokeGUIEvent(new GUIChangeMapType(type));
	}

	private void _onMapTypeSwitchState()
	{
		var tween = GetTree().CreateTween();
		if (_isMapTypesVisible)
		{
			tween.TweenProperty(_mapTypesSprite, "position", _mapTypesSprite.Position + new Vector2(-293, 0), 0.4f);
			_isMapTypesVisible = false;
		}
		else
		{
			tween.TweenProperty(_mapTypesSprite, "position", _mapTypesSprite.Position + new Vector2(293, 0), 0.4f);
			_isMapTypesVisible = true;
		}
	}
}
