using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;
/// <summary>
/// Event handler for the ZoomChanged event.
/// </summary>
public delegate void ZoomChangedEventHandler();
/// <summary>
/// A 2D camera that can be moved and zoomed.
/// </summary>
public partial class Camera : Camera2D
{
	/// <summary>
	/// The min zoom level.
	/// </summary>
	public float MinZoom { get; private set; } = 0.2f;
	/// <summary>
	/// The max zoom level.
	/// </summary>
	public float MaxZoom { get; set; } = 10f;
	
	/// <summary>
	/// The global bounds of the camera.
	/// </summary>
	public Rect2 GlobalBounds { get; private set; } = new(-Vector2.Inf, Vector2.Inf);

	/// <summary>
	/// Movemennt speed of the camera.
	/// </summary>
	public float MovementSpeed { get; set; } = 400f;
	/// <summary>
	/// Pan Speed of the camera.
	/// </summary>
	public float PanSpeed { get; set; } = 0.7f;
	/// <summary>
	/// Zoom speed of the camera.
	/// </summary>
	public float ZoomSpeed { get; set; } = 0.03f;

	/// <summary>
	/// Is zoom enabled.
	/// </summary>
	public bool EnableZoom { get; set; } = true;
	
	/// <summary>
	/// Event emitted when the zoom level changes.
	/// </summary>
	public event ZoomChangedEventHandler ZoomChanged;

	
	/// <summary>
	/// Called when the node enters the scene tree for the first time.
	/// </summary>
	public override void _Ready()
	{
		GetViewport().SizeChanged += () => Reset(GlobalBounds);
	}

	
	/// <summary>
	/// Called every frame. 'delta' is the elapsed time since the previous frame.
	/// </summary>
	/// <param name="delta"></param>
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
	
	
	/// <summary>
	/// Handles input events.
	/// </summary>
	/// <param name="event"></param>
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
	
	/// <summary>
	/// Resets the camera to the specified global bounds.
	/// </summary>
	/// <param name="globalBounds"></param>
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
	/// <summary>
	/// Moves the camera to the specified province.
	/// </summary>
	/// <param name="provinceId"></param>
	/// <param name="animDuration"></param>
	public void GoToProvince(int provinceId, float animDuration = 0.4f)
	{
		var mapCoords = EngineState.MapInfo.Scenario.Map[provinceId].CenterOfWeight;

		var moveTween = GetTree().CreateTween();
		moveTween.TweenProperty(this, "global_position", mapCoords, animDuration);

		var zoomTween = moveTween.Parallel();
		zoomTween.TweenProperty(this, "zoom", new Vector2(MaxZoom, MaxZoom), animDuration);
	}

	/// <summary>
	/// Moves the camera to the specified map coordinates.
	/// </summary>
	/// <param name="mapCoords"></param>
	/// <param name="animDuration"></param>
	public void GoTo(Vector2 mapCoords, float animDuration = 0.4f)
	{
		var globalCoords = AdjustNewGlobalPosition(MapToGlobal(mapCoords));
		var moveTween = GetTree().CreateTween();
		moveTween.TweenProperty(this, "global_position", globalCoords, animDuration);
	}

	/// <summary>
	/// Moves camera on specified delta.
	/// </summary>
	/// <param name="delta"></param>
	private void Move(Vector2 delta)
	{
		Translate(delta);
		AdjustPosition();
	}

	
	/// <summary>
	/// Zooms the camera at the specified local point.
	/// </summary>
	/// <param name="factor"></param>
	/// <param name="localPoint"></param>
	private void ZoomAtPoint(float factor, Vector2 localPoint)
	{
		var globalPoint = GetGlobalTransform() * localPoint;
		factor = ComputeZoomFactor(factor);

		Zoom += Zoom * factor;
		GlobalPosition += factor * (globalPoint - GlobalPosition);

		AdjustPosition();
		ZoomChanged?.Invoke();
	}

	
	/// <summary>
	/// Computes the zoom factor.
	/// </summary>
	/// <param name="factor"></param>
	/// <returns></returns>
	private float ComputeZoomFactor(float factor)
	{
		var minFactor = MinZoom / Mathf.Max(Zoom.X, Zoom.Y) - 1f;
		var maxFactor = MaxZoom / Mathf.Min(Zoom.X, Zoom.Y) - 1f;
		return Mathf.Clamp(factor, minFactor, maxFactor);
	}

	
	/// <summary>
	/// Converts map coordinates to global coordinates.
	/// </summary>
	/// <param name="mapCoords"></param>
	/// <returns></returns>
	private Vector2 MapToGlobal(Vector2 mapCoords)
	{
		return GlobalBounds.Position + mapCoords;
	}

	
	/// <summary>
	/// Adjusts the camera position to stay within the global bounds.
	/// </summary>
	private void AdjustPosition()
	{
		GlobalPosition = AdjustNewGlobalPosition(GlobalPosition);
	}
	/// <summary>
	/// Adjusts the new global position to stay within the global bounds.
	/// </summary>
	/// <param name="newGlobalPosition"></param>
	/// <returns></returns>
	private Vector2 AdjustNewGlobalPosition(Vector2 newGlobalPosition)
	{
		var visibleSize = GetVisibleRectSizeGlobal();

		var min = GlobalBounds.Position + visibleSize * 0.5f;
		var max = GlobalBounds.End - 0.5f * visibleSize;

		return newGlobalPosition.Clamp(min, max);
	}
	/// <summary>
	/// Gets the visible rect size in global coordinates.
	/// </summary>
	/// <returns></returns>
	private Vector2 GetVisibleRectSizeGlobal()
	{
		return GetViewport().GetVisibleRect().Size / Zoom;
	}
}
