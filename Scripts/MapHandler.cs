using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class MapHandler : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	private Image mapMap;
	private ShaderMaterial mapMaterial;
	public override void _Ready()
	{
		mapMap = this.Texture.GetImage();
		mapMaterial = this.Material as ShaderMaterial;
		mapMaterial.SetShaderParameter("colors", new Vector3[] {
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f),
			new Vector3(1.0f, 1.0f, 1.0f)
		});

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
			mapMaterial.SetShaderParameter("selectedID", tileId);
			//mapMaterial.SetShaderParameter("colors[" + tileId.ToString() + "]", new Vector3(1.0f, 1.0f, 1.0f));
			mapMaterial.SetShaderParameter("colors", new Vector3[] {
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				/*new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f),
				new Vector3(1.0f, 1.0f, 1.0f)*/
			});
		}
	}
}