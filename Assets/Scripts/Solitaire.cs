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
    private int _solverCount = 0;
    private static uint currentSeed = (uint)System.DateTime.Now.Ticks;

    public int solverCount
    {
        get => _solverCount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NewDeal();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewDeal()
    {
        currentSeed = (uint)System.DateTime.Now.Ticks;
        DealWithSeed(currentSeed);
    }

    public void RestartDeal()
    {
        DealWithSeed(currentSeed);
    }

    public static List<GameObject> ShuffleDeck(List<GameObject> unshuffledDeck, uint seed)
    {
        Unity.Mathematics.Random r = new Unity.Mathematics.Random(seed);
        List<GameObject> shuffledDeck = new List<GameObject>();
        //Step 1: For each remaining unshuffled letter
        for (int n = unshuffledDeck.Count; n > 0; n--)
        {
            //Step 2: Randomly select one of the remaining unshuffled letters
            int k = r.NextInt(n);

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
        if (solverCount == 0)
        {
            StartCoroutine(AttemptSolve());
        }
    }

    private void ForceSolve()
    {
        StartCoroutine(AttemptSolve());
    }

    public void DealWithSeed(uint seed)
    {
        isInitialized = false;
        if (cardsInPlay != null)
        {             // clear existing cards
            foreach (GameObject card in cardsInPlay)
            {
                Destroy(card);
            }
        }
        cardsInPlay = ShuffleDeck(GenerateDeck(), seed);

        StartCoroutine(Deal(cardsInPlay));
    }

    IEnumerator AttemptSolve()
    {
        _solverCount++;
        WaitForSeconds waitforSeconds = new WaitForSeconds(0.2f);
        GameObject topWasteCard = null;
        if (wasteSlot.transform.childCount != 0)
        {
            foreach (Transform card in wasteSlot.transform)
            {
                topWasteCard = card.gameObject;
            }
           
            if (aces.GetComponent<Aces>().TryToPlaceCard(topWasteCard))
            {
                yield return waitforSeconds;
                Solve();
            }
        }
        
        foreach (GameObject column in tableau.GetComponent<Columns>().columnObjects)
        {
            GameObject topCard = null;
            if (column.transform.childCount == 0)
            {
                continue;
            }
            foreach (Transform cardInColumn in column.transform)
            {
                topCard = cardInColumn.gameObject;
            }
            if (aces.GetComponent<Aces>().TryToPlaceCard(topCard))
            {
                yield return waitforSeconds;
                Solve();
            }
        }
        _solverCount--;
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
        bool wasPlaced = aces.GetComponent<Aces>().TryToPlaceCard(card);

        if (!wasPlaced)
        {
            wasPlaced = tableau.GetComponent<Columns>().TryToPlaceCard(card);
        }
        return wasPlaced;

    }

    public void AttemptToPlayAcePileCard(GameObject card)
    {
        tableau.GetComponent<Columns>().TryToPlaceCard(card);
    }

    public void AttemptToPlayCardStack(GameObject card)
    {
        tableau.GetComponent<Columns>().TryToPlaceCardStack(card.GetComponent<Selectable>().stackedWithSubsequentSiblings);
    }

}
