using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;

namespace EuropeDominationDemo.Scripts;

public delegate void ZoomChangedEventHandler();

public partial class Camera : Camera2D
{
	public float MinZoom { get; private set; } = 0.2f;
	public float MaxZoom { get; set; } = 10f;

	public Rect2 GlobalBounds { get; private set; } = new(-Vector2.Inf, Vector2.Inf);

	public float MovementSpeed { get; set; } = 400f;
	public float PanSpeed { get; set; } = 0.7f;
	public float ZoomSpeed { get; set; } = 0.03f;

	public bool EnableZoom { get; set; } = true;
	
	public event ZoomChangedEventHandler ZoomChanged;

	public override void _Ready()
	{
		GetViewport().SizeChanged += () => Reset(GlobalBounds);
	}

	public override void _Process(double delta)
	{
		var movement = Input.GetVector(
			"camera_move_left",
			"camera_move_right",
			"camera_move_up",
			"camera_move_down"
		);

		Move(movement * ((float)delta * MovementSpeed) / Zoom);
	}

	public void HandleInput(InputEvent @event)
	{
		if (EnableZoom)
		{
			if (@event.IsAction("camera_zoom_in"))
				ZoomAtPoint(ZoomSpeed, GetLocalMousePosition());

			if (@event.IsAction("camera_zoom_out"))
				ZoomAtPoint(-ZoomSpeed, GetLocalMousePosition());
		}

		if (Input.IsActionPressed("camera_mouse_pan") && @event is InputEventMouseMotion mouseMotionEvent)
			Move(PanSpeed * -mouseMotionEvent.Relative / Zoom);
	}

	public void Reset(Rect2 globalBounds)
	{
		
		var viewportSize = GetViewport().GetVisibleRect().Size;

		// Compute the new camera bounds.
		GlobalBounds = globalBounds;

		// Center the camera relative to the map.
		GlobalPosition = globalBounds.GetCenter();

		// Set `Zoom` to cover the whole map.
		Zoom = viewportSize / globalBounds.Size;

		// Force uniform zoom to preserve aspect ratios.
		Zoom = Vector2.One * Mathf.Max(Zoom.X, Zoom.Y);

		// Update `MinZoom` to avoid zooming too far out.
		MinZoom = Mathf.Max(Zoom.X, Zoom.Y);

		// Ensure that `MinZoom` <= `MaxZoom`.
		MaxZoom = Mathf.Max(MinZoom, MaxZoom);
	}

	public void GoToProvince(int provinceId, float animDuration = 0.4f)
	{
		var mapCoords = EngineState.MapInfo.Scenario.Map[provinceId].CenterOfWeight;

		var moveTween = GetTree().CreateTween();
		moveTween.TweenProperty(this, "global_position", mapCoords, animDuration);

		var zoomTween = moveTween.Parallel();
		zoomTween.TweenProperty(this, "zoom", new Vector2(MaxZoom, MaxZoom), animDuration);
	}

	public void GoTo(Vector2 mapCoords, float animDuration = 0.4f)
	{
		var globalCoords = AdjustNewGlobalPosition(MapToGlobal(mapCoords));
		var moveTween = GetTree().CreateTween();
		moveTween.TweenProperty(this, "global_position", globalCoords, animDuration);
	}

	private void Move(Vector2 delta)
	{
		Translate(delta);
		AdjustPosition();
	}

	private void ZoomAtPoint(float factor, Vector2 localPoint)
	{
		var globalPoint = GetGlobalTransform() * localPoint;
		factor = ComputeZoomFactor(factor);

		Zoom += Zoom * factor;
		GlobalPosition += factor * (globalPoint - GlobalPosition);

		AdjustPosition();
		ZoomChanged?.Invoke();
	}

	private float ComputeZoomFactor(float factor)
	{
		var minFactor = MinZoom / Mathf.Max(Zoom.X, Zoom.Y) - 1f;
		var maxFactor = MaxZoom / Mathf.Min(Zoom.X, Zoom.Y) - 1f;
		return Mathf.Clamp(factor, minFactor, maxFactor);
	}

	private Vector2 MapToGlobal(Vector2 mapCoords)
	{
		return GlobalBounds.Position + mapCoords;
	}

	private void AdjustPosition()
	{
		GlobalPosition = AdjustNewGlobalPosition(GlobalPosition);
	}

	private Vector2 AdjustNewGlobalPosition(Vector2 newGlobalPosition)
	{
		var visibleSize = GetVisibleRectSizeGlobal();

		var min = GlobalBounds.Position + visibleSize * 0.5f;
		var max = GlobalBounds.End - 0.5f * visibleSize;

		return newGlobalPosition.Clamp(min, max);
	}

	private Vector2 GetVisibleRectSizeGlobal()
	{
		return GetViewport().GetVisibleRect().Size / Zoom;
	}
}
