using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios;

public class CountryData
{

	public int Id { get; }

	public string Name;
	public Vector3 Color;
	public Modifiers Modifiers;

	public CountryData(int id, string name, Vector3 color, Modifiers modifiers )
	{
		Id = id;
		Name = name;
		Color = color;
		Modifiers = modifiers;
	}
}
