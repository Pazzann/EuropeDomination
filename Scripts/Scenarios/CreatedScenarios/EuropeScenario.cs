using System;
using System.Collections.Generic;
using System.Linq;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.GlobalStates;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Army.Regiments;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;
using EuropeDominationDemo.Scripts.Scenarios.Goods;
using EuropeDominationDemo.Scripts.Scenarios.Goods.Weapon.LandWeapon;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using EuropeDominationDemo.Scripts.Scenarios.Technology;
using Godot;
using Steamworks;

namespace EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;

[Serializable]
public class EuropeScenario : Scenario
{
    public EuropeScenario()
    {
        
        Settings = new ScenarioSettings();
        
        Goods =
        [
            new HarvestedGood(0, "Iron", new Vector3(0.5f, 0.3f, 0.0f), 4.5f),
            new HarvestedGood(1, "Wheat", new Vector3(0.7f, 0.8f, 0.0f), 3.5f),
            new HarvestedGood(2, "Wood", new Vector3(0.0f, 0.7f, 0.4f), 2.5f),
            new InfantryWeapon(3, "Iron Sword", new Vector3(1.0f, 0.2f, 0.3f), 1.5f, 0.01f, 0.01f, 0.0f, 1.0f,
                Modifiers.DefaultModifiers(additionalTrainingEfficiency: 1.3f), 10),
            new ConsumableGood(4, "Tea", new Vector3(0.7f, 0.9f, 0.3f), 0.5f, Modifiers.DefaultModifiers(productionEfficiency: 1.5f, maxManpowerEfficiency: 1.5f), 4f),
            new HarvestedGood(5, "Stone", new Vector3(0.0f, 0.5f, 0.7f), 2.5f),
            new HarvestedGood(6, "Flax", new Vector3(0.2f, 0.8f, 0.4f), 2.5f),
            new HarvestedGood(7, "Cotton", new Vector3(0.7f, 0.7f, 0.7f), 2.5f),
            new HarvestedGood(8, "Coal", new Vector3(0.1f, 0.1f, 0.1f), 2.5f),
            new HarvestedGood(9, "Salt", new Vector3(0.9f, 0.9f, 0.9f), 2.5f)
        ];


        Date = new DateTime(1700, 1, 1);

        
        WastelandProvinceColors = new Dictionary<int, Vector3>
        {
            { 230, new Vector3(1.0f, 1.0f, 0.0f) },
            { 333, new Vector3(0.0f, 0.5f, 1.0f) }
        };
        WaterColor = new Vector3(0.2f, 0.5f, 1.0f);
        UncolonizedColor = new Vector3(0.7f, 1.0f, 1.0f);

        Recipes = new Recipe[] {
            new(0,new Dictionary<int, double>
            {
                { 0, 1 },
                { 2, 0.5 }
            }, 3, 1f),
            new(1,new Dictionary<int, double>
            {
                { 0, 1 }
            }, 4, 4f)
        };

        Battles = new List<BattleData>();

        Terrains = [new("Mountains", 0, new Vector3(0.1f, 0.0f, 0.0f), Modifiers.DefaultModifiers()), new("Plain", 1, new Vector3(0.2f, 0.2f, 0.6f), Modifiers.DefaultModifiers()), new("Forest", 2, new Vector3(0.4f, 0.2f, 0.0f), Modifiers.DefaultModifiers()), new("Field", 3, new Vector3(0.4f, 0.8f, 0.0f), Modifiers.DefaultModifiers()), new("Coast", 4, new Vector3(0.1f, 0.8f, 0.3f), Modifiers.DefaultModifiers())
        ];

        Buildings =
        [
            new("Workshop", 0, 100, Good.DefaultGoods(Goods.Length), 100, 0,
                false, Modifiers.DefaultModifiers(1.5f))
        ];
        
        TechnologyTrees = new[]
        {
            new TechnologyTree("Economy",new List<TechnologyLevel>
            {
                new(new List<Technology.Technology>
                {
                    new("Way of being efficient",Modifiers.DefaultModifiers(productionEfficiency: 1.5f), 100, 100, Good.DefaultGoods(Goods.Length))
                }, new DateTime(1701, 1, 1)),
                new(new List<Technology.Technology>
                {
                    new("A weapon of not destruction",Modifiers.DefaultModifiers(), 100, 100, Good.DefaultGoods(Goods.Length, new Dictionary<int, double>(){{0, 2.5f}}), recipyToUnlock: 0),
                    new("Time to build", Modifiers.DefaultModifiers(), 100, 100, Good.DefaultGoods(Goods.Length), buildingToUnlock: 0)
                }, new DateTime(1702, 1, 1))
            })
        };

        Map = new ProvinceData.ProvinceData[421]
        {
            new UncolonizedProvinceData(0, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(1, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(2, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(3, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(4, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(5, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(6, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(7, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(8, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(9, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(10, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(11, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(12, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(13, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(14, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(15, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(16, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(17, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(18, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(19, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(20, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(21, "London", 2, 2,Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(22, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(23,  "London", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(24, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(25, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(26, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(27, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(28, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(29, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(30, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(31, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(32, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(33, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(34, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(35, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(36, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(37, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(38, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(39, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(40, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(41, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(42, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(43, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(44, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(45, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(46, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(47, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(48, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(49, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(50, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(51, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(52, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(53, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(54, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(55, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(56, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(57, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(58, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(59, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(60, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(61, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(62, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(63, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(64, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(65, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(66, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(67, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(68, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(69, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(70, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(71, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(72, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(73, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(74, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(75, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(76, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(77, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(78, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(79, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(80, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(81, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(82, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(83, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(84, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(85, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(86, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(87, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(88, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(89, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(90, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(91, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(92, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(93, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(94, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(95, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(96, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(97, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(98, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(99, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(100, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(101, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(102, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(103, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(104, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(105, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(106, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(107, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(108, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(109, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(110, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(111, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(112, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(113, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(114, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(115, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(116, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(117, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(118, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(119, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(120, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(121, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(122, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(123, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(124, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(125, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(126, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(127, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(128, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(129, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(130, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(131, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(132, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(133, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(134, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(135, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(136, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(137, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(138, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(139, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(140, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(141, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(142, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(143, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(144, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(145, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(146, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(147, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(148, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(149, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(150, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(151, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(152, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(153, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(154, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(155, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(156, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(157, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(158, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(159, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(160, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(161, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(162, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(163, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(164, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(165, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(166, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(167, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(168, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(169, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(170, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(171, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(172, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(173, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(174, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(175, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(176, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(177, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(178, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(179, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(180, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(181, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(182, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(183, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(184, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(185, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(186, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(187, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(188, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(189, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(190, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(191, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(192, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(193, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(194, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(195, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(196, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(197, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(198, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(199, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(200, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(201, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(202, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(203, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(204, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(205, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(206, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(207, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(208, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(209, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(210, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(211, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(212, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(213, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(214, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(215, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(216, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(217, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(218, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(219, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(220, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(221, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(222, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(223, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(224, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(225, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(226, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(227, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(228, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(229, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new WastelandProvinceData(230, "DefaultName"),
            new UncolonizedProvinceData(231, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(232, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(233, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(234, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(235, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(236, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(237, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(238, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(239, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(240, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(241, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(242, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(243, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(244, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(245, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(246, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(247, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(248, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(249, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(250, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(251, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(252, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(253, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(254, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(255, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(256, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(257, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(258, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(259, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(260, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(261, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(262, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(263, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(264, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(265, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(266, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(267, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(268, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(269, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(270, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(271, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(272, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(273, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(274, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(275, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(276, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(277, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(278, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(279, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(280, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(281, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(282, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(283, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(284, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(285, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(286, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(287, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(288, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(289, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(290, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(291, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(292, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(293, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(294, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(295, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(296, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(297, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(298, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(299, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(300, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(301, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(302, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(303, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(304, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(305, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(306, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(307, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(308, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(309, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(310, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(311, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(312, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(313, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(314, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(315, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(316, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(317, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(318, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(319, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(320, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(321, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(322, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(323, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(324, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(325, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(326, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(327, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(328, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(329, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(330, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(331, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new UncolonizedProvinceData(332, "DefaultName", 2, 2, Modifiers.DefaultModifiers()),
            new WastelandProvinceData(333, "DefaultName"),
            new SeaProvinceData(334, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(335, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(336, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(337, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(338, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(339, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(340, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(341, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(342, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(343, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(344, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(345, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(346, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(347, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(348, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(349, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(350, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(351, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(352, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(353, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(354, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(355, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(356, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(357, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(358, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(359, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(360, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(361, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(362, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(363, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(364, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(365, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(366, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(367, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(368, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(369, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(370, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(371, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(372, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(373, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(374, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(375, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(376, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(377, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(378, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(379, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(380, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(381, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(382, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(383, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(384, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(385, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(386, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(387, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(388, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(389, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(390, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(391, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(392, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(393, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(394, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(395, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(396, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(397, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(398, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(399, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(400, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(401, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(402, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(403, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(404, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(405, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(406, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(407, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(408, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(409, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(410, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(411, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(412, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(413, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(414, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(415, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(416, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(417, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(418, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(419, "BlackSea", Modifiers.DefaultModifiers()),
            new SeaProvinceData(420, "BlackSea", Modifiers.DefaultModifiers())
        };

        Map = GameMath.CalculateBorderProvinces(Map, GlobalResources.MapTexture);
        var centers = GameMath.CalculateCenterOfProvinceWeight(GlobalResources.MapTexture, Map.Length);
        for (var i = 0; i < Map.Length; i++) Map[i].CenterOfWeight = centers[i];


       
        
        Countries = new Dictionary<int, CountryData>
        {
            {
                0,
                new CountryData(0, "Great Britain", new Vector3(0.0f, 1.0f, 0.0f), Modifiers.DefaultModifiers(), 1000,
                    1200, new List<General>(), new List<Admiral>(), new List<UnitData>(), new List<Template>(),
                    new Dictionary<int, List<DiplomacyAgreement>>(), 0, new Dictionary<Vector3I, int>(), new List<int>(), new List<int>(), Modifiers.DefaultModifiers(), new Dictionary<int, bool>())
            },
            {
                1,
                new CountryData(1, "France", new Vector3(0.0f, 0.0f, 1.0f), Modifiers.DefaultModifiers(), 200, 500,
                    new List<General>(), new List<Admiral>(), new List<UnitData>(), new List<Template>(),
                    new Dictionary<int, List<DiplomacyAgreement>>(), 1, new Dictionary<Vector3I, int>(), new List<int>(), new List<int>(), Modifiers.DefaultModifiers(), new Dictionary<int, bool>())
            },
            {
                2,
                new CountryData(2, "Sweden", new Vector3(1.0f, 0.0f, 0.0f), Modifiers.DefaultModifiers(), 300, 600,
                    new List<General>(), new List<Admiral>(), new List<UnitData>(), new List<Template>(),
                    new Dictionary<int, List<DiplomacyAgreement>>(), 2, new Dictionary<Vector3I, int>(), new List<int>(), new List<int>(), Modifiers.DefaultModifiers(), new Dictionary<int, bool>())
            }
        };
        
    }

    

    public override Dictionary<int, CountryData> Countries { get; }
    public sealed override ProvinceData.ProvinceData[] Map { get; set; }
    public sealed override DateTime Date { get; set; }
    public override Dictionary<int, Vector3> WastelandProvinceColors { get; set; }
    public override Vector3 WaterColor { get; set; }
    public override Vector3 UncolonizedColor { get; set; }

    public override Good[] Goods { get; }
    public override Terrain[] Terrains { get; }
    public override Building[] Buildings { get; }

    public override Recipe[] Recipes { get; set; }
    public override List<BattleData> Battles { get; set; }
    public override TechnologyTree[] TechnologyTrees { get; }
    public override Dictionary<ulong, int> PlayerList { get; set; }
    public sealed override ScenarioSettings Settings { get; set; }
}