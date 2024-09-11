using System;

namespace EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;

public abstract class DiplomacyAgreement
{
    public int Consequenter;
    public int Initiator;
    public DateTime StartDate;

    public DiplomacyAgreement(int initior, int consequenter, DateTime startDate)
    {
        Initiator = initior;
        Consequenter = consequenter;
        StartDate = startDate;
    }
}