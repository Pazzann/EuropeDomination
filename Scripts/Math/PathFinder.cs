using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Scenarios;

namespace EuropeDominationDemo.Scripts.Math;

public static class PathFinder
{
    public static int[] FindWayFromAToB(int a, int b, Scenario scenario)
    {
        ProvinceData aProvince = scenario.Map[a];
        float[] minimalDistance = new float[scenario.Map.Length];
        
        for (int i = 0; i < minimalDistance.Length; i++)
        {
            minimalDistance[i] = float.MaxValue;
        }

        int currentVertex = a;
        minimalDistance[a] = 0;
        int[] p = new int[scenario.Map.Length];
        bool[] wasVisited = new bool[scenario.Map.Length];
        for ( int i = 0; i < minimalDistance.Length; i++)
        {
            currentVertex = -1;
            for (int j = 0; j < minimalDistance.Length; j++)
            {
                if (!wasVisited[j] && (minimalDistance[j] < minimalDistance[currentVertex] || currentVertex == -1))
                    currentVertex = j;
            }
            
            for (int j = 0; j < scenario.Map[currentVertex].BorderProvinces.Length; j++)
            {
                int provinceId = scenario.Map[currentVertex].BorderProvinces[j];
                float distance = (scenario.Map[currentVertex].CenterOfWeight - scenario.Map[provinceId].CenterOfWeight).Length();
                if (minimalDistance[provinceId] - minimalDistance[currentVertex] - distance > Constants.PRECISION)
                {
                    minimalDistance[provinceId] = minimalDistance[currentVertex] + distance;
                    p[provinceId] = currentVertex;
                }
            }
        }
        List<int> path = new List<int>();

        path.Add(b);
        for (int currentProvince = b; p[currentProvince] != a; currentProvince = p[currentProvince])
        {
            path.Add(p[currentProvince]);
        }

        return path.ToArray().Reverse().ToArray();
    }
}