using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Text;
using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public class GameMath
{
	public static int GetProvinceId(Color color)
	{
		return (int)((color.R + color.G * 256.0f + color.B * 256.0f * 256.0f) * 255.0f) - 1;
	}

	public static Vector2[] CalculateCenterOfProvinceWeight(Image mapTexture, int provinceCount)
	{
		var xCoords = new int[provinceCount];
		var yCoords = new int[provinceCount];
		var sumPixels = new int[provinceCount];

		for (var y = 1; y < mapTexture.GetHeight(); y++)
		{
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
		}

		var centers = new Vector2[provinceCount];

		for (var i = 0; i < provinceCount; i++)
		{
			centers[i] = new Vector2(xCoords[i] / sumPixels[i], yCoords[i] / sumPixels[i]);
		}

		return centers;
	}

	// public static Vector2 CalculateCenterOfStateWeight(Image mapTexture, HashSet<int> provincesIdsOfState)
	// {
	// 	var xCoords = 0;
	// 	var yCoords = 0;
	// 	var sumPixels = 0;
	// 	for (var y = 1; y < mapTexture.GetHeight(); y++)
	// 	{
	// 		for (var x = 1; x < mapTexture.GetWidth(); x++)
	// 		{
	// 			var pixel = mapTexture.GetPixelv(new Vector2I(x, y));
	// 			
	// 			if (pixel.A < 1.0f)
	// 				continue;
	// 			
	// 			var tileId = GetProvinceId(pixel);
	// 			
	// 			if (provincesIdsOfState.Contains(tileId))
	// 			{
	// 				xCoords += x;
	// 				yCoords += y;
	// 				sumPixels++;
	// 			}
	// 		}
	// 	}
	//
	// 	if (sumPixels == 0)
	// 	{
	// 		return Vector2.Zero;
	// 	}
	//
	// 	return new Vector2(xCoords / sumPixels, yCoords / sumPixels);
	// }
	//
	// public static int ClosestIdCenterToPoint(ProvinceData[] countryProvinces, Vector2 center)
	// {
	// 	int res = 0;
	// 	for (int i = 0; i < countryProvinces.Length; i++)
	// 	{
	// 		if ((center - countryProvinces[res].CenterOfWeight).Length() >
	// 			(center - countryProvinces[i].CenterOfWeight).Length())
	// 		{
	// 			res = i;
	// 		}
	// 	}
	//
	// 	return res;
	// }
	//
	// public static (IPath, float) FindSuitableTextPath(int stateId, StateMap map, float letterAspectRatio, int letterCount, Image mapImg) {
	// 	
	// }

	// public static Vector2[] FindSquarePointsInsideState(ProvinceData[] countryProvinces, Image mapTexture,
	// 	int squareSide)
	// {
	// 	List<Vector2> points = new List<Vector2>();
	// 	var ids = ListIdsFromProvinces(countryProvinces);
	// 	for (int y = squareSide; y < mapTexture.GetHeight(); y += squareSide)
	// 	{
	// 		for (int x = squareSide; x < mapTexture.GetWidth(); x += squareSide)
	// 		{
	// 			Color pixel = mapTexture.GetPixelv(new Vector2I(x, y));
	// 			if (pixel.A < 1.0f)
	// 				continue;
	// 			var tileId = GetProvinceId(pixel);
	// 			if (ids.Contains(tileId))
	// 				points.Add(new Vector2(x, y));
	// 		}
	// 	}
	//
	// 	return points.ToArray();
	// }

	public static ProvinceData[] CalculateBorderProvinces(ProvinceData[] map, Image mapTexture)
	{
		Dictionary<int, List<int>> borders = new Dictionary<int, List<int>>();
		int tId;
		for (int y = 1; y < mapTexture.GetHeight() - 1; y++)
		{
			for (int x = 1; x < mapTexture.GetWidth() - 1; x++)
			{
				Color owner = mapTexture.GetPixelv(new Vector2I(x, y));
				tId = GetProvinceId(owner);
				if (owner.A < 1.0f)
					continue;

				Color a = mapTexture.GetPixelv(new Vector2I(x, y + 1));
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
						if (!borders[GetProvinceId(owner)].Contains(tId))
						{
							borders[GetProvinceId(owner)].Add(tId);
						}
					}
					else
					{
						var l = new List<int>();
						l.Add(tId);
						borders.Add(GetProvinceId(owner), l);
					}
			}
		}

		foreach (KeyValuePair<int, List<int>> entry in borders)
		{
			map[entry.Key].BorderderingProvinces = entry.Value.ToArray();
		}

		return map;
	}

	public static HashSet<int> ListIdsFromProvinces(ProvinceData[] provinces)
	{
		var ids = new HashSet<int>();
		for (int i = 0; i < provinces.Length; i++)
		{
			ids.Add(provinces[i].Id);
		}

		return ids;
	}
}
