using System;
using System.Linq;
using System.Reflection;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments.Land;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

/// <summary>
/// Represents an advanced label component within a vertical box container.
/// </summary>
public partial class AdvancedLabel : VBoxContainer
{
	private PackedScene _labelPrefab;
	private PackedScene _textureRectPrefab;
	private PackedScene _hBoxPrefab;
	private Font _font;
	
	private HBoxContainer _lastReference;
	
	public override void _Ready()
	{
		_labelPrefab = GD.Load<PackedScene>("res://Prefabs/CustomNodes/AdvancedLabel/Label.tscn");
		_textureRectPrefab = GD.Load<PackedScene>("res://Prefabs/CustomNodes/AdvancedLabel/TextureRect.tscn");
		_hBoxPrefab = GD.Load<PackedScene>("res://Prefabs/CustomNodes/AdvancedLabel/LineContainer.tscn");
		
		_font = GD.Load<Font>("res://Fonts/VeniceClassic.ttf");
		
	}

	#region Builder
	/// <summary>
	/// Clears the label.
	/// </summary>
	/// <returns></returns>
	public AdvancedLabel Clear()
	{
		foreach (var child in GetChildren())
			child.Free();
		_lastReference = null;
		return this;
	} 
	/// <summary>
	/// Creates a new line.
	/// </summary>
	/// <returns></returns>
	public AdvancedLabel NewLine()
	{
		_lastReference = _hBoxPrefab.Instantiate() as HBoxContainer;
		AddChild(_lastReference);
		return this;
	}
	/// <summary>
	/// Validates the label.
	/// </summary>
	/// <exception cref="Exception"></exception>
	public void _validate()
	{
		if(_lastReference == null)
			throw new Exception("You must call NewLine() before adding elements");
	}
	
	/// <summary>
	/// Adds a header to the label.
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public AdvancedLabel Header(string text)
	{
		_validate();

		var a = _labelPrefab.Instantiate() as Label;
		a.LabelSettings = new LabelSettings();
		a.LabelSettings.Font = _font;
		a.LabelSettings.FontSize = 25;
		a.LabelSettings.OutlineColor = new Color(1, 1, 0);
		a.LabelSettings.OutlineSize = 1;
		a.Text = text;
		_lastReference.AddChild(a);
		
		return this;
	}
	/// <summary>
	/// Appends a text to the label.
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public AdvancedLabel Append(string text)
	{
		_validate();
		var a = _labelPrefab.Instantiate() as Label;
		a.Text = text;
		a.LabelSettings = new LabelSettings();
		a.LabelSettings.Font = _font;
		a.LabelSettings.FontSize = 25;
		_lastReference.AddChild(a);
		return this;
	}
	/// <summary>
	/// Appends a colored text to the label.
	/// </summary>
	/// <param name="text"></param>
	/// <param name="color"></param>
	/// <returns></returns>
	public AdvancedLabel AppendColored(string text, Color color){
		_validate();
		
		var a = _labelPrefab.Instantiate() as Label;
		a.Text = text;
		a.LabelSettings = new LabelSettings();
		a.LabelSettings.Font = _font;
		a.LabelSettings.FontSize = 25;
		a.LabelSettings.FontColor = color;
		_lastReference.AddChild(a);
		
		return this;
	}
	/// <summary>
	/// Appends the non-zero goods.
	/// </summary>
	/// <param name="goods"></param>
	/// <returns></returns>
	public AdvancedLabel AppendNonZeroGoods(double[] goods)
	{
		_validate();
		for (var id = 0; id < goods.Length; id++)
			if (goods[id] > 0)
				ShowGood(id).Append($": {goods[id]:F}");

		NewLine();
		
		return this;
	}
	/// <summary>
	/// Shows the building.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public AdvancedLabel ShowBuilding(int id)
	{
		_validate();
		
		var a = _textureRectPrefab.Instantiate() as TextureRect;
		
		a.Texture = GlobalResources.BuildingSpriteFrames.GetFrameTexture("default", id);
		a.Size = new Vector2(32, 32);
		_lastReference.AddChild(a);
		
		return this;
	}
	/// <summary>
	/// Shows the good.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public AdvancedLabel ShowGood(int id)
	{
		_validate();
		
		var a = _textureRectPrefab.Instantiate() as TextureRect;
		a.Texture = GlobalResources.GoodSpriteFrames.GetFrameTexture("goods", id);
		a.Size = new Vector2(32, 32);
		_lastReference.AddChild(a);
		
		return this;
	}
	
	
	/// <summary>
	/// Appends the modifiers.
	/// </summary>
	/// <param name="modifiers"></param>
	/// <returns></returns>
	public AdvancedLabel AppendModifiers(Modifiers modifiers)
	{
		_validate();
		
		var defMod = Modifiers.DefaultModifiers();
		
		foreach (var propertyInfo in modifiers.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
		{
			var val = propertyInfo.GetValue(modifiers);
			var defVal = propertyInfo.GetValue(defMod);



			var hasNegativeMeaning = (bool)propertyInfo.CustomAttributes.Where(d => d.AttributeType == typeof(HasNegativeMeaning)).ToArray()[0]
				.ConstructorArguments[0].Value!;
			
			if ((float)val! - (float)defVal! > EngineVariables.Eps)
			{
				NewLine();
				Append($"{propertyInfo.Name}: ");
				if (propertyInfo.Name.Contains("Bonus"))
				{
					if ((float)val >= 0)
						AppendColored($"+{val}", !hasNegativeMeaning ? new Color(0, 1, 0) : new Color(1, 0, 0));
					else
						AppendColored($"-{val}", hasNegativeMeaning ? new Color(0, 1, 0) : new Color(1, 0, 0));
				}
				else
				{
					if ((float)val >= 1.0f)
						AppendColored($"+{Mathf.RoundToInt(100 * ((float)val - 1.0f))}%", !hasNegativeMeaning ? new Color(0, 1, 0) : new Color(1, 0, 0));
					else
						AppendColored($"-{Mathf.RoundToInt(100 * ((float)val - 1.0f))}%", hasNegativeMeaning ? new Color(0, 1, 0) : new Color(1, 0, 0));
				}
			}
		}
		
		return this;
	}
	
	
	
	#endregion

	#region Factory

	
	/// <summary>
	/// Shows the province data info box.
	/// </summary>
	/// <param name="provinceData"></param>
	public void ShowProvinceDataInfoBox(ProvinceData provinceData)
	{
		Clear().NewLine();
		
		Header(provinceData.Name).NewLine();
		if (provinceData is LandProvinceData landProvinceData)
		{
			Append("Good: " + EngineState.MapInfo.Scenario.Goods[landProvinceData.Good].Name).NewLine();
			Append("Terrain: " + EngineState.MapInfo.Scenario.Terrains[landProvinceData.Terrain].Name).NewLine();
		}

		if (provinceData is LandColonizedProvinceData landprovinceData)
		{
			Append("Development: " + landprovinceData.Development).NewLine();
			AppendNonZeroGoods(landprovinceData.Resources);
		}

		if (EngineState.DebugMode)
		{
			Header("Debug Data").NewLine();
			Append("Id: " + provinceData.Id).NewLine();
			if (provinceData is LandColonizedProvinceData landColonizedProvinceData)
				Append("Owner ID: " + landColonizedProvinceData.Owner);
		}

	}
	
	/// <summary>
	/// Shows the regiment data info box.
	/// </summary>
	/// <param name="armyRegiment"></param>
	public void ShowBattleRegimentData(Regiment armyRegiment)
	{
		Clear().NewLine();
		if (armyRegiment == null)
		{
			Header("Empty Space");
			return;
		}

		Header(armyRegiment.Name).NewLine();

		switch (armyRegiment)
		{
			case ArmyInfantryRegiment:
				Append("Type: Army Infantry").NewLine();
				break;
			case ArmyCavalryRegiment:
				Append("Type: Army Cavalry").NewLine();
				break;
			case ArmyArtilleryRegiment:
				Append("Type: Army Artillery").NewLine();
				break;
		}

		//infoBoxBuilder.ShowNotZeroGoods(armyRegiment.Resources);
		Append($"Manpower: {armyRegiment.Manpower}").NewLine();
		Append($"Morale: {armyRegiment.Morale}");
	}
	
	/// <summary>
	/// Shows the good data info box.
	/// </summary>
	/// <param name="building"></param>
	public void ShowBuildingData(Building building)
	{
		Clear().NewLine()
			.Header(building.Name).NewLine()
			.Append($"Cost: {building.Cost}").NewLine()
			.Append("Resource Cost: ").AppendNonZeroGoods(building.ResourceCost)
			.Header("Modifiers:")
			.AppendModifiers(building.Modifiers);
	}
	
	/// <summary>
	///	Shows the technology data info box.
	/// </summary>
	/// <param name="technology"></param>
	public void ShowTechnologyData(Technology technology)
	{
		Clear().NewLine()
			.Header(technology.TechnologyName).NewLine()
			.Append($"Cost: {technology.InitialCost}").NewLine()
			.Append($"Research time: {technology.ResearchTime}").NewLine();
		
		if (Good.IsDifferentFromNull(technology.ResourcesRequired))
			Append("Resource Cost: ").NewLine().AppendNonZeroGoods(technology.ResourcesRequired);
		
		if (technology.BuildingToUnlock > -1)
			Append($"Building to unlock: ").ShowBuilding(technology.BuildingToUnlock).NewLine();
		
		if (technology.RecipyToUnlock > -1)
			Append("Recipy To Unlock: ")
				.ShowGood(EngineState.MapInfo.Scenario.Recipes[technology.RecipyToUnlock].Output).NewLine();
		
		var nonDefaultModifiers = new RichTextLabelBuilder().ShowModifiers(technology.Modifiers);
		if (!string.IsNullOrEmpty(nonDefaultModifiers.ToString()))
			Header("Modifiers:").AppendModifiers(technology.Modifiers);
		
	}
	/// <summary>
	/// Shows the dev button data.
	/// </summary>
	/// <param name="provinceData"></param>
	public void ShowDevButtonData(LandColonizedProvinceData provinceData)
	{
		var req = EngineState.MapInfo.Scenario.Settings.ResourceAndCostRequirmentsToDev(provinceData.Development);
		Clear().NewLine()
			.Header("Needed To Dev").NewLine()
			.Append($"Cost: {req.Key}").NewLine()
			.AppendNonZeroGoods(req.Value);
	}
	
	#endregion

 
}
