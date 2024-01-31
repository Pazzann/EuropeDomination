using Godot;
using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI;

public partial class GUIResources : Control
{

	private void _clearInfo(float[] resources)
	{
		for(int i = 0; i < resources.Length; i++)
		{
			(GetChild(i).GetChild(2) as Label).Text = "+0t/m";
			(GetChild(i).GetChild(2) as Label).SelfModulate = new Color(1.0f, 1.0f, 1.0f);
		}

	}

	public void DrawResources(ProvinceData data){
		for(int i = 0; i < data.Resources.Length; i++)
		{
			(GetChild(i).GetChild(1) as Label).Text = data.Resources[i].ToString("N1");
		}
		
		_clearInfo(data.Resources);
		
		(GetChild((int)data.Good).GetChild(2) as Label).Text = "+" + data.ProductionRate.ToString("N1") + "t/m";
		if (data.ProductionRate > 0)
			(GetChild((int)data.Good).GetChild(2) as Label).SelfModulate = new Color(0.0f, 1.0f, 0.0f);
		else
			(GetChild((int)data.Good).GetChild(2) as Label).SelfModulate = new Color(1.0f, 0.0f, 0.0f);
	}
}

