﻿using Godot;

namespace EuropeDominationDemo.Scripts;

public static class Utils
{
    public static Vector2I RoundToInt(this Vector2 v)
    {
        var (x, y) = v;
        return new Vector2I(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
}

public class Dsu
{
    private readonly int[] _parent;
    private readonly int[] _size;

    public Dsu(int count)
    {
        _parent = new int[count];
        _size = new int[count];

        for (var i = 0; i < count; ++i)
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