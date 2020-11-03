using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        HQ.OnGoldDeposit += UpdateText;
        HQ.OnGoldUsed += UpdateText;
        gm = GameManager.Get();
        UpdateText();
    }

    public void UpdateText()
    {
        goldText.text = gm.gold.ToString();
    }

    private void OnDestroy()
    {
        HQ.OnGoldDeposit -= UpdateText;
        HQ.OnGoldUsed -= UpdateText;
    }
}
