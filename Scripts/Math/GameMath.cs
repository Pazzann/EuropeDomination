using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios;
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

    public static BezierCurve FindBezierCurve(ProvinceData[] countryProvinces)
    {
        var bezierCurve = new BezierCurve();
        float maxAdjacentSidesSum = 0.0f;
        if (countryProvinces.Length == 1)
        {
            return bezierCurve;
        }

        if (countryProvinces.Length == 2)
        {
            bezierCurve.Segment1 = countryProvinces[0].CenterOfWeight;
            bezierCurve.Segment1 = countryProvinces[1].CenterOfWeight;
            bezierCurve.Vertex = (countryProvinces[0].CenterOfWeight - countryProvinces[1].CenterOfWeight) / 2;
            return bezierCurve;
        }

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
                    
                    if (CheckLongest(sides[0], sides[1], sides[2], angles[0], angles[2], angles[1], maxAdjacentSidesSum))
                    {
                        bezierCurve.Segment1 = countryProvinces[j].CenterOfWeight;
                        bezierCurve.Segment2 = countryProvinces[k].CenterOfWeight;
                        bezierCurve.Vertex = countryProvinces[i].CenterOfWeight;
                        maxAdjacentSidesSum = sides[0] + sides[1];
                        GD.Print(bezierCurve);
                        continue;
                    }
                    if (CheckLongest(sides[0], sides[2], sides[1], angles[0], angles[1], angles[2], maxAdjacentSidesSum))
                    {
                        bezierCurve.Segment1 = countryProvinces[i].CenterOfWeight;
                        bezierCurve.Segment2 = countryProvinces[k].CenterOfWeight;
                        bezierCurve.Vertex = countryProvinces[j].CenterOfWeight;
                        maxAdjacentSidesSum = sides[0] + sides[2];
                        continue;
                    }
                    if (CheckLongest(sides[1], sides[2], sides[0], angles[1], angles[0], angles[2], maxAdjacentSidesSum))
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

    public static bool CheckLongest(
        float a, float b, float c,
        float abCos, float bcCos, float acCos,
        float maxCurrLength)
    {
        if (abCos > 0)
            return false;
        if (a + b < maxCurrLength)
            return false;
        if (a + b < b + c && bcCos < 0)
            return false;
        if (a + b < a + c && acCos < 0)
            return false;
        return true;
    }
}