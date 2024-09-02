using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using EuropeDominationDemo.Scripts.Math;
using Godot;

namespace EuropeDominationDemo.Scripts.Text;

public class TextSolver2
{
    private static readonly float[] CandidateAngles = {
        //Mathf.Pi
        0f,
        Mathf.Pi / 4f,
        Mathf.Pi / 6f,
        Mathf.Pi / 8f,
        Mathf.Pi / 10f,
    };
    
    private readonly ILayer _layer;
    private readonly int _areaId;
    private readonly string _text;
    private readonly float _letterAspectRatio;
    
    public TextSolver2(ILayer layer, int areaId, string text, float letterAspectRatio)
    {
        _layer = layer;
        _areaId = areaId;
        _text = text;
        _letterAspectRatio = letterAspectRatio;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public List<CurvedText> FitText()
    {
        return _layer.GetContours(_areaId)
            .Select(contour => FitText(contour, 0.1f))
            .Where(contour => contour != null)
            .Select(contour => contour.Value)
            .ToList();
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private CurvedText? FitText(Polygon contour, float eps)
    {
        var minFontSize = 1f;
        var maxFontSize = 100f;

        CurvedText? bestText = null;

        while (maxFontSize - minFontSize > eps)
        {
            var fontSize = (minFontSize + maxFontSize) / 2;
            var letterSize = new Vector2(_letterAspectRatio * fontSize, fontSize);

            var text = TryFitText(contour, letterSize);

            if (text != null)
            {
                minFontSize = fontSize;
                bestText = text;
            }
            else
                maxFontSize = fontSize;
        }

        //GD.Print(bestText.Value.FontSize);
        return bestText;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private CurvedText? TryFitText(Polygon contour, Vector2 letterSize)
    {
        SolidPath<Sector>? bestPath = null;
        
        for (var i = 0; i < contour.Vertices.Count; ++i)
        {
            for (var j = i + 1; j < contour.Vertices.Count; ++j)
            {
                var a = contour.Vertices[i];
                var b = contour.Vertices[j];
            
                var distance = a.DistanceSquaredTo(b);
                var minDistance = letterSize.X * _text.Length * 1.5f;
                
                if (distance * distance < minDistance * minDistance)
                    continue;
            
                foreach (var angle in CandidateAngles)
                {
                    var (upper, lower) = Sector.WithAngle(a, b, angle);
                    FindOptimalPath(contour, upper, letterSize, ref bestPath);
                    FindOptimalPath(contour, lower, letterSize, ref bestPath);
                    
                    if (bestPath != null)
                        return new CurvedText(_text, letterSize.Y, bestPath);
                }
            }
        }

        return null;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private void FindOptimalPath(Polygon contour, in Sector sector, Vector2 letterSize, ref SolidPath<Sector>? best)
    {
        var arcLength = sector.ArcLength();
        var letterWidth = letterSize.X / arcLength;
        
        var leftMostLetter = 0.5f - letterWidth * (_text.Length / 2f + 1f);
        var rightMostLetter = 0.5f + letterWidth * (_text.Length / 2f + 1f);

        if (leftMostLetter * arcLength < letterSize.X * 1.5f || (1f - rightMostLetter) * arcLength < letterSize.X * 1.5f)
            return;
        
        var cutSector = new Sector(sector.Center, sector.GetPoint(leftMostLetter), sector.GetPoint(rightMostLetter));
        var path = new SolidPath<Sector>(cutSector, letterSize.Y);

        if (!CheckCollisions(path) || !contour.Intersects(path))
            return;

        best = path;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
    private bool CheckCollisions(in SolidPath<Sector> path)
    {
        const int checkPoints = 7;
        
        for (var i = 0; i < 7; ++i)
        {
            if (!ContainsPoint(path.GetPoint(1f / (checkPoints * 2) + i * 1f / checkPoints)))
                return false;
            
            if (!ContainsPoint(path.GetPointUpper(1f / (checkPoints * 2) + i * 1f / checkPoints)))
                return false;
            
            if (!ContainsPoint(path.GetPointLower(1f / (checkPoints * 2) + i * 1f / checkPoints)))
                return false;
        }
        
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    private bool ContainsPoint(Vector2 point)
    {
        return _layer.GetAreaId(point.RoundToInt()) == _areaId;
    }
}