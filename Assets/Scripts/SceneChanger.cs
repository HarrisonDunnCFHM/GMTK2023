using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public AudioManager audioManager; 

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
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
        PlayerData.StartNewRun();
        LoadMapScene();
    }

    public void ContinueGame()
    {
        PlayerData.LoadFromPlayerPrefs();
    }

    public void LoadMainMenu()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.InitiateMusicFade(0);
        SceneManager.LoadScene(0);
    }

    public void LoadMapScene()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.InitiateMusicFade(1);
        SceneManager.LoadScene(1);
    }

    public void LoadBattleScene()
    {
        SceneManager.LoadScene(2);
    }
}
