
using Godot;

namespace EuropeDominationDemo.Scripts.Enums;

public static class MapDefaultColors
{
    public static Vector3 Unselectable = new Vector3(1, 0, 0);
    public static Vector3 OwnProvince = new Vector3(1, 1, 0);
    public static Vector3 Selectable = new Vector3(0, 1, 0);
    public static Vector3 WaterUnselectable = new Vector3(0, 0, 0.3f);
    public static Color ResourceIncrease = new Color(0, 1, 0);
    public static Color ResourceDecrease = new Color(1, 0, 0);
    
    public static Color EmptyInBattle = new Color(0.9f, 0.9f, 0.9f);
    public static Color AttackerInfantryInBattle = new Color(1, 0.1f, 0);
    public static Color AttackerCavalryInBattle = new Color(1, 0.3f, 0);
    public static Color AttackerArtilleryInBattle = new Color(1, 0.6f, 0);
    public static Color DefenderInfantryInBattle = new Color(0, 0.1f, 1);
    public static Color DefenderCavalryInBattle = new Color(0, 0.3f, 1);
    public static Color DefenderArtilleryInBattle = new Color(0, 0.6f, 1);
}