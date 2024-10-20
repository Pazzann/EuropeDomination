using System;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenes;
using Godot;
using NUnit.Framework;


namespace Tests;


public class GameMathTests
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

    [Test]
    public void Test2()
    {
        int i = 0;
        Console.WriteLine("Testsss");
        GD.Print("NUnitTest");
        var main = (MainScene)GD.Load<PackedScene>("res://main.tscn").Instantiate();
        Assert.That("main", Is.EqualTo(main.Name.ToString()));
    }

}