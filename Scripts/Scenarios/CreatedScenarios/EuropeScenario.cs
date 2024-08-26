﻿using System;
using System.Collections.Generic;
using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Math;
using EuropeDominationDemo.Scripts.Scenarios.Army;
using EuropeDominationDemo.Scripts.Scenarios.Buildings;
using EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
using EuropeDominationDemo.Scripts.Scenarios.SpecialBuildings;
using Godot;

namespace EuropeDominationDemo.Scripts.Scenarios.CreatedScenarios;

public class EuropeScenario : Scenario
{
    public override Dictionary<int, CountryData> Countries { get; }
    public sealed override List<ArmyUnitData> ArmyUnits { get; set; }
    public sealed override ProvinceData.ProvinceData[] Map { get; set; }
    public sealed override DateTime Date { get; set; }
    public sealed override Image MapTexture { get; set; }
    public override Dictionary<int, Vector3> WastelandProvinceColors { get; set; }
    public override Vector3 WaterColor { get; set; }
    public override Vector3 UncolonizedColor { get; set; }


    public EuropeScenario(Image mapTexture)
    {
        MapTexture = mapTexture;

        Date = new DateTime(1444, 11, 12);
        

        Countries = new Dictionary<int, CountryData>()
        {
            {
                0,
                new CountryData(0, "Great Britain", new Vector3(0.0f, 1.0f, 0.0f), Modifiers.DefaultModifiers(), 100,
                    300, new List<General>())
            },
            { 1, new CountryData(1, "France", new Vector3(0.0f, 0.0f, 1.0f), Modifiers.DefaultModifiers(), 200, 200,new List<General>()) },
            { 2, new CountryData(2, "Sweden", new Vector3(1.0f, 0.0f, 0.0f), Modifiers.DefaultModifiers(), 300, 100,new List<General>()) },
        };
        WastelandProvinceColors = new Dictionary<int, Vector3>()
        {
            { 230, new Vector3(1.0f, 1.0f, 0.0f) },
            { 333, new Vector3(0.0f, 0.5f, 1.0f) }
        };
        WaterColor = new Vector3(0.2f, 0.5f, 1.0f);
        UncolonizedColor = new Vector3(0.7f, 1.0f, 1.0f);

        Map = new ProvinceData.ProvinceData[421]
        {
            new LandProvinceData(0, 0, "London", Terrain.Coast, Good.Iron, 10, new float[] { 0, 3 },
                new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(1, 0, "London", Terrain.Field, Good.Iron, 20, new float[] { 0, 1 },
                new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(2, 0, "London", Terrain.Field, Good.Iron, 30, new float[] { 3, 0 },
                new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(3, 0, "London", Terrain.Forest, Good.Iron, 30, new float[] { 0, 3 },
                new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(4, 0, "London", Terrain.Plain, Good.Iron, 30, new float[] { 4, 0 },
                new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(5, 0, "Paris", Terrain.Plain, Good.Iron, 30, new float[] { 0, 4 }, new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(6, 2, "Lorem", Terrain.Plain, Good.Iron, 30, new float[] { 3, 0 }, new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(7, 2, "FlashBang", Terrain.Plain, Good.Wheat, 1, new float[] { 1, 0 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(8, 2, "CommunistPigs", Terrain.Plain, Good.Wheat, 3, new float[] { 0, 5 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(9, 2, "Berlin", Terrain.Mountains, Good.Wheat, 1, new float[] { 5, 0 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(10, 2, "LibertarianTown", Terrain.Mountains, Good.Wheat, 1, new float[] { 6, 0 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(11, 1, "Liberty", Terrain.Mountains, Good.Wheat, 1, new float[] { 0, 6 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(12, 1, "NY", Terrain.Coast, Good.Wheat, 1, new float[] { 7, 0 }, new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(13, 1, "Los Angeles", Terrain.Coast, Good.Wheat, 1, new float[] { 0, 7 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(14, 1, "LibertarianTown", Terrain.Mountains, Good.Wheat, 1, new float[] { 6, 0 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(15, 1, "Liberty", Terrain.Mountains, Good.Wheat, 1, new float[] { 0, 6 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(16, 1, "NY", Terrain.Coast, Good.Wheat, 1, new float[] { 7, 0 }, new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(17, 1, "Los Angeles", Terrain.Coast, Good.Wheat, 1, new float[] { 0, 7 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(18, 1, "LibertarianTown", Terrain.Mountains, Good.Wheat, 1, new float[] { 6, 0 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(19, 1, "Liberty", Terrain.Mountains, Good.Wheat, 1, new float[] { 0, 6 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(20, 1, "NY", Terrain.Coast, Good.Wheat, 1, new float[] { 7, 0 }, new List<Building>(),
                Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new LandProvinceData(21, 1, "Los Angeles", Terrain.Coast, Good.Wheat, 1, new float[] { 0, 7 },
                new List<Building>(), Modifiers.DefaultModifiers(), new SpecialBuilding[3] {null, null, null}, null),
            new UncolonizedProvinceData(22, "DefaultName"),
            new UncolonizedProvinceData(23, "DefaultName"),
            new UncolonizedProvinceData(24, "DefaultName"),
            new UncolonizedProvinceData(25, "DefaultName"),
            new UncolonizedProvinceData(26, "DefaultName"),
            new UncolonizedProvinceData(27, "DefaultName"),
            new UncolonizedProvinceData(28, "DefaultName"),
            new UncolonizedProvinceData(29, "DefaultName"),
            new UncolonizedProvinceData(30, "DefaultName"),
            new UncolonizedProvinceData(31, "DefaultName"),
            new UncolonizedProvinceData(32, "DefaultName"),
            new UncolonizedProvinceData(33, "DefaultName"),
            new UncolonizedProvinceData(34, "DefaultName"),
            new UncolonizedProvinceData(35, "DefaultName"),
            new UncolonizedProvinceData(36, "DefaultName"),
            new UncolonizedProvinceData(37, "DefaultName"),
            new UncolonizedProvinceData(38, "DefaultName"),
            new UncolonizedProvinceData(39, "DefaultName"),
            new UncolonizedProvinceData(40, "DefaultName"),
            new UncolonizedProvinceData(41, "DefaultName"),
            new UncolonizedProvinceData(42, "DefaultName"),
            new UncolonizedProvinceData(43, "DefaultName"),
            new UncolonizedProvinceData(44, "DefaultName"),
            new UncolonizedProvinceData(45, "DefaultName"),
            new UncolonizedProvinceData(46, "DefaultName"),
            new UncolonizedProvinceData(47, "DefaultName"),
            new UncolonizedProvinceData(48, "DefaultName"),
            new UncolonizedProvinceData(49, "DefaultName"),
            new UncolonizedProvinceData(50, "DefaultName"),
            new UncolonizedProvinceData(51, "DefaultName"),
            new UncolonizedProvinceData(52, "DefaultName"),
            new UncolonizedProvinceData(53, "DefaultName"),
            new UncolonizedProvinceData(54, "DefaultName"),
            new UncolonizedProvinceData(55, "DefaultName"),
            new UncolonizedProvinceData(56, "DefaultName"),
            new UncolonizedProvinceData(57, "DefaultName"),
            new UncolonizedProvinceData(58, "DefaultName"),
            new UncolonizedProvinceData(59, "DefaultName"),
            new UncolonizedProvinceData(60, "DefaultName"),
            new UncolonizedProvinceData(61, "DefaultName"),
            new UncolonizedProvinceData(62, "DefaultName"),
            new UncolonizedProvinceData(63, "DefaultName"),
            new UncolonizedProvinceData(64, "DefaultName"),
            new UncolonizedProvinceData(65, "DefaultName"),
            new UncolonizedProvinceData(66, "DefaultName"),
            new UncolonizedProvinceData(67, "DefaultName"),
            new UncolonizedProvinceData(68, "DefaultName"),
            new UncolonizedProvinceData(69, "DefaultName"),
            new UncolonizedProvinceData(70, "DefaultName"),
            new UncolonizedProvinceData(71, "DefaultName"),
            new UncolonizedProvinceData(72, "DefaultName"),
            new UncolonizedProvinceData(73, "DefaultName"),
            new UncolonizedProvinceData(74, "DefaultName"),
            new UncolonizedProvinceData(75, "DefaultName"),
            new UncolonizedProvinceData(76, "DefaultName"),
            new UncolonizedProvinceData(77, "DefaultName"),
            new UncolonizedProvinceData(78, "DefaultName"),
            new UncolonizedProvinceData(79, "DefaultName"),
            new UncolonizedProvinceData(80, "DefaultName"),
            new UncolonizedProvinceData(81, "DefaultName"),
            new UncolonizedProvinceData(82, "DefaultName"),
            new UncolonizedProvinceData(83, "DefaultName"),
            new UncolonizedProvinceData(84, "DefaultName"),
            new UncolonizedProvinceData(85, "DefaultName"),
            new UncolonizedProvinceData(86, "DefaultName"),
            new UncolonizedProvinceData(87, "DefaultName"),
            new UncolonizedProvinceData(88, "DefaultName"),
            new UncolonizedProvinceData(89, "DefaultName"),
            new UncolonizedProvinceData(90, "DefaultName"),
            new UncolonizedProvinceData(91, "DefaultName"),
            new UncolonizedProvinceData(92, "DefaultName"),
            new UncolonizedProvinceData(93, "DefaultName"),
            new UncolonizedProvinceData(94, "DefaultName"),
            new UncolonizedProvinceData(95, "DefaultName"),
            new UncolonizedProvinceData(96, "DefaultName"),
            new UncolonizedProvinceData(97, "DefaultName"),
            new UncolonizedProvinceData(98, "DefaultName"),
            new UncolonizedProvinceData(99, "DefaultName"),
            new UncolonizedProvinceData(100, "DefaultName"),
            new UncolonizedProvinceData(101, "DefaultName"),
            new UncolonizedProvinceData(102, "DefaultName"),
            new UncolonizedProvinceData(103, "DefaultName"),
            new UncolonizedProvinceData(104, "DefaultName"),
            new UncolonizedProvinceData(105, "DefaultName"),
            new UncolonizedProvinceData(106, "DefaultName"),
            new UncolonizedProvinceData(107, "DefaultName"),
            new UncolonizedProvinceData(108, "DefaultName"),
            new UncolonizedProvinceData(109, "DefaultName"),
            new UncolonizedProvinceData(110, "DefaultName"),
            new UncolonizedProvinceData(111, "DefaultName"),
            new UncolonizedProvinceData(112, "DefaultName"),
            new UncolonizedProvinceData(113, "DefaultName"),
            new UncolonizedProvinceData(114, "DefaultName"),
            new UncolonizedProvinceData(115, "DefaultName"),
            new UncolonizedProvinceData(116, "DefaultName"),
            new UncolonizedProvinceData(117, "DefaultName"),
            new UncolonizedProvinceData(118, "DefaultName"),
            new UncolonizedProvinceData(119, "DefaultName"),
            new UncolonizedProvinceData(120, "DefaultName"),
            new UncolonizedProvinceData(121, "DefaultName"),
            new UncolonizedProvinceData(122, "DefaultName"),
            new UncolonizedProvinceData(123, "DefaultName"),
            new UncolonizedProvinceData(124, "DefaultName"),
            new UncolonizedProvinceData(125, "DefaultName"),
            new UncolonizedProvinceData(126, "DefaultName"),
            new UncolonizedProvinceData(127, "DefaultName"),
            new UncolonizedProvinceData(128, "DefaultName"),
            new UncolonizedProvinceData(129, "DefaultName"),
            new UncolonizedProvinceData(130, "DefaultName"),
            new UncolonizedProvinceData(131, "DefaultName"),
            new UncolonizedProvinceData(132, "DefaultName"),
            new UncolonizedProvinceData(133, "DefaultName"),
            new UncolonizedProvinceData(134, "DefaultName"),
            new UncolonizedProvinceData(135, "DefaultName"),
            new UncolonizedProvinceData(136, "DefaultName"),
            new UncolonizedProvinceData(137, "DefaultName"),
            new UncolonizedProvinceData(138, "DefaultName"),
            new UncolonizedProvinceData(139, "DefaultName"),
            new UncolonizedProvinceData(140, "DefaultName"),
            new UncolonizedProvinceData(141, "DefaultName"),
            new UncolonizedProvinceData(142, "DefaultName"),
            new UncolonizedProvinceData(143, "DefaultName"),
            new UncolonizedProvinceData(144, "DefaultName"),
            new UncolonizedProvinceData(145, "DefaultName"),
            new UncolonizedProvinceData(146, "DefaultName"),
            new UncolonizedProvinceData(147, "DefaultName"),
            new UncolonizedProvinceData(148, "DefaultName"),
            new UncolonizedProvinceData(149, "DefaultName"),
            new UncolonizedProvinceData(150, "DefaultName"),
            new UncolonizedProvinceData(151, "DefaultName"),
            new UncolonizedProvinceData(152, "DefaultName"),
            new UncolonizedProvinceData(153, "DefaultName"),
            new UncolonizedProvinceData(154, "DefaultName"),
            new UncolonizedProvinceData(155, "DefaultName"),
            new UncolonizedProvinceData(156, "DefaultName"),
            new UncolonizedProvinceData(157, "DefaultName"),
            new UncolonizedProvinceData(158, "DefaultName"),
            new UncolonizedProvinceData(159, "DefaultName"),
            new UncolonizedProvinceData(160, "DefaultName"),
            new UncolonizedProvinceData(161, "DefaultName"),
            new UncolonizedProvinceData(162, "DefaultName"),
            new UncolonizedProvinceData(163, "DefaultName"),
            new UncolonizedProvinceData(164, "DefaultName"),
            new UncolonizedProvinceData(165, "DefaultName"),
            new UncolonizedProvinceData(166, "DefaultName"),
            new UncolonizedProvinceData(167, "DefaultName"),
            new UncolonizedProvinceData(168, "DefaultName"),
            new UncolonizedProvinceData(169, "DefaultName"),
            new UncolonizedProvinceData(170, "DefaultName"),
            new UncolonizedProvinceData(171, "DefaultName"),
            new UncolonizedProvinceData(172, "DefaultName"),
            new UncolonizedProvinceData(173, "DefaultName"),
            new UncolonizedProvinceData(174, "DefaultName"),
            new UncolonizedProvinceData(175, "DefaultName"),
            new UncolonizedProvinceData(176, "DefaultName"),
            new UncolonizedProvinceData(177, "DefaultName"),
            new UncolonizedProvinceData(178, "DefaultName"),
            new UncolonizedProvinceData(179, "DefaultName"),
            new UncolonizedProvinceData(180, "DefaultName"),
            new UncolonizedProvinceData(181, "DefaultName"),
            new UncolonizedProvinceData(182, "DefaultName"),
            new UncolonizedProvinceData(183, "DefaultName"),
            new UncolonizedProvinceData(184, "DefaultName"),
            new UncolonizedProvinceData(185, "DefaultName"),
            new UncolonizedProvinceData(186, "DefaultName"),
            new UncolonizedProvinceData(187, "DefaultName"),
            new UncolonizedProvinceData(188, "DefaultName"),
            new UncolonizedProvinceData(189, "DefaultName"),
            new UncolonizedProvinceData(190, "DefaultName"),
            new UncolonizedProvinceData(191, "DefaultName"),
            new UncolonizedProvinceData(192, "DefaultName"),
            new UncolonizedProvinceData(193, "DefaultName"),
            new UncolonizedProvinceData(194, "DefaultName"),
            new UncolonizedProvinceData(195, "DefaultName"),
            new UncolonizedProvinceData(196, "DefaultName"),
            new UncolonizedProvinceData(197, "DefaultName"),
            new UncolonizedProvinceData(198, "DefaultName"),
            new UncolonizedProvinceData(199, "DefaultName"),
            new UncolonizedProvinceData(200, "DefaultName"),
            new UncolonizedProvinceData(201, "DefaultName"),
            new UncolonizedProvinceData(202, "DefaultName"),
            new UncolonizedProvinceData(203, "DefaultName"),
            new UncolonizedProvinceData(204, "DefaultName"),
            new UncolonizedProvinceData(205, "DefaultName"),
            new UncolonizedProvinceData(206, "DefaultName"),
            new UncolonizedProvinceData(207, "DefaultName"),
            new UncolonizedProvinceData(208, "DefaultName"),
            new UncolonizedProvinceData(209, "DefaultName"),
            new UncolonizedProvinceData(210, "DefaultName"),
            new UncolonizedProvinceData(211, "DefaultName"),
            new UncolonizedProvinceData(212, "DefaultName"),
            new UncolonizedProvinceData(213, "DefaultName"),
            new UncolonizedProvinceData(214, "DefaultName"),
            new UncolonizedProvinceData(215, "DefaultName"),
            new UncolonizedProvinceData(216, "DefaultName"),
            new UncolonizedProvinceData(217, "DefaultName"),
            new UncolonizedProvinceData(218, "DefaultName"),
            new UncolonizedProvinceData(219, "DefaultName"),
            new UncolonizedProvinceData(220, "DefaultName"),
            new UncolonizedProvinceData(221, "DefaultName"),
            new UncolonizedProvinceData(222, "DefaultName"),
            new UncolonizedProvinceData(223, "DefaultName"),
            new UncolonizedProvinceData(224, "DefaultName"),
            new UncolonizedProvinceData(225, "DefaultName"),
            new UncolonizedProvinceData(226, "DefaultName"),
            new UncolonizedProvinceData(227, "DefaultName"),
            new UncolonizedProvinceData(228, "DefaultName"),
            new UncolonizedProvinceData(229, "DefaultName"),
            new WastelandProvinceData(230, "DefaultName"),
            new UncolonizedProvinceData(231, "DefaultName"),
            new UncolonizedProvinceData(232, "DefaultName"),
            new UncolonizedProvinceData(233, "DefaultName"),
            new UncolonizedProvinceData(234, "DefaultName"),
            new UncolonizedProvinceData(235, "DefaultName"),
            new UncolonizedProvinceData(236, "DefaultName"),
            new UncolonizedProvinceData(237, "DefaultName"),
            new UncolonizedProvinceData(238, "DefaultName"),
            new UncolonizedProvinceData(239, "DefaultName"),
            new UncolonizedProvinceData(240, "DefaultName"),
            new UncolonizedProvinceData(241, "DefaultName"),
            new UncolonizedProvinceData(242, "DefaultName"),
            new UncolonizedProvinceData(243, "DefaultName"),
            new UncolonizedProvinceData(244, "DefaultName"),
            new UncolonizedProvinceData(245, "DefaultName"),
            new UncolonizedProvinceData(246, "DefaultName"),
            new UncolonizedProvinceData(247, "DefaultName"),
            new UncolonizedProvinceData(248, "DefaultName"),
            new UncolonizedProvinceData(249, "DefaultName"),
            new UncolonizedProvinceData(250, "DefaultName"),
            new UncolonizedProvinceData(251, "DefaultName"),
            new UncolonizedProvinceData(252, "DefaultName"),
            new UncolonizedProvinceData(253, "DefaultName"),
            new UncolonizedProvinceData(254, "DefaultName"),
            new UncolonizedProvinceData(255, "DefaultName"),
            new UncolonizedProvinceData(256, "DefaultName"),
            new UncolonizedProvinceData(257, "DefaultName"),
            new UncolonizedProvinceData(258, "DefaultName"),
            new UncolonizedProvinceData(259, "DefaultName"),
            new UncolonizedProvinceData(260, "DefaultName"),
            new UncolonizedProvinceData(261, "DefaultName"),
            new UncolonizedProvinceData(262, "DefaultName"),
            new UncolonizedProvinceData(263, "DefaultName"),
            new UncolonizedProvinceData(264, "DefaultName"),
            new UncolonizedProvinceData(265, "DefaultName"),
            new UncolonizedProvinceData(266, "DefaultName"),
            new UncolonizedProvinceData(267, "DefaultName"),
            new UncolonizedProvinceData(268, "DefaultName"),
            new UncolonizedProvinceData(269, "DefaultName"),
            new UncolonizedProvinceData(270, "DefaultName"),
            new UncolonizedProvinceData(271, "DefaultName"),
            new UncolonizedProvinceData(272, "DefaultName"),
            new UncolonizedProvinceData(273, "DefaultName"),
            new UncolonizedProvinceData(274, "DefaultName"),
            new UncolonizedProvinceData(275, "DefaultName"),
            new UncolonizedProvinceData(276, "DefaultName"),
            new UncolonizedProvinceData(277, "DefaultName"),
            new UncolonizedProvinceData(278, "DefaultName"),
            new UncolonizedProvinceData(279, "DefaultName"),
            new UncolonizedProvinceData(280, "DefaultName"),
            new UncolonizedProvinceData(281, "DefaultName"),
            new UncolonizedProvinceData(282, "DefaultName"),
            new UncolonizedProvinceData(283, "DefaultName"),
            new UncolonizedProvinceData(284, "DefaultName"),
            new UncolonizedProvinceData(285, "DefaultName"),
            new UncolonizedProvinceData(286, "DefaultName"),
            new UncolonizedProvinceData(287, "DefaultName"),
            new UncolonizedProvinceData(288, "DefaultName"),
            new UncolonizedProvinceData(289, "DefaultName"),
            new UncolonizedProvinceData(290, "DefaultName"),
            new UncolonizedProvinceData(291, "DefaultName"),
            new UncolonizedProvinceData(292, "DefaultName"),
            new UncolonizedProvinceData(293, "DefaultName"),
            new UncolonizedProvinceData(294, "DefaultName"),
            new UncolonizedProvinceData(295, "DefaultName"),
            new UncolonizedProvinceData(296, "DefaultName"),
            new UncolonizedProvinceData(297, "DefaultName"),
            new UncolonizedProvinceData(298, "DefaultName"),
            new UncolonizedProvinceData(299, "DefaultName"),
            new UncolonizedProvinceData(300, "DefaultName"),
            new UncolonizedProvinceData(301, "DefaultName"),
            new UncolonizedProvinceData(302, "DefaultName"),
            new UncolonizedProvinceData(303, "DefaultName"),
            new UncolonizedProvinceData(304, "DefaultName"),
            new UncolonizedProvinceData(305, "DefaultName"),
            new UncolonizedProvinceData(306, "DefaultName"),
            new UncolonizedProvinceData(307, "DefaultName"),
            new UncolonizedProvinceData(308, "DefaultName"),
            new UncolonizedProvinceData(309, "DefaultName"),
            new UncolonizedProvinceData(310, "DefaultName"),
            new UncolonizedProvinceData(311, "DefaultName"),
            new UncolonizedProvinceData(312, "DefaultName"),
            new UncolonizedProvinceData(313, "DefaultName"),
            new UncolonizedProvinceData(314, "DefaultName"),
            new UncolonizedProvinceData(315, "DefaultName"),
            new UncolonizedProvinceData(316, "DefaultName"),
            new UncolonizedProvinceData(317, "DefaultName"),
            new UncolonizedProvinceData(318, "DefaultName"),
            new UncolonizedProvinceData(319, "DefaultName"),
            new UncolonizedProvinceData(320, "DefaultName"),
            new UncolonizedProvinceData(321, "DefaultName"),
            new UncolonizedProvinceData(322, "DefaultName"),
            new UncolonizedProvinceData(323, "DefaultName"),
            new UncolonizedProvinceData(324, "DefaultName"),
            new UncolonizedProvinceData(325, "DefaultName"),
            new UncolonizedProvinceData(326, "DefaultName"),
            new UncolonizedProvinceData(327, "DefaultName"),
            new UncolonizedProvinceData(328, "DefaultName"),
            new UncolonizedProvinceData(329, "DefaultName"),
            new UncolonizedProvinceData(330, "DefaultName"),
            new UncolonizedProvinceData(331, "DefaultName"),
            new UncolonizedProvinceData(332, "DefaultName"),
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
            new SeaProvinceData(420, "BlackSea", Modifiers.DefaultModifiers()),
        };

        ArmyUnits = new List<ArmyUnitData>()
        {
            new ArmyUnitData("George Floyd", 1, 3, Modifiers.DefaultModifiers(), new List<ArmyRegiment>(), null),
            new ArmyUnitData("Idk", 1, 5, Modifiers.DefaultModifiers(),new List<ArmyRegiment>(), null),
            new ArmyUnitData("FunnyName", 1, 6, Modifiers.DefaultModifiers(), new List<ArmyRegiment>(), null),
            new ArmyUnitData("Length", 1, 7, Modifiers.DefaultModifiers(), new List<ArmyRegiment>(), null),
        };

        Map = GameMath.CalculateBorderProvinces(Map, MapTexture);
        var centers = GameMath.CalculateCenterOfProvinceWeight(MapTexture, Map.Length);
        for (var i = 0; i < Map.Length; i++)
        {
            Map[i].CenterOfWeight = centers[i];
        }
    }
}