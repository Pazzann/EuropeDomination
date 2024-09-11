using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

namespace EuropeDominationDemo.Scripts.Math;

public static class PathFinder
{
    public static int[] FindPathFromAToB(int a, int b, ProvinceData[] provinces, float eps = 0.1f)
    {
        // O(V^2) Dijkstra algorithm implementation  

        var provinceDictionary = new Dictionary<int, ProvinceData>();
        foreach (var province in provinces) provinceDictionary.Add(province.Id, province);

        var minDistanceToProvince = new Dictionary<int, float>();
        var parent = new Dictionary<int, int>();
        var visited = new Dictionary<int, bool>();

        foreach (var province in provinceDictionary)
        {
            minDistanceToProvince.Add(province.Key, float.MaxValue);
            parent.Add(province.Key, -1);
            visited.Add(province.Key, false);
        }

        minDistanceToProvince[a] = 0;


        for (var i = 0; i < minDistanceToProvince.Count; i++)
        {
            var currProvince = -1;

            foreach (var j in minDistanceToProvince)
                if (!visited[j.Key] && (currProvince == -1 ||
                                        minDistanceToProvince[j.Key] < minDistanceToProvince[currProvince]))
                    currProvince = j.Key;

            if (System.Math.Abs(currProvince - float.MaxValue) < eps)
                break;

            foreach (var provinceId in provinceDictionary[currProvince].BorderderingProvinces
                         .Where(p => provinceDictionary.ContainsKey(p)))
            {
                var distance = (provinceDictionary[currProvince].CenterOfWeight -
                                provinceDictionary[provinceId].CenterOfWeight).Length();

                if (minDistanceToProvince[provinceId] - (minDistanceToProvince[currProvince] + distance) > eps)
                {
                    minDistanceToProvince[provinceId] = minDistanceToProvince[currProvince] + distance;
                    parent[provinceId] = currProvince;
                }
            }

            visited[currProvince] = true;
        }

        // Recover the shortest path from a to b 

        var path = new List<int> { b };

        for (var currProvince = b; parent[currProvince] != a; currProvince = parent[currProvince])
        {
            if (parent[currProvince] == -1)
                return new int[0];
            path.Add(parent[currProvince]);
        }

        path.Add(a);

        return path.ToArray();
    }

    public static bool CheckConnectionFromAToB(int a, int b, ProvinceData[] provinces)
    {
        var provinceDictionary = new Dictionary<int, ProvinceData>();
        foreach (var province in provinces) provinceDictionary.Add(province.Id, province);
        var visited = new Dictionary<int, bool>();

        foreach (var province in provinceDictionary) visited.Add(province.Key, false);
        _dfs(a, provinceDictionary, visited);

        return visited[b];
    }

    private static void _dfs(int currProvince, Dictionary<int, ProvinceData> provinceDictionary,
        Dictionary<int, bool> visited)
    {
        visited[currProvince] = true;
        foreach (var provinceId in provinceDictionary[currProvince].BorderderingProvinces
                     .Where(p => provinceDictionary.ContainsKey(p)))
            if (!visited[provinceId])
                _dfs(provinceId, provinceDictionary, visited);
    }


    public static Dictionary<int, bool> CheckConnectionFromAToOthers(int a, ProvinceData[] provinces)
    {
        var provinceDictionary = new Dictionary<int, ProvinceData>();
        foreach (var province in provinces) provinceDictionary.Add(province.Id, province);
        var visited = new Dictionary<int, bool>();

        foreach (var province in provinceDictionary) visited.Add(province.Key, false);
        _dfs(a, provinceDictionary, visited);

        return visited;
    }
}