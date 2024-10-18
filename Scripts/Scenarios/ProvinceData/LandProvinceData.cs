using System;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;
[Serializable]
public abstract class LandProvinceData : ProvinceData
{
    public int Good { get; set; }
    public int Terrain { get; set; }

    public LandProvinceData(int id, string name, int terrain, int good) : base(id, name)
    {
        Terrain = terrain;
        Good = good;
    }
}