using System;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.Technology;

namespace EuropeDominationDemo.Scripts.Utils;


[Obsolete("Use AdvancedLabel instead")]
public class RichTextLabelFactory
{
    public static RichTextLabelBuilder ProvinceDataInfoBox(ProvinceData provinceData)
	{
		var infoBoxBuilder = new RichTextLabelBuilder().Header(provinceData.Name).NewLine();
		if (provinceData is LandProvinceData landProvinceData)
		{
			infoBoxBuilder.AppendText("Good: " + EngineState.MapInfo.Scenario.Goods[landProvinceData.Good].Name).NewLine();
			infoBoxBuilder.AppendText("Terrain: " + EngineState.MapInfo.Scenario.Terrains[landProvinceData.Terrain].Name).NewLine();
		}

		if (provinceData is LandColonizedProvinceData landprovinceData)
		{
			infoBoxBuilder.AppendText("Development: " + landprovinceData.Development).NewLine();
			infoBoxBuilder.ShowNonZeroGoods(landprovinceData.Resources);
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
		var req = EngineState.MapInfo.Scenario.Settings.ResourceAndCostRequirmentsToDev(provinceData.Development);
		return new RichTextLabelBuilder()
			.Header("Needed To Dev").NewLine()
			.AppendText($"Cost: {req.Key}").NewLine()
			.ShowNonZeroGoods(req.Value);
	}

	public static RichTextLabelBuilder BuildingData(Building building)
	{
		return new RichTextLabelBuilder()
			.Header(building.Name).NewLine()
			.AppendText($"Cost: {building.Cost}").NewLine()
			.AppendText("Resource Cost: ").ShowNonZeroGoods(building.ResourceCost)
			.Header("Modifiers:")
			.ShowModifiers(building.Modifiers);
	}

	public static RichTextLabelBuilder TechnologyData(Technology technology)
	{
		var technologyData = new RichTextLabelBuilder()
			.Header(technology.TechnologyName).NewLine()
			.AppendText($"Cost: {technology.InitialCost}").NewLine()
			.AppendText($"Research time: {technology.ResearchTime}").NewLine();
		
		var nonZeroGoods = new RichTextLabelBuilder().ShowNonZeroGoods(technology.ResourcesRequired);
		if (!string.IsNullOrEmpty(nonZeroGoods.ToString()))
			technologyData.AppendText("Resource Cost: ").Append(nonZeroGoods);
		
		if (technology.BuildingToUnlock > -1)
			technologyData.AppendText($"Building to unlock: ").ShowBuilding(technology.BuildingToUnlock).NewLine();
		
		if (technology.RecipyToUnlock > -1)
			technologyData.AppendText("Recipy To Unlock: ")
				.ShowGood(EngineState.MapInfo.Scenario.Recipes[technology.RecipyToUnlock].Output).NewLine();
		
		var nonDefaultModifiers = new RichTextLabelBuilder().ShowModifiers(technology.Modifiers);
		if (!string.IsNullOrEmpty(nonDefaultModifiers.ToString()))
			technologyData.Header("Modifiers:").Append(nonDefaultModifiers);
		
		return technologyData;
	}
}