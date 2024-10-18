using System;

namespace EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;

[Serializable]
public class War : DiplomacyAgreement
{
    public War(int initior, int consequenter, DateTime startDate) : base(initior, consequenter, startDate)
    {
    }
}