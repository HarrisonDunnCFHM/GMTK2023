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

    [SerializeField] GameObject offerMenu;
    [SerializeField] float deckSpacingModifier;
    [SerializeField] float cardSpacingModifier;
    [SerializeField] GameObject cardThumbPrefab;
    [SerializeField] GameObject selectButtonPrefab;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] int maxBattles = 3;

    List<DeckList> decks;
    public List<BattleAttributes> battles;

    SceneChanger sceneChanger;

    // Start is called before the first frame update
    void Start()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();


        SetUpPlayerData();
    }
    
    private void SetUpPlayerData()
    {
        if (deckManager == null) { deckManager = FindObjectOfType<DeckManager>(); }
        if (PlayerData.runProgress == 0)
        {
            currentLives = PlayerData.lives;
            currentLivesText.text = currentLives.ToString() + " lives";
            PresentNewStartingDecks();
        }
        else
        {
            LoadPlayerData();
            SetUpBattles();

        }
    }

    private void LoadPlayerData()
    {
        deckManager.deckList = PlayerData.LoadPlayerDeck();
        currentLives = PlayerData.lives;
        currentLivesText.text = currentLives.ToString() + " lives";
    }

    public void PresentNewStartingDecks()
    {
        decks = GenerateAvailableStartingDecks();
        float cardHeight = cardThumbPrefab.GetComponent<RectTransform>().rect.height;
        float cardWidth = cardThumbPrefab.GetComponent<RectTransform>().rect.width;
        offerMenu.SetActive(true);
        for(int deck = 0; deck < decks.Count; deck++)
        {
            //generate & splay card thumbs
            for(int card = 0; card < decks[deck].deckList.Count; card++)
            {
                GameObject nextCard = Instantiate(cardThumbPrefab, transform.position, Quaternion.identity);
                nextCard.transform.parent = offerMenu.transform;
                //set splay height and column position
                float newXPos = ((cardWidth * deck) - (cardWidth * (decks.Count - 1) / 2)) * deckSpacingModifier;
                float newYPos = ((cardHeight * (decks[deck].deckList.Count - 1) / 2) - (cardHeight * card)) * cardSpacingModifier;
                Vector3 nextCardPos = new Vector3(newXPos, newYPos, 0);
                nextCard.GetComponent<RectTransform>().localPosition = nextCardPos;
                //set card info
                nextCard.GetComponent<CardDetails>().SetDetails(decks[deck].deckList[card]);
            }
            //create selection button
            GameObject selectButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity);
            selectButton.transform.parent = offerMenu.transform;
            float buttonXPos = ((cardWidth * deck) - (cardWidth * (decks.Count - 1) / 2)) * deckSpacingModifier;
            float buttonYPos = ((cardHeight * (decks[deck].deckList.Count - 1) / 2) - (cardHeight * decks[deck].deckList.Count + 1)) * cardSpacingModifier;
            Vector3 buttonPos = new Vector3(buttonXPos, buttonYPos, 0);
            selectButton.GetComponent<RectTransform>().localPosition = buttonPos;
            //set button method
            int deckCopy = deck ;
            selectButton.GetComponent<Button>().onClick.AddListener(() => PlayerData.AddCardsToPlayerDeck(decks[deckCopy].deckList));
            selectButton.GetComponent<Button>().onClick.AddListener(() => CompleteDeckSelection());
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
    
    public void CompleteDeckSelection()
    {
        deckManager.deckList = PlayerData.deckList;
        SetUpBattles();
        offerMenu.SetActive(false);
    }

    public void SetUpBattles()
    {
        //assemble possible battle list
        battles = new(Resources.LoadAll<BattleAttributes>("Battles").ToList());
        //create battle buttons
        //max 3 battles?
        int battlesToGenerate = Mathf.Min(maxBattles, battles.Count);
        Debug.Log("attempting to generate battles: " + battlesToGenerate);
        float buttonWidth = selectButtonPrefab.GetComponent<RectTransform>().rect.width;
        for (int battle = 0; battle < battlesToGenerate; battle++)
        {
            GameObject battleButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity, mainCanvas.transform);
            //battleButton.transform.parent = mainCanvas.transform;
            float newXPos = ((buttonWidth * battle) - (buttonWidth * (battlesToGenerate - 1 )/ 2)) * deckSpacingModifier;
            battleButton.transform.localPosition = new Vector3(newXPos, 0, 0);
            int tempBattleNum = battle;
            battleButton.GetComponent<Button>().onClick.AddListener(() => SelectNextBattle(battles[tempBattleNum]));
            battleButton.GetComponentInChildren<TextMeshProUGUI>().text = battles[tempBattleNum].name;
        }
    }

    public void SelectNextBattle(BattleAttributes nextBattle)
    {
        PlayerData.nextBattle = nextBattle;
        sceneChanger.LoadBattleScene();
    }
}
