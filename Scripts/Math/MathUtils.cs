using System;
using System.Runtime.CompilerServices;
using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public static class MathUtils
{
    private static readonly Random Rng = new();

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static Vector2 VectorCenter(Vector2 posA, Vector2 posB)
    {
        return (posA + posB) / 2;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static Vector2 GetPerpendicular(this Vector2 v)
    {
        return new Vector2(v.Y, -v.X);
    }

    public static Vector2 ProjectOntoLine(this Vector2 p, Vector2 a, Vector2 b)
    {
        var u0 = a;
        var u1 = b - a;
        return (p - u0).Dot(u1) / u1.Dot(u1) * u1 + u0;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public static Vector2I RoundToInt(this Vector2 v)
    {
        var (x, y) = v;
        return new Vector2I(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float RandFloat()
    {
        return Rng.NextSingle();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static int RandIntBetween(int min, int max)
    {
        return Rng.Next(min, max + 1);
    }
}