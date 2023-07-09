using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullDeckListViewer : MonoBehaviour
{
    List<PlayerCardAttributes> fullDeckList;
    [SerializeField] GameObject cardThumbPrefab;
    [SerializeField] GameObject deckListWindow;
    [SerializeField] float cardSpacingModifier;
    [SerializeField] float deckScale = 0.75f;

    public List<GameObject> generatedDeck;
    List<PlayerCardAttributes> currentDeckList;
    List<PlayerCardAttributes> battleDeckList;

    private void Awake()
    {
        generatedDeck = new();
    }

    public void ShowDeckList()
    {
        DestroyGeneratedDeck();
        fullDeckList = PlayerData.deckList;
        float cardHeight = cardThumbPrefab.GetComponent<RectTransform>().rect.height * deckScale;
        float cardWidth = cardThumbPrefab.GetComponent<RectTransform>().rect.width;
        deckListWindow.SetActive(true);
        //generate & splay card thumbs
        for (int card = 0; card < fullDeckList.Count; card++)
        {
            GameObject nextCard = Instantiate(cardThumbPrefab, transform.position, Quaternion.identity);
            nextCard.transform.parent = deckListWindow.transform;
            //set splay height and column position
            //float newXPos = ((cardWidth * deck) - (cardWidth * (deckOffer.Count - 1) / 2)) * deckSpacingModifier;
            float newYPos = ((cardHeight * (fullDeckList.Count - 1) / 2) - (cardHeight * card)) * cardSpacingModifier;
            Vector3 nextCardPos = new Vector3(0, newYPos, 0);
            nextCard.GetComponent<RectTransform>().localPosition = nextCardPos;
            nextCard.GetComponent<RectTransform>().localScale = new Vector3(deckScale, deckScale, 1);
            //set card info
            nextCard.GetComponent<CardDetails>().SetDetails(fullDeckList[card]);
            generatedDeck.Add(nextCard);
        }
        ////create selection button
        //GameObject selectButton = Instantiate(selectButtonPrefab, transform.position, Quaternion.identity);
        //selectButton.transform.parent = deckListWindow.transform;
        //float buttonXPos = ((cardWidth * deck) - (cardWidth * (deckOffer.Count - 1) / 2)) * deckSpacingModifier;
        //float buttonYPos = ((cardHeight * (deckOffer[deck].deckList.Count - 1) / 2) - (cardHeight * deckOffer[deck].deckList.Count + 1)) * cardSpacingModifier;
        //Vector3 buttonPos = new Vector3(buttonXPos, buttonYPos, 0);
        //selectButton.GetComponent<RectTransform>().localPosition = buttonPos;
        ////set button method
        //int deckCopy = deck;
        //selectButton.GetComponent<Button>().onClick.AddListener(() => PlayerData.AddCardsToPlayerDeck(deckOffer[deckCopy].deckList));
        //selectButton.GetComponent<Button>().onClick.AddListener(() => ConfirmCardSelection());
    }

    public void ToggleDeckWindow()
    {
        if(deckListWindow.activeSelf)
        {
            DestroyGeneratedDeck();
            deckListWindow.SetActive(false);
        }
        else
        {
            ShowDeckList();
        }
    }

    public void DestroyGeneratedDeck()
    {
        if(generatedDeck.Count == 0) { return; }
        foreach(GameObject card in generatedDeck)
        {
            Destroy(card);
        }
        generatedDeck = new();
    }

}
