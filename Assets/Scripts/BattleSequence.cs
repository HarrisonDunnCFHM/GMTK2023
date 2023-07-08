using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleSequence : MonoBehaviour
{

    [SerializeField] private EnemyAttackPicker attackPicker;
    [SerializeField] DeckManager playerDeck;
    Vector3 playerDeckLocation;
    [SerializeField] float cardDelay = 0.2f;
    [SerializeField] GameObject cardPrefab;

    [SerializeField] TextMeshProUGUI currentPlayerPowerText;
    [SerializeField] TextMeshProUGUI deckSizeText;

    int currentPlayerPower = 0;
    int cardsLeftInDeck = 0;
    public bool battleOver = false;

    List<GameObject> playedCardsList = new();
    [SerializeField] GameObject winMenuPopup;
    [SerializeField] GameObject lossMenuPopup;
    [SerializeField] GameObject gameOverMenuPopUp;

    // Start is called before the first frame update
    void Start()
    {
        playerDeckLocation = playerDeck.transform.position;
    }

    public void UpdatePlayerDeckCount()
    {
        cardsLeftInDeck = playerDeck.battleDeckList.Count;
    }


    // Update is called once per frame
    void Update()
    {
        currentPlayerPowerText.text = "Current Attack Power: " + currentPlayerPower.ToString();
        deckSizeText.text = "Cards left in deck: " + cardsLeftInDeck;
    }

    public IEnumerator PlayOutAttack()
    {
        if (playedCardsList.Count > 0)
        {
            foreach (GameObject card in playedCardsList)
            {
                Destroy(card);
            }
            playedCardsList = new();
        }
        currentPlayerPower = 0;
        EnemyAttack lastAttack = attackPicker.lastAttack;
        int enemyAttack = attackPicker.lastAttackStrength;
        int cardsPlayed = 0;
        while (currentPlayerPower < enemyAttack)
        {
            yield return new WaitForSeconds(cardDelay);
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
                int playedPower = nextPlayed.power;
                Element playedElement = nextPlayed.element;
                currentPlayerPower += playedPower;
                GameObject cardPlayedObject = Instantiate(cardPrefab, transform.position, Quaternion.identity);
                cardPlayedObject.GetComponentInChildren<TextMeshProUGUI>().text = playedElement.ToString() + " " + playedPower.ToString();
                cardPlayedObject.transform.position = new Vector3 (playerDeckLocation.x + cardsPlayed, playerDeckLocation.y,0);
                playedCardsList.Add(cardPlayedObject);
                cardsPlayed++;
                //if (CheckForBattleOver()) { break; }
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
