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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PlayerCardAttributes PlayCard()
    {
        if(battleDeckList.Count <= 0) { return null; }
        PlayerCardAttributes cardToReturn = battleDeckList[0];
        Debug.Log("returning " + cardToReturn.ToString());
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