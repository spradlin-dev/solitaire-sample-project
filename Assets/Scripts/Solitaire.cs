using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;

public class Solitaire : MonoBehaviour
{

    public static string[] suits = new string[] {"H", "C", "D", "S"};
    public static string[] faces = new string[] {"A", "2", "3", "4", "5", "6", "7", "8", "9", "T", "J", "Q", "K"};
    public static List<string> deck;
    public GameObject cardPrefab;
    public GameObject tableau;
    public GameObject deckSlot;
    public GameObject wasteSlot;

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
        deck = ShuffleDeck(GenerateDeck());

        Deal(deck);
    }

    public static List<string> ShuffleDeck(List<string> unshuffledDeck)
    {
        System.Random r = new System.Random();
        List<string> shuffledDeck = new List<string>();
        //Step 1: For each remaining unshuffled letter
        for (int n = unshuffledDeck.Count; n > 0; n--)
        {
            //Step 2: Randomly select one of the remaining unshuffled letters
            int k = r.Next(n);

            //Step 3: Place the selected letter in the shuffled collection
            string temp = unshuffledDeck[k];
            shuffledDeck.Add(temp);

            //Step 4: Remove the selected letter from the unshuffled collection
            unshuffledDeck.RemoveAt(k);
        }
        return shuffledDeck;
    }

    public static List<string> GenerateDeck() 
    {
     List<string> newDeck = new List<string>();

    foreach (string s in suits)
    {
        foreach (string f in faces) 
        {
           newDeck.Add(f+s); 
        }
     }
     return newDeck;
    }

    void Deal(List<string> deckTodeal)
    {
        float zOffset = 0.01f;
        // deal 28 cards to tableau
        int columnLength = tableau.GetComponent<Columns>().columnObjects.Length;
        for (int i = 0; i < columnLength; i++)
        {
            for (int j = i; j < columnLength; j++)
            {
                string card = deckTodeal[0];
                deckTodeal.RemoveAt(0);
                GameObject column = tableau.GetComponent<Columns>().columnObjects[j];
                GameObject cardObject = Instantiate(cardPrefab, column.transform.position, Quaternion.identity, column.transform);
                cardObject.name = card;
                if (j == i)
                {
                    cardObject.GetComponent<Selectable>().isFaceUp = true;
                }

            }
        }
        // deal remaining cards to deck slot
        foreach (string card in deckTodeal) 
        {
            GameObject cardObject = Instantiate(cardPrefab, new Vector3(deckSlot.transform.position.x, deckSlot.transform.position.y, deckSlot.transform.position.z - zOffset), Quaternion.identity, deckSlot.transform);
            cardObject.name = card; 
            zOffset += 0.01f;

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

}
