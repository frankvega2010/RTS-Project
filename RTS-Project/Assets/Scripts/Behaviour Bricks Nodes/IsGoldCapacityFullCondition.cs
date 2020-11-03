using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase

[Condition("Frank/IsGoldCapacityFull")]
[Help("Checks whether the miner has enough capacity to carry more gold or not.")]
public class IsGoldCapacityFullCondition : ConditionBase
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
                if (villager.goldHolding < villager.goldCapacity)
                    return false;
            }

            return true;
        }
        else
        {
            if (villager != null)
            {
                if (villager.goldHolding < villager.goldCapacity)
                    return true;
            }

            return false;
        }
    }
} // class IsGoldCapacityFullCondition 