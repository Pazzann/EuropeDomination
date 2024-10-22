using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using Godot;

namespace EuropeDominationDemo.Scripts.Utils.Math;

/// <summary>
/// Utility class for game math.
/// </summary>
public class GameMath
{
	/// <summary>
	/// Gets the province ID from a color.
	/// </summary>
	/// <param name="color"></param>
	/// <returns></returns>
	public static int GetProvinceId(Color color)
	{
		return (int)((color.R + color.G * 256.0f + color.B * 256.0f * 256.0f) * 255.0f) - 1;
	}

	/// <summary>
	/// Calculates the center of provinces based on the map texture.
	/// </summary>
	/// <param name="mapTexture"></param>
	/// <param name="provinceCount"></param>
	/// <returns></returns>
	public static Vector2[] CalculateCenterOfProvinceWeight(Image mapTexture, int provinceCount)
	{
		var xCoords = new int[provinceCount];
		var yCoords = new int[provinceCount];
		var sumPixels = new int[provinceCount];

		for (var y = 1; y < mapTexture.GetHeight(); y++)
		for (var x = 1; x < mapTexture.GetWidth(); x++)
		{
			var pixel = mapTexture.GetPixelv(new Vector2I(x, y));

			if (pixel.A < 1.0f)
				continue;

			var tileId = GetProvinceId(pixel);

			xCoords[tileId] += x;
			yCoords[tileId] += y;

			sumPixels[tileId]++;
		}

		var centers = new Vector2[provinceCount];

		for (var i = 0; i < provinceCount; i++)
			centers[i] = new Vector2(xCoords[i] / sumPixels[i], yCoords[i] / sumPixels[i]);

		return centers;
	}

	/// <summary>
	/// Calculates the border provinces based on the map texture.
	/// </summary>
	/// <param name="map"></param>
	/// <param name="mapTexture"></param>
	/// <returns></returns>
	public static ProvinceData[] CalculateBorderProvinces(ProvinceData[] map, Image mapTexture)
	{
		var borders = new Dictionary<int, List<int>>();
		int tId;
		for (var y = 1; y < mapTexture.GetHeight() - 1; y++)
		for (var x = 1; x < mapTexture.GetWidth() - 1; x++)
		{
			var owner = mapTexture.GetPixelv(new Vector2I(x, y));
			tId = GetProvinceId(owner);
			if (owner.A < 1.0f)
				continue;

			var a = mapTexture.GetPixelv(new Vector2I(x, y + 1));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);

			a = mapTexture.GetPixelv(new Vector2I(x, y - 1));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);

			a = mapTexture.GetPixelv(new Vector2I(x + 1, y));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);

			a = mapTexture.GetPixelv(new Vector2I(x - 1, y + 1));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);


			if (tId != GetProvinceId(owner))
				if (borders.ContainsKey(GetProvinceId(owner)))
				{
					if (!borders[GetProvinceId(owner)].Contains(tId)) borders[GetProvinceId(owner)].Add(tId);
				}
				else
				{
					var l = new List<int>();
					l.Add(tId);
					borders.Add(GetProvinceId(owner), l);
				}
		}

		foreach (var entry in borders) map[entry.Key].BorderderingProvinces = entry.Value.ToArray();

		return map;
	}

	
	/// <summary>
	/// Gets the list of province IDs from a list of provinces.
	/// </summary>
	/// <param name="provinces"></param>
	/// <returns></returns>
	public static HashSet<int> ListIdsFromProvinces(ProvinceData[] provinces)
	{
		var ids = new HashSet<int>();
		for (var i = 0; i < provinces.Length; i++) ids.Add(provinces[i].Id);

		return ids;
	}
}
