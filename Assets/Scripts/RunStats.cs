using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RunStats : MonoBehaviour
{
    [SerializeField] List<Image> heartBank;
    [SerializeField] Sprite heartFullSprite;
    [SerializeField] Sprite heartBrokenSprite;
    [SerializeField] TextMeshProUGUI winsCountText;
    [SerializeField] TextMeshProUGUI deckSizeText;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRunStats();
    }

    private void UpdateRunStats()
    {
        //assesshearts
        for(int hearts = 0; hearts < 3; hearts++)
        {
            if (hearts <= PlayerData.lives - 1)
            {
                heartBank[hearts].sprite = heartFullSprite;
            }
            else
            {
                heartBank[hearts].sprite = heartBrokenSprite;
            }
        }
        winsCountText.text = "Wins: " + PlayerData.winsThisRun + "/10";
        if(PlayerData.deckList == null) { return; }
        deckSizeText.text = "Deck Size: " + PlayerData.deckList.Count;
    }
}
