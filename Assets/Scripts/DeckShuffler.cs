using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class DeckShuffler 
{
    private static readonly Random rng = new();

    public static void ShuffleDeck<Card>(this List<Card> deckList)
    {
        if (deckList.Count > 0)
        {
            int cardOldPosition = deckList.Count;
            while (cardOldPosition > 1)
            {
                cardOldPosition--;
                int cardNewPosition = rng.Next(cardOldPosition + 1);
                Card value = deckList[cardNewPosition];
                deckList[cardNewPosition] = deckList[cardOldPosition];
                deckList[cardOldPosition] = value;
            }
        }
    }
}
