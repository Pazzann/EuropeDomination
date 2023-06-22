using Godot;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.UI;

public partial class GUIResources : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void DrawResources(ProvinceData data){
		for(int i = 0; i < data.Resources.Length; i++)
		{
			(GetChild(i).GetChild(1) as Label).Text = data.Resources[i].ToString("N1");
		}

		(GetChild((int)data.Good).GetChild(2) as Label).Text = "+" + data.ProductionRate.ToString("N1") + "t/m";
	}
}

