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
	public override void _Ready()
	{
		mapMap = this.Texture.GetImage();
		mapMaterial = this.Material as ShaderMaterial;
		_mapData = new MapData(new DemoScenario());
		mapMaterial.SetShaderParameter("colors", _mapData.MapColors);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		
		if (@event is not InputEventMouseButton mbe || mbe.ButtonIndex != MouseButton.Left || !mbe.Pressed) return;
		
		var mousePos = this.GetLocalMousePosition();
		var iMousePos = new Vector2I((int)(mousePos.X + this.Texture.GetSize().X/2), (int)(mousePos.Y + this.Texture.GetSize().Y/2));
		if (mapMap.GetUsedRect().HasPoint(iMousePos))
		{
			var tileId = GameMath.GameMath.GetProvinceID(mapMap.GetPixelv(iMousePos));
			if(tileId < 0  || tileId >= _mapData.Scenario.ProvinceCount)
				return;
			mapMaterial.SetShaderParameter("selectedID", tileId);
			_mapData.Scenario.Map[tileId].Color = new Vector3(0.0f, 0.0f, 0.0f);
			mapMaterial.SetShaderParameter("colors", _mapData.MapColors);
		}
	}
}
