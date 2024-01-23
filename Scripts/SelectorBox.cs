using Godot;
using System.Linq;

namespace EuropeDominationDemo.Scripts;
public partial class SelectorBox : Godot.ColorRect
{

	private bool _isMouseDown = false;
	private Vector2 _startMousePos;
	private Vector2 _endMousePos;
	private MapHandler _mapHandler;
	private bool _wasSelectedUnit = false;

	public override void _Ready()
	{
		_mapHandler = GetNode<MapHandler>("../Map");
	}

	public override void _Input(InputEvent @event)
	{
		switch (@event)
		{
			case InputEventMouseButton { ButtonIndex: MouseButton.Left} when _isMouseDown && !@event.IsPressed():
				_isMouseDown = false;
				_endMousePos = GetGlobalMousePosition();
				_selectionEnded();
				Size = Vector2.Zero;
				break;
			case InputEventMouseButton { ButtonIndex: MouseButton.Left}:
			{
				if (@event.IsPressed())
				{
					_isMouseDown = true;
					_startMousePos = GetGlobalMousePosition();
					GlobalPosition = _startMousePos;
					Size = new Vector2(1, 1);
				}

				break;
			}
			case InputEventMouseMotion:
			{
				if(_isMouseDown)
					_update();
				break;
			}
		}

		if (_wasSelectedUnit)
		{
			_wasSelectedUnit = false;
			return;
		}
		_mapHandler.HandleInput(@event);
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
		var trueRect = GetGlobalRect();
		trueRect = new Rect2(new Vector2(Scale.X > 0 ? trueRect.Position.X : trueRect.Position.X - trueRect.Size.X, Scale.Y > 0 ? trueRect.Position.Y : trueRect.Position.Y - trueRect.Size.Y), trueRect.Size);
		var selectedUnits = (from unit in allUnits where ((ArmyUnit)unit).IsInsideRect(trueRect) select unit as ArmyUnit).ToList();
		ArmyUnit.SelectUnits(allUnits, selectedUnits);
		_mapHandler.CurrentSelectedUnits = selectedUnits;
		if (selectedUnits.Count > 0)
			_wasSelectedUnit = true;
	}
}
