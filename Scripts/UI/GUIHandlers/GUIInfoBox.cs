using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
using EuropeDominationDemo.Scripts.Utils;
using Godot;

namespace EuropeDominationDemo.Scripts.UI.GUIHandlers;

public partial class GUIInfoBox : GUIHandler
{
	private PanelContainer _infoBox;
	private RichTextLabel _infoLabel;

	public override void Init()
	{
		_infoLabel = GetNode<RichTextLabel>("BoxContainer/MarginContainer/RichTextLabel");
		_infoBox = GetNode<PanelContainer>("BoxContainer");
		_infoLabel.BbcodeEnabled = true;
	}

	public override void _Process(double delta)
	{
		_infoBox.Position = GetViewport().GetMousePosition() + new Vector2(10, 10);
	}

	public override void InputHandle(InputEvent @event)
	{
	}

	public override void ToGUIHandleEvent(ToGUIEvent @event)
	{
		switch (@event)
		{
			case ToGUIShowInfoBoxEvent e:
				_infoLabel.Text = "";
				_infoBox.Size = new Vector2(10, 10);
				_infoLabel.Text = e.RichTextLabelBuilder.Text;
				Visible = true;
				return;
			case ToGUIHideInfoBox:
				_infoLabel.Text = "";
				_infoBox.Size = new Vector2(10, 10);
				Visible = false;
				return;
			default:
				return;
		}
	}
}

public static class InfoBoxFactory
{
	public static RichTextLabelBuilder ProvinceDataInfoBox(ProvinceData provinceData)
	{
		var infoBoxBuilder = new RichTextLabelBuilder().Header(provinceData.Name).NewLine();
		if (provinceData is LandProvinceData landProvinceData)
		{
			infoBoxBuilder.AppendText("Good: " + landProvinceData.Good.Name).NewLine();
			infoBoxBuilder.AppendText("Terrain: " + landProvinceData.Terrain.Name).NewLine();
		}

		if (provinceData is LandColonizedProvinceData landprovinceData)
		{
			infoBoxBuilder.AppendText("Developement: " + landprovinceData.Development).NewLine();
			infoBoxBuilder.ShowNotZeroGoods(landprovinceData.Resources);
		}

		if (EngineState.DebugMode)
		{
			infoBoxBuilder.Header("Debug Data").NewLine();
			infoBoxBuilder.AppendText("Id: " + provinceData.Id).NewLine();
			if (provinceData is LandColonizedProvinceData landColonizedProvinceData)
				infoBoxBuilder.AppendText("Owner ID: " + landColonizedProvinceData.Owner);
		}

		return infoBoxBuilder;
	}

	public static RichTextLabelBuilder BattleRegimentData(ArmyRegiment armyRegiment)
	{
		var infoBoxBuilder = new RichTextLabelBuilder();
		if (armyRegiment == null)
		{
			infoBoxBuilder.Header("Empty Space");
			return infoBoxBuilder;
		}

		infoBoxBuilder.Header(armyRegiment.Name).NewLine();

		switch (armyRegiment)
		{
			case ArmyInfantryRegiment:
				infoBoxBuilder.AppendText("Type: Army Infantry").NewLine();
				break;
			case ArmyCavalryRegiment:
				infoBoxBuilder.AppendText("Type: Army Cavalry").NewLine();
				break;
			case ArmyArtilleryRegiment:
				infoBoxBuilder.AppendText("Type: Army Artillery").NewLine();
				break;
		}

		//infoBoxBuilder.ShowNotZeroGoods(armyRegiment.Resources);
		infoBoxBuilder.AppendText($"Manpower: {armyRegiment.Manpower}").NewLine();
		infoBoxBuilder.AppendText($"Morale: {armyRegiment.Morale}");

		return infoBoxBuilder;
	}

	public static RichTextLabelBuilder DevButtonData(LandColonizedProvinceData provinceData)
	{
		var req = Settings.ResourceAndCostRequirmentsToDev(provinceData.Development);
		return new RichTextLabelBuilder()
			.Header("Needed To Dev").NewLine()
			.AppendText($"Cost: {req.Key}").NewLine()
			.ShowNotZeroGoods(req.Value);
	}

	public static RichTextLabelBuilder BuildingData(Building building)
	{
		return new RichTextLabelBuilder()
			.Header(building.Name).NewLine()
			.AppendText($"Cost: {building.Cost}").NewLine()
			.AppendText("Resource Cost: ").ShowNotZeroGoods(building.ResourceCost)
			.Header("Modifiers:")
			.ShowModifiers(building.Modifiers);
	}

	public static RichTextLabelBuilder TechologyData(Technology technology)
	{
		var a = new RichTextLabelBuilder()
			.Header(technology.TechnologyName).NewLine()
			.AppendText($"Cost: {technology.InitialCost}").NewLine()
			.AppendText($"Research time: {technology.ResearchTime}").NewLine();
		if (Good.IsDifferentFromNull(technology.ResourcesRequired))
			a.AppendText("Resource Cost: ").ShowNotZeroGoods(technology.ResourcesRequired);
		if (technology.BuildingToUnlock > -1)
			a.AppendText($"Building to unlock: ").ShowBuilding(technology.BuildingToUnlock).NewLine();
		if(technology.RecipyToUnlock > -1)
			a.AppendText("Recipy To Unlock: ").ShowGood(EngineState.MapInfo.Scenario.Recipes[technology.RecipyToUnlock].Output.Id).NewLine();
		if (Modifiers.IsDifferentFromDefault(technology.Modifiers))
			a.Header("Modifiers:").ShowModifiers(technology.Modifiers);
		return a;
	}
}
