using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class MapManager : MonoBehaviour
{
    public DeckManager deckManager;

    public int currentLives;
    [SerializeField] TextMeshProUGUI currentLivesText;
    public int currentWins;
    [SerializeField] TextMeshProUGUI currentWinsText;
    public int deckSize;
    [SerializeField] TextMeshProUGUI currentDeckSizeText;

    [SerializeField] int winsNeededToComplete = 5;
    [SerializeField] GameObject successfulRunPopUp;
    public bool runOver = false;

    [SerializeField] GameObject offerMenu;
    [SerializeField] float deckSpacingModifier;
    [SerializeField] float cardSpacingModifier;
    [SerializeField] GameObject cardThumbPrefab;
    [SerializeField] GameObject cardFullPrefab;
    [SerializeField] GameObject selectButtonPrefab;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] int maxBattles = 3;

    List<DeckList> deckOffer;
    List<PlayerCardAttributes> cardOffer;
    public List<BattleAttributes> battles;

    SceneChanger sceneChanger;

    // Start is called before the first frame update
    void Start()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
        if (CheckForSuccessfulRun()) { return;}
        SetUpPlayerData();
    }
    
    private bool CheckForSuccessfulRun()
    {
        if (PlayerData.winsThisRun >= winsNeededToComplete)
        {
            //good job!
            successfulRunPopUp.SetActive(true);
            return true;
        }
        return false;
    }

    private void SetUpPlayerData()
    {
        if (deckManager == null) { deckManager = FindObjectOfType<DeckManager>(); }
        if (PlayerData.runProgress == 0)
        {
            PresentNewStartingDecks();
        }
        else
        {
            PresentNewCardOffer();
        }
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        deckManager.deckList = PlayerData.LoadPlayerDeck();
        currentLives = PlayerData.lives;
        currentLivesText.text = "health: "+ currentLives;
        currentWins = PlayerData.winsThisRun;
        currentWinsText.text = currentWins + " / " + winsNeededToComplete + " wins";
        if (deckManager.deckList == null) { deckSize = 0; }
        else { deckSize = deckManager.deckList.Count; }
        currentDeckSizeText.text = "deck size: " + deckSize;
    }

    public void PresentNewStartingDecks()
    {
        deckOffer = GenerateAvailableStartingDecks();
        float cardHeight = cardThumbPrefab.GetComponent<RectTransform>().rect.height;
        float cardWidth = cardThumbPrefab.GetComponent<RectTransform>().rect.width;
        offerMenu.SetActive(true);
        for(int deck = 0; deck < deckOffer.Count; deck++)
        {
            //generate & splay card thumbs
            for(int card = 0; card < deckOffer[deck].deckList.Count; card++)
            {
                GameObject nextCard = Instantiate(cardThumbPrefab, transform.position, Quaternion.identity);
                nextCard.transform.parent = offerMenu.transform;
                //set splay height and column position
                float newXPos = ((cardWidth * deck) - (cardWidth * (deckOffer.Count - 1) / 2)) * deckSpacingModifier;
                float newYPos = ((cardHeight * (deckOffer[deck].deckList.Count - 1) / 2) - (cardHeight * card)) * cardSpacingModifier;
                Vector3 nextCardPos = new Vector3(newXPos, newYPos, 0);
                nextCard.GetComponent<RectTransform>().localPosition = nextCardPos;
                //set card info
                nextCard.GetComponent<CardDetails>().SetDetails(deckOffer[deck].deckList[card]);
            }
            //create selection button
            GameObject selectButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity);
            selectButton.transform.parent = offerMenu.transform;
            float buttonXPos = ((cardWidth * deck) - (cardWidth * (deckOffer.Count - 1) / 2)) * deckSpacingModifier;
            float buttonYPos = ((cardHeight * (deckOffer[deck].deckList.Count - 1) / 2) - (cardHeight * deckOffer[deck].deckList.Count + 1)) * cardSpacingModifier;
            Vector3 buttonPos = new Vector3(buttonXPos, buttonYPos, 0);
            selectButton.GetComponent<RectTransform>().localPosition = buttonPos;
            //set button method
            int deckCopy = deck ;
            selectButton.GetComponent<Button>().onClick.AddListener(() => PlayerData.AddCardsToPlayerDeck(deckOffer[deckCopy].deckList));
            selectButton.GetComponent<Button>().onClick.AddListener(() => ConfirmCardSelection());
        }
    }
    
    private List<DeckList> GenerateAvailableStartingDecks()
    {
        if(CardManager.allDecks.Count == 0) { return null; }
        List<DeckList> newDecks = new List<DeckList>();
        foreach(DeckList deck in CardManager.allDecks)
        {
            if(deck.isStarter && deck.isUnlocked)
            {
                newDecks.Add(deck);
            }
        }

        return newDecks;
    }

    public void PresentNewCardOffer()
    {
        cardOffer = GenerateCardRewards();
        float cardHeight = cardFullPrefab.GetComponent<RectTransform>().rect.height;
        float cardWidth = cardFullPrefab.GetComponent<RectTransform>().rect.width;
        offerMenu.SetActive(true);
        for (int newCard = 0; newCard < cardOffer.Count; newCard++)
        {
            //generate & splay cards
            GameObject nextCard = Instantiate(cardFullPrefab, transform.position, Quaternion.identity);
            nextCard.transform.parent = offerMenu.transform;
            //set position
            float newXPos = ((cardWidth * newCard) - (cardWidth * (cardOffer.Count - 1) / 2)) * deckSpacingModifier;
            Vector3 nextCardPos = new Vector3(newXPos, 0, 0);
            nextCard.GetComponent<RectTransform>().localPosition = nextCardPos;
            //set card info
            nextCard.GetComponent<CardDetails>().SetDetails(cardOffer[newCard]);
            //create selection button
            GameObject selectButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity);
            selectButton.transform.parent = offerMenu.transform;
            float buttonXPos = ((cardWidth * newCard) - (cardWidth * (cardOffer.Count - 1) / 2)) * deckSpacingModifier;
            float buttonYPos = -cardHeight/2 * cardSpacingModifier;
            Vector3 buttonPos = new Vector3(buttonXPos, buttonYPos, 0);
            selectButton.GetComponent<RectTransform>().localPosition = buttonPos;
            //set button method
            int cardCopy = newCard;
            selectButton.GetComponent<Button>().onClick.AddListener(() => PlayerData.AddCardsToPlayerDeck(cardOffer[cardCopy]));
            selectButton.GetComponent<Button>().onClick.AddListener(() => ConfirmCardSelection());
        }
    }

    public List<PlayerCardAttributes> GenerateCardRewards()
    {
        if(CardManager.allCards.Count == 0) { return null; }
        List<PlayerCardAttributes> newCards = new();
        for(int newCard = 0; newCard < 3; newCard++)
        {
            int randomCardToPick = UnityEngine.Random.Range(0, CardManager.allCards.Count);
            newCards.Add(CardManager.allCards[randomCardToPick]);
        }
        return newCards;
    }

    public void ConfirmCardSelection()
    {
        deckManager.deckList = PlayerData.deckList;
        SetUpBattles();
        deckSize = deckManager.deckList.Count;
        currentDeckSizeText.text = "deck size: " + deckSize;
        offerMenu.SetActive(false);
    }

    public void SetUpBattles()
    {
        //assemble possible battle list
        battles = GenerateDynamicBattles();
        //create battle buttons
        //max 3 battles?
        int battlesToGenerate = battles.Count;
        Debug.Log("attempting to generate battles: " + battlesToGenerate);
        float buttonWidth = selectButtonPrefab.GetComponent<RectTransform>().rect.width;
        for (int battle = 0; battle < battlesToGenerate; battle++)
        {
            int tempBattleNum = battle;
            GameObject battleButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity, mainCanvas.transform);
            //battleButton.transform.parent = mainCanvas.transform;
            float newXPos = ((buttonWidth * battle) - (buttonWidth * (battlesToGenerate - 1 )/ 2)) * deckSpacingModifier;
            battleButton.transform.localPosition = new Vector3(newXPos, 0, 0);
            //battle details text
            List<TextMeshProUGUI> battleTexts = battleButton.GetComponentsInChildren<TextMeshProUGUI>().ToList();
            foreach(TextMeshProUGUI text in battleTexts)
            {
                if(text.name == "Choice Details")
                {
                    text.text = battles[tempBattleNum].totalPower + " power // " + battles[tempBattleNum].totalAttacks + " enemies ("
                        + battles[tempBattleNum].preferredElement.ToString() + ")";
                }
                else
                {
                    text.text = "Battle";
                }
            }
            battleButton.GetComponent<Button>().onClick.AddListener(() => SelectNextBattle(battles[tempBattleNum]));
            //battleButton.GetComponentInChildren<TextMeshProUGUI>().text = battles[tempBattleNum].name;
        }
    }

    public List<BattleAttributes> GenerateDynamicBattles()
    {
        //get all attacks
        List<EnemyAttackAttributes> allAttacks = new(Resources.LoadAll<EnemyAttackAttributes>("Enemies").ToList());
        //refine attacks based on progression
        List<EnemyAttackAttributes> possibleAttacks = new();
        int currentProgress = PlayerData.runProgress + PlayerData.winsThisRun;
        foreach(EnemyAttackAttributes attack in allAttacks)
        {
            if(attack.strength <= currentProgress + 2 && attack.strength >= PlayerData.winsThisRun)
            {
                possibleAttacks.Add(attack);
            }
        }
        //if no matching attacks, use all
        if(possibleAttacks.Count == 0)
        {
            possibleAttacks = allAttacks;
        }
        //generate 3 battles with max total strength defied by run progress?
        List<BattleAttributes> newBattles = new();
        currentProgress = (PlayerData.runProgress + PlayerData.winsThisRun + 1) * Mathf.Max(PlayerData.winsThisRun, 3);
        for(int battle = 0; battle < 3; battle++)
        {
            BattleAttributes nextBattle = new();
            nextBattle.enemyAttacks = new();
            int battleStrength = UnityEngine.Random.Range(currentProgress, currentProgress * PlayerData.winsThisRun);

            int firstEnemy = UnityEngine.Random.Range(0, possibleAttacks.Count);
            EnemyAttackAttributes enemy = possibleAttacks[firstEnemy];
            Element preferredElement = enemy.element;
            nextBattle.preferredElement = preferredElement;
            Debug.Log("Battle #" + (battle + 1) + " prefers " + preferredElement.ToString());
            nextBattle.enemyAttacks.Add(possibleAttacks[firstEnemy]);
            nextBattle.totalPower += possibleAttacks[firstEnemy].strength;
            nextBattle.preferredAttacks++;
            nextBattle.totalAttacks++;

            while ((nextBattle.totalPower <= battleStrength && nextBattle.totalAttacks < 5) || nextBattle.totalAttacks < 2)
            {
                int nextEnemy = UnityEngine.Random.Range(0, possibleAttacks.Count);
                EnemyAttackAttributes nextEnemyDetails = possibleAttacks[nextEnemy];
                if (nextEnemyDetails.element == preferredElement)
                {
                    nextBattle.enemyAttacks.Add(possibleAttacks[nextEnemy]);
                    nextBattle.totalPower += possibleAttacks[nextEnemy].strength;
                    nextBattle.preferredAttacks++;
                    nextBattle.totalAttacks++;
                    Debug.Log("Added a preferred " + preferredElement.ToString() + " card to Battle " + (battle + 1));
                }
                else if (nextBattle.preferredAttacks > nextBattle.offElementAttacks)
                {
                    nextBattle.enemyAttacks.Add(possibleAttacks[nextEnemy]);
                    nextBattle.totalPower += possibleAttacks[nextEnemy].strength;
                    nextBattle.offElementAttacks++;
                    nextBattle.totalAttacks++;
                    Debug.Log("Added an off " + possibleAttacks[nextEnemy].element.ToString() + " card to Battle " + (battle + 1));
                }
                else
                {
                    Debug.Log("didn't add an off " + possibleAttacks[nextEnemy].element.ToString() + " to Battle " + (battle + 1) + "("
                      + nextBattle.totalAttacks + " vs " + nextBattle.offElementAttacks + ")");
                }
            }
            newBattles.Add(nextBattle);
        }
        return newBattles;
    }

    public void SelectNextBattle(BattleAttributes nextBattle)
    {
        PlayerData.nextBattle = nextBattle;
        sceneChanger.LoadBattleScene();
    }
}
