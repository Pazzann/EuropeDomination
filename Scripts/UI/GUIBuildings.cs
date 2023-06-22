using Godot;
using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;

namespace EuropeDominationDemo.Scripts.UI;

public partial class GUIBuildings : Node2D
{
	private GUI _gui;
	private Sprite2D _guiBuildingsMenu;
	private MapHandler _mapHandler;
	private List<Building> _currentBuildings = new List<Building>();

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

	public void ShowBuildings(List<Building> buildings)
	{
		var unlockedBuildings = 4;


		int g = 0;
		foreach (var building in buildings)
		{
			if (building.IsFinished)
			{
				(GetChild(g).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(1.0f, 1.0f, 1.0f);
				(GetChild(g).GetChild(0) as AnimatedSprite2D).Frame = building.ID;
				(GetChild(g).GetChild(1) as ProgressBar).Visible = false;
			}
			else
			{
				(GetChild(g).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(0.5f, 0.5f, 0.5f);
				(GetChild(g).GetChild(0) as AnimatedSprite2D).Frame = building.ID;
				var progressBar = GetChild(g).GetChild(1) as ProgressBar;
				progressBar.Visible = true;
				progressBar.MaxValue = building.TimeToBuild;
				progressBar.Value = building.BuildingTime;
			}

			(GetChild(g).GetChild(2) as Sprite2D).Visible = false;
			(GetChild(g) as Button).Disabled = true;
			(GetChild(g).GetChild(3) as Sprite2D).Visible = true;
			(GetChild(g).GetChild(3).GetChild(0) as Button).Disabled = false;
			g++;
		}

		for (int i = buildings.Count; i < 10; i++)
		{
			
			(GetChild(i).GetChild(0) as AnimatedSprite2D).Frame = 0;
			(GetChild(i).GetChild(1) as ProgressBar).Visible = false;
			
			(GetChild(i).GetChild(3) as Sprite2D).Visible = false;
			(GetChild(i).GetChild(3).GetChild(0) as Button).Disabled = true;
			if (i > unlockedBuildings - 1)
			{
				(GetChild(i).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(0.5f, 0.5f, 0.5f);
				(GetChild(i).GetChild(2) as Sprite2D).Visible = true;
				(GetChild(i) as Button).Disabled = true;
			}
			else
			{
				(GetChild(i).GetChild(0) as AnimatedSprite2D).SelfModulate = new Color(1.0f, 1.0f, 1.0f);
				(GetChild(i).GetChild(2) as Sprite2D).Visible = false;
				(GetChild(i) as Button).Disabled = false;
			}
		}

		_currentBuildings = buildings;
		if (_guiBuildingsMenu.Visible)
			_showBuildingsMenu();
	}


	private void _showBuildingsMenu()
	{
		_guiBuildingsMenu.Visible = true;
		var workshopSprite = _guiBuildingsMenu.GetChild(0) as AnimatedSprite2D;
		(workshopSprite.GetChild(0) as Label).Text = Building.Buildings[1].Cost.ToString();
		if (_currentBuildings.Exists(i => i.ID == 1))
		{
			workshopSprite.SelfModulate = new Color(0.5f, 1.0f, 0.5f);
			(workshopSprite.GetChild(1) as Button).Disabled = true;
		}
		else
		{
			workshopSprite.SelfModulate = new Color(1.0f, 1.0f, 1.0f);
			(workshopSprite.GetChild(1) as Button).Disabled = false;
		}
	}

	private void _onShowPressed()
	{
		_showBuildingsMenu();
	}


	private void _on_destroy_button_pressed(long extra_arg_0)
	{
		if (extra_arg_0 < _currentBuildings.Count)
		{
			_currentBuildings.RemoveAt((int)extra_arg_0);
		}
		
	}


	private void _on_workshop_button_pressed()
	{
		_mapHandler.AddBuilding(new Workshop());
	}


	private void _on_exit_button_pressed()
	{
		_guiBuildingsMenu.Visible = false;
	}
}









