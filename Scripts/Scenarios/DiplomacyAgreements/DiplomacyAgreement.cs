using System;

namespace EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;

public class DiplomacyAgreement
{
    public int Initiator;
    public int Consequenter;
    public DateTime StartDate;

    public DiplomacyAgreement(int initior, int consequenter, DateTime startDate)
    {
        Initiator = initior;
        Consequenter = consequenter;
        StartDate = startDate;
    }
}