using System;
using System.Text.Json.Serialization;

namespace EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;

[Serializable]
[JsonDerivedType(typeof(War), typeDiscriminator: "war")]
[JsonDerivedType(typeof(TradeAgreement), typeDiscriminator: "tradeAgreeement")]
public abstract class DiplomacyAgreement
{
    public int Consequenter { get; set; }
    public int Initiator { get; set; }
    public DateTime StartDate { get; set; }

    public DiplomacyAgreement(int initior, int consequenter, DateTime startDate)
    {
        Initiator = initior;
        Consequenter = consequenter;
        StartDate = startDate;
    }
}