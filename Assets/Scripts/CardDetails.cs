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
        myIconImage.sprite = elementIcons[elementIndex];
        myCardImage.color = cardColors[elementIndex];
        //switch (inputCard.element)
        //{
        //    case Element.Earth:
        //        myIconImage.sprite = elementIcons[0];
        //        myCardImage.color = cardColors[0];
        //        break;
        //    case Element.Fire:
        //        myIconImage.sprite = elementIcons[1];
        //        break;
        //    case Element.Water:
        //        myIconImage.sprite = elementIcons[2];
        //        break;
        //    case Element.Wind:
        //        myIconImage.sprite = elementIcons[3];
        //        break;
        //    default:
        //        break;
        //}
        nameText.text = inputCard.name;
        powerText.text = inputCard.power.ToString();
        rulesText.text = inputCard.rules;
    }
}
