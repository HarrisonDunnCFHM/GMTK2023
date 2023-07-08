using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class DeckList : ScriptableObject
{
    public List<PlayerCardAttributes> deckList;
    public bool isStarter;
    public bool isUnlocked;
}
