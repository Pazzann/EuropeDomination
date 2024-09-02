using System.Runtime.CompilerServices;
using Godot;

namespace EuropeDominationDemo.Scripts;

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

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public int Find(int set)
    {
        if (_parent[set] == set)
            return set;

        _parent[set] = Find(_parent[set]);
        return _parent[set];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    public int GetSize(int set)
    {
        return _size[set];
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
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