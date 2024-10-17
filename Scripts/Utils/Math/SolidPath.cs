using Godot;

namespace EuropeDominationDemo.Scripts.Math;

public readonly struct SolidPath<T> : IPath where T : IPath
{
    private readonly T _path;
    private readonly float _thickness;

    private readonly Vector2 _normal;

    public SolidPath(T path, float thickness)
    {
        _path = path;
        _thickness = thickness;
        _normal = ComputeNormal(_path);
    }

    public Vector2 GetPoint(float t)
    {
        return _path.GetPoint(t);
    }

    public Vector2 GetPointUpper(float t)
    {
        return _path.GetPoint(t) + _normal * (_thickness * 0.5f);
    }

    public Vector2 GetPointLower(float t)
    {
        return _path.GetPoint(t) - _normal * (_thickness * 0.5f);
    }

    public Vector2 GetTangent(float t)
    {
        return _path.GetTangent(t);
    }

    // public Vector2 GetRandomPoint()
    // {
    //     var type = MathUtils.RandIntBetween(0, 1);
    //
    //     switch (type)
    //     {
    //         case 0:
    //         {
    //             var t = MathUtils.RandFloat();
    //             var off = MathUtils.RandFloat() * _thickness - _thickness * 0.5f;
    //             return _path.GetPoint(t) + _normal * off;
    //         }
    //         case 1:
    //         {
    //             var t = MathUtils.RandFloat();
    //             var off = MathUtils.RandFloat() * _thickness + _thickness * 0.5f;
    //             return _path.GetPoint(t) + _normal * off;
    //         }
    //         // case 2:
    //         // {
    //         //     var start = GetPoint(0f);
    //         //     var end = GetPoint(1f);
    //         //     var leftNormal = _path.GetTangent(0f).GetPerpendicular().Normalized();
    //         //     var rightNormal = _path.GetTangent(1f).GetPerpendicular().Normalized();
    //         //     var leftSide = new Segment(start - leftNormal * (_thickness * 0.5f), start + leftNormal * (_thickness * 0.5f));
    //         //     return leftSide.Point0
    //         // }
    //         default:
    //             throw new Exception();
    //     }
    // }

    public bool Intersects(in Segment segment)
    {
        var offset = _normal * (_thickness * 0.5f);
        return _path.Intersects(segment.Translated(offset)) || _path.Intersects(segment.Translated(-offset));

        // var start = GetPoint(0f);
        // var end = GetPoint(1f);

        // var leftNormal = _path.GetTangent(0f).GetPerpendicular().Normalized();
        // var rightNormal = _path.GetTangent(1f).GetPerpendicular().Normalized();
        //
        // var leftSide = new Segment(start - leftNormal * (_thickness * 0.5f), start + leftNormal * (_thickness * 0.5f));
        // var rightSide = new Segment(end - rightNormal * (_thickness * 0.5f), end + rightNormal * (_thickness * 0.5f));
        //
        // return _path.Intersects(segment.Translated(offset)) || _path.Intersects(segment.Translated(-offset)) ||
        //        leftSide.Intersects(segment) || rightSide.Intersects(segment);
    }

    private static Vector2 ComputeNormal(in T path)
    {
        var normal = path.GetTangent(0.5f).GetPerpendicular();
        return normal.Normalized();
    }
}