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
        allCards = Resources.LoadAll<PlayerCardAttributes>("Cards").ToList();
    }

    public static void InitializeDecksList()
    {
        allDecks = Resources.LoadAll<DeckList>("Decks").ToList();
    }
}

