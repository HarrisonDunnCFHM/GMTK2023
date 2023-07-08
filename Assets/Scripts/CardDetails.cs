using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDetails : MonoBehaviour
{
    public TextMeshProUGUI elementText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI rulesText;
    public Image myIconImage;
    public Image myCardImage;
    public List<Sprite> elementIcons;
    public List<Color> cardColors;

    public CardDetails(PlayerCardAttributes inputCard)
    {
        SetDetails(inputCard);
    }

    public void SetDetails(PlayerCardAttributes inputCard)
    {
        //elementText.text = inputCard.element.ToString();
        int elementIndex = (int)inputCard.element;
       
        //switch (inputCard.element)
        //{
        //    case Element.Earth:
        //        elementIndex = 0;
        //        break;
        //    case Element.Fire:
        //        elementIndex = 1;
        //        break;
        //    case Element.Water:
        //        elementIndex = 2;
        //        break;
        //    case Element.Wind:
        //        elementIndex = 3;
        //        break;
        //    default:
        //        break;
        //}
        myIconImage.sprite = elementIcons[elementIndex];
        myCardImage.color = cardColors[elementIndex]; 
        nameText.text = inputCard.name;
        powerText.text = inputCard.power.ToString();
        rulesText.text = inputCard.rules;
    }
}
