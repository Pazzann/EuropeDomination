---
title: Game  Math Class
---
# Introduction

This document will walk you through the implementation of the <SwmToken path="/Scripts/Utils/Math/GameMath.cs" pos="10:4:4" line-data="public class GameMath">`GameMath`</SwmToken> class in <SwmPath>[Scripts/Utils/Math/GameMath.cs](/Scripts/Utils/Math/GameMath.cs)</SwmPath>.

The <SwmToken path="/Scripts/Utils/Math/GameMath.cs" pos="10:4:4" line-data="public class GameMath">`GameMath`</SwmToken> class provides utility functions for handling game map data, specifically for calculating province IDs, centers, borders, and listing province IDs.

We will cover:

1. How province IDs are calculated from colors.
2. How the center of provinces is determined.
3. How border provinces are identified.
4. How a list of province IDs is generated from a list of provinces.

# Calculating province IDs from colors

<SwmSnippet path="Scripts/Utils/Math/GameMath.cs" line="17">

---

The <SwmToken path="/Scripts/Utils/Math/GameMath.cs" pos="42:7:7" line-data="			var tileId = GetProvinceId(pixel);">`GetProvinceId`</SwmToken> method converts a color to a province ID. This is essential for mapping pixel data to game provinces.

```
	public static int GetProvinceId(Color color)
	{
		return (int)((color.R + color.G * 256.0f + color.B * 256.0f * 256.0f) * 255.0f) - 1;
	}
```

---

</SwmSnippet>

# Calculating the center of provinces

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="21">

---

The <SwmToken path="/Scripts/Utils/Math/GameMath.cs" pos="28:9:9" line-data="	public static Vector2[] CalculateCenterOfProvinceWeight(Image mapTexture, int provinceCount)">`CalculateCenterOfProvinceWeight`</SwmToken> method calculates the geometric center of each province based on the map texture. This helps in determining the central point of each province for various game mechanics.

```

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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="33">

---

We iterate through each pixel in the map texture to accumulate the x and y coordinates for each province.

```

		for (var y = 1; y < mapTexture.GetHeight(); y++)
		for (var x = 1; x < mapTexture.GetWidth(); x++)
		{
			var pixel = mapTexture.GetPixelv(new Vector2I(x, y));

			if (pixel.A < 1.0f)
				continue;
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="41">

---

For each pixel, we determine the province ID and update the coordinates and pixel count.

```

			var tileId = GetProvinceId(pixel);

			xCoords[tileId] += x;
			yCoords[tileId] += y;

			sumPixels[tileId]++;
		}
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="49">

---

Finally, we calculate the average coordinates for each province to determine the center.

```

		var centers = new Vector2[provinceCount];

		for (var i = 0; i < provinceCount; i++)
			centers[i] = new Vector2(xCoords[i] / sumPixels[i], yCoords[i] / sumPixels[i]);

		return centers;
	}
```

---

</SwmSnippet>

# Identifying border provinces

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="57">

---

The <SwmToken path="/Scripts/Utils/Math/GameMath.cs" pos="64:9:9" line-data="	public static ProvinceData[] CalculateBorderProvinces(ProvinceData[] map, Image mapTexture)">`CalculateBorderProvinces`</SwmToken> method identifies the border provinces by comparing adjacent pixels in the map texture. This is useful for determining province boundaries.

```

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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="75">

---

We iterate through each pixel and check its neighbors to identify border changes.

```

			var a = mapTexture.GetPixelv(new Vector2I(x, y + 1));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);

			a = mapTexture.GetPixelv(new Vector2I(x, y - 1));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="83">

---

We continue checking adjacent pixels to ensure all borders are identified.

```

			a = mapTexture.GetPixelv(new Vector2I(x + 1, y));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);

			a = mapTexture.GetPixelv(new Vector2I(x - 1, y + 1));
			if (owner != a && a.A > 0.5f)
				tId = GetProvinceId(a);
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/GameMath.cs" line="91">

---

If a border is detected, we update the borders dictionary.

```


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
```

---

</SwmSnippet>

<SwmSnippet path="Scripts/Utils/Math/GameMath.cs" line="105">

---

Finally, we update the map data with the identified borders.

```

		foreach (var entry in borders) map[entry.Key].BorderderingProvinces = entry.Value.ToArray();

		return map;
	}
```

---

</SwmSnippet>

# Listing province IDs

<SwmSnippet path="Scripts/Utils/Math/GameMath.cs" line="117">

---

The <SwmToken path="/Scripts/Utils/Math/GameMath.cs" pos="117:10:10" line-data="	public static HashSet&lt;int&gt; ListIdsFromProvinces(ProvinceData[] provinces)">`ListIdsFromProvinces`</SwmToken> method generates a set of province IDs from a list of provinces. This is useful for quickly accessing province data.

```
	public static HashSet<int> ListIdsFromProvinces(ProvinceData[] provinces)
	{
		var ids = new HashSet<int>();
		for (var i = 0; i < provinces.Length; i++) ids.Add(provinces[i].Id);

		return ids;
	}
```

---

</SwmSnippet>

This concludes the walkthrough of the <SwmToken path="/Scripts/Utils/Math/GameMath.cs" pos="10:4:4" line-data="public class GameMath">`GameMath`</SwmToken> class. Each method serves a specific purpose in handling game map data, making it easier to manage and manipulate province information.

<SwmMeta version="3.0.0" repo-id="Z2l0aHViJTNBJTNBRXVyb3BlRG9taW5hdGlvbkRlbW8lM0ElM0FQYXp6YW5u" repo-name="EuropeDominationDemo"><sup>Powered by [Swimm](https://app.swimm.io/)</sup></SwmMeta>
