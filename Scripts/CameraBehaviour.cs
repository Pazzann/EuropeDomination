using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;

namespace EuropeDominationDemo.Scripts;

public delegate void ZoomChangedEventHandler();

public partial class CameraBehaviour : Camera2D
{
    public float MinZoom = 0.3f;
    public float MaxZoom = 10f;

    public float MovementSpeed = 1f;
    public float PanSpeed = 1f;
    public float ZoomSpeed = 0.05f;
    
    private Direction _movementDirection = Direction.None;

    public bool EnableZoom = true;

    private float CameraZoom
    {
        get => Zoom.X;
        set => Zoom = new Vector2(value, value);
    }

    public event ZoomChangedEventHandler ZoomChanged;

    public override void _Ready()
    {
        Enabled = true;
        GetViewport().SizeChanged += LimitToMap;
    }

    public override void _Process(double delta)
    {
        var movement = new Vector2I();

        if ((_movementDirection & Direction.Left) != 0)
            movement.X -= 1;

        if ((_movementDirection & Direction.Right) != 0)
            movement.X += 1;

        if ((_movementDirection & Direction.Up) != 0)
            movement.Y -= 1;

        if ((_movementDirection & Direction.Down) != 0)
            movement.Y += 1;

        Translate(new Vector2(movement.X, movement.Y).Normalized() * MovementSpeed * CameraZoom);
    }

    public void InputHandle(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && EnableZoom)
        {
            var zoomPoint = GetGlobalMousePosition();

            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown) ZoomAtPoint(-ZoomSpeed, zoomPoint);

            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp) ZoomAtPoint(ZoomSpeed, zoomPoint);

            ZoomChanged?.Invoke();
        }

        if (@event is InputEventKey { Pressed: true } keyEvent)
            switch (keyEvent.Keycode)
            {
                case Key.Up:
                    _movementDirection |= Direction.Up;
                    break;
                case Key.Down:
                    _movementDirection |= Direction.Down;
                    break;
                case Key.Right:
                    _movementDirection |= Direction.Right;
                    break;
                case Key.Left:
                    _movementDirection |= Direction.Left;
                    break;
            }

        if (Input.IsActionPressed("pan_2d") && @event is InputEventMouseMotion mouseMotionEvent)
            Translate(PanSpeed * -mouseMotionEvent.Relative / Zoom);
    }

    private void ZoomAtPoint(float factor, Vector2 point)
    {
        factor = Mathf.Clamp(factor, MinZoom / Zoom.X - 1f, MaxZoom / Zoom.X - 1f);
        Position += (point - Position) * factor;
        Zoom += Zoom * factor;
    }

    public void LimitToMap()
    {
        var mapSize = EngineState.MapInfo.Scenario.MapTexture.GetSize();
        var viewportSize = GetViewport().GetVisibleRect().Size;

        Position = mapSize / 2;

        CameraZoom = Mathf.Max(viewportSize.X / mapSize.X, viewportSize.Y / mapSize.Y);
        MinZoom = Mathf.Max(viewportSize.X / mapSize.X, viewportSize.Y / mapSize.Y);

        LimitLeft = 0;
        LimitRight = mapSize.X;
        LimitTop = 0;
        LimitBottom = mapSize.Y;
    }

    public void GoToProvince(int provinceId)
    {
        var target = EngineState.MapInfo.Scenario.Map[provinceId].CenterOfWeight;

        GetTree()
            .CreateTween()
            .TweenProperty(this, "position", target, 0.4f);

        GetTree()
            .CreateTween()
            .TweenProperty(this, "zoom", new Vector2(MaxZoom, MaxZoom), 0.4f);
    }

    [Flags]
    private enum Direction
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Down = 1 << 3
    }
}