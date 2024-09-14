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
	private RichTextLabel _totalModifiersFromConsumableGoodsLabel;
	
	private void _consumableGoodsInit()
	{
		_consumableGoodScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIConsumableGoodMenu.tscn");
		_consumableGoodSpawner =
			GetNode<VBoxContainer>(
				"TabContainer/Consumption/MarginContainer/VBoxContainer/ScrollContainer/ConsumableGoodsSpawner");
		_totalModifiersFromConsumableGoodsLabel =
			GetNode<RichTextLabel>(
				"TabContainer/Consumption/MarginContainer/VBoxContainer/PanelContainer/MarginContainer/TotalBonusesFromConsumableGoods");

		foreach (ConsumableGood good in EngineState.MapInfo.Scenario.Goods.Where(d=> d is ConsumableGood))
		{
			var a = _consumableGoodScene.Instantiate() as PanelContainer;
			var consumptionContainer = a.GetChild(0).GetChild(0);

			consumptionContainer.GetChild<AnimatedTextureRect>(0).SetFrame(good.Id);
			consumptionContainer.GetChild(1).GetChild<RichTextLabel>(0).Text = new RichTextLabelBuilder()
				.Header(good.Name).NewLine()
				.AppendText($"Consumption: {good.ConsumptionPerMonthToActivateBonus}t/m").NewLine()
				.Header("Modifiers:")
				.ShowModifiers(good.Modifiers).Text;
			consumptionContainer.GetChild<Button>(2).Text = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods.ContainsKey(good.Id) ? "Disable" : "Enable";
			consumptionContainer.GetChild<Button>(2).Pressed += () => _changeConsumableGoodStatus(good.Id);
			a.SelfModulate = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods.ContainsKey(good.Id) ? EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoods[good.Id] ? new Color(0, 1, 0, 1) : new Color(1, 0, 0, 1) : new Color(1, 1, 1, 1);
			_consumableGoodSpawner.AddChild(a);
		}
		_totalModifiersFromConsumableGoodsLabel.Text = new RichTextLabelBuilder().Header("Total modifiiers from consumption:").ShowModifiers(EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoodsModifiers).Text;
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
		_totalModifiersFromConsumableGoodsLabel.Text = new RichTextLabelBuilder().Header("Total modifiiers from consumption:").ShowModifiers(EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].ConsumableGoodsModifiers).Text;
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
					animatedTextureRect.CurrentAnimation = $"{X}:{Y}";
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
		InvokeGUIEvent(new GUIShowInfoBox(InfoBoxFactory.TechologyData(technology)));
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
		_showArmyTemplateToEdit(
			EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates[templateId] as
				ArmyRegimentTemplate);
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
		
		var properties = options[index].GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
		for (var i = 0; i < 5; i++)
		{
			_goodArmyTemplateSpawner.GetChild(i).GetChild<Label>(1).Text = properties[i].Name;
		}
	}

	private void _onSaveArmyTemplatePressed()
	{
		if (_currentEditingArmyTemplate != null)
		{
			_currentEditingArmyTemplate.Name = _lineEditNameArmyTemplate.Text;
			switch (_currentEditingArmyTemplate)
			{
				case ArmyInfantryRegimentTemplate template:
				{
					var weaponIndex = _goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var helmetIndex = _goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var armorIndex = _goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var bootsIndex = _goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0)
						.GetChild<AnimatedTextureRect>(0)
						.FrameIndex;

					template.Weapon = weaponIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon;
					template.Helmet = helmetIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet;
					template.Armor = armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor;
					template.Boots = bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots;
					template.Additional = additionalSlotIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood;

					break;
				}
				case ArmyCavalryRegimentTemplate template:
				{
					var weaponIndex = _goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var horseIndex = _goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var helmetIndex = _goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var armorIndex = _goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0)
						.GetChild<AnimatedTextureRect>(0)
						.FrameIndex;

					template.Weapon = weaponIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon;
					template.Horse = horseIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[horseIndex] as Horse;
					template.Helmet = helmetIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet;
					template.Armor = armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor;
					template.Additional = additionalSlotIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood;
					break;
				}
				case ArmyArtilleryRegimentTemplate template:
				{
					var weaponIndex = _goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var bootsIndex = _goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var armorIndex = _goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var wheelIndex = _goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0)
						.GetChild<AnimatedTextureRect>(0)
						.FrameIndex;


					template.Weapon = weaponIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[weaponIndex] as ArtilleryWeapon;
					template.Boots = bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots;
					template.Armor = armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor;
					template.Wheel = wheelIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[wheelIndex] as Wheel;
					template.Additional = additionalSlotIndex == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood;
					break;
				}
			}
			//need to call for template change in all units;
		}
		else
		{
			Type[] options = {typeof(ArmyInfantryRegimentTemplate), typeof(ArmyCavalryRegimentTemplate), typeof(ArmyArtilleryRegimentTemplate)};
		
			var properties = options[_optionButtonTypeArmyTemplate.Selected].GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			
			var indexes = new int[5];
			for (var i = 0; i < 5; i++)
			{
				indexes[i] = _goodArmyTemplateSpawner.GetChild(i).GetChild(0).GetChild<AnimatedTextureRect>(0).FrameIndex;
			}
			
			EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(
				 options[_optionButtonTypeArmyTemplate.Selected].GetConstructor(
					 new [] {typeof(string), typeof(int), properties[0].FieldType, properties[1].FieldType, properties[2].FieldType,
					 properties[3].FieldType, properties[4].FieldType}).Invoke(new object[] {
					_lineEditNameArmyTemplate.Text,
					EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count,
					indexes[0] == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[indexes[0]],
					indexes[1] == -1 ? null : EngineState.MapInfo.Scenario.Goods[indexes[1]],
					indexes[2] == -1 ? null : EngineState.MapInfo.Scenario.Goods[indexes[2]],
					indexes[3] == -1 ? null : EngineState.MapInfo.Scenario.Goods[indexes[3]],
					indexes[4] == -1
						? null
						: EngineState.MapInfo.Scenario.Goods[indexes[4]]
				 }) as Template);
			
			/*switch (_optionButtonTypeArmyTemplate.Selected)
			{
				case 0:
				{
					var weaponIndex = _goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var helmetIndex = _goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var armorIndex = _goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var bootsIndex = _goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0)
						.GetChild<AnimatedTextureRect>(0)
						.FrameIndex;

					EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(
						new ArmyInfantryRegimentTemplate(
							_lineEditNameArmyTemplate.Text,
							EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count,
							weaponIndex == -1
								? null
								: EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon,
							helmetIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet,
							armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor,
							bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots,
							additionalSlotIndex == -1
								? null
								: EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood
						));

					break;
				}
				case 1:
				{
					var weaponIndex = _goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var horseIndex = _goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var helmetIndex = _goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var armorIndex = _goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0)
						.GetChild<AnimatedTextureRect>(0)
						.FrameIndex;

					EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(
						new ArmyCavalryRegimentTemplate(
							_lineEditNameArmyTemplate.Text,
							EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count,
							weaponIndex == -1
								? null
								: EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon,
							horseIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[horseIndex] as Horse,
							helmetIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet,
							armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor,
							additionalSlotIndex == -1
								? null
								: EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood
						));

					break;
				}
				case 2:
				{
					var weaponIndex = _goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var bootsIndex = _goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var armorIndex = _goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var wheelIndex = _goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0)
						.GetChild<AnimatedTextureRect>(0)
						.FrameIndex;

					EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(
						new ArmyArtilleryRegimentTemplate(
							_lineEditNameArmyTemplate.Text,
							EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count,
							weaponIndex == -1
								? null
								: EngineState.MapInfo.Scenario.Goods[weaponIndex] as ArtilleryWeapon,
							bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots,
							armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor,
							wheelIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[wheelIndex] as Wheel,
							additionalSlotIndex == -1
								? null
								: EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood
						));
					break;
				}
			}*/
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
		
		var properties = options[_optionButtonTypeArmyTemplate.Selected].GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
		_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => properties[buttonId].FieldType.IsInstanceOfType(d)).ToArray());
	}

	private void _showGoodEditBox(Good[] goods)
	{
		_guiGoodEditPanel.ChangeGoods(goods);
		_guiGoodEditPanel.Visible = true;
	}

	private void _showArmyTemplateToEdit(ArmyRegimentTemplate armyTemplate)
	{
		_templateDesignerPanel.Visible = true;
		_currentEditingArmyTemplate = armyTemplate;
		_lineEditNameArmyTemplate.Text = armyTemplate.Name;
		switch (armyTemplate)
		{
			case ArmyInfantryRegimentTemplate armyInfantry:
			{
				_optionButtonTypeArmyTemplate.Selected = 0;

				if (armyInfantry.Weapon == null)
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyInfantry.Weapon.Id);

				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";


				if (armyInfantry.Helmet == null)
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyInfantry.Helmet.Id);

				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text = "Helmet";

				if (armyInfantry.Armor == null)
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyInfantry.Armor.Id);

				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Armor";

				if (armyInfantry.Boots == null)
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyInfantry.Boots.Id);

				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Boots";

				if (armyInfantry.Additional == null)
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyInfantry.Additional.Id);

				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";

				return;
			}
			case ArmyCavalryRegimentTemplate armyCavallary:
			{
				_optionButtonTypeArmyTemplate.Selected = 1;

				if (armyCavallary.Weapon == null)
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyCavallary.Weapon.Id);

				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";


				if (armyCavallary.Horse == null)
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyCavallary.Horse.Id);

				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text = "Horse";

				if (armyCavallary.Helmet == null)
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyCavallary.Helmet.Id);

				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Helmet";

				if (armyCavallary.Armor == null)
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyCavallary.Armor.Id);

				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Armor";

				if (armyCavallary.Additional == null)
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyCavallary.Additional.Id);

				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";


				return;
			}
			case ArmyArtilleryRegimentTemplate armyArtillery:
			{
				_optionButtonTypeArmyTemplate.Selected = 2;

				if (armyArtillery.Weapon == null)
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyArtillery.Weapon.Id);

				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";


				if (armyArtillery.Boots == null)
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyArtillery.Boots.Id);

				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text = "Boots";

				if (armyArtillery.Armor == null)
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyArtillery.Armor.Id);

				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Armor";

				if (armyArtillery.Wheel == null)
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyArtillery.Wheel.Id);

				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Wheel";

				if (armyArtillery.Additional == null)
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.SetFrame(armyArtillery.Additional.Id);

				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";

				return;
			}
		}
	}

	#endregion

	
}
