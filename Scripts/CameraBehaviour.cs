using System.Linq.Expressions;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;

namespace EuropeDominationDemo.Scripts;


public delegate void ChangeZoomDelegate();

public partial class CameraBehaviour : Camera2D
{
	
		
		private Vector2 _zoom;
		private Vector2 _cameraPosition;
		private float _maxZoom = 10f;
		private float _minZoom = 0.3f;
		private float _zoomChangeSpeed = 0.05f;
		private float _zoomMovement = 20.0f;
		private const float PanSpeed = 1f;

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

			GetViewport().SizeChanged += LimitToMap;
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
				var zoomPoint = GetGlobalMousePosition();
				if (mbe.ButtonIndex == MouseButton.WheelDown && _zoomable)
				{
					_zoomAtPoint(-_zoomChangeSpeed, zoomPoint);
				}
				if (mbe.ButtonIndex == MouseButton.WheelUp && _zoomable)
				{
					_zoomAtPoint(_zoomChangeSpeed, zoomPoint);
				}
				
				
				ChangeZoom.Invoke();
			}
			if (@event is InputEventKey eventKey)
			{
				switch (eventKey.Keycode)
				{
					case Key.Up:
						_moveUp = eventKey.Pressed;
						break;
					case Key.Down:
						_moveDown = eventKey.Pressed;
						break;
					case Key.Right:
						_moveRight = eventKey.Pressed;
						break;
					case Key.Left:
						_moveLeft = eventKey.Pressed;
						break;
					default:
						break;
				}
			}
			
			if (Input.IsActionPressed("pan_2d") && @event is InputEventMouseMotion mouseEvent)
				GlobalPosition += PanSpeed * -mouseEvent.Relative / Zoom;
		}
		
		
		private void _zoomAtPoint(float factor, Vector2 point)
		{
			factor = Mathf.Clamp(factor, _minZoom / Zoom.X - 1f, _maxZoom/ Zoom.X - 1f);

			GlobalPosition += (point - GlobalPosition) * factor;
			Zoom += Zoom * factor;
		}


		public void LimitToMap()
		{
			Vector2 mapSize = EngineState.MapInfo.Scenario.MapTexture.GetSize();
			Vector2 screenSize = GetViewport().GetVisibleRect().Size;

			this.Position = mapSize / 2;
			this.Zoom = new Vector2(System.Math.Max(screenSize.X / mapSize.X, screenSize.Y / mapSize.Y),
				                    System.Math.Max(screenSize.X / mapSize.X, screenSize.Y / mapSize.Y));
			_minZoom = System.Math.Max(screenSize.X / mapSize.X, screenSize.Y / mapSize.Y);
			
			this.LimitLeft = 0;
			this.LimitRight = (int)mapSize.X;
			this.LimitTop = 0;
			this.LimitBottom = (int)mapSize.Y;
		}

		public void GoToProvince(int provinceId)
		{
			Vector2 target = EngineState.MapInfo.Scenario.Map[provinceId].CenterOfWeight;
			Tween tween = GetTree().CreateTween();
			Tween tween2 = GetTree().CreateTween();
			tween.TweenProperty(this, "position", target, 0.4f);
			tween2.TweenProperty(this, "zoom", new Vector2(_maxZoom, _maxZoom), 0.4f);
		}

}
