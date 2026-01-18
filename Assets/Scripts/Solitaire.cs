using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using System.Collections;

public class Solitaire : MonoBehaviour
{

    public static string[] suits = new string[] { "H", "C", "D", "S" };
    public static string[] faces = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K" };
    public GameObject cardPrefab;
    public GameObject tableau;
    public GameObject aces;
    public GameObject deckSlot;
    public GameObject wasteSlot;
    private List<GameObject> cardsInPlay;
    public bool isInitialized = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayCards();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayCards()
    {
        isInitialized = false;
        if (cardsInPlay != null) {             // clear existing cards
            foreach (GameObject card in cardsInPlay)
            {
                Destroy(card);
            }
        }
        cardsInPlay = ShuffleDeck(GenerateDeck());

        StartCoroutine(Deal(cardsInPlay));
    }

    public static List<GameObject> ShuffleDeck(List<GameObject> unshuffledDeck)
    {
        System.Random r = new System.Random();
        List<GameObject> shuffledDeck = new List<GameObject>();
        //Step 1: For each remaining unshuffled letter
        for (int n = unshuffledDeck.Count; n > 0; n--)
        {
            //Step 2: Randomly select one of the remaining unshuffled letters
            int k = r.Next(n);

            //Step 3: Place the selected letter in the shuffled collection
            GameObject temp = unshuffledDeck[k];
            shuffledDeck.Add(temp);

            //Step 4: Remove the selected letter from the unshuffled collection
            unshuffledDeck.RemoveAt(k);
        }
        return shuffledDeck;
    }

    public List<GameObject> GenerateDeck()
    {
        List<GameObject> newDeck = new List<GameObject>();

        foreach (string s in suits)
        {
            foreach (string f in faces)
            {
                GameObject newCard = Instantiate(cardPrefab);
                newCard.name = f + s;
                newDeck.Add(newCard);
            }
        }
        return newDeck;
    }

    IEnumerator Deal(List<GameObject> deckTodeal)
    {
        WaitForSeconds waitforSeconds = new WaitForSeconds(0.05f);
        List<GameObject> deckToDealCopy = new List<GameObject>(deckTodeal);
        // deal 28 cards to tableau
        int columnLength = tableau.GetComponent<Columns>().columnObjects.Length;
        for (int i = 0; i < columnLength; i++)
        {
            yield return waitforSeconds;
            for (int j = i; j < columnLength; j++)
            {
                yield return waitforSeconds;
                GameObject card = deckToDealCopy[0];
                deckToDealCopy.RemoveAt(0);
                GameObject column = tableau.GetComponent<Columns>().columnObjects[j];
                card.transform.parent = column.transform;
                if (j == i)
                {
                    card.GetComponent<Selectable>().isFaceUp = true;
                }
            }
        }
        // deal remaining cards to deck slot
        foreach (GameObject card in deckToDealCopy)
        {
            card.transform.parent = deckSlot.transform;

        }
        isInitialized = true;
        
    }
    public void Solve()
    {
        StartCoroutine(AttemptSolve());
    }

    IEnumerator AttemptSolve()
    {
        WaitForSeconds waitforSeconds = new WaitForSeconds(0.05f);
        GameObject topWasteCard = null;
        foreach (Transform card in wasteSlot.transform) {
            topWasteCard = card.gameObject;
        }
        if (topWasteCard != null)
        {
            yield return waitforSeconds;
            print("playing waste card"); print(topWasteCard);
            if (aces.GetComponent<Aces>().TryToPlaceCard(topWasteCard))
            {
                Solve();
            }
        }
        foreach (GameObject column in tableau.GetComponent<Columns>().columnObjects)
        {
            yield return waitforSeconds;

            GameObject topCard = null;

            foreach (Transform cardInColumn in column.transform)
            {
                topCard = cardInColumn.gameObject;
            }
            if (topCard == null)
            {
                continue;
            }
            print("playing card"); print(topCard);
            if (aces.GetComponent<Aces>().TryToPlaceCard(topCard))
            {
                Solve();
            }
            // attempt to play card

        }
    }

    public void Deal3ToWaste()
    {
        // To be implemented
        GameObject[] deckCards = new GameObject[] { };

        foreach (Transform card in deckSlot.transform)
        {
            deckCards = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Append(deckCards, card.gameObject));
        }

        // deal UP TO 3 cards (less if there aren't 3 cards available)
        int dealCount = Mathf.Min(3, deckCards.Length);

        for (int i = 0; i < dealCount; i++)
        {
            GameObject card = deckCards[deckCards.Length - 1 - i];
            card.transform.parent = wasteSlot.transform;
        }
    }

    public void ResetStockFromWaste()
    {
        // To be implemented
        GameObject[] wasteCards = new GameObject[] { };
        foreach (Transform card in wasteSlot.transform)
        {
            wasteCards = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Append(wasteCards, card.gameObject));
        }
        for (int i = wasteCards.Length - 1; i >= 0; i--)
        {
            GameObject oldCard = wasteCards[i];
            oldCard.transform.parent = deckSlot.transform;
        }
    }

    public bool AttemptToPlayCard(GameObject card)
    {
        // To be implemented
        print("Attempt to play card: " + card.name);
        bool wasPlaced = aces.GetComponent<Aces>().TryToPlaceCard(card);

        if (!wasPlaced)
        {
            print("Could not place card in aces: " + card.name);
            wasPlaced = tableau.GetComponent<Columns>().TryToPlaceCard(card);
        }
        return wasPlaced;

    }

    public void AttemptToPlayAcePileCard(GameObject card)
    {
        print("ATTEMPTING TO PLAY ACE PILE CARD: " + card.name);
        tableau.GetComponent<Columns>().TryToPlaceCard(card);
    }

    public void AttemptToPlayCardStack(GameObject card)
    {
        print("ATTEMPTING TO PLAY CARD STACK: " + card.name);
        tableau.GetComponent<Columns>().TryToPlaceCardStack(card.GetComponent<Selectable>().stackedWithSubsequentSiblings);
    }

}
