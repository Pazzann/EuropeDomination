using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios;
using EuropeDominationDemo.Scripts;
using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public class GameMath
{
    public static int GetProvinceID(Color color)
    {
        return (int)((color.R + color.G * 256.0f) * 255.0f) - 1;
    }

    public static Vector2[] CalculateCenterOfProvinceWeight(Image mapTexture, int provinceCount)
    {
        int[] xCoords = new int[provinceCount];
        int[] yCoords = new int[provinceCount];
        int[] sumPixels = new int[provinceCount];
        for (int y = 1; y < mapTexture.GetHeight(); y++)
        {
            for (int x = 1; x < mapTexture.GetWidth(); x++)
            {
                Color pixel = mapTexture.GetPixelv(new Vector2I(x, y));
                if (pixel.A < 1.0f)
                    continue;
                var tileId = GetProvinceID(pixel);
                xCoords[tileId] += x;
                yCoords[tileId] += y;
                sumPixels[tileId]++;
            }
        }

        Vector2[] centers = new Vector2[provinceCount];

        for (int i = 0; i < provinceCount; i++)
        {
            centers[i] = new Vector2(xCoords[i] / sumPixels[i], yCoords[i] / sumPixels[i]);
        }

        return centers;
    }

    public static Vector2 CalculateCenterOfStateWeight(Image mapTexture, HashSet<int> provincesIdsOfState)
    {
        int xCoords = 0;
        int yCoords = 0;
        int sumPixels = 0;
        for (int y = 1; y < mapTexture.GetHeight(); y++)
        {
            for (int x = 1; x < mapTexture.GetWidth(); x++)
            {
                Color pixel = mapTexture.GetPixelv(new Vector2I(x, y));
                if (pixel.A < 1.0f)
                    continue;
                var tileId = GetProvinceID(pixel);
                if (provincesIdsOfState.Contains(tileId))
                {
                    xCoords += x;
                    yCoords += y;
                    sumPixels++;
                }
            }
        }

        if (sumPixels == 0)
        {
            return Vector2.Zero;
        }

        return new Vector2(xCoords / sumPixels, yCoords / sumPixels);
    }

    public static int ClosestIdCenterToPoint(ProvinceData[] countryProvinces, Vector2 center)
    {
        int res = 0;
        for (int i = 0; i < countryProvinces.Length; i++)
        {
            if ((center - countryProvinces[res].CenterOfWeight).Length() >
                (center - countryProvinces[i].CenterOfWeight).Length())
            {
                res = i;
            }
        }

        return res;
    }

    public static (ThickArc, float) FindSuitableTextPath(int stateId, StateMap map, float letterAspectRatio, int letterCount) {
        return (null, 30f);
        var border = map.GetStateBorder(stateId);
        var vertices = border.Vertices;
        var center = new Vector2();

        var bestTextSize = 0f;
        ThickArc bestPath = null;

        var findPath = (Vector2 p0, Vector2 p1, Vector2 p2) => {
            var arc = new Arc(p0, p1, p2);

            var test = (float letterHeight) => {
                var path = ThickArc.fitText(arc, letterCount, new Vector2(letterHeight * letterAspectRatio, letterHeight));
                return path != null && !border.Intersects(path);
            };

            float left = 1f, right = 100f;

            while (right - left > 0.01f) {
                float mid = (left + right) / 2;

                if (test(mid))
                    left = mid;
                else
                    right = mid;
            }

            var fontSize = left;
            var path = ThickArc.fitText(arc, letterCount, new Vector2(fontSize * letterAspectRatio, fontSize));

            if ((fontSize > bestTextSize && path != null) || bestPath == null) {
                bestPath = path;
                bestTextSize = fontSize;
            }
        };

        for (int i = 0; i < vertices.Count; ++i) {
            for (int j = 0; j < vertices.Count; ++j) {
                if (i == j)
                    continue;

                var p0 = vertices[i];
                var p1 = vertices[j];
                var p2 = center;

                if (p0.IsEqualApprox(p1) || p1.IsEqualApprox(p2) || p0.IsEqualApprox(p2))
                    continue;

                findPath(p0, p1, p2);
                findPath(p0, p2, p1);
                findPath(p1, p0, p2);
            }
        }

        return (bestPath, bestTextSize);
    }

    public static BezierCurve FindBezierCurve(ProvinceData[] countryProvinces)
    {
        var bezierCurve = BezierCurve.GetDefault();
        float maxAdjacentSidesSum = 0.0f;
        if (countryProvinces.Length == 1)
            return bezierCurve;
        if (countryProvinces.Length == 2)
            return bezierCurve;

        for (int i = 0; i < countryProvinces.Length; i++)
        {
            for (int j = i + 1; j < countryProvinces.Length; j++)
            {
                for (int k = j + 1; k < countryProvinces.Length; k++)
                {
                    var sides = new float[3];
                    sides[0] = (countryProvinces[i].CenterOfWeight - countryProvinces[j].CenterOfWeight).Length();
                    sides[1] = (countryProvinces[i].CenterOfWeight - countryProvinces[k].CenterOfWeight).Length();
                    sides[2] = (countryProvinces[j].CenterOfWeight - countryProvinces[k].CenterOfWeight).Length();

                    var angles = new float[3];
                    angles[0] = GeometryMath.VertexAngleCos(sides[0], sides[1], sides[2]);
                    angles[1] = GeometryMath.VertexAngleCos(sides[0], sides[2], sides[1]);
                    angles[2] = GeometryMath.VertexAngleCos(sides[1], sides[2], sides[0]);

                    if (sides[0] + sides[1] > maxAdjacentSidesSum && angles[0] <= -0.5)
                    {
                        bezierCurve.Segment1 = countryProvinces[j].CenterOfWeight;
                        bezierCurve.Segment2 = countryProvinces[k].CenterOfWeight;
                        bezierCurve.Vertex = countryProvinces[i].CenterOfWeight;
                        maxAdjacentSidesSum = sides[0] + sides[1];
                        continue;
                    }

                    if (sides[0] + sides[2] > maxAdjacentSidesSum && angles[1] <= -0.5)
                    {
                        bezierCurve.Segment1 = countryProvinces[i].CenterOfWeight;
                        bezierCurve.Segment2 = countryProvinces[k].CenterOfWeight;
                        bezierCurve.Vertex = countryProvinces[j].CenterOfWeight;
                        maxAdjacentSidesSum = sides[0] + sides[2];
                        continue;
                    }

                    if (sides[1] + sides[2] > maxAdjacentSidesSum && angles[2] <= -0.5)
                    {
                        bezierCurve.Segment1 = countryProvinces[j].CenterOfWeight;
                        bezierCurve.Segment2 = countryProvinces[i].CenterOfWeight;
                        bezierCurve.Vertex = countryProvinces[k].CenterOfWeight;
                        maxAdjacentSidesSum = sides[1] + sides[2];
                    }
                }
            }
        }

        return bezierCurve;
    }

    public static Vector2[] FindSquarePointsInsideState(ProvinceData[] countryProvinces, Image mapTexture,
        int squareSide)
    {
        List<Vector2> points = new List<Vector2>();
        var ids = ListIdsFromProvinces(countryProvinces);
        for (int y = squareSide; y < mapTexture.GetHeight(); y += squareSide)
        {
            for (int x = squareSide; x < mapTexture.GetWidth(); x += squareSide)
            {
                Color pixel = mapTexture.GetPixelv(new Vector2I(x, y));
                if (pixel.A < 1.0f)
                    continue;
                var tileId = GetProvinceID(pixel);
                if (ids.Contains(tileId))
                    points.Add(new Vector2(x, y));
            }
        }

        return points.ToArray();
    }

    public static BezierCurve FindBezierCurveFromPoints(Vector2[] points)
    {
        var bezierCurve = BezierCurve.GetDefault();
        float maxAdjacentSidesSum = 0.0f;

        for (int i = 0; i < points.Length; i++)
        {
            for (int j = i + 1; j < points.Length; j++)
            {
                for (int k = j + 1; k < points.Length; k++)
                {
                    var sides = new float[3];
                    sides[0] = (points[i] - points[j]).Length();
                    sides[1] = (points[i] - points[k]).Length();
                    sides[2] = (points[j] - points[k]).Length();

                    var angles = new float[3];
                    angles[0] = GeometryMath.VertexAngleCos(sides[0], sides[1], sides[2]);
                    angles[1] = GeometryMath.VertexAngleCos(sides[0], sides[2], sides[1]);
                    angles[2] = GeometryMath.VertexAngleCos(sides[1], sides[2], sides[0]);

                    if (sides[0] + sides[1] > maxAdjacentSidesSum && angles[0] <= 0)
                    {
                        bezierCurve.Segment1 = points[j];
                        bezierCurve.Segment2 = points[k];
                        bezierCurve.Vertex = points[i];
                        maxAdjacentSidesSum = sides[0] + sides[1];
                        continue;
                    }

                    if (sides[0] + sides[2] > maxAdjacentSidesSum && angles[1] <= 0)
                    {
                        bezierCurve.Segment1 = points[i];
                        bezierCurve.Segment2 = points[k];
                        bezierCurve.Vertex = points[j];
                        maxAdjacentSidesSum = sides[0] + sides[2];
                        continue;
                    }

                    if (sides[1] + sides[2] > maxAdjacentSidesSum && angles[2] <= 0)
                    {
                        bezierCurve.Segment1 = points[j];
                        bezierCurve.Segment2 = points[i];
                        bezierCurve.Vertex = points[k];
                        maxAdjacentSidesSum = sides[1] + sides[2];
                    }
                }
            }
        }

        return bezierCurve;
    }

    public static ProvinceData[] CalculateBorderProvinces(ProvinceData[] map, Image mapTexture)
    {
        Dictionary<int, List<int>> borders = new Dictionary<int, List<int>>();
        int tId;
        for (int y = 1; y < mapTexture.GetHeight() - 1; y++)
        {
            for (int x = 1; x < mapTexture.GetWidth() - 1; x++)
            {
                Color owner = mapTexture.GetPixelv(new Vector2I(x, y));
                tId = GetProvinceID(owner);
                if (owner.A < 1.0f)
                    continue;

                Color a = mapTexture.GetPixelv(new Vector2I(x, y + 1));
                if (owner != a && a.A > 0.5f)
                    tId = GetProvinceID(a);

                a = mapTexture.GetPixelv(new Vector2I(x, y - 1));
                if (owner != a && a.A > 0.5f)
                    tId = GetProvinceID(a);

                a = mapTexture.GetPixelv(new Vector2I(x + 1, y));
                if (owner != a && a.A > 0.5f)
                    tId = GetProvinceID(a);

                a = mapTexture.GetPixelv(new Vector2I(x - 1, y + 1));
                if (owner != a && a.A > 0.5f)
                    tId = GetProvinceID(a);


                if (tId != GetProvinceID(owner))
                    if (borders.ContainsKey(GetProvinceID(owner)))
                    {
                        if (!borders[GetProvinceID(owner)].Contains(tId))
                        {
                            borders[GetProvinceID(owner)].Add(tId);
                        }
                    }
                    else
                    {
                        var l = new List<int>();
                        l.Add(tId);
                        borders.Add(GetProvinceID(owner), l);
                    }
            }
        }

        foreach (KeyValuePair<int, List<int>> entry in borders)
        {
            map[entry.Key].BorderProvinces = entry.Value.ToArray();
        }

        return map;
    }

    public static HashSet<int> ListIdsFromProvinces(ProvinceData[] provinces)
    {
        var ids = new HashSet<int>();
        for (int i = 0; i < provinces.Length; i++)
        {
            ids.Add(provinces[i].Id);
        }

        return ids;
    }
}