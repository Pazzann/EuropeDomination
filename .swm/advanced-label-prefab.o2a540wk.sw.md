---
title: Advanced Label  Prefab
---
# Introduction

This document will walk you through the implementation of the Advanced Label Prefab feature.

The feature introduces a custom label component with advanced formatting capabilities.

We will cover:

1. Overall structure of the <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="16:6:6" line-data="public partial class AdvancedLabel : VBoxContainer">`AdvancedLabel`</SwmToken> class.
2. Explanation of each method in the builder region.
3. Explanation of each method in the factory region.
4. Example usage of the <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="16:6:6" line-data="public partial class AdvancedLabel : VBoxContainer">`AdvancedLabel`</SwmToken> class.

# Overall structure of the <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="16:6:6" line-data="public partial class AdvancedLabel : VBoxContainer">`AdvancedLabel`</SwmToken> class

<SwmSnippet path="/Scripts/Utils/AdvancedLabel.cs" line="16">

---

The <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="16:6:6" line-data="public partial class AdvancedLabel : VBoxContainer">`AdvancedLabel`</SwmToken> class extends <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="16:10:10" line-data="public partial class AdvancedLabel : VBoxContainer">`VBoxContainer`</SwmToken> and provides methods to dynamically build complex labels with various elements like text, images, and modifiers.

```
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
```

---

</SwmSnippet>

# Builder region methods

The builder region contains methods to construct and modify the label.

### Clear method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="39">

---

Clears all children from the label and resets the last reference.

```
	public AdvancedLabel Clear()
	{
		foreach (var child in GetChildren())
			child.Free();
		_lastReference = null;
		return this;
	} 
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="47:5:5" line-data="	public AdvancedLabel NewLine()">`NewLine`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="47">

---

Adds a new horizontal box container to the label, allowing new elements to be added on a new line.

```
	public AdvancedLabel NewLine()
	{
		_lastReference = _hBoxPrefab.Instantiate() as HBoxContainer;
		AddChild(_lastReference);
		return this;
	}
```

---

</SwmSnippet>

### \_validate method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="54">

---

Ensures that a new line has been added before adding elements. Throws an exception if not.

```
	public void _validate()
	{
		if(_lastReference == null)
			throw new Exception("You must call NewLine() before adding elements");
	}
```

---

</SwmSnippet>

### Header method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="61">

---

Adds a header text to the label with specific styling.

```
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
```

---

</SwmSnippet>

### Append method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="77">

---

Adds plain text to the label.

```
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
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="89:5:5" line-data="	public AdvancedLabel AppendColored(string text, Color color){">`AppendColored`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="89">

---

Adds colored text to the label.

```
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
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="103:5:5" line-data="	public AdvancedLabel AppendNonZeroGoods(double[] goods)">`AppendNonZeroGoods`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="103">

---

Adds goods to the label if their quantity is greater than zero.

```
	public AdvancedLabel AppendNonZeroGoods(double[] goods)
	{
		_validate();
		for (var id = 0; id < goods.Length; id++)
			if (goods[id] > 0)
				ShowGood(id).Append($": {goods[id]}");

		NewLine();
		
		return this;
	}
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="115:5:5" line-data="	public AdvancedLabel ShowBuilding(int id)">`ShowBuilding`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="115">

---

Adds a building image to the label.

```
	public AdvancedLabel ShowBuilding(int id)
	{
		_validate();
		
		var a = _textureRectPrefab.Instantiate() as TextureRect;
		
		a.Texture = GlobalResources.BuildingSpriteFrames.GetFrameTexture("default", id);
		a.Size = new Vector2(32, 32);
		_lastReference.AddChild(a);
		
		return this;
	}
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="108:1:1" line-data="				ShowGood(id).Append($&quot;: {goods[id]}&quot;);">`ShowGood`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="128">

---

Adds a good image to the label.

```
	public AdvancedLabel ShowGood(int id)
	{
		_validate();
		
		var a = _textureRectPrefab.Instantiate() as TextureRect;
		a.Texture = GlobalResources.GoodSpriteFrames.GetFrameTexture("goods", id);
		a.Size = new Vector2(32, 32);
		_lastReference.AddChild(a);
		
		return this;
	}
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="142:5:5" line-data="	public AdvancedLabel AppendModifiers(Modifiers modifiers)">`AppendModifiers`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="142">

---

Adds text with modifiers to the label, including handling of positive and negative values.

```
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
```

---

</SwmSnippet>

# Factory region methods

The factory region contains methods to display various types of data in the label.

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="190:5:5" line-data="	public void ShowProvinceDataInfoBox(ProvinceData provinceData)">`ShowProvinceDataInfoBox`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="190">

---

Displays information about a province, including its name, goods, and terrain.

```
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
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="218:5:5" line-data="	public void ShowBattleRegimentData(Regiment armyRegiment)">`ShowBattleRegimentData`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="218">

---

Displays information about a battle regiment, including its type, manpower, and morale.

```
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
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="248:5:5" line-data="	public void ShowBuildingData(Building building)">`ShowBuildingData`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="248">

---

Displays information about a building, including its cost, resource cost, and modifiers.

```
	public void ShowBuildingData(Building building)
	{
		Clear().NewLine()
			.Header(building.Name).NewLine()
			.Append($"Cost: {building.Cost}").NewLine()
			.Append("Resource Cost: ").AppendNonZeroGoods(building.ResourceCost)
			.Header("Modifiers:")
			.AppendModifiers(building.Modifiers);
	}
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="259:5:5" line-data="	public void ShowTechnologyData(Technology technology)">`ShowTechnologyData`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="259">

---

Displays information about a technology, including its cost, research time, and required resources.

```
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
```

---

</SwmSnippet>

### <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="282:5:5" line-data="	public void ShowDevButtonData(LandColonizedProvinceData provinceData)">`ShowDevButtonData`</SwmToken> method

<SwmSnippet path="Scripts/Utils/AdvancedLabel.cs" line="282">

---

Displays development button data, including the cost and required resources.

```
	public void ShowDevButtonData(LandColonizedProvinceData provinceData)
	{
		var req = EngineState.MapInfo.Scenario.Settings.ResourceAndCostRequirmentsToDev(provinceData.Development);
		Clear().NewLine()
			.Header("Needed To Dev").NewLine()
			.Append($"Cost: {req.Key}").NewLine()
			.AppendNonZeroGoods(req.Value);
	}
```

---

</SwmSnippet>

# Example usage

<SwmSnippet path="/Scripts/UI/GUIHandlers/GUICountryWindow.cs" line="118">

---

The following snippet demonstrates how to use the <SwmToken path="/Scripts/UI/GUIHandlers/GUICountryWindow.cs" pos="118:10:10" line-data="			consumptionContainer.GetChild(1).GetChild&lt;AdvancedLabel&gt;(0)._Ready();">`AdvancedLabel`</SwmToken> class to display information about a good in the GUI.

```
			consumptionContainer.GetChild(1).GetChild<AdvancedLabel>(0)._Ready();
			consumptionContainer.GetChild(1).GetChild<AdvancedLabel>(0)
				.Clear().NewLine()
				.Header(good.Name).NewLine()
				.Append($"Consumption: {good.ConsumptionPerMonthToActivateBonus}t/m").NewLine()
				.Header("Modifiers:")
				.AppendModifiers(good.Modifiers);
```

---

</SwmSnippet>

This example shows how to initialize the <SwmToken path="/Scripts/Utils/AdvancedLabel.cs" pos="16:6:6" line-data="public partial class AdvancedLabel : VBoxContainer">`AdvancedLabel`</SwmToken>, clear it, add a header, append consumption information, and add modifiers.

<SwmMeta version="3.0.0" repo-id="Z2l0aHViJTNBJTNBRXVyb3BlRG9taW5hdGlvbkRlbW8lM0ElM0FQYXp6YW5u" repo-name="EuropeDominationDemo"><sup>Powered by [Swimm](https://app.swimm.io/)</sup></SwmMeta>
