using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase

[Condition("Frank/IsGoldCapacityFull")]
[Help("Checks whether the miner has enough capacity to carry more gold or not.")]
public class IsGoldCapacityFullCondition : ConditionBase
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
                if (miner.goldHolding < miner.goldCapacity)
                    return false;
            }

            return true;
        }
        else
        {
            if (miner != null)
            {
                if (miner.goldHolding < miner.goldCapacity)
                    return true;
            }

            return false;
        }
    }
} // class IsGoldCapacityFullCondition 