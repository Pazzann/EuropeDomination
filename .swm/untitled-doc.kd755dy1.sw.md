---
title: Untitled doc
---
# Introduction

This document will walk you through the implementation of the <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="10:6:6" line-data="public static class PathFinder">`PathFinder`</SwmToken> class.

The <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="10:6:6" line-data="public static class PathFinder">`PathFinder`</SwmToken> class provides methods to find paths and check connections between provinces using graph algorithms.

We will cover:

1. The main method for finding paths.
2. The initialization of data structures.
3. The Dijkstra algorithm implementation.
4. The depth-first search (DFS) for connection checks.

# Path finding method

<SwmSnippet path="Scripts/Scenarios/BattleData.cs" line="30">

---

The main method in this class is <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="20:9:9" line-data="    public static int[] FindPathFromAToB(int a, int b, ProvinceData[] provinces, float eps = 0.1f)">`FindPathFromAToB`</SwmToken>. It finds the shortest path between two provinces using Dijkstra's algorithm.

```
    public List<ArmyRegiment> Regiments { get; set; }
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="23">

---

We initialize several dictionaries to store province data, distances, parents, and visited status.

```

        var provinceDictionary = new Dictionary<int, ProvinceData>();
        foreach (var province in provinces) provinceDictionary.Add(province.Id, province);

        var minDistanceToProvince = new Dictionary<int, float>();
        var parent = new Dictionary<int, int>();
        var visited = new Dictionary<int, bool>();
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="30">

---

Next, we set initial values for these dictionaries. All distances are set to infinity, parents to -1, and visited status to false.

```

        foreach (var province in provinceDictionary)
        {
            minDistanceToProvince.Add(province.Key, float.MaxValue);
            parent.Add(province.Key, -1);
            visited.Add(province.Key, false);
        }
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="37">

---

We start the algorithm by setting the distance to the starting province to 0.

```

        minDistanceToProvince[a] = 0;


        for (var i = 0; i < minDistanceToProvince.Count; i++)
        {
            var currProvince = -1;
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="44">

---

We then iterate to find the province with the minimum distance that hasn't been visited yet.

```

            foreach (var j in minDistanceToProvince)
                if (!visited[j.Key] && (currProvince == -1 ||
                                        minDistanceToProvince[j.Key] < minDistanceToProvince[currProvince]))
                    currProvince = j.Key;

            if (System.Math.Abs(currProvince - float.MaxValue) < eps)
                break;
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="52">

---

For each neighboring province, we calculate the distance and update the minimum distance and parent if a shorter path is found.

```

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
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="65">

---

After processing all neighbors, we mark the current province as visited.

```

            visited[currProvince] = true;
        }

        // Recover the shortest path from a to b 

        var path = new List<int> { b };
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="72">

---

Finally, we recover the shortest path by backtracking from the destination province to the start.

```

        for (var currProvince = b; parent[currProvince] != a; currProvince = parent[currProvince])
        {
            if (parent[currProvince] == -1)
                return new int[0];
            path.Add(parent[currProvince]);
        }
```

---

</SwmSnippet>

<SwmSnippet path="Scripts/Utils/Math/PathFinder.cs" line="79">

---

We add the starting province to the path and return the path as an array.

```

        path.Add(a);

        return path.ToArray();
    }
```

---

</SwmSnippet>

# Connection check method

<SwmSnippet path="Scripts/Utils/Math/PathFinder.cs" line="92">

---

The <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="92:7:7" line-data="    public static bool CheckConnectionFromAToB(int a, int b, ProvinceData[] provinces)">`CheckConnectionFromAToB`</SwmToken> method checks if there is a path between two provinces using DFS.

```
    public static bool CheckConnectionFromAToB(int a, int b, ProvinceData[] provinces)
    {
        var provinceDictionary = new Dictionary<int, ProvinceData>();
        foreach (var province in provinces) provinceDictionary.Add(province.Id, province);
        var visited = new Dictionary<int, bool>();
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="97">

---

We initialize the visited dictionary and call the <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="99:1:1" line-data="        _dfs(a, provinceDictionary, visited);">`_dfs`</SwmToken> helper method to perform the depth-first search.

```

        foreach (var province in provinceDictionary) visited.Add(province.Key, false);
        _dfs(a, provinceDictionary, visited);

        return visited[b];
    }

    /// <summary>
    /// Depth-first search algorithm.
    /// </summary>
    /// <param name="currProvince"></param>
    /// <param name="provinceDictionary"></param>
    /// <param name="visited"></param>
    private static void _dfs(int currProvince, Dictionary<int, ProvinceData> provinceDictionary,
        Dictionary<int, bool> visited)
    {
        visited[currProvince] = true;
        foreach (var provinceId in provinceDictionary[currProvince].BorderderingProvinces
                     .Where(p => provinceDictionary.ContainsKey(p)))
            if (!visited[provinceId])
                _dfs(provinceId, provinceDictionary, visited);
    }
```

---

</SwmSnippet>

# Connection to all provinces

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="119">

---

The <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="127:13:13" line-data="    public static Dictionary&lt;int, bool&gt; CheckConnectionFromAToOthers(int a, ProvinceData[] provinces)">`CheckConnectionFromAToOthers`</SwmToken> method checks connections from one province to all others.

```


    /// <summary>
    /// Checks the connection from a to others.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="provinces"></param>
    /// <returns></returns>
    public static Dictionary<int, bool> CheckConnectionFromAToOthers(int a, ProvinceData[] provinces)
    {
        var provinceDictionary = new Dictionary<int, ProvinceData>();
        foreach (var province in provinces) provinceDictionary.Add(province.Id, province);
        var visited = new Dictionary<int, bool>();
```

---

</SwmSnippet>

<SwmSnippet path="/Scripts/Utils/Math/PathFinder.cs" line="132">

---

Similar to the previous method, we initialize the visited dictionary and use the <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="99:1:1" line-data="        _dfs(a, provinceDictionary, visited);">`_dfs`</SwmToken> method to mark all reachable provinces.

```

        foreach (var province in provinceDictionary) visited.Add(province.Key, false);
        _dfs(a, provinceDictionary, visited);

        return visited;
    }
}
```

---

</SwmSnippet>

This concludes the walkthrough of the <SwmToken path="/Scripts/Utils/Math/PathFinder.cs" pos="10:6:6" line-data="public static class PathFinder">`PathFinder`</SwmToken> class. The class provides efficient methods for pathfinding and connection checking using well-known graph algorithms.

<SwmMeta version="3.0.0" repo-id="Z2l0aHViJTNBJTNBRXVyb3BlRG9taW5hdGlvbkRlbW8lM0ElM0FQYXp6YW5u" repo-name="EuropeDominationDemo"><sup>Powered by [Swimm](https://app.swimm.io/)</sup></SwmMeta>
