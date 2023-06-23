using Godot;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Math;
using static System.Math;

namespace EuropeDominationDemo.Scripts;

public class  StateMap
{
	private const int MAX_CONTROL_VERTICES = 100;

	private IDictionary<int, Polygon> stateHulls = new Dictionary<int, Polygon>(); 

	public StateMap(MapData data, Image map)
	{
		var countries = data.Scenario.Countries;

		var stateProvinces = new Dictionary<int, HashSet<int>>();
		var provinceStates = new Dictionary<int, int>();

		foreach (var entry in countries)
			stateProvinces.Add(entry.Value, GameMath.ListIdsFromProvinces(data.Scenario.CountryProvinces(entry.Value)));

		var provinceStateQuery = from entry in stateProvinces
								 from province in entry.Value
								 select (province, entry.Key);

		foreach (var (province, state) in provinceStateQuery) {
			provinceStates.Add(province, state);
		}

		var mapWidth = map.GetWidth();
		var mapHeight = map.GetHeight();

		var getStateId = (int x, int y, int def) => {
			var color = map.GetPixel(x, y);

			if (color.A != 1f)
				return def;

			var provinceId = GameMath.GetProvinceID(color);

			if (provinceId < 0)
				return def;
			
			return provinceStates[provinceId];
		};

		var getStateIdChecked = (int x, int y, int def) => {
			if (x < 0 || x >= mapWidth || y < 0 || y >= mapHeight)
				return def;
			else
				return getStateId(x, y, def);
		};

		bool[,] isOnBorder = new bool[mapWidth, mapHeight];
		int[,] stateId = new int[mapWidth, mapHeight];

		var firstStateCell = new Dictionary<int, Vector2I>();

		for (int i = 0; i < map.GetHeight(); ++i) {
			for (int j = 0; j < map.GetWidth(); ++j) {
				var curState = getStateId(j, i, -1);

				if (curState == -1)
					continue;

				Vector2I[] neighbors = {
					new Vector2I(j + 1, i),
					new Vector2I(j, i + 1),
					new Vector2I(j - 1, i),
					new Vector2I(j, i - 1),
					new Vector2I(j + 1, i - 1),
					new Vector2I(j - 1, i + 1),
					new Vector2I(j + 1, i + 1),
					new Vector2I(j - 1, i - 1)
				};

				int borderDegree = 0;
				
				foreach (var neighbor in neighbors) {
					if (getStateIdChecked(neighbor.X, neighbor.Y, curState) != curState || getStateId(neighbor.X, neighbor.Y, -1) == -1) {
						borderDegree++;
					}
				}

				if (borderDegree >= 1) {
					isOnBorder[j, i] = true;
					stateId[j, i] = curState;

					if (!firstStateCell.ContainsKey(curState))
						firstStateCell.Add(curState, new Vector2I(j, i));
				}
			}
		}

		var stateBorders = new Dictionary<int, List<Vector2I>>();

		bool[,] visited = new bool[mapWidth, mapHeight];

		foreach (var entry in firstStateCell) {
			stateBorders.Add(entry.Key, new List<Vector2I>());

			var queue = new Stack<Vector2I>();
			queue.Push(entry.Value);

			while (queue.Count != 0) {
				var (x, y) = queue.Pop();

				stateBorders[entry.Key].Add(new Vector2I(x, y));
				visited[x, y] = true;

				Vector2I[] neighbors = {
					new Vector2I(x + 1, y),
					new Vector2I(x, y + 1),
					new Vector2I(x - 1, y),
					new Vector2I(x, y - 1),
					new Vector2I(x + 1, y - 1),
					new Vector2I(x - 1, y + 1),
					new Vector2I(x + 1, y + 1),
					new Vector2I(x - 1, y - 1)
				};

				foreach (var neighbor in neighbors) {
					var (nx, ny) = neighbor;

					if (nx >= 0 && nx < mapWidth && ny >= 0 && ny < mapHeight && !visited[nx, ny] && isOnBorder[nx, ny] && stateId[x, y] == stateId[nx, ny])
						queue.Push(new Vector2I(nx, ny));
				}
			}
		}

		foreach (var entry in stateBorders) {
			int step = Max(1, entry.Value.Count / MAX_CONTROL_VERTICES);
			var points = entry.Value; 
			var hull = new Polygon();

			for (int j = 0; j < points.Count; j += step)
				hull.AddVertex(new Vector2(points[j].X, points[j].Y));

			hull.SortVertices();
			stateHulls.Add(entry.Key, hull);
		}
	}

	public Polygon GetStateBorder(int state) {
		return stateHulls[state];
	}
}
