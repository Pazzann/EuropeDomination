using Godot;
using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.UI.GUIHandlers;
using EuropeDominationDemo.Scripts.UI.GUIPrefabs;
using EuropeDominationDemo.Scripts.Utils;

public partial class GUICountryWindow : GUIHandler
{
	private PanelContainer _templateDesignerPanel;
	private ArmyRegimentTemplate _currentEditingArmyTemplate = null;
	private LineEdit _lineEditNameArmyTemplate;
	private OptionButton _optionButtonTypeArmyTemplate;
	private GridContainer _goodArmyTemplateSpawner;
	
	private PackedScene _armyRegimentTemplateScene;
	private VBoxContainer _armyRegimentTemplateContainer;

	private GUIGoodEditPanel _guiGoodEditPanel;
	private int _currentlySelectingArmyTemplateGoodId;
	
	
	public override void Init()
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

		for (int i = 0; i < 5; i++)
		{
			int b = i;
			_goodArmyTemplateSpawner.GetChild(b).GetChild(0).GetChild(0).GetChild<Button>(0).Pressed += (() => { _onChooseGoodPressed(b);});
		}

	}
	
	public override void InputHandle(InputEvent @event)
	{
		
	}


	private void _showCountryData()
	{
		foreach (var child in _armyRegimentTemplateContainer.GetChildren())
		{
			child.QueueFree();
		}

		foreach (var template in EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Where(a => a is ArmyRegimentTemplate))
		{
			var a = _armyRegimentTemplateScene.Instantiate();
			a.GetChild(0).GetChild<Label>(0).Text = template.Name;
			a.GetChild(0).GetChild<Button>(1).Pressed += () => _onEditArmyTemplatePressed(template.Id);
			_armyRegimentTemplateContainer.AddChild(a);
		}
		
	}
	
	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowCountryWindowEvent:
				_showCountryData();
				Visible = true;
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

	private void _onEditArmyTemplatePressed(int templateId)
	{
		_showArmyTemplateToEdit(EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates[templateId] as ArmyRegimentTemplate);
	}

	private void _onGoodEditPanelGoodChangePressed(int goodId)
	{
		_goodArmyTemplateSpawner.GetChild(_currentlySelectingArmyTemplateGoodId).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(goodId);
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
		for (int i = 0; i < 5; i++)
		{
			int b = i;
			_goodArmyTemplateSpawner.GetChild(b).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		}
	}

	private void _onOptionArmyTemplateButtonItemSelected(int index)
	{
		for (int i = 0; i < 5; i++)
		{
			int b = i;
			_goodArmyTemplateSpawner.GetChild(b).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		}
		switch (index)
		{
			case 0:
			{
				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";
				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text= "Helmet";
				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Armor";
				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Boots";
				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";
				
				return;
			}
			case 1:
			{
				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";
				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text= "Horse";
				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Helmet";
				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Armor";
				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";
					
				return;
			}
			case 2:
			{
				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";
				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text= "Boots";
				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Armor";
				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Wheel";
				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";
				
				return;
			}
				
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
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;

					template.Weapon = weaponIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon;
					template.Helmet = helmetIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet;
					template.Armor = armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor;
					template.Boots = bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots;
					template.AdditionalSlot = additionalSlotIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood;
					
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
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					
					template.Weapon = weaponIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon;
					template.Horse = horseIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[horseIndex] as Horse;
					template.Helmet = helmetIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet;
					template.Armor = armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor;
					template.AdditionalSlot = additionalSlotIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood;
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
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					
					
					template.Weapon = weaponIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[weaponIndex] as ArtilleryWeapon;
					template.Boots = bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots;
					template.Armor = armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor;
					template.Wheel = wheelIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[wheelIndex] as Wheel;
					template.AdditionalSlot = additionalSlotIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood;
					break;
				}
			}
			//need to call for template change in all units;
		}
		else
		{
			switch (_optionButtonTypeArmyTemplate.Selected)
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
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					
					EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(new ArmyInfantryRegimentTemplate(
						_lineEditNameArmyTemplate.Text,
						EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count,
						weaponIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon,
						helmetIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet,
						armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor,
						bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots,
						additionalSlotIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood
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
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					
					EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(new ArmyCavalryRegimentTemplate(
						_lineEditNameArmyTemplate.Text,
						EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count,
						weaponIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[weaponIndex] as InfantryWeapon,
						horseIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[horseIndex] as Horse,
						helmetIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[helmetIndex] as Helmet,
						armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor,
						additionalSlotIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood
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
					var additionalSlotIndex = _goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0)
						.FrameIndex;
					
					EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Add(new ArmyArtilleryRegimentTemplate(
						_lineEditNameArmyTemplate.Text,
						EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Count,
						weaponIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[weaponIndex] as ArtilleryWeapon,
						bootsIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[bootsIndex] as Boots,
						armorIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[armorIndex] as Armor,
						wheelIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[wheelIndex] as Wheel,
						additionalSlotIndex == -1 ? null : EngineState.MapInfo.Scenario.Goods[additionalSlotIndex] as AdditionalSlotGood
					));
					break;
				}
			}
			
		}
		
		_templateDesignerPanel.Visible = false;
		_currentEditingArmyTemplate = null;
		_showCountryData();
	}

	private void _onDeleteArmyTemplateButtonPressed()
	{
		if(_currentEditingArmyTemplate == null)
			return;
		EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Remove(_currentEditingArmyTemplate);
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
		switch (_optionButtonTypeArmyTemplate.Selected)
		{
			case 0:
			{
				switch (buttonId)
				{
					case 0:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is InfantryWeapon).ToArray());
						return;
					case 1:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Helmet).ToArray());
						return;
					case 2:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Armor).ToArray());
						return;
					case 3:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Boots).ToArray());
						return;
					case 4:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is AdditionalSlotGood).ToArray());
						return;
				}
				return;
			}
			case 1:
			{
				switch (buttonId)
				{
					case 0:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is InfantryWeapon).ToArray());
						return;
					case 1:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Horse).ToArray());
						return;
					case 2:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Helmet).ToArray());
						return;
					case 3:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Armor).ToArray());
						return;
					case 4:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is AdditionalSlotGood).ToArray());
						return;
				}
				return;
			}
			case 2:
			{
				switch (buttonId)
				{
					case 0:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is ArtilleryWeapon).ToArray());
						return;
					case 1:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Boots).ToArray());
						return;
					case 2:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Armor).ToArray());
						return;
					case 3:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is Wheel).ToArray());
						return;
					case 4:
						_showGoodEditBox(EngineState.MapInfo.Scenario.Goods.Where(d => d is AdditionalSlotGood).ToArray());
						return;
				}
				
				return;
			}
		}
	}
	
	private void _showGoodEditBox(Good[] goods)
	{
		_guiGoodEditPanel.ChangeGoods(goods);
		_guiGoodEditPanel.Visible = true;
	}
	private void _showArmyTemplateToEdit(ArmyRegimentTemplate armyTemplate)
	{
		_templateDesignerPanel.Visible = true;
		_currentEditingArmyTemplate	= armyTemplate;
		_lineEditNameArmyTemplate.Text = armyTemplate.Name;
		switch (armyTemplate)
		{
			case ArmyInfantryRegimentTemplate armyInfantry:
			{
				_optionButtonTypeArmyTemplate.Selected = 0;
				
				if(armyInfantry.Weapon == null)
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyInfantry.Weapon.Id);
				
				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";
				
				
				if(armyInfantry.Helmet == null)
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyInfantry.Helmet.Id);
				
				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text= "Helmet";
				
				if(armyInfantry.Armor == null)
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyInfantry.Armor.Id);
				
				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Armor";
				
				if(armyInfantry.Boots == null)
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyInfantry.Boots.Id);
				
				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Boots";
				
				if(armyInfantry.AdditionalSlot == null)
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyInfantry.AdditionalSlot.Id);
				
				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";
				
				return;
			}
			case ArmyCavalryRegimentTemplate armyCavallary:
			{
				_optionButtonTypeArmyTemplate.Selected = 1;
				
				if(armyCavallary.Weapon == null)
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyCavallary.Weapon.Id);
				
				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";
				
				
				if(armyCavallary.Horse == null)
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyCavallary.Horse.Id);
				
				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text= "Horse";
				
				if(armyCavallary.Helmet == null)
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyCavallary.Helmet.Id);
				
				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Helmet";
				
				if(armyCavallary.Armor == null)
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyCavallary.Armor.Id);
				
				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Armor";
				
				if(armyCavallary.AdditionalSlot == null)
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyCavallary.AdditionalSlot.Id);
				
				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";
				
					
				return;
			}
			case ArmyArtilleryRegimentTemplate armyArtillery:
			{
				_optionButtonTypeArmyTemplate.Selected = 2;
				
				if(armyArtillery.Weapon == null)
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyArtillery.Weapon.Id);
				
				_goodArmyTemplateSpawner.GetChild(0).GetChild<Label>(1).Text = "Weapon";
				
				
				if(armyArtillery.Boots == null)
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(1).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyArtillery.Boots.Id);
				
				_goodArmyTemplateSpawner.GetChild(1).GetChild<Label>(1).Text= "Boots";
				
				if(armyArtillery.Armor == null)
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(2).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyArtillery.Armor.Id);
				
				_goodArmyTemplateSpawner.GetChild(2).GetChild<Label>(1).Text = "Armor";
				
				if(armyArtillery.Wheel == null)
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(3).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyArtillery.Wheel.Id);
				
				_goodArmyTemplateSpawner.GetChild(3).GetChild<Label>(1).Text = "Wheel";
				
				if(armyArtillery.AdditionalSlot == null)
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
				else
					_goodArmyTemplateSpawner.GetChild(4).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(armyArtillery.AdditionalSlot.Id);
				
				_goodArmyTemplateSpawner.GetChild(4).GetChild<Label>(1).Text = "Additional";
				
				return;
			}
				
		}
	}
}
