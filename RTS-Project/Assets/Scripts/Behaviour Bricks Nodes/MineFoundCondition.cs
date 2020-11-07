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

    [InParam("Miner")]
    public GameObject minerGO;

    public override bool Check()
    {
        Miner miner = minerGO.GetComponent<Miner>();

        if(boolCondition)
        {
            if (miner != null)
            {
                if (miner.mineSeen && !initialState)
                    return true;
            }

            return false;
        }
        else
        {
            if (miner != null)
            {
                if (miner.mineSeen && !initialState)
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