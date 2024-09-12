using System.Collections.Generic;
using System.Reflection;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using EuropeDominationDemo.Scripts.UI.Events.ToGUI;
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
				_infoLabel.Text = e.InfoBoxBuilder.Text;
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
	public static InfoBoxBuilder ProvinceDataInfoBox(ProvinceData provinceData)
	{
		var infoBoxBuilder = new InfoBoxBuilder().Header(provinceData.Name).NewLine();
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

	public static InfoBoxBuilder BattleRegimentData(ArmyRegiment armyRegiment)
	{
		var infoBoxBuilder = new InfoBoxBuilder();
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

	public static InfoBoxBuilder DevButtonData(LandColonizedProvinceData provinceData)
	{
		var req = Settings.ResourceAndCostRequirmentsToDev(provinceData.Development);
		return new InfoBoxBuilder()
			.Header("Needed To Dev").NewLine()
			.AppendText($"Cost: {req.Key}").NewLine()
			.ShowNotZeroGoods(req.Value);
	}

	public static InfoBoxBuilder BuildingData(Building building)
	{
		return new InfoBoxBuilder()
			.Header(building.Name).NewLine()
			.AppendText($"Cost: {building.Cost}").NewLine()
			.AppendText("Resource Cost: ").ShowNotZeroGoods(building.ResourceCost)
			.Header("Modifiers:")
			.ShowModifiers(building.Modifiers);
	}

	public static InfoBoxBuilder TechologyData(Technology technology)
	{
		var a = new InfoBoxBuilder()
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

public class InfoBoxBuilder
{
	public string Text { get; private set; } = "";

	public InfoBoxBuilder AppendText(string text)
	{
		Text += text;
		return this;
	}

	public InfoBoxBuilder Header(string text)
	{
		Text += $"[b][color=yellow]{text}[/color][/b]";
		return this;
	}

	public InfoBoxBuilder NewLine()
	{
		Text += "\n";
		return this;
	}

	public InfoBoxBuilder StartBold()
	{
		Text += "[b]";
		return this;
	}

	public InfoBoxBuilder EndBold()
	{
		Text += "[/b]";
		return this;
	}

	public InfoBoxBuilder StartColor(string color)
	{
		Text += $"[color={color}]";
		return this;
	}

	public InfoBoxBuilder EndColor()
	{
		Text += "[/color]";
		return this;
	}

	public InfoBoxBuilder ShowNotZeroGoods(double[] resources)
	{
		var goodIndexes = new List<int>();
		for (var i = 0; i < resources.Length; i++)
			if (resources[i] > 0)
				goodIndexes.Add(i);

		foreach (var index in goodIndexes)
			Text +=
				$"[img=30px, center]{GlobalResources.GoodSpriteFrames.GetFrameTexture("goods", index).ResourcePath}[/img]: {resources[index]}";

		NewLine();
		return this;
	}

	public InfoBoxBuilder ShowGood(int id)
	{
		Text +=
			$"[img=30px, center]{GlobalResources.GoodSpriteFrames.GetFrameTexture("goods", id).ResourcePath}[/img]";
		return this;
	}

	public InfoBoxBuilder ShowBuilding(int id)
	{
		Text +=
			$"[img=30px, center]{GlobalResources.BuildingSpriteFrames.GetFrameTexture("default", id).ResourcePath}[/img]";
		return this;
	}

	public InfoBoxBuilder ShowModifiers(Modifiers modifiers)
	{
		var defMod = Modifiers.DefaultModifiers();

		foreach (var propertyInfo in modifiers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			var val = propertyInfo.GetValue(modifiers);
			var defVal = propertyInfo.GetValue(defMod);
			if ((float)val - (float)defVal > EngineVariables.Eps)
			{
				NewLine();
				AppendText($"{propertyInfo.Name}: ");
				if (propertyInfo.Name.Contains("Bonus"))
				{
					if ((float)val >= 0)
						StartColor("green").AppendText($"+{val}").EndColor();
					else
						StartColor("red").AppendText($"-{val}").EndColor();
				}
				else
				{
					if ((float)val >= 1.0f)
						StartColor("green").AppendText($"+{Mathf.RoundToInt(100 * ((float)val - 1.0f))}%").EndColor();
					else
						StartColor("red").AppendText($"-{Mathf.RoundToInt(100 * ((float)val - 1.0f))}%").EndColor();
				}
			}
		}

		return this;
	}
}
