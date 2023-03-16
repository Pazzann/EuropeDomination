using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Scenarios;
using Godot;

namespace EuropeDominationDemo.Scripts.GameMath;

public class GameMath
{
    public static int GetProvinceID(Color color){
        return (int)((color.R + color.G*256.0f)*255.0f) - 1;
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
                if(pixel.A < 1.0f)
                    continue;
                var tileId = GetProvinceID(pixel);
                xCoords[tileId] += x;
                yCoords[tileId] += y;
                sumPixels[tileId]++;
            }
        }

        Vector2[] centers = new Vector2[provinceCount];
        
        for(int i = 0; i < provinceCount; i++)
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
                if(pixel.A < 1.0f)
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
        
        return new Vector2(xCoords/sumPixels, yCoords/sumPixels);
    }

    public static int ClosestIdCenterToPoint(ProvinceData[] countryProvinces, Vector2 center)
    {
        int res = 0;
        for (int i = 0; i < countryProvinces.Length; i++)
        {
            if ((center + countryProvinces[res].CenterOfWeight).Length() >
                (center + countryProvinces[i].CenterOfWeight).Length())
            {
                res = i;
            }

        }
        return res;
    }
    
}