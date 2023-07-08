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

    int currentPlayerPower = 0;

    List<GameObject> playedCardsList = new();

    // Start is called before the first frame update
    void Start()
    {
        playerDeck.GenerateBattleDeck();
        playerDeckLocation = playerDeck.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerPowerText.text = "Current Attack Power: " + currentPlayerPower.ToString();
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
        int enemyAttack = attackPicker.lastAttackStrength;
        int cardsPlayed = 0;
        while (currentPlayerPower < enemyAttack)
        {
            yield return new WaitForSeconds(cardDelay);
            if (playerDeck.battleDeckList.Count <= 0)
            {
                Debug.Log("out of cards!");
                break;
            }
            else
            {
                PlayerCardAttributes nextPlayed = playerDeck.PlayCard();
                int playedPower = nextPlayed.power;
                Element playedElement = nextPlayed.element;
                currentPlayerPower += playedPower;
                GameObject cardPlayedObject = Instantiate(cardPrefab, transform.position, Quaternion.identity);
                cardPlayedObject.GetComponentInChildren<TextMeshProUGUI>().text = playedElement.ToString() + " " + playedPower.ToString();
                cardPlayedObject.transform.position = new Vector3 (playerDeckLocation.x + cardsPlayed, playerDeckLocation.y,0);
                playedCardsList.Add(cardPlayedObject);
                Debug.Log("played " + playedElement.ToString() + " " + playedPower.ToString());
                cardsPlayed++;
            }
        }
        Debug.Log("enemy overcome!");
        yield return null;
    }

}
