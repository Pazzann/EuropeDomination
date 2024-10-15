using System.Threading.Tasks;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

public static class SteamForGodotUtils
{
    public static ImageTexture ImageTextureFromSteamImage(Steamworks.Data.Image? img)
    {
        return ImageTexture.CreateFromImage(Image.CreateFromData((int)img?.Width, (int)img?.Height, false,
            Image.Format.Rgba8, img?.Data));
    }
}