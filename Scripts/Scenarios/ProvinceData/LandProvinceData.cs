using EuropeDominationDemo.Scripts.Enums;
using EuropeDominationDemo.Scripts.Scenarios.Goods;

namespace EuropeDominationDemo.Scripts.Scenarios.ProvinceData;

public abstract class LandProvinceData : ProvinceData
{
    public readonly Terrain Terrain;
    public readonly Good Good;

    public LandProvinceData(int id, string name, Terrain terrain, Good good):base(id, name)
    {
        Terrain = terrain;
        Good = good;
    }
}