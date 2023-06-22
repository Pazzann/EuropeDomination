using Godot;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts.Math;
using static System.Math;

namespace EuropeDominationDemo.Scripts;

public class  StateMap
{
	private const int MAX_CONTROL_VERTICES = 10;

	private IDictionary<int, Polygon> stateHulls = new Dictionary<int, Polygon>(); 

	public StateMap(MapData data, Image map)
	{
		var countries = data.Scenario.Countries;

		var stateProvinces = new Dictionary<int, HashSet<int>>();
		var provinceStates = new Dictionary<int, int>();
		var stateBorders = new Dictionary<int, List<Vector2I>>();

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
			if (x > 0 || x >= mapWidth || y < 0 || y >= mapHeight)
				return def;
			else
				return getStateId(x, y, def);
		};

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
					// new Vector2I(j + 1, i - 1),
					// new Vector2I(j - 1, i + 1),
					// new Vector2I(j + 1, i + 1),
					// new Vector2I(j - 1, i - 1)
				};

				var isOnBorder = false;
				
				foreach (var neighbor in neighbors) {
					if (getStateIdChecked(neighbor.X, neighbor.Y, curState) != curState || getStateId(neighbor.X, neighbor.Y, -1) == -1) {
						isOnBorder = true;
						break;
					}
				}

				if (isOnBorder) {
					if (!stateBorders.ContainsKey(curState))
						stateBorders.Add(curState, new List<Vector2I>());

					stateBorders[curState].Add(new Vector2I(j, i));
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
