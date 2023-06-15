using Godot;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using EuropeDominationDemo.Scripts.Enums;

namespace EuropeDominationDemo.Scripts.UI;

public partial class GUIResources : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void DrawResources(float[] goods){
		for(int i = 0; i < goods.Length; i++)
		{
			(GetChild(i).GetChild(1) as Label).Text = goods[i].ToString();
		}
	}
}

