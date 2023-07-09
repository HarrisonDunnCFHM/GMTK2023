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
        int elementIndex = (int)inputCard.element;
        myIconImage.sprite = elementIcons[elementIndex];
        myCardImage.color = cardColors[elementIndex]; 
        nameText.text = inputCard.name;
        powerText.text = inputCard.power.ToString();
        rulesText.text = inputCard.rules;
    }

    public IEnumerator MoveCard(Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsed = 0f;
        while (elapsed < time)
        {
            GetComponent<RectTransform>().localPosition = Vector3.Lerp(startPos, endPos, elapsed/time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        GetComponent<RectTransform>().localPosition = endPos;
    }
}
