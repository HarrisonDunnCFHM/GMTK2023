using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private void Start()
    {
        if(CardManager.allDecks.Count == 0)
        {
            CardManager.InitializeDecksList();
        }
        if (CardManager.allCards.Count == 0)
        {
            CardManager.InitializeCardList();
        }
    }

    private void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void StartNewGame()
    {
        PlayerData.lives = PlayerData.startingLives;
        PlayerData.runProgress = 0;
        PlayerData.ClearDeckList();
        LoadMapScene();
    }

    public void ContinueGame()
    {
        PlayerData.LoadFromPlayerPrefs();
    }

    public void LoadMapScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene(2);
    }

}
