using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.Text;

public interface ILayer
{
    int GetAreaId(Vector2I tile);
    List<Polygon> GetContours(int areaId);
}

public class MapContours
{
    public ILayer Provinces { get; private set; }
    public ILayer States { get; private set; }
    
    public MapContours(MapData mapData, Image mapImage)
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
            provinceToState.TryAdd(province, state);
        }

        var mapWidth = mapImage.GetWidth();
        var mapHeight = mapImage.GetHeight();

        var provinceId = new int[mapWidth, mapHeight];
        var stateId = new int[mapWidth, mapHeight];

        for (var i = 0; i < mapWidth; ++i)
        {
            for (var j = 0; j < mapHeight; ++j)
            {
                var color = mapImage.GetPixel(i, j);

                if (Mathf.Abs(color.A - 1f) > 0.01f)
                {
                    provinceId[i, j] = stateId[i, j] = -1;
                    continue;
                }

                provinceId[i, j] = GameMath.GetProvinceId(color);
                stateId[i, j] = provinceToState.GetValueOrDefault(provinceId[i, j], -1);
            }
        }

        //Provinces = new Layer(mapWidth, mapHeight, provinceId);
        States = new Layer(mapWidth, mapHeight, stateId);
    }

    private class Layer : ILayer
    {
        private readonly int _width, _height;
        private readonly int[,] _areaId;
        private readonly Dictionary<int, List<Polygon>> _contours;

        public Layer(int width, int height, int[,] areaId)
        {
            if (areaId.Length != width * height)
                throw new ArgumentException("`areaId` length must be equal to `width` * `height`");

            _width = width;
            _height = height;
            _areaId = areaId;
            _contours = new Dictionary<int, List<Polygon>>();

            BuildContours();
        }
        
        public int GetAreaId(Vector2I tile)
        {
            if (!IsInBounds(tile.X, tile.Y))
                return -1;

            return _areaId[tile.X, tile.Y];
        }

        public List<Polygon> GetContours(int areaId)
        {
            return _contours[areaId];
        }

        private void BuildContours()
        {
            var isOnBorder = new bool[_width, _height];
            Span<Vector2I> neighbors = stackalloc Vector2I[8];

            for (var i = 0; i < _width; ++i)
            {
                for (var j = 0; j < _height; ++j)
                {
                    if (_areaId[i, j] == -1)
                        continue;

                    GetTileNeighbors(i, j, ref neighbors);

                    foreach (var neighbor in neighbors)
                    {
                        if (_areaId[i, j] == GetAreaId(neighbor))
                        {
                            isOnBorder[i, j] = true;
                            break;
                        }
                    }
                }
            }

            var graph = new Dictionary<Vector2I, HashSet<Vector2I>>();

            for (var i = 0; i < _width; ++i)
            {
                for (var j = 0; j < _height; ++j)
                {
                    if (isOnBorder[i, j])
                        graph.Add(new Vector2I(i, j), new HashSet<Vector2I>());
                }
            }

            for (var i = 0; i < _width; ++i)
            {
                for (var j = 0; j < _height; ++j)
                {
                    if (_areaId[i, j] == -1 || !isOnBorder[i, j])
                        continue;

                    GetTileNeighbors(i, j, ref neighbors);

                    foreach (var neighbor in neighbors)
                    {
                        if (IsInBounds(neighbor) && isOnBorder[neighbor.X, neighbor.Y] && _areaId[i, j] == GetAreaId(neighbor))
                            graph[new Vector2I(i, j)].Add(neighbor);
                    }
                }
            }

            var component = new int[_width, _height];

            for (var i = 0; i < _width; ++i)
            {
                for (var j = 0; j < _height; ++j)
                {
                    if (_areaId[i, j] != -1)
                        component[i, j] = i * _height + j;
                }
            }

            var dsu = new Dsu(_width * _height);

            for (var i = 0; i < _width; ++i)
            {
                for (var j = 0; j < _height; ++j)
                {
                    if (_areaId[i, j] == -1)
                        continue;

                    GetTileNeighbors(i, j, ref neighbors);

                    foreach (var neighbor in neighbors)
                    {
                        if (_areaId[i, j] == GetAreaId(neighbor))
                            dsu.Union(component[i, j], component[neighbor.X, neighbor.Y]);
                    }
                }
            }

            for (var i = 0; i < _width; ++i)
            {
                for (var j = 0; j < _height; ++j)
                {
                    if (_areaId[i, j] == -1)
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
                
                if (!hulls.ContainsKey(component[x, y]))
                    hulls.Add(component[x, y], new List<Vector2I>());

                hulls[component[x, y]].Add(vertex);

                foreach (var neighbor in graph[vertex].Where(neighbor => !visited.Contains(neighbor)))
                {
                    stack.Push(neighbor);
                    visited.Add(neighbor);
                }
            }

            var mapArea = _width * _height;

            foreach (var hull in hulls.Values)
            {
                if (hull.Count == 0)
                    continue;

                var areaId = _areaId[hull[0].X, hull[0].Y];

                // TODO: use a better area heuristic.
                if ((float)hull.Count * hull.Count / mapArea < 1e-4f)
                    continue;

                var clusterSize = hull.Count / 30;

                if (clusterSize == 0)
                    clusterSize = 1;
                
                GD.Print("cnt: ", hull.Count);

                var vertices = hull
                    .Where((_, i) => i % clusterSize == 0)
                    .Select(vertex => new Vector2(vertex.X, vertex.Y));
                
                if (!_contours.ContainsKey(areaId))
                    _contours.Add(areaId, new List<Polygon>());
                
                _contours[areaId].Add(new Polygon(vertices));
            }
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < _width && y >= 0 && y < _height;
        }
        
        private bool IsInBounds(Vector2I tile)
        {
            return IsInBounds(tile.X, tile.Y);
        }

        private static void GetTileNeighbors(int x, int y, ref Span<Vector2I> neighbors)
        {
            neighbors[0] = new Vector2I(x + 1, y);
            neighbors[1] = new Vector2I(x - 1, y);
            neighbors[2] = new Vector2I(x, y + 1);
            neighbors[3] = new Vector2I(x, y - 1);
            neighbors[4] = new Vector2I(x + 1, y + 1);
            neighbors[5] = new Vector2I(x + 1, y - 1);
            neighbors[6] = new Vector2I(x - 1, y + 1);
            neighbors[7] = new Vector2I(x - 1, y - 1);
        }
    }
}