using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase

[Condition("Frank/IsHoldingAnyGold")]
[Help("Checks whether the miner has any gold carrying.")]
public class IsHoldingAnyGoldCondition : ConditionBase
{
    [InParam("boolCondition")]
    public bool boolCondition;

    [InParam("Villager")]
    public GameObject villagerGO;

    public override bool Check()
    {
        Villager villager = villagerGO.GetComponent<Villager>();

        if(boolCondition)
        {
            if (villager != null)
            {
                if (villager.goldHolding != 0)
                    return true;
            }

            return false;
        }
        else
        {
            if (villager != null)
            {
                if (villager.goldHolding != 0)
                    return false;
            }

            return true;
        }
    }
} // class IsHoldingAnyGoldCondition