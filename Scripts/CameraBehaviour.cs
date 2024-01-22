using Godot;

namespace EuropeDominationDemo.Scripts;

public partial class CameraBehaviour : Camera2D
{

		private MapHandler _mapHandler;
		
		private Vector2 _zoom;
		private Vector2 _cameraPosition;
		private float _maxZoom = 5f;
		private float _minZoom = 0.3f;
		private float _zoomChangeSpeed = 0.05f;
		private float _zoomMovement = 20.0f;
		private const float PanSpeed = 13f;

		private bool _moveUp = false;
		private bool _moveDown = false;
		private bool _moveLeft = false;
		private bool _moveRight = false;
		private float _moveSpeed = 1.0f;


		private bool _zoomable = true;


		public void SetZoomable(bool zoomable)
		{
			_zoomable = zoomable;
			GD.Print(_zoomable);
		}
		
		public bool IsZoomed()
		{
			return _zoom.X < 3.0f;
		}

		
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_zoom = this.Zoom;
			_cameraPosition = this.Position;
			Enabled = true;
			_mapHandler = GetNode<Sprite2D>("../Map") as MapHandler;
		}

		public override void _PhysicsProcess(double delta)
		{
			if (_moveUp)
				_cameraPosition.Y -= _moveSpeed * _zoom.X * 10 / _maxZoom;
			if (_moveRight)
				_cameraPosition.X += _moveSpeed * _zoom.X * 10 / _maxZoom;
			if (_moveDown)
				_cameraPosition.Y += _moveSpeed * _zoom.X * 10 / _maxZoom;
			if (_moveLeft)
				_cameraPosition.X -= _moveSpeed * _zoom.X * 10 / _maxZoom;
			if(_moveUp || _moveDown || _moveLeft || _moveRight)
				this.Position = _cameraPosition;
		}


		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventMouseButton mbe)
			{
				if (mbe.ButtonIndex == MouseButton.WheelDown && _zoomable)
				{
					if (_zoom.X - _zoomChangeSpeed > _minZoom)
					{
						_zoom.X -= _zoomChangeSpeed; 
						_zoom.Y -= _zoomChangeSpeed;
						this.Zoom = _zoom;
					}
				}
				if (mbe.ButtonIndex == MouseButton.WheelUp && _zoomable)
				{
					if (_zoom.X + _zoomChangeSpeed < _maxZoom)
					{
						_zoom.X += _zoomChangeSpeed;
						_zoom.Y += _zoomChangeSpeed;
						this.Zoom = _zoom;
					}
				}

				if (_zoom.X < 3.0f)
				{
					_mapHandler.MapMaterial.SetShaderParameter("viewMod", 1);
					_mapHandler.HideObjectsOnMap();
				}
				else
				{
					_mapHandler.MapMaterial.SetShaderParameter("viewMod", 0);
					_mapHandler.ShowObjectsOnMap();
				}
			}
			if (@event is InputEventKey eventKey)
			{
				if (eventKey.Keycode == Key.Up)
					_moveUp = eventKey.Pressed;
				if (eventKey.Keycode == Key.Down)
					_moveDown = eventKey.Pressed;
				if (eventKey.Keycode == Key.Right)
					_moveRight = eventKey.Pressed;
				if (eventKey.Keycode == Key.Left)
					_moveLeft = eventKey.Pressed;
			}
			if (Input.IsActionPressed("pan_2d") && @event is InputEventMouseMotion mouseEvent)
				GlobalPosition += PanSpeed * -mouseEvent.Relative.Normalized() / Zoom;
		}

}
