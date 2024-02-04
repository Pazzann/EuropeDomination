using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios;
using Microsoft.VisualBasic;

namespace EuropeDominationDemo.Scripts.Math;

public static class PathFinder
{
	public static int[] FindPathFromAToB(int a, int b, Scenario scenario, float eps = 0.1f)
	{
		// O(V^2) Dijkstra algorithm implementation  
		
		var minDistanceToProvince = new float[scenario.Map.Length];
		
		for (var i = 0; i < minDistanceToProvince.Length; ++i)
			minDistanceToProvince[i] = float.MaxValue;
		
		minDistanceToProvince[a] = 0;
		
		var parent = new int[scenario.Map.Length];
		var visited = new bool[scenario.Map.Length];
		
		for (var i = 0; i < minDistanceToProvince.Length; ++i)
		{
			var curProvince = -1;
			
			for (var j = 0; j < minDistanceToProvince.Length; ++j)
			{
				if (!visited[j] && (curProvince == -1 || minDistanceToProvince[j] < minDistanceToProvince[curProvince]))
					curProvince = j;
			}
			
			foreach (var provinceId in scenario.Map[curProvince].BorderderingProvinces)
			{
				var distance = (scenario.Map[curProvince].CenterOfWeight - scenario.Map[provinceId].CenterOfWeight).Length();
				
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
}
