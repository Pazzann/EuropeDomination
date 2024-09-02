using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;


namespace EuropeDominationDemo.Scripts.Math;

public static class PathFinder
{
	public static int[] FindPathFromAToB(int a, int b, ProvinceData[] provinces, float eps = 0.1f)
	{
		// O(V^2) Dijkstra algorithm implementation  
		
		var minDistanceToProvince = new float[provinces.Length];
		
		for (var i = 0; i < minDistanceToProvince.Length; ++i)
			minDistanceToProvince[i] = float.MaxValue;
		
		minDistanceToProvince[a] = 0;
		
		var parent = new int[provinces.Length];
		var visited = new bool[provinces.Length];
		
		for (var i = 0; i < minDistanceToProvince.Length; ++i)
		{
			var curProvince = -1;
			
			for (var j = 0; j < minDistanceToProvince.Length; ++j)
			{
				if (!visited[j] && (curProvince == -1 || minDistanceToProvince[j] < minDistanceToProvince[curProvince]))
					curProvince = j;
			}
			
			foreach (var provinceId in provinces[curProvince].BorderderingProvinces)
			{
				var distance = (provinces[curProvince].CenterOfWeight - provinces[provinceId].CenterOfWeight).Length();
				
				if (minDistanceToProvince[provinceId] - (minDistanceToProvince[curProvince] + distance) > eps)
				{
					minDistanceToProvince[provinceId] = minDistanceToProvince[curProvince] + distance;
					parent[provinceId] = curProvince;
				}
			}

			visited[curProvince] = true;
		}
		
		// Recover the shortest path from a to b 
		
		var path = new List<int> { b };

		for (var curProvince = b; parent[curProvince] != a; curProvince = parent[curProvince])
			path.Add(parent[curProvince]);
		
		path.Add(a);

		return path.ToArray();
	}

	public static bool CheckConnectionFromAToB(int a, int b, ProvinceData[] provinces, float eps = 0.1f)
	{
		return true;
	}
}
