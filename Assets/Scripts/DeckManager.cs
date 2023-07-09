using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<PlayerCardAttributes> deckList;

    public List<PlayerCardAttributes> battleDeckList;

    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerDeck();
        if (FindObjectOfType<BattleSequence>())
        {
            GenerateBattleDeck();
            //FindObjectOfType<BattleSequence>().UpdatePlayerDeckCount();
        }
    }

    private void LoadPlayerDeck()
    {
        if (PlayerData.deckList == null)
        {
            //deckList = new List<PlayerCardAttributes>();
        }
        else if (PlayerData.deckList.Count >= 0)
        {
            deckList = PlayerData.deckList;
        }
        else
        {
            //deckList = new List<PlayerCardAttributes>();
        }
    }

    public PlayerCardAttributes PlayCard()
    {
        if(battleDeckList.Count <= 0) { return null; }
        PlayerCardAttributes cardToReturn = battleDeckList[0];
        battleDeckList.Remove(battleDeckList[0]);
        return cardToReturn;
    }

    public void GenerateBattleDeck()
    {
        battleDeckList = new List<PlayerCardAttributes>(deckList);
        battleDeckList.ShuffleDeck();
    }

    public void ShuffleList()
    {
        deckList.ShuffleDeck();
    }    
}