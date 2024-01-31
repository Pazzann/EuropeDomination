using Godot;

namespace EuropeDominationDemo.Scripts;


public delegate void ChangeZoomDelegate();

public partial class CameraBehaviour : Camera2D
{
	
		
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

		public event ChangeZoomDelegate ChangeZoom = null;


		public void SetZoomable(bool zoomable)
		{
			_zoomable = zoomable;
		}
		
		public override void _Ready()
		{
			_zoom = this.Zoom;
			_cameraPosition = this.Position;
			Enabled = true;

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


		public void InputHandle(InputEvent @event)
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
				
				
				ChangeZoom.Invoke();
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
