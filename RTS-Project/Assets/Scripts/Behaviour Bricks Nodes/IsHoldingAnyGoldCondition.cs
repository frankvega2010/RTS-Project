using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase

[Condition("Frank/IsHoldingAnyGold")]
[Help("Checks whether the miner has any gold carrying.")]
public class IsHoldingAnyGoldCondition : ConditionBase
{
    [InParam("boolCondition")]
    public bool boolCondition;

    [InParam("Miner")]
    public GameObject minerGO;

    public override bool Check()
    {
        Miner miner = minerGO.GetComponent<Miner>();

        if(boolCondition)
        {
            if (miner != null)
            {
                if (miner.goldHolding != 0)
                    return true;
            }

            return false;
        }
        else
        {
            if (miner != null)
            {
                if (miner.goldHolding != 0)
                    return false;
            }

            return true;
        }
    }
} // class IsHoldingAnyGoldCondition