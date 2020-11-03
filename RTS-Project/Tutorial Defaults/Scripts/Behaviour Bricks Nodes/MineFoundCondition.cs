using UnityEngine;

using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework; // ConditionBase

[Condition("Frank/MineFound")]
[Help("Checks whether the miner has found a mine in his FOV or not.")]
public class MineFoundCondition : ConditionBase
{
    [InParam("boolCondition")]
    public bool boolCondition;

    [InParam("initialState")]
    public bool initialState;

    [InParam("Villager")]
    public GameObject villagerGO;

    public override bool Check()
    {
        Villager villager = villagerGO.GetComponent<Villager>();

        if(boolCondition)
        {
            if (villager != null)
            {
                if (villager.mineSeen && !initialState)
                    return true;
            }

            return false;
        }
        else
        {
            if (villager != null)
            {
                if (villager.mineSeen && !initialState)
                    return false;
            }

            if(initialState)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
    }
} // class MineFoundCondition 