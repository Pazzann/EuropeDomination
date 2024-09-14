using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;

namespace EuropeDominationDemo.Scripts;

public delegate void ZoomChangedEventHandler();

public partial class Camera : Camera2D
{
    public float MinZoom { get; private set; } = 0.2f;
    public float MaxZoom { get; set; } = 10f;

    public Rect2 Bounds { get; private set; } = new(-Vector2.Inf, Vector2.Inf);

    public float MovementSpeed { get; set; } = 400f;
    public float PanSpeed { get; set; } = 0.7f;
    public float ZoomSpeed { get; set; } = 0.03f;

    public bool EnableZoom { get; set; } = true;
    public event ZoomChangedEventHandler ZoomChanged;

    public override void _Ready()
    {
        GetViewport().SizeChanged += Reset;
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
    
    public override void _Input(InputEvent @event)
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

    public void Reset()
    {
        var mapSize = (Vector2)EngineState.MapInfo.Scenario.MapTexture.GetSize();
        var viewportSize = GetViewport().GetVisibleRect().Size;

        // Center the camera relative to the map.
        Position = mapSize / 2f;

        // Cap camera's zoom to keep the visible viewport entirely within the map.
        MinZoom = Mathf.Max(viewportSize.X / mapSize.X, viewportSize.Y / mapSize.Y);
        MaxZoom = Mathf.Max(MinZoom, MaxZoom);

        // Reset camera zoom, showing the whole map.
        Zoom = Vector2.One * MinZoom;

        Bounds = new Rect2(Vector2.Zero, mapSize);
    }

    public void GoToProvince(int provinceId)
    {
        // `target` is in the global coordinate system.
        var target = EngineState.MapInfo.Scenario.Map[provinceId].CenterOfWeight;

        GetTree()
            .CreateTween()
            .TweenProperty(this, "global_position", target, 0.4f);

        GetTree()
            .CreateTween()
            .TweenProperty(this, "zoom", new Vector2(MaxZoom, MaxZoom), 0.4f);
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

    private void AdjustPosition()
    {
        var visibleSize = GetVisibleRectSizeGlobal();

        var min = Bounds.Position + visibleSize * 0.5f;
        var max = Bounds.End - 0.5f * visibleSize;

        GlobalPosition = GlobalPosition.Clamp(min, max);
    }

    private Vector2 GetVisibleRectSizeGlobal()
    {
        return GetViewport().GetVisibleRect().Size / Zoom;
    }
}