using System.Threading.Tasks;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils;

/// <summary>
/// Utility class for Steam for Godot.
/// </summary>
public static class SteamForGodotUtils
{
    /// <summary>
    /// Converts a Steam image to a Godot image texture.
    /// </summary>
    /// <param name="img">
    /// The Steam image.
    /// </param>
    /// <returns>
    /// The Godot image texture.
    /// </returns>
    public static ImageTexture ImageTextureFromSteamImage(Steamworks.Data.Image? img)
    {
        return ImageTexture.CreateFromImage(Image.CreateFromData((int)img?.Width, (int)img?.Height, false,
            Image.Format.Rgba8, img?.Data));
    }
}