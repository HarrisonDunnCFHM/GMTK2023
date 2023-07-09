using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleSequence : MonoBehaviour
{

    [SerializeField] private EnemyAttackPicker attackPicker;
    [SerializeField] DeckManager playerDeck;
    [SerializeField] float cardDelay = 0.2f;
    [SerializeField] GameObject cardThumbPrefab;
    [SerializeField] Vector3 playedCardSpawnPos;

    //[SerializeField] TextMeshProUGUI currentPlayerPowerText;
    //[SerializeField] TextMeshProUGUI deckSizeText;

    [SerializeField] TextMeshProUGUI tutorialText;

    int currentPlayerPower = 0;
    int cardsLeftInDeck = 0;
    public bool battleOver = false;

    List<GameObject> playedCardsList = new();
    [SerializeField] GameObject winMenuPopup;
    [SerializeField] GameObject lossMenuPopup;
    [SerializeField] GameObject gameOverMenuPopUp;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] RunStats runStats;
    [SerializeField] Image runUIDeckImage;
    [SerializeField] TextMeshProUGUI runUIDeckSize;

    [SerializeField] List<Sprite> deckSprites;

    void Start()
    {
        runStats.inBattle = true;
        UpdatePlayerDeckCount(playerDeck.deckList.Count);
        if(PlayerData.runProgress <= 1)
        {
            tutorialText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
    }
    public void UpdatePlayerDeckCount(int listCount)
    {
        cardsLeftInDeck = listCount;
        runUIDeckSize.text = "Cards Left: " + cardsLeftInDeck.ToString() + "/" + PlayerData.deckList.Count;
        if(cardsLeftInDeck == 0)
        {
            runUIDeckImage.sprite = deckSprites[5];
        }
        else if (cardsLeftInDeck == 1)
        {
            runUIDeckImage.sprite = deckSprites[4];
        }
        else if (cardsLeftInDeck == 2)
        {
            runUIDeckImage.sprite = deckSprites[3];
        }
        else if (cardsLeftInDeck < 5)
        {
            runUIDeckImage.sprite = deckSprites[2];
        }
        else if (cardsLeftInDeck < 10)
        {
            runUIDeckImage.sprite = deckSprites[1];
        }
        else 
        {
            runUIDeckImage.sprite = deckSprites[0];
        }
    }

    public IEnumerator PlayOutAttack()
    {
        yield return new WaitForSeconds(cardDelay);
        currentPlayerPower = 0;
        EnemyAttack lastAttack = attackPicker.lastAttack;
        int enemyAttack = attackPicker.lastAttackStrength;
        Element enemyElement = attackPicker.lastAttackElement;
        float newXPos = lastAttack.myAttackColumnXValue;
        float cardColumnYStart = lastAttack.myAttackColumnYValue;
        int cardsPlayed = 0;
        float cardHeight = cardThumbPrefab.GetComponent<RectTransform>().rect.height;
        while (currentPlayerPower < enemyAttack)
        {
            if (playerDeck.battleDeckList.Count <= 0)
            {
                Debug.Log("out of cards!");
                CheckForBattleOver();
                break;
            }
            else
            {
                PlayerCardAttributes nextPlayed = playerDeck.PlayCard();
                cardsLeftInDeck = playerDeck.battleDeckList.Count;
                Element playedElement = nextPlayed.element;
                int playedPower = nextPlayed.power;
                string matchingElement = "";
                if (playedElement == enemyElement)
                {
                    matchingElement = "!! (" + playedPower + ")";
                    playedPower *= 2;
                }
                currentPlayerPower += playedPower;
                GameObject cardPlayedObject = Instantiate(cardThumbPrefab, transform.position, Quaternion.identity, mainCanvas.transform);
                cardPlayedObject.GetComponent<CardDetails>().SetDetails(nextPlayed);
                float newYPos = (cardsPlayed + 1) * cardHeight + cardColumnYStart - cardHeight/2;
                Vector3 destinationPos = new Vector3(newXPos, newYPos, 0);
                cardPlayedObject.GetComponent<RectTransform>().localPosition = playedCardSpawnPos;
                StartCoroutine(cardPlayedObject.GetComponent<CardDetails>().MoveCard(playedCardSpawnPos, destinationPos, cardDelay));

                playedCardsList.Add(cardPlayedObject);
                cardsPlayed++;
                UpdatePlayerDeckCount(playerDeck.battleDeckList.Count);
                yield return new WaitForSeconds(cardDelay);

            }
        }
        lastAttack.used = true;
        CheckForBattleOver();
        attackPicker.resolvingAttack = false;
        yield return null;
    }

    public bool CheckForBattleOver()
    {
        bool battleWon = true;
        foreach (EnemyAttack attack in attackPicker.generatedAttacksList)
        {
            if (!attack.used)
            {
                battleWon = false;

                break;
            }
        }
        if (battleWon)
        {
            ResolveBattle(battleWon);
            return battleWon;
        }
        if (cardsLeftInDeck <= 0)
        {
            ResolveBattle(battleWon); 
        }
        return battleWon;
    }

    public void ResolveBattle(bool winStatus)
    {
        if (battleOver) { return; }
        battleOver = true;
        PlayerData.SaveDeckList(playerDeck.deckList);
        PlayerData.runProgress++;
        if(winStatus)
        {
            //do good stuff
            PlayerData.winsThisRun++;
            winMenuPopup.SetActive(true);
        }
        else
        {
            //uh oh
            PlayerData.UpdateLives(-1);
            if(PlayerData.lives <= 0)
            {
                gameOverMenuPopUp.SetActive(true);
            }
            lossMenuPopup.SetActive(true);
        }
    }
}
