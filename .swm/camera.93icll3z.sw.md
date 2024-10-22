---
title: Camera
---
# Introduction

This document will walk you through the implementation of the <SwmToken path="/Scripts/Utils/Camera.cs" pos="13:6:6" line-data="public partial class Camera : Camera2D">`Camera`</SwmToken> class.

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="13:6:6" line-data="public partial class Camera : Camera2D">`Camera`</SwmToken> class extends <SwmToken path="/Scripts/Utils/Camera.cs" pos="13:10:10" line-data="public partial class Camera : Camera2D">`Camera2D`</SwmToken> and provides additional functionality for zooming, panning, and moving within specified bounds.

We will cover:

1. Initialization of camera properties.
2. Handling viewport size changes.
3. Processing input for movement and zoom.
4. Resetting and adjusting camera bounds.
5. Moving the camera to specific locations.
6. Utility functions for coordinate conversion and position adjustment.

# Initialization of camera properties

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="13">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="13:6:6" line-data="public partial class Camera : Camera2D">`Camera`</SwmToken> class initializes several properties to control its behavior, such as zoom levels, movement speed, and pan speed.

```
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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="28">

---

Additional properties for movement and zoom speeds are defined here:

```

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
```

---

</SwmSnippet>

# Handling viewport size changes

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="51">

---

When the node enters the scene tree for the first time, it subscribes to the viewport size change event to reset the camera bounds accordingly.

```

	
	/// <summary>
	/// Called when the node enters the scene tree for the first time.
	/// </summary>
	public override void _Ready()
	{
		GetViewport().SizeChanged += () => Reset(GlobalBounds);
	}
```

---

</SwmSnippet>

# Processing input for movement and zoom

<SwmSnippet path="Scripts/Utils/Camera.cs" line="66">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="66:7:7" line-data="	public override void _Process(double delta)">`_Process`</SwmToken> method handles frame-by-frame movement based on user input.

```
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
```

---

</SwmSnippet>

<SwmSnippet path="Scripts/Utils/Camera.cs" line="83">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="83:5:5" line-data="	public void HandleInput(InputEvent @event)">`HandleInput`</SwmToken> method processes input events for zooming and panning.

```
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
```

---

</SwmSnippet>

# Resetting and adjusting camera bounds

<SwmSnippet path="Scripts/Utils/Camera.cs" line="102">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="58:14:14" line-data="		GetViewport().SizeChanged += () =&gt; Reset(GlobalBounds);">`Reset`</SwmToken> method sets the camera to the specified global bounds and adjusts the zoom to cover the entire map.

```
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
```

---

</SwmSnippet>

# Moving the camera to specific locations

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="133">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="130:5:5" line-data="	public void GoToProvince(int provinceId, float animDuration = 0.4f)">`GoToProvince`</SwmToken> method moves the camera to a specified province with an animation.

```

		var moveTween = GetTree().CreateTween();
		moveTween.TweenProperty(this, "global_position", mapCoords, animDuration);

		var zoomTween = moveTween.Parallel();
		zoomTween.TweenProperty(this, "zoom", new Vector2(MaxZoom, MaxZoom), animDuration);
	}
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="140">

---

Similarly, the <SwmToken path="/Scripts/Utils/Camera.cs" pos="146:5:5" line-data="	public void GoTo(Vector2 mapCoords, float animDuration = 0.4f)">`GoTo`</SwmToken> method moves the camera to specified map coordinates.

```

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
```

---

</SwmSnippet>

# Utility functions for coordinate conversion and position adjustment

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="152">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="157:5:5" line-data="	private void Move(Vector2 delta)">`Move`</SwmToken> method translates the camera by a specified delta and adjusts its position.

```

	/// <summary>
	/// Moves camera on specified delta.
	/// </summary>
	/// <param name="delta"></param>
	private void Move(Vector2 delta)
	{
		Translate(delta);
		AdjustPosition();
	}
```

---

</SwmSnippet>

<SwmSnippet path="Scripts/Utils/Camera.cs" line="169">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="169:5:5" line-data="	private void ZoomAtPoint(float factor, Vector2 localPoint)">`ZoomAtPoint`</SwmToken> method zooms the camera at a specified local point and adjusts the position.

```
	private void ZoomAtPoint(float factor, Vector2 localPoint)
	{
		var globalPoint = GetGlobalTransform() * localPoint;
		factor = ComputeZoomFactor(factor);

		Zoom += Zoom * factor;
		GlobalPosition += factor * (globalPoint - GlobalPosition);

		AdjustPosition();
		ZoomChanged?.Invoke();
	}
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="180">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="187:5:5" line-data="	private float ComputeZoomFactor(float factor)">`ComputeZoomFactor`</SwmToken> method calculates the appropriate zoom factor based on the current zoom levels.

```

	
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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="193">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="200:5:5" line-data="	private Vector2 MapToGlobal(Vector2 mapCoords)">`MapToGlobal`</SwmToken> method converts map coordinates to global coordinates.

```

	
	/// <summary>
	/// Converts map coordinates to global coordinates.
	/// </summary>
	/// <param name="mapCoords"></param>
	/// <returns></returns>
	private Vector2 MapToGlobal(Vector2 mapCoords)
	{
		return GlobalBounds.Position + mapCoords;
	}
```

---

</SwmSnippet>

<SwmSnippet path="Scripts/Utils/Camera.cs" line="209">

---

The <SwmToken path="/Scripts/Utils/Camera.cs" pos="160:1:1" line-data="		AdjustPosition();">`AdjustPosition`</SwmToken> method ensures the camera stays within the global bounds.

```
	private void AdjustPosition()
	{
		GlobalPosition = AdjustNewGlobalPosition(GlobalPosition);
	}
```

---

</SwmSnippet>

<SwmSnippet path="Scripts/Utils/Camera.cs" line="218">

---

<SwmToken path="/Scripts/Utils/Camera.cs" pos="218:5:5" line-data="	private Vector2 AdjustNewGlobalPosition(Vector2 newGlobalPosition)">`AdjustNewGlobalPosition`</SwmToken> clamps the new global position within the bounds here:

```
	private Vector2 AdjustNewGlobalPosition(Vector2 newGlobalPosition)
	{
		var visibleSize = GetVisibleRectSizeGlobal();

		var min = GlobalBounds.Position + visibleSize * 0.5f;
		var max = GlobalBounds.End - 0.5f * visibleSize;

		return newGlobalPosition.Clamp(min, max);
	}
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Camera.cs" line="231">

---

This code snippet returns the size of the visible rectangle in global coordinates by dividing the size of the visible rectangle obtained from the viewport by the value of `Zoom`.

```c#
	private Vector2 GetVisibleRectSizeGlobal()
	{
		return GetViewport().GetVisibleRect().Size / Zoom;
	}
```

---

</SwmSnippet>

&nbsp;

This concludes the walkthrough of the <SwmToken path="/Scripts/Utils/Camera.cs" pos="13:6:6" line-data="public partial class Camera : Camera2D">`Camera`</SwmToken> class implementation. Each section highlights the key design decisions and how they fit into the overall functionality of the camera.

<SwmMeta version="3.0.0" repo-id="Z2l0aHViJTNBJTNBRXVyb3BlRG9taW5hdGlvbkRlbW8lM0ElM0FQYXp6YW5u" repo-name="EuropeDominationDemo"><sup>Powered by [Swimm](https://app.swimm.io/)</sup></SwmMeta>
