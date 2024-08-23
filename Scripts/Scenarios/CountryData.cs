using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.Army;
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

	public CountryData(int id, string name, Vector3 color, Modifiers modifiers, int money, int manpower, List<General> generals)
	{
		Id = id;
		Name = name;
		Color = color;
		Modifiers = modifiers;
		Money = money;
		Manpower = manpower;
		Generals = generals;
	}
}
