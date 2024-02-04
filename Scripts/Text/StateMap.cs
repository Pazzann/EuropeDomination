using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;
using static System.Math;

namespace EuropeDominationDemo.Scripts.Text;

public class StateMap
{
	private const int MaxVerticesPerHull = 30;

	private readonly IDictionary<int, Polygon> _states = new Dictionary<int, Polygon>();
	private readonly int[,] _stateId;

	public StateMap(MapData mapData, Image mapImage)
	{
		var countries = mapData.Scenario.Countries;

		var stateProvinces = new Dictionary<int, HashSet<int>>();
		var provinceStates = new Dictionary<int, int>();

		foreach (var entry in countries)
			stateProvinces.Add(entry.Value.Id, GameMath.ListIdsFromProvinces(mapData.Scenario.CountryProvinces(entry.Value.Id)));

		var provinceStateQuery = 
			from entry in stateProvinces
			from province in entry.Value
			select (province, entry.Key);

		foreach (var (province, state) in provinceStateQuery) {
			provinceStates.Add(province, state);
		}

		var mapWidth = mapImage.GetWidth();
		var mapHeight = mapImage.GetHeight();

		int GetStateId(int x, int y, int def, float eps = 0.01f)
		{
			if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
				return def;
			
			var color = mapImage.GetPixel(x, y);

			// Special case for border & void pixels
			if (Mathf.Abs(color.A - 1f) > eps) 
				return def;

			var provinceId = GameMath.GetProvinceId(color);
			return provinceId < 0 ? def : provinceStates[provinceId];
		}

		var isOnBorder = new bool[mapWidth, mapHeight];
		_stateId = new int[mapWidth, mapHeight];

		var firstStateCell = new Dictionary<int, Vector2I>();

		for (var i = 0; i < mapImage.GetHeight(); ++i) {
			for (var j = 0; j < mapImage.GetWidth(); ++j) {
				var curState = GetStateId(j, i, -1);
				_stateId[j, i] = curState;

				if (curState == -1)
					continue;

				var neighbors = new Vector2I[] {
					new(j + 1, i),
					new(j, i + 1),
					new(j - 1, i),
					new(j, i - 1),
					new(j + 1, i - 1),
					new(j - 1, i + 1),
					new(j + 1, i + 1),
					new(j - 1, i - 1)
				};

				var borderDegree = 0;
				
				foreach (var neighbor in neighbors) {
					if (GetStateId(neighbor.X, neighbor.Y, curState) != curState || GetStateId(neighbor.X, neighbor.Y, -1) == -1) {
						borderDegree++;
					}
				}

				if (borderDegree >= 1) {
					isOnBorder[j, i] = true;

					if (!firstStateCell.ContainsKey(curState))
						firstStateCell.Add(curState, new Vector2I(j, i));
				}
			}
		}

		var stateBorders = new Dictionary<int, List<Vector2I>>();
		var visited = new bool[mapWidth, mapHeight];

		foreach (var entry in firstStateCell) {
			stateBorders.Add(entry.Key, new List<Vector2I>());

			var stack = new Stack<Vector2I>();
			stack.Push(entry.Value);

			while (stack.Count != 0) {
				var (x, y) = stack.Pop();

				stateBorders[entry.Key].Add(new Vector2I(x, y));
				visited[x, y] = true;

				var neighbors = new Vector2I[] {
					new(x + 1, y),
					new(x, y + 1),
					new(x - 1, y),
					new(x, y - 1),
					new(x + 1, y - 1),
					new(x - 1, y + 1),
					new(x + 1, y + 1),
					new(x - 1, y - 1)
				};

				foreach (var neighbor in neighbors) {
					var (nx, ny) = neighbor;

					if (nx >= 0 && nx < mapWidth && ny >= 0 && ny < mapHeight && !visited[nx, ny] && isOnBorder[nx, ny] && _stateId[x, y] == _stateId[nx, ny])
						stack.Push(new Vector2I(nx, ny));
				}
			}
		}

		foreach (var (key, points) in stateBorders) {
			var step = Max(1, points.Count / MaxVerticesPerHull);
			var hull = new Polygon();

			for (var i = 0; i < points.Count; i += step) {
				var sum = new Vector2();
				var clusterSize = Min(step, points.Count - i);

				for (var j = i; j < i + clusterSize; ++j)
					sum += new Vector2(points[j].X, points[j].Y);

				hull.AddVertex(sum / clusterSize);
			}

			hull.SortVertices();
			_states.Add(key, hull);
		}
	}

	public Polygon GetStateBorder(int state) {
		return _states[state];
	}

	public int GetState(Vector2 pos) {
		return _stateId[Mathf.RoundToInt(pos.X), Mathf.RoundToInt(pos.Y)];
	}
}
