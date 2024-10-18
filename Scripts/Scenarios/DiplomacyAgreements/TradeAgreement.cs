﻿using System;

namespace EuropeDominationDemo.Scripts.Scenarios.DiplomacyAgreements;

[Serializable]
public class TradeAgreement : DiplomacyAgreement
{
    public TradeAgreement(int initior, int consequenter, DateTime startDate) : base(initior, consequenter, startDate)
    {
    }
}