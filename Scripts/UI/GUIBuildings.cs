using Godot;
using System;

namespace EuropeDominationDemo.Scripts.UI;

public partial class GUIBuildings : Node2D
{

	private GUI _gui;
	private Sprite2D _guiBuildingsMenu;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_gui = GetParent().GetParent().GetParent() as GUI;
		_guiBuildingsMenu = GetParent().GetNode<Sprite2D>("BuildingMenu");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
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
	
	private void _on_exit_button_pressed()
	{
		_guiBuildingsMenu.Visible = false;
	}

}






