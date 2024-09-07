using Godot;
using System;
using System.Linq;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Utils;

namespace EuropeDominationDemo.Scripts.UI.GUIPrefabs;
public partial class GUIMilitaryTrainingCamp : Control
{
	private VBoxContainer _templateContainer;
	private VBoxContainer _queueContainer;
	private GridContainer _goodContainer;
	private Label _selectedUnitNameLabel;
	private Label _trainingTimeLabel;
	private ArmyRegimentTemplate _currentSelectedTemplate = null;
	private MilitaryTrainingCamp _militaryTrainingCamp;
	
	private PackedScene _queueScene;
	private PackedScene _templateScene;
	public void Init()
	{
		_templateContainer =
			GetNode<VBoxContainer>(
				"PanelContainer/MarginContainer/HBoxContainer/VBoxContainer2/PanelContainer2/ScrollContainer/TemplateContainer");
		_queueContainer =
			GetNode<VBoxContainer>(
				"PanelContainer/MarginContainer/HBoxContainer/VBoxContainer2/PanelContainer/ScrollContainer/QueueContainer");
		_goodContainer =
			GetNode<GridContainer>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/GoodContainer");
		_selectedUnitNameLabel =
			GetNode<Label>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer2/SelectedNameLabel");
		_trainingTimeLabel = GetNode<Label>("PanelContainer/MarginContainer/HBoxContainer/VBoxContainer/TrainingTime");


		_queueScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUIQueueRegiment.tscn");
		_templateScene = GD.Load<PackedScene>("res://Prefabs/GUI/Modules/GUISelectTemplate.tscn");
	}

	public void ShowInfo(MilitaryTrainingCamp militaryTrainingCamp)
	{
		_clearQueueInfo();
		_militaryTrainingCamp = militaryTrainingCamp;

		foreach (var armyRegiment in militaryTrainingCamp.TrainingList)
		{
			var a = _queueScene.Instantiate();
			a.GetChild<Label>(0).Text = armyRegiment.Name;
			a.GetChild<ProgressBar>(1).Value = armyRegiment.TimeFromStartOfTheTraining;
			a.GetChild<ProgressBar>(1).MaxValue = armyRegiment.TrainingTime;
			_queueContainer.AddChild(a);
		}
	}

	private void _clearInfo()
	{
		_selectedUnitNameLabel.Text = "Empty";
		_currentSelectedTemplate = null;
		_trainingTimeLabel.Text = "Time: 0";
		_goodContainer.GetChild(0).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		_goodContainer.GetChild(1).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		_goodContainer.GetChild(2).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		_goodContainer.GetChild(3).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
		_goodContainer.GetChild(4).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetEmptyFrame();
	}

	private void _clearQueueInfo()
	{
		
		foreach (var child in _queueContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	private void _clearTemplateInfo()
	{
		foreach (var child in _templateContainer.GetChildren())
		{
			child.QueueFree();
		}
	}

	public void UpdateTemplates()
	{
		_clearTemplateInfo();
		foreach (var template in EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates.Where(d=>d is ArmyRegimentTemplate))
		{
			var a = _templateScene.Instantiate();
			a.GetChild<Label>(0).Text = template.Name;
			a.GetChild<Button>(1).Pressed += () => _selectUnitTemplate(template.Id);
			_templateContainer.AddChild(a);
		}
	}

	private void _onTrainUnitPressed()
	{
		if(_currentSelectedTemplate != null)
			_militaryTrainingCamp.TrainingList.Enqueue(new ArmyRegiment(_currentSelectedTemplate.Name, 0, _currentSelectedTemplate.Id, 0, _currentSelectedTemplate.TrainingTime, false, 0, 0));
	}
	
	private void _selectUnitTemplate(int unitTemplateId)
	{
		_clearInfo();
		_currentSelectedTemplate = EngineState.MapInfo.Scenario.Countries[EngineState.PlayerCountryId].RegimentTemplates[unitTemplateId] as ArmyRegimentTemplate;
		_selectedUnitNameLabel.Text = _currentSelectedTemplate.Name;
		_trainingTimeLabel.Text = "Time: " + _currentSelectedTemplate.TrainingTime;
		switch (_currentSelectedTemplate)
		{
			case ArmyInfantryRegimentTemplate template:
			{
				_goodContainer.GetChild(0).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Weapon?.Id ?? -1);
				_goodContainer.GetChild(1).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Helmet?.Id ?? -1);
				_goodContainer.GetChild(2).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Armor?.Id ?? -1);
				_goodContainer.GetChild(3).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Boots?.Id ?? -1);
				_goodContainer.GetChild(4).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.AdditionalSlot?.Id ?? -1);
				return;
			}
			case ArmyCavalryRegimentTemplate template:
			{
				_goodContainer.GetChild(0).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Weapon?.Id ?? -1);
				_goodContainer.GetChild(1).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Horse?.Id ?? -1);
				_goodContainer.GetChild(2).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Helmet?.Id ?? -1);
				_goodContainer.GetChild(3).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Armor?.Id ?? -1);
				_goodContainer.GetChild(4).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.AdditionalSlot?.Id ?? -1);
				return;
			}
			case ArmyArtilleryRegimentTemplate template:
			{
				_goodContainer.GetChild(0).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Weapon?.Id ?? -1);
				_goodContainer.GetChild(1).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Boots?.Id ?? -1);
				_goodContainer.GetChild(2).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Armor?.Id ?? -1);
				_goodContainer.GetChild(3).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.Wheel?.Id ?? -1);
				_goodContainer.GetChild(4).GetChild(0).GetChild(0).GetChild<AnimatedTextureRect>(0).SetFrame(template.AdditionalSlot?.Id ?? -1);
				return;
			}
		}
	}
}
