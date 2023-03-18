namespace EuropeDominationDemo.Scripts.Math;

public class GeometryMath
{
    public static float VertexAngleCos(float a, float b, float c)
    {
        return (a*a + b*b - c*c)/(2 * a * b);
    }
}