using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Text;

public class StateMap
{
    //private const int MaxVerticesPerHull = 30;

    private readonly Dictionary<int, List<Polygon>> _stateContours;
    private readonly int[,] _stateId;

    private readonly int _mapWidth, _mapHeight;

    public StateMap(MapData mapData, Image mapImage)
    {
        var countries = mapData.Scenario.Countries;

        var stateProvinces = new Dictionary<int, HashSet<int>>();
        var provinceToState = new Dictionary<int, int>();

        foreach (var entry in countries)
        {
            var provinces = GameMath.ListIdsFromProvinces(mapData.Scenario.CountryProvinces(entry.Value.Id));
            stateProvinces.Add(entry.Value.Id, provinces);
        }

        var provinceStateQuery =
            from entry in stateProvinces
            from province in entry.Value
            select (province, entry.Key);

        foreach (var (province, state) in provinceStateQuery)
        {
            provinceToState.Add(province, state);
        }

        _mapWidth = mapImage.GetWidth();
        _mapHeight = mapImage.GetHeight();

        _stateId = new int[_mapWidth, _mapHeight];
        _stateContours = new Dictionary<int, List<Polygon>>();

        BuildContours(mapImage, provinceToState);
    }

    public IReadOnlyList<Polygon> GetStateContours(int state)
    {
        return _stateContours[state];
    }

    public int GetStateId(Vector2I pos)
    {
        if (!IsInBounds(pos))
            return -1;

        return _stateId[pos.X, pos.Y];
    }

    private void BuildContours(Image map, IReadOnlyDictionary<int, int> provinceToState)
    {
        for (var i = 0; i < _mapWidth; ++i)
        {
            for (var j = 0; j < _mapHeight; ++j)
            {
                if (i < 0 || i >= _mapWidth || j < 0 || j >= _mapHeight)
                {
                    _stateId[i, j] = -1;
                    continue;
                }

                var color = map.GetPixel(i, j);

                if (Mathf.Abs(color.A - 1f) > 0.01f)
                {
                    _stateId[i, j] = -1;
                    continue;
                }

                var provinceId = GameMath.GetProvinceId(color);
                _stateId[i, j] = provinceToState[provinceId];
            }
        }
        
        var isOnBorder = new bool[_mapWidth, _mapHeight];
        
        for (var i = 0; i < _mapWidth; ++i)
        {
            for (var j = 0; j < _mapHeight; ++j)
            {
                if (_stateId[i, j] == -1)
                    continue;
                
                var neighbors = new Vector2I[]
                {
                    new(i + 1, j),
                    new(i, j + 1),
                    new(i - 1, j),
                    new(i, j - 1),
                    new(i + 1, j - 1),
                    new(i - 1, j + 1),
                    new(i + 1, j + 1),
                    new(i - 1, j - 1)
                };
                
                if (neighbors.Any(neighbor => GetStateId(neighbor) != _stateId[i, j]))
                {
                    isOnBorder[i, j] = true;
                }
            }
        }

        var graph = new Dictionary<Vector2I, HashSet<Vector2I>>();

        for (var i = 0; i < _mapWidth; ++i)
        {
            for (var j = 0; j < _mapHeight; ++j)
            {
                if (_stateId[i, j] == -1)
                    continue;
                
                var neighbors = new Vector2I[]
                {
                    new(i + 1, j),
                    new(i, j + 1),
                    new(i - 1, j),
                    new(i, j - 1),
                    new(i + 1, j - 1),
                    new(i - 1, j + 1),
                    new(i + 1, j + 1),
                    new(i - 1, j - 1)
                };

                if (isOnBorder[i, j])
                {
                    foreach (var neighbor in neighbors)
                    {
                        if (GetStateId(neighbor) == _stateId[i, j] && isOnBorder[neighbor.X, neighbor.Y])
                        {
                            graph.TryAdd(new Vector2I(i, j), new HashSet<Vector2I>());
                            graph[new Vector2I(i, j)].Add(neighbor);

                            //graph.TryAdd(neighbor, new HashSet<Vector2I>());
                        }
                    }
                }
            }
        }

        var component = new int[_mapWidth, _mapHeight];

        for (var i = 0; i < _mapWidth; ++i)
        {
            for (var j = 0; j < _mapHeight; ++j)
            {
                if (_stateId[i, j] == -1)
                    continue;
                
                component[i, j] = i * _mapHeight + j;
            }
        }

        var dsu = new Dsu(_mapWidth * _mapHeight);

        for (var i = 0; i < _mapWidth; ++i)
        {
            for (var j = 0; j < _mapHeight; ++j)
            {
                if (_stateId[i, j] == -1)
                    continue;
                
                var neighbors = new Vector2I[]
                {
                    new(i + 1, j),
                    new(i, j + 1),
                    new(i - 1, j),
                    new(i, j - 1),
                    new(i + 1, j - 1),
                    new(i - 1, j + 1),
                    new(i + 1, j + 1),
                    new(i - 1, j - 1)
                };

                foreach (var neighbor in neighbors)
                {
                    if (GetStateId(neighbor) == _stateId[i, j])
                    {
                        dsu.Union(component[i, j], component[neighbor.X, neighbor.Y]);
                    }
                }
            }
        }

        for (var i = 0; i < _mapWidth; ++i)
        {
            for (var j = 0; j < _mapHeight; ++j)
            {
                if (_stateId[i, j] == -1)
                    continue;
                
                component[i, j] = dsu.Find(component[i, j]);
            }
        }

        var start = new Dictionary<int, Vector2I>();

        foreach (var vertex in graph.Keys.Where(vertex => !start.ContainsKey(component[vertex.X, vertex.Y])))
        {
            start.Add(component[vertex.X, vertex.Y], vertex);
        }

        var stack = new Stack<Vector2I>();
        var visited = new HashSet<Vector2I>();

        var hulls = new Dictionary<int, List<Vector2I>>();

        foreach (var vertex in start.Values)
            stack.Push(vertex);

        while (stack.Count > 0)
        {
            var vertex = stack.Pop();
            var (x, y) = vertex;

            hulls.TryAdd(component[x, y], new List<Vector2I>());
            hulls[component[x, y]].Add(vertex);

            foreach (var neighbor in graph[vertex].Where(neighbor => !visited.Contains(neighbor)))
            {
                stack.Push(neighbor);
                visited.Add(neighbor);
            }
        }

        var mapArea = _mapWidth * _mapWidth;

        GD.Print("cc: ", hulls.Count);

        foreach (var hull in hulls.Values)
        {
            if (hull.Count == 0)
                continue;

            var state = _stateId[hull[0].X, hull[0].Y];

            if ((float)hull.Count / mapArea < 1e-4f)
                continue;

            //GD.Print("cnt: ", hull.Count);

            var clusterSize = hull.Count / 30;

            if (clusterSize == 0)
                clusterSize = 1;

            var vertices = hull
                .Where((_, i) => i % clusterSize == 0)
                .Select(vertex => new Vector2(vertex.X, vertex.Y));

            _stateContours.TryAdd(state, new List<Polygon>());
            _stateContours[state].Add(new Polygon(vertices));
        }
    }

    private bool IsInBounds(Vector2I pixel)
    {
        return pixel.X >= 0 && pixel.X < _mapWidth && pixel.Y >= 0 && pixel.Y < _mapHeight;
    }
}

internal class Dsu
{
    private readonly int[] _parent;
    private readonly int[] _size;

    public Dsu(int nodeCount)
    {
        _parent = new int[nodeCount];
        _size = new int[nodeCount];

        for (var i = 0; i < nodeCount; ++i)
        {
            _parent[i] = i;
            _size[i] = 1;
        }
    }

    public int Find(int node)
    {
        if (_parent[node] == node)
            return node;

        _parent[node] = Find(_parent[node]);
        return _parent[node];
    }

    public void Union(int a, int b)
    {
        a = Find(a);
        b = Find(b);

        if (a == b)
            return;

        if (_size[a] < _size[b])
            (a, b) = (b, a);

        _size[a] += _size[b];
        _parent[b] = a;
    }
}