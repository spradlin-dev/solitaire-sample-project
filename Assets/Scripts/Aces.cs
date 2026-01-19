using UnityEngine;

public class Aces : MonoBehaviour
{
    public GameObject[] acePiles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TryToPlaceCard(GameObject card)
    {
        Selectable cardData = card.GetComponent<Selectable>();

        foreach (GameObject acePile in acePiles)
        {
            if (acePile.transform.childCount == 0)
            {
                if (cardData.faceValue == 1)
                {
                    card.transform.parent = acePile.transform;
                    card.transform.position = acePile.transform.position;
                    return true;
                }
                continue;
            }
            GameObject topCard = null;
            foreach (Transform cardInPile in acePile.transform)
            {
                topCard = cardInPile.gameObject;
            }
            
            Selectable topCardData = topCard.GetComponent<Selectable>();
            if (cardData.suit == topCardData.suit && cardData.faceValue == topCardData.faceValue + 1)
            {
                card.transform.parent = acePile.transform;
                card.transform.position = acePile.transform.position;
                return true;
            }
        }
        return false;
    }
}
