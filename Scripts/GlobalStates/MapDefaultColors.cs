using Godot;

namespace EuropeDominationDemo.Scripts.Enums;

public static class MapDefaultColors
{
    public static Vector3 Unselectable = new(1, 0, 0);
    public static Vector3 OwnProvince = new(1, 1, 0);
    public static Vector3 Selectable = new(0, 1, 0);
    public static Vector3 WaterUnselectable = new(0, 0, 0.3f);
    public static Color ResourceIncrease = new(0, 1, 0);
    public static Color ResourceDecrease = new(1, 0, 0);

    public static Color EmptyInBattle = new(0.9f, 0.9f, 0.9f);
    public static Color AttackerInfantryInBattle = new(1, 0.1f, 0);
    public static Color AttackerCavalryInBattle = new(1, 0.3f, 0);
    public static Color AttackerArtilleryInBattle = new(1, 0.6f, 0);
    public static Color DefenderInfantryInBattle = new(0, 0.1f, 1);
    public static Color DefenderCavalryInBattle = new(0, 0.3f, 1);
    public static Color DefenderArtilleryInBattle = new(0, 0.6f, 1);
}