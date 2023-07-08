using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardDetails : MonoBehaviour
{
    public TextMeshProUGUI elementText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI rulesText;

    public CardDetails(PlayerCardAttributes inputCard)
    {
        SetDetails(inputCard);
    }

    public void SetDetails(PlayerCardAttributes inputCard)
    {
        elementText.text = inputCard.element.ToString();
        nameText.text = inputCard.name;
        powerText.text = inputCard.power.ToString();
        rulesText.text = inputCard.rules;
    }
}
