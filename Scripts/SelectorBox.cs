using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;

namespace EuropeDominationDemo.Scripts;
public partial class SelectorBox : Godot.ColorRect
{

	private bool _isMouseDown = false;
	private Vector2 _startMousePos;
	private Vector2 _endMousePos;

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left})
		{
			if (_isMouseDown && !@event.IsPressed())
			{
				_isMouseDown = false;
				_endMousePos = GetGlobalMousePosition();
				_selectionEnded();
				Size = Vector2.Zero;
			}
			else if (@event.IsPressed())
			{
				_isMouseDown = true;
				_startMousePos = GetGlobalMousePosition();
				GlobalPosition = _startMousePos;
				Size = new Vector2(1, 1);
			}
		}
		if (@event is InputEventMouseMotion)
			if(_isMouseDown)
				_update();
	}

	private void _update()
	{
		Scale = new Vector2(GetGlobalMousePosition().X > _startMousePos.X ? 1 : -1, GetGlobalMousePosition().Y > _startMousePos.Y ? 1 : -1);
		Size = (GetGlobalMousePosition() - _startMousePos) * Scale;
	}

	private void _selectionEnded()
	{
		var allUnits = GetTree().GetNodesInGroup("ArmyUnit");
		if (allUnits == null)
			return;
		var selectedUnits = (from unit in allUnits where ((ArmyUnit)unit).IsInsideRect(GetGlobalRect()) select unit as ArmyUnit).ToList();
		ArmyUnit.SelectUnits(allUnits, selectedUnits);
	}
}
