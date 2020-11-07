using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCUI : MonoBehaviour
{
    public enum NPCStates
    {
        Idle,
        Patrol,
        Mine,
        Return,
        Mark,
        AllStates
    }

    public Image icon;
    public TextMeshProUGUI goldText;

    public Sprite IdleIcon;
    public Sprite Patrolcon;
    public Sprite MineIcon;
    public Sprite ReturnIcon;
    public Sprite MarkIcon;

    public void UpdateText(string newText)
    {
        goldText.text = newText;
    }

    public void UpdateIcon(NPCStates state)
    {
        switch (state)
        {   
            case NPCStates.Idle:
                icon.sprite = IdleIcon;
                break;
            case NPCStates.Patrol:
                icon.sprite = Patrolcon;
                break;
            case NPCStates.Mine:
                icon.sprite = MineIcon;
                break;
            case NPCStates.Return:
                icon.sprite = ReturnIcon;
                break;
            case NPCStates.Mark:
                icon.sprite = MarkIcon;
                break;
            case NPCStates.AllStates:
                break;
            default:
                break;
        }
    }

    
}
