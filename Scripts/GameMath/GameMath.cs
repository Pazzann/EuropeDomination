using Godot;

namespace EuropeDominationDemo.Scripts.GameMath;

public class GameMath
{
    public static int GetProvinceID(Color color){
        return (int)((color.R + color.G*256.0f)*255.0f) - 1;
    }
}