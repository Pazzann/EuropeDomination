using EuropeDominationDemo.Scripts.Math;
using Godot;

namespace EuropeDominationTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.That(GameMath.GetProvinceId(new Color("#010000")), Is.EqualTo(0));
        Assert.That(GameMath.GetProvinceId(new Color("#010100")), Is.EqualTo(256));
        Assert.Pass();
    }
}