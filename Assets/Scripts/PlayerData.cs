using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData 
{
    //current progress
    public static int lives = 3;
    public static int runProgress = 0;

    //meta progress
    public static int completedRuns = 0;
    public static int winningRuns = 0;
    
    public const int startingLives = 3;
    
    public static List<PlayerCardAttributes> deckList;

    public static List<PlayerCardAttributes> LoadPlayerDeck()
    {
        return deckList;
    }

    public static void ClearDeckList()
    {
        deckList = new List<PlayerCardAttributes>();
    }

    public static void AddCardsToPlayerDeck(PlayerCardAttributes newCard)
    {
        if(deckList == null) { ClearDeckList(); }
        deckList.Add(newCard);
    }

    public static void AddCardsToPlayerDeck(List<PlayerCardAttributes> newCards)
    {
        if (deckList == null) { ClearDeckList(); }
        deckList.AddRange(newCards);
    }

    public static void RemoveCardFromPlayerDeck(PlayerCardAttributes cutCard)
    {
        deckList.Remove(cutCard);
    }

    public static void UpdateLives(int lifeChange)
    {
        lives += lifeChange;
        if (lives < 0) { lives = 0; }
    }

    public static void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("lives", lives);
        PlayerPrefs.SetInt("runProgress", runProgress);
        PlayerPrefs.SetInt("completedRuns", completedRuns);
        PlayerPrefs.SetInt("winningRuns", winningRuns);
        PlayerPrefs.Save();
    }

    public static void LoadFromPlayerPrefs()
    {
        lives = PlayerPrefs.GetInt("lives", PlayerData.startingLives );
        runProgress = PlayerPrefs.GetInt("runProgress", 0);
        completedRuns = PlayerPrefs.GetInt("completedRuns", 0);
        winningRuns = PlayerPrefs.GetInt("winningRuns", 0);
    }

    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
