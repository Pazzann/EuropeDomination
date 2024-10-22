using System;
using System.Collections.Generic;
using System.Reflection;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

[Obsolete("Use AdvancedLabel instead")]
public class RichTextLabelBuilder
{
    public string Text { get; private set; } = "";

    public RichTextLabelBuilder Append (RichTextLabelBuilder text)
    {
	    Text += text.Text;
	    return this;
    }

    public override string ToString()
    {
	    return Text;
    }
    
    public static RichTextLabelBuilder operator + (RichTextLabelBuilder text1, RichTextLabelBuilder text2) => text1.Append(text2);

    public RichTextLabelBuilder AppendText(string text)
	{
		Text += text;
		return this;
	}

	public RichTextLabelBuilder Header(string text)
	{
		Text += $"[b][color=yellow]{text}[/color][/b]";
		return this;
	}

	public RichTextLabelBuilder NewLine()
	{
		Text += "\n";
		
		return this;
	}

	public RichTextLabelBuilder StartBold()
	{
		Text += "[b]";
		return this;
	}

	public RichTextLabelBuilder EndBold()
	{
		Text += "[/b]";
		return this;
	}

	public RichTextLabelBuilder StartColor(string color)
	{
		Text += $"[color={color}]";
		return this;
	}

	public RichTextLabelBuilder EndColor()
	{
		Text += "[/color]";
		return this;
	}

	public RichTextLabelBuilder ShowNonZeroGoods(double[] resources)
	{

		for (var id = 0; id < resources.Length; id++)
			if (resources[id] > 0)
				ShowGood(id).AppendText($": {resources[id]}");

		NewLine();
		
		return this;
	}

	public RichTextLabelBuilder ShowGood(int id)
	{
		Text +=
			$"[img=30px, center]{GlobalResources.GoodSpriteFrames.GetFrameTexture("goods", id).ResourcePath}[/img]";
		return this;
	}

	public RichTextLabelBuilder ShowBuilding(int id)
	{
		Text +=
			$"[img=30px, center]{GlobalResources.BuildingSpriteFrames.GetFrameTexture("default", id).ResourcePath}[/img]";
		return this;
	}

	public RichTextLabelBuilder ShowModifiers(Modifiers modifiers)
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