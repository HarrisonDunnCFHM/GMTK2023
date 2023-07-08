using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class CardManager
{

    public static List<PlayerCardAttributes> allCards = new();
    public static List<DeckList> allDecks = new();
    
    // Start is called before the first frame update
    public static void InitializeCardList()
    {
        Debug.Log("loading all cards");
        allCards = Resources.LoadAll<PlayerCardAttributes>("Cards").ToList();
        Debug.Log("cards loaded: " + allCards.Count);
    }

    public static void InitializeDecksList()
    {
        allDecks = Resources.LoadAll<DeckList>("Decks").ToList();
    }
}

