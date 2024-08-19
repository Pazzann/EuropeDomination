using EuropeDominationDemo.Scripts.Math;
using Godot;

namespace EuropeDominationDemo.Scripts.Text;

public class TextSolver
{
    private static readonly float[] CandidateAngles = {
        Mathf.Pi / 4f,
        Mathf.Pi / 6f,
        Mathf.Pi / 8f,
        Mathf.Pi / 10f,
    };

    private readonly StateMap _map;
    private readonly Polygon _border;

    private readonly int _stateId;
    private readonly string _text;
    private readonly float _letterAspectRatio;

    public TextSolver(StateMap map, string text, int stateId, float letterAspectRatio)
    {
        _map = map;
        _border = map.GetStateBorder(stateId);
        _stateId = stateId;
        _text = text;
        _letterAspectRatio = letterAspectRatio;
    }
    
    public CurvedText FitText(float eps = 0.1f)
    {
        var borderVertices = _border.Vertices;
        
        var bestFontSize = 0f;
        var bestPath = new SolidPath<Sector>(new Sector(), 0f);
        
        var path = new SolidPath<Sector>(new Sector(), 0f);
        
        for (var i = 0; i < borderVertices.Count; i += 2) {
            for (var j = i + 1; j < borderVertices.Count; j += 2) {
                if (i == j)
                    continue;

                var p0 = borderVertices[i];
                var p1 = borderVertices[j];

                if (p0.IsEqualApprox(p1))
                    continue;

                foreach (var angle in CandidateAngles) {
                    var (sector1, sector2) = Sector.WithAngle(p0, p1, angle);
                    float fontSize;

                    if ((fontSize = FindPath(sector1, ref path, eps)) - bestFontSize > eps)
                    {
                        bestFontSize = fontSize;
                        (bestPath, path) = (path, bestPath);
                    }
                    
                    if ((fontSize = FindPath(sector2, ref path, eps)) - bestFontSize > eps)
                    {
                        bestFontSize = fontSize;
                        (bestPath, path) = (path, bestPath);
                    }
                }
            }
        }

        GD.Print("font size: ", bestFontSize);
        return new CurvedText(_text, bestFontSize, bestPath);
    }

    private float FindPath(in Sector sector, ref SolidPath<Sector> path, float eps)
    {
        var minFontSize = 1f;
        var maxFontSize = 50f;
        
        while (maxFontSize - minFontSize > eps)
        {
            var fontSize = (minFontSize + maxFontSize) / 2;
            var letterSize = new Vector2(_letterAspectRatio * fontSize, fontSize);
            
            if (TryFitText(sector, letterSize, ref path))
                minFontSize = fontSize;
            else
                maxFontSize = fontSize;
        }

        return minFontSize;
    }

    private bool TryFitText(in Sector extendedSector, in Vector2 letterSize, ref SolidPath<Sector> path)
    {
        if (_map.GetState(extendedSector.GetPoint(0.5f)) != _stateId)
            return false;

        var arcLength = extendedSector.ArcLength();
        var letterWidth = letterSize.X / arcLength;
        
        var leftMostLetter = 0.5f - letterWidth * (_text.Length / 2f + 1f);
        var rightMostLetter = 0.5f + letterWidth * (_text.Length / 2f + 1f);

        if (leftMostLetter * arcLength < letterSize.X * 1.5f || (1f - rightMostLetter) * arcLength < letterSize.X * 1.5f)
            return false;

        // if (leftMostLetter < 0.1f || rightMostLetter > 0.9f)
        //     return false;

        var sector = new Sector(extendedSector.Center, extendedSector.GetPoint(leftMostLetter), extendedSector.GetPoint(rightMostLetter));
        
        path.Path = sector;
        path.Thickness = letterSize.Y;
        
        return !_border.Intersects(path);
    }
}