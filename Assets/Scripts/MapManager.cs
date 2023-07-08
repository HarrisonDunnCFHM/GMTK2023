using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public DeckManager deckManager;

    public int currentLives;
    [SerializeField] TextMeshProUGUI currentLivesText;

    [SerializeField] GameObject offerMenu;
    [SerializeField] float deckSpacingModifier;
    [SerializeField] float cardSpacingModifier;
    [SerializeField] GameObject cardThumbPrefab;
    [SerializeField] GameObject deckSelectButtonPrefab;

    List<DeckList> decks;

    // Start is called before the first frame update
    void Start()
    {
        if(deckManager == null) { deckManager = FindObjectOfType<DeckManager>(); }
        if (PlayerData.runProgress == 0)
        {
            currentLives = PlayerData.lives;
            currentLivesText.text = currentLives.ToString() + " lives";
            PresentNewStartingDecks();
        }
        else 
        { 
            LoadPlayerData(); 
        }
        Debug.Log("ready with " + PlayerData.lives + " lives");
    }

    

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log("offering " + decks.Count.ToString() + " decks");
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
            GameObject selectButton = Instantiate(deckSelectButtonPrefab, transform.position, Quaternion.identity);
            selectButton.transform.parent = offerMenu.transform;
            float buttonXPos = ((cardWidth * deck) - (cardWidth * (decks.Count - 1) / 2)) * deckSpacingModifier;
            float buttonYPos = ((cardHeight * (decks[deck].deckList.Count - 1) / 2) - (cardHeight * decks[deck].deckList.Count + 1)) * cardSpacingModifier;
            Vector3 buttonPos = new Vector3(buttonXPos, buttonYPos, 0);
            selectButton.GetComponent<RectTransform>().localPosition = buttonPos;
            //set button method
            Debug.Log("setting button value to " + deck);
            int deckCopy = deck ;
            selectButton.GetComponent<Button>().onClick.AddListener(() => ButtonDebug(deckCopy));
            selectButton.GetComponent<Button>().onClick.AddListener(() => PlayerData.AddCardsToPlayerDeck(decks[deckCopy].deckList));
            selectButton.GetComponent<Button>().onClick.AddListener(() => CompleteDeckSelection());
        }
    }

    public void CompleteDeckSelection()
    {
        deckManager.deckList = PlayerData.deckList;
        offerMenu.SetActive(false);
    }

    public void ButtonDebug(int deckNumber)
    {
        Debug.Log(deckNumber);
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

}
