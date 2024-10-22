using System;
using System.Linq;
using System.Reflection;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using EuropeDominationDemo.Scripts.UI.Events.GUI;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using EuropeDominationDemo.Scripts.UI.GUIPrefabs;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

public partial class GUICountryWindow : GUIHandler
{


	public override void Init()
	{
		GetChild<TabContainer>(0).MouseEntered += () => InvokeGUIEvent(new GUIHideInfoBoxEvent());
		_consumableGoodsInit();
		_technologyInit();
		_armyInit();
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowCountryWindowEvent:
				_showCountryData();
				Visible = true;
				return;
			case ToGUIUpdateCountryWindow:
				_updateCountryData();
				return;
			case ToGuiShowLandProvinceDataEvent:
			case ToGUIShowArmyViewerEvent:
			case ToGUIShowDiplomacyWindow:
			{
				Visible = false;
				return;
			}
		}
	}
	
	public override void InputHandle(InputEvent @event)
	{
	}


	private void _showCountryData()
	{
		foreach (var child in _armyRegimentTemplateContainer.GetChildren()) child.QueueFree();

		foreach (var template in EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates
					 .Where(a => a is ArmyRegimentTemplate))
		{
			var a = _armyRegimentTemplateScene.Instantiate();
			a.GetChild(0).GetChild<Label>(0).Text = template.Name;
			a.GetChild(0).GetChild<Button>(1).Pressed += () => _onEditArmyTemplatePressed(template.Id);
			_armyRegimentTemplateContainer.AddChild(a);
		}

		_updateCountryData();
		
	}

	private void _updateCountryData()
	{
		
		_updateTechnologyWindow();
		_consumableGoodsUpdate();
		
	}

	#region Overview

	

	#endregion
	

	#region Economy

	

	#endregion
	
	#region Consumable Goods

	private PackedScene _consumableGoodScene;
	private	VBoxContainer _consumableGoodSpawner;
	private AdvancedLabel _totalModifiersFromConsumableGoodsLabel;
	
	private void _consumableGoodsInit()
	{
		_consumableGoodScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIConsumableGoodMenu.tscn");
		_consumableGoodSpawner =
			GetNode<VBoxContainer>(
				"TabContainer/Consumption/MarginContainer/VBoxContainer/ScrollContainer/ConsumableGoodsSpawner");
		_totalModifiersFromConsumableGoodsLabel =
			GetNode<AdvancedLabel>(
				"TabContainer/Consumption/MarginContainer/VBoxContainer/PanelContainer/MarginContainer/TotalBonusesFromConsumableGoods");
		_totalModifiersFromConsumableGoodsLabel._Ready();
		foreach (ConsumableGood good in EngineState.MapInfo.Scenario.Goods.Where(d=> d is ConsumableGood))
		{
			var a = _consumableGoodScene.Instantiate() as PanelContainer;
			var consumptionContainer = a.GetChild(0).GetChild(0);

			consumptionContainer.GetChild<AnimatedTextureRect>(0).SpriteFrames = GlobalResources.GoodSpriteFrames;
			consumptionContainer.GetChild<AnimatedTextureRect>(0).SetFrame(good.Id);
			consumptionContainer.GetChild(1).GetChild<AdvancedLabel>(0)._Ready();
			consumptionContainer.GetChild(1).GetChild<AdvancedLabel>(0)
				.Clear().NewLine()
				.Header(good.Name).NewLine()
				.Append($"Consumption: {good.ConsumptionPerMonthToActivateBonus}t/m").NewLine()
				.Header("Modifiers:")
				.AppendModifiers(good.Modifiers);
			consumptionContainer.GetChild<Button>(2).Text = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods.ContainsKey(good.Id) ? "Disable" : "Enable";
			consumptionContainer.GetChild<Button>(2).Pressed += () => _changeConsumableGoodStatus(good.Id);
			a.SelfModulate = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods.ContainsKey(good.Id) ? EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods[good.Id] ? new Color(0, 1, 0, 1) : new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);
			_consumableGoodSpawner.AddChild(a);
		}
		_totalModifiersFromConsumableGoodsLabel.Clear().NewLine().Header("Total modifiers from consumption:").AppendModifiers(EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoodsModifiers);
	}

	private void _consumableGoodsUpdate()
	{
		int i = 0;
		foreach (ConsumableGood good in EngineState.MapInfo.Scenario.Goods.Where(d => d is ConsumableGood))
		{
			var a = _consumableGoodSpawner.GetChild<PanelContainer>(i);
			a.GetChild(0).GetChild(0).GetChild<Button>(2).Text = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods.ContainsKey(good.Id) ? "Disable" : "Enable";
			a.SelfModulate = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods.ContainsKey(good.Id) ? EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods[good.Id] ? new Color(0, 1, 0, 1) : new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);
			i++;
		}
		_totalModifiersFromConsumableGoodsLabel.Clear().NewLine().Header("Total modifiers from consumption:").AppendModifiers(EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoodsModifiers);
	}

	private void _changeConsumableGoodStatus(int goodId)
	{
		InvokeGUIEvent(new GUIChangeConsumableGoodStatus(goodId));
	}
	
	
	#endregion

	#region Technology

	private TabContainer _technologyTreeContainer;
	
	private PackedScene _technologyTreeScene;
	private PackedScene _technologyLevelScene;
	private PackedScene _technologyScene;

	private void _technologyInit()
	{
		_technologyTreeScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUITechnologyTree.tscn");
		_technologyLevelScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUITechnologyLevel.tscn");
		_technologyScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUITechnology.tscn");

		_technologyTreeContainer =
			GetNode<TabContainer>("TabContainer/Technology/MarginContainer/TechnologyTreeSpawner");

		for (int X = 0; X < EngineState.MapInfo.Scenario.TechnologyTrees.Length;X++)
		{
			var technologyTree = EngineState.MapInfo.Scenario.TechnologyTrees[X];
			var technologyTreeInstance = _technologyTreeScene.Instantiate();
			technologyTreeInstance.Name = technologyTree.Name;

			var technologyLevelSpawner = technologyTreeInstance.GetChild(0).GetChild(0).GetChild<VBoxContainer>(0);
			for (int Y = 0; Y < technologyTree.TechnologyLevels.Count; Y++) 
			{
				var technologyLevel = technologyTree.TechnologyLevels[Y];
				var technologyLevelInstance = _technologyLevelScene.Instantiate();
				technologyLevelInstance.GetChild(0).GetChild(0).GetChild<Label>(0).Text =
					technologyLevel.Date.Year.ToString();
				var technologySpawner = technologyLevelInstance.GetChild(0).GetChild(0).GetChild<GridContainer>(1);
				for (int Z = 0; Z < technologyLevel.Technologies.Count; Z++)
				{
					var technology = technologyLevel.Technologies[Z];
					var technologyInstance = _technologyScene.Instantiate();
					var x = X;
					var y = Y;
					var z = Z;
					technologyInstance.GetChild<Button>(2).Pressed +=
						() => _pressedOnTechnologyButton(new Vector3I(x, y, z));
					technologyInstance.GetChild<Button>(2).MouseEntered +=
						() => _showInfoBoxTechnology(technology);
					technologyInstance.GetChild<Button>(2).MouseExited +=
						() => InvokeGUIEvent(new GUIHideInfoBoxEvent());
					technologyInstance.GetChild(0).GetChild(0).GetChild<Label>(0).Text = technology.TechnologyName;
					technologyInstance.GetChild<ProgressBar>(1).MaxValue = technology.ResearchTime;
					var animatedTextureRect = technologyInstance.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(1);
					animatedTextureRect.SpriteFrames = GlobalResources.TechnologySpriteFrames;
					animatedTextureRect.CurrentAnimation = $"{X}-{Y}";
					animatedTextureRect.SetFrame(Z);
					technologySpawner.AddChild(technologyInstance);
				}
				
				
				technologyLevelSpawner.AddChild(technologyLevelInstance);
			}
			
			_technologyTreeContainer.AddChild(technologyTreeInstance);
		}
	}

	private void _updateTechnologyWindow()
	{
		for (int x = 0; x < EngineState.MapInfo.Scenario.TechnologyTrees.Length; x++)
		{
			for (int y = 0; y < EngineState.MapInfo.Scenario.TechnologyTrees[x].TechnologyLevels.Count; y++)
			{
				for (int z = 0; z < EngineState.MapInfo.Scenario.TechnologyTrees[x].TechnologyLevels[y].Technologies.Count; z++)
				{
					var technology = _technologyTreeContainer.GetChild(x).GetChild(0).GetChild(0).GetChild(0)
						.GetChild(y).GetChild(0).GetChild(0).GetChild(1).GetChild<PanelContainer>(z);
					
					technology.GetChild<Button>(2).Disabled = !(((y == 0 || EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ResearchedTechnologies[x][y-1].Any(d=>d)) && !EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ResearchedTechnologies[x][y][z]) && EngineState.MapInfo.Scenario.TechnologyTrees[x].TechnologyLevels[y].Technologies[z].CheckIfMeetsRequirements(EngineState.PlayerCountryId));
					technology.SelfModulate = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ResearchedTechnologies[x][y][z] ? new Color(0, 0.8f, 0) : new Color(1, 1, 1);
					if (EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].CurrentlyResearching
						.ContainsKey(new Vector3I(x, y, z)))
					{
						technology.GetChild<ProgressBar>(1).Visible = true;
						technology.GetChild<ProgressBar>(1).Value = EngineState.MapInfo.Scenario
							.Countries[EngineState.PlayerCountryId].CurrentlyResearching[new Vector3I(x, y, z)];
						technology.GetChild<Button>(2).Disabled = true;
					}
					else
					{
						technology.GetChild<ProgressBar>(1).Visible = false;
					}
				}
			}
		}
	}

	private void _showInfoBoxTechnology(Technology technology)
	{
		InvokeGUIEvent(new GUIShowInfoBox());
		GUIInfoBox.Info.ShowTechnologyData(technology);
	}

	private void _pressedOnTechnologyButton(Vector3I technologyId)
	{
		InvokeGUIEvent(new GUIBeginResearch(technologyId));
	}
	
	

	#endregion

	#region Army
	
	private VBoxContainer _armyRegimentTemplateContainer;

	private PackedScene _armyRegimentTemplateScene;
	private ArmyRegimentTemplate _currentEditingArmyTemplate;
	private int _currentlySelectingArmyTemplateGoodId;
	private GridContainer _goodArmyTemplateSpawner;

	private GUIGoodEditPanel _guiGoodEditPanel;
	private LineEdit _lineEditNameArmyTemplate;
	private OptionButton _optionButtonTypeArmyTemplate;
	private PanelContainer _templateDesignerPanel;

	private void _armyInit()
	{
		_templateDesignerPanel =
			GetNode<PanelContainer>(
				"TabContainer/Army/MarginContainer/VBoxContainer/TabContainer/Templates/HBoxContainer/TemplateDesigner");
		_lineEditNameArmyTemplate =
			GetNode<LineEdit>(
				"TabContainer/Army/MarginContainer/VBoxContainer/TabContainer/Templates/HBoxContainer/TemplateDesigner/MarginContainer/VBoxContainer/HBoxContainer2/LineEdit");
		_optionButtonTypeArmyTemplate = GetNode<OptionButton>(
			"TabContainer/Army/MarginContainer/VBoxContainer/TabContainer/Templates/HBoxContainer/TemplateDesigner/MarginContainer/VBoxContainer/OptionButton");
		_goodArmyTemplateSpawner =
			GetNode<GridContainer>(
				"TabContainer/Army/MarginContainer/VBoxContainer/TabContainer/Templates/HBoxContainer/TemplateDesigner/MarginContainer/VBoxContainer/GoodContainer");

		for (int i = 0; i < 5; i++)
				_goodArmyTemplateSpawner.GetChild(i).GetChild(0).GetChild<AnimatedTextureRect>(0).SpriteFrames = GlobalResources.GoodSpriteFrames;


		_armyRegimentTemplateScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIRegimentTemplate.tscn");
		_armyRegimentTemplateContainer = GetNode<VBoxContainer>(
			"TabContainer/Army/MarginContainer/VBoxContainer/TabContainer/Templates/HBoxContainer/TemplatesList/VBoxContainer/ScrollContainer/ReadyTemplatesSpawner");

		_guiGoodEditPanel = GetNode<GUIGoodEditPanel>("GoodEditPanel");
		_guiGoodEditPanel.Init();

		for (var i = 0; i < 5; i++)
		{
			var b = i;
			_goodArmyTemplateSpawner.GetChild(b).GetChild(0).GetChild(0).GetChild<Button>(0).Pressed +=
				() => { _onChooseGoodPressed(b); };
		}
	}

	private void _onEditArmyTemplatePressed(int templateId)
	{
		var armyTemplate =
			EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates[templateId] as
				ArmyRegimentTemplate;
		
		_templateDesignerPanel.Visible = true;
		_currentEditingArmyTemplate = armyTemplate;
		_lineEditNameArmyTemplate.Text = armyTemplate.Name;

		switch (armyTemplate)
		{
			case ArmyInfantryRegimentTemplate:
			{
				_optionButtonTypeArmyTemplate.Selected = 0;

				break;
			}
			case ArmyCavalryRegimentTemplate:
			{
				_optionButtonTypeArmyTemplate.Selected = 1;

				break;
			}
			case ArmyArtilleryRegimentTemplate:
			{
				_optionButtonTypeArmyTemplate.Selected = 2;

				break;
			}
		}
		
		Type[] options = {typeof(ArmyInfantryRegimentTemplate), typeof(ArmyCavalryRegimentTemplate), typeof(ArmyArtilleryRegimentTemplate)};

		var properties = options[_optionButtonTypeArmyTemplate.Selected].GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

		for (int i = 0; i < 5; i++)
		{
			if (properties[i].GetValue(armyTemplate) == null)
				_goodArmyTemplateSpawner.GetChild(i).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
			else
			{
				_goodArmyTemplateSpawner.GetChild(i).GetChild(0).GetChild<AnimatedTextureRect>(0)
					.SetFrame((properties[i].GetValue(armyTemplate) as Good)?.Id ?? -1);
			}
			
			_goodArmyTemplateSpawner.GetChild(i).GetChild<Label>(1).Text = properties[i].Name;
		}
	}

	private void _onGoodEditPanelGoodChangePressed(int goodId)
	{
		_goodArmyTemplateSpawner.GetChild(_currentlySelectingArmyTemplateGoodId).GetChild(0)
			.GetChild<AnimatedTextureRect>(0).SetFrame(goodId);
		_guiGoodEditPanel.Visible = false;
	}

	private void _onCloseMenuPressed()
	{
		Visible = false;
	}

	private void _onCreateNewArmyTemplatePressed()
	{
		_templateDesignerPanel.Visible = true;
		_cleanTemplateDesignInfo();
	}

	private void _cleanTemplateDesignInfo()
	{
		_lineEditNameArmyTemplate.Text = "";
		_optionButtonTypeArmyTemplate.Selected = 0;
		for (var i = 0; i < 5; i++)
		{
			_goodArmyTemplateSpawner.GetChild(i).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		}
	}

	private void _onOptionArmyTemplateButtonItemSelected(int index)
	{
		for (var i = 0; i < 5; i++)
		{
			_goodArmyTemplateSpawner.GetChild(i).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		}

		Type[] options = {typeof(ArmyInfantryRegimentTemplate), typeof(ArmyCavalryRegimentTemplate), typeof(ArmyArtilleryRegimentTemplate)};
		
		var properties = options[index].GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
		for (var i = 0; i < 5; i++)
		{
			_goodArmyTemplateSpawner.GetChild(i).GetChild<Label>(1).Text = properties[i].Name;
		}
	}

	private void _onSaveArmyTemplatePressed()
	{
		Type[] options = {typeof(ArmyInfantryRegimentTemplate), typeof(ArmyCavalryRegimentTemplate), typeof(ArmyArtilleryRegimentTemplate)};
		
		var properties = options[_optionButtonTypeArmyTemplate.Selected].GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			
		var indexes = new int[properties.Length];
		for (var i = 0; i < indexes.Length; i++)
		{
			indexes[i] = _goodArmyTemplateSpawner.GetChild(i).GetChild(0).GetChild<AnimatedTextureRect>(0).FrameIndex;
		}

		if (_currentEditingArmyTemplate != null &&
			_currentEditingArmyTemplate.GetType() == options[_optionButtonTypeArmyTemplate.Selected])
		{
			_currentEditingArmyTemplate.Name = _lineEditNameArmyTemplate.Text;
			
			for (var i = 0; i < properties.Length; i++)
			{
				properties[i].SetValue(
					_currentEditingArmyTemplate,
					indexes[i] == -1 ? null : EngineState.MapInfo.Scenario.Goods[indexes[i]]
					);
			}
		}
		else
		{
			var constructorArguments = new object[3 + properties.Length];
			constructorArguments[0] = _lineEditNameArmyTemplate.Text;
			constructorArguments[1] = _currentEditingArmyTemplate?.Id ?? EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count;
			constructorArguments[2] = EngineState.PlayerCountryId;
			for (int i = 0; i < 5; i++)
			{
				constructorArguments[3 + i] =
					indexes[i] == -1 ? null : EngineState.MapInfo.Scenario.Goods[indexes[i]];
			}

			var newArmyRegimentTemplate = options[_optionButtonTypeArmyTemplate.Selected].GetConstructor(
				new[] { typeof(string), typeof(int), typeof(int), properties[0].PropertyType, properties[1].PropertyType, properties[2].PropertyType,
					properties[3].PropertyType, properties[4].PropertyType }).Invoke(constructorArguments);

			if (_currentEditingArmyTemplate != null) EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates[_currentEditingArmyTemplate.Id] = newArmyRegimentTemplate as ArmyRegimentTemplate;
			else EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(newArmyRegimentTemplate  as Template);
		}

		_templateDesignerPanel.Visible = false;
		_currentEditingArmyTemplate = null;
		_showCountryData();
	}

	private void _onDeleteArmyTemplateButtonPressed()
	{
		if (_currentEditingArmyTemplate == null)
			return;
		EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates
			.Remove(_currentEditingArmyTemplate);
		_currentEditingArmyTemplate = null;
		_templateDesignerPanel.Visible = false;
		_showCountryData();
	}

	private void _onCancelArmyTemplateEditionButtonPressed()
	{
		_templateDesignerPanel.Visible = false;
	}

	private void _onChooseGoodPressed(int buttonId)
	{
		_currentlySelectingArmyTemplateGoodId = buttonId;
		
		Type[] options = {typeof(ArmyInfantryRegimentTemplate), typeof(ArmyCavalryRegimentTemplate), typeof(ArmyArtilleryRegimentTemplate)};
		
		var properties = options[_optionButtonTypeArmyTemplate.Selected].GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
		_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => properties[buttonId].PropertyType.IsInstanceOfType(d)).ToArray());
	}

	private void _showGoodEditBox(Good[] goods)
	{
		_guiGoodEditPanel.ChangeGoods(goods);
		_guiGoodEditPanel.Visible = true;
	}

	#endregion
}
