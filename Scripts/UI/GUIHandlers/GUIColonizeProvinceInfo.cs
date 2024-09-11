using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using Godot;

public partial class GUIColonizeProvinceInfo : GUIHandler
{
	private UncolonizedProvinceData _currentlyShownProvince;

	public override void Init()
	{
	}

	public override void InputHandle(InputEvent @event)
	{
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowUncolonizedProvinceData e:
			{
				_currentlyShownProvince = e.UncolonizedProvinceData;
				Visible = true;
				return;
			}
		}
	}
}
