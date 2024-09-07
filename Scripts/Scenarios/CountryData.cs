using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class CountryData
{
	
	public int Id { get; }

	public string Name;
	public Vector3 Color;
	public Modifiers Modifiers;

	public int Money = 100;
	public int Manpower = 100;

	public List<General> Generals;
	public List<Admiral> Admirals;
	public List<UnitData> Units;
	public List<Template> RegimentTemplates;

	public Dictionary<int, List<DiplomacyAgreement>> DiplomacyAgreements;
	public CountryData(int id, string name, Vector3 color, Modifiers modifiers, int money, int manpower, List<General> generals, List<Admiral> admirals, List<UnitData> units, List<Template> templates, Dictionary<int, List<DiplomacyAgreement>> diplomacyAgreements)
	{
		Id = id;
		Name = name;
		Color = color;
		Modifiers = modifiers;
		Money = money;
		Manpower = manpower;
		Generals = generals;
		Admirals = admirals;
		Units = units;
		RegimentTemplates = templates;
		DiplomacyAgreements = diplomacyAgreements;
	}
}
