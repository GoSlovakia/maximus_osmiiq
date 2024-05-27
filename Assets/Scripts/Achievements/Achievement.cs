using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class AllAchievements
{
    public Achievement[] All;
}
[System.Serializable]
public class Achievement
{
    public string ID;
    public string Name;
    public string DescriptionEN;
    public string Condition;
    public string Type;
    public int XP;
    public int QUI;
    public int QI;
    private VariableType varType;
    public int Value;
    public int Value2;
    private ComparisonEnum comparisonType;
    public int From;

    public string Var;
    public string Comparison;

    public VariableType VarType
    {
        get
        {
            if (varType != VariableType.NULL)
                return varType;
            else
            {
                //varType = (VariableType)Enum.Parse(typeof(VariableType), Var);

                varType = Enum.GetValues(typeof(VariableType)).Cast<VariableType>().Where(x => x.GetVarName() == Var).SingleOrDefault();
                //Debug.Log(Var + varType);
                return varType;
            }
        }

        set
        {
            varType = value;
        }
    }

    public ComparisonEnum ComparisonType
    {
        get
        {
            if (comparisonType != ComparisonEnum.NULL)
                return comparisonType;
            else
            {
               // Debug.Log("Comparison Get " + Comparison);
                comparisonType = Enum.GetValues(typeof(ComparisonEnum)).Cast<ComparisonEnum>().Where(x => x.GetComparison() == Comparison).SingleOrDefault();
               // Debug.Log("Comparison Result " + comparisonType.GetComparison());
                return comparisonType;
            }
        }

        set
        {
            comparisonType = value;
        }
    }
}



public enum VariableType
{

    NULL,
    [TagStat("SavedAvatar")]
    SavedAvatar,
    [TagStat("BoostersBought")]
    BoostersBought,
    [TagStat("FriendsAdded")]
    FriendsAdded,
    [TagStat("TradesDone")]
    TradesDone,
    [TagStat("ConsecutiveDailyOffers")]
    ConsecutiveDailyOffers,
    [TagStat("CommonCards")]
    CommonCards,
    [TagStat("RareCards")]
    RareCards,
    [TagStat("EpicCards")]
    EpicCards,
    [TagStat("LegendaryCards")]
    LegendaryCards,
    [TagStat("UniqueCards")]
    UniqueCards,
    [TagStat("DuplicateCards")]
    DuplicateCards,
    [TagStat("AllSetsCompleted")]
    AllSetsCompleted,
    [TagStat("SetsCompletedDomain")]
    SetsCompletedDomain,
    [TagStat("AllCardsFromSeason")]
    AllCardsFromSeason
}

public enum ComparisonEnum
{
    [ComparisonTag("NULL")]
    NULL,
    [ComparisonTag("EQUAL")]
    EQUAL,
    [ComparisonTag("MORE_OR_EQUAL")]
    MORE_OR_EQUAL,
    [ComparisonTag("LESS_OR_EQUAL")]
    LESS_OR_EQUAL
}