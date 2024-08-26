using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using Godot;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Utils;

namespace EuropeDominationDemo.Scripts.UI;

public partial class GUIResources : VBoxContainer
{
	private PackedScene GUIResource;

	public void Init()
	{
		//TODO: change
		var defaultProvince = EngineState.MapInfo.Scenario.Map[0] as LandProvinceData;

		GUIResource = GD.Load<PackedScene>("res://Prefabs/GUI/GUIResource.tscn");
		(GetParent() as ScrollContainer).CustomMinimumSize = new Vector2(450.0f, 40.0f * defaultProvince.Resources.Length);
		CustomMinimumSize = new Vector2(450.0f, 40.0f * defaultProvince.Resources.Length);
		
		for (int i = 0; i < defaultProvince.Resources.Length; i++)
		{
			var a = GUIResource.Instantiate();
			(a.GetChild(0).GetChild(0).GetChild(0) as AnimatedTextureRect).SetFrame(i);
			AddChild(a);
		}
	}
	
	private void _clearInfo(float[] resources)
	{
		for (int i = 0; i < resources.Length; i++)
		{
			(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).Text = "+0t/m";
			(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).SelfModulate = new Color(1.0f, 1.0f, 1.0f);
		}
	}

	public void DrawResources(LandProvinceData data)
	{
		var AllResourcesChange = new float[data.Resources.Length];

		for (int i = 0; i < data.Resources.Length; i++)
		{
			AllResourcesChange[i] = 0;
		}

		_clearInfo(data.Resources);

		foreach (LandProvinceData provinceData in EngineState.MapInfo.Scenario.Map.Where(dat => dat is LandProvinceData d && d.HarvestedTransport != null && d.HarvestedTransport.ProvinceIdTo == data.Id))
		{
			AllResourcesChange[(int)provinceData.HarvestedTransport.TransportationGood] +=
				provinceData.HarvestedTransport.Amount;
		}

		AllResourcesChange[(int)data.Good] += data.ProductionRate;
		if (data.HarvestedTransport != null)
			AllResourcesChange[(int)data.HarvestedTransport.TransportationGood] -= data.HarvestedTransport.Amount;

		for (int i = 0; i < data.Resources.Length; i++)
		{
			(GetChild(i).GetChild(0).GetChild(0).GetChild(1) as Label).Text = data.Resources[i].ToString("N1");
			(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).Text = ((AllResourcesChange[i] >= 0) ? "+" : "-") +
																			  AllResourcesChange[i].ToString("N1") + "t/m";
			if (AllResourcesChange[i] >= 0)
				(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).SelfModulate = MapDefaultColors.ResourceIncrease;
			else
				(GetChild(i).GetChild(0).GetChild(0).GetChild(2) as Label).SelfModulate = MapDefaultColors.ResourceDecrease;
		}
	}
}
