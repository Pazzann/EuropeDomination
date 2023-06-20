using Godot;
using System;
using System.Collections.Generic;

namespace EuropeDominationDemo.Scripts.UI;

public partial class GUIBuildings : Node2D
{

	private GUI _gui;
	private Sprite2D _guiBuildingsMenu;
	private MapHandler _mapHandler;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gui = GetParent().GetParent().GetParent() as GUI;
		_guiBuildingsMenu = GetParent().GetNode<Sprite2D>("BuildingMenu");
		_mapHandler = GetParent().GetParent().GetParent().GetParent().GetParent().GetNode<MapHandler>("Map");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void CloseBuildingMenu()
	{
		_guiBuildingsMenu.Visible = false;
	}

	public void ShowBuildings(List<int> ids)
	{
		for (int i = 0; i < ids.Count; i++)
		{
			int id = ids[i];
			(GetChild(i).GetChild(0) as AnimatedSprite2D).Frame = id;
		}

		for (int i = ids.Count; i < 10; i++)
		{
			(GetChild(i).GetChild(0) as AnimatedSprite2D).Frame = 0;
		}
		
	}
	
	
	
	private void _on_0_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_1_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_2_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_3_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_4_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}

	
	private void _on_5_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_6_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_7_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_8_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}


	private void _on_9_pressed()
	{
		_guiBuildingsMenu.Visible = true;
	}
	
	private void _on_workshop_button_pressed()
	{
		_mapHandler.AddBuilding(1);
	}

	
	private void _on_exit_button_pressed()
	{
		_guiBuildingsMenu.Visible = false;
	}

}








