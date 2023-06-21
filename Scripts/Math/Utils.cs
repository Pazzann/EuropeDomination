namespace EuropeDominationDemo.Scripts.Math;

class Utils {
    private Utils() {}

    public static float det(float a, float b, float c, float d) {
        return a * d - b * c;
    }
}