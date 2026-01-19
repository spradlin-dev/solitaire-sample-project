using System;
using UnityEngine;

public class Columns : MonoBehaviour
{
    public GameObject[] columnObjects;

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
        foreach (GameObject column in columnObjects)
        {
            if (card.transform.parent == column.transform)
            {
                // card is already in this column
                continue;
            }
            if (column.transform.childCount == 0)
            {
                if (card.GetComponent<Selectable>().faceValue == 13)
                {
                    card.transform.parent = column.transform;
                    return true;
                }
                continue;
            } 
            GameObject topCard = null;
            foreach (Transform cardInColumn in column.transform)
            {
                topCard = cardInColumn.gameObject;
            }
            Selectable cardData = card.GetComponent<Selectable>();
            Selectable topCardData = topCard.GetComponent<Selectable>();
            if (((cardData.isBlack && topCardData.isRed) || (cardData.isRed && topCardData.isBlack)) && cardData.faceValue == topCardData.faceValue - 1)
            {
                card.transform.parent = column.transform;
                return true;
            }
        }
        return false;
    }

    public bool TryToPlaceCardStack(GameObject[] stack)
    {
        foreach (GameObject column in columnObjects)
        {
            if (stack[0].transform.parent == column.transform)
            {
                // stack is already in this column
                continue;
            }
            if (column.transform.childCount == 0 )
            {
                if (stack[0].GetComponent<Selectable>().faceValue == 13)
                {
                    foreach (GameObject card in stack)
                    {
                        card.transform.parent = column.transform;
                    }
                    return true;
                }
                continue;
            } 
            GameObject topCard = null;
            foreach (Transform cardInColumn in column.transform)
            {
                topCard = cardInColumn.gameObject;
            }
            Selectable bottomCardData = stack[0].GetComponent<Selectable>();
            if (topCard == null)
            {
                // empty column, only accept kings
                if (bottomCardData.faceValue == 13)
                {
                    foreach (GameObject card in stack)
                    {
                        card.transform.parent = column.transform;
                    }
                    return true;
                }
            }
            else
            {
                Selectable topCardData = topCard.GetComponent<Selectable>();
                if (((bottomCardData.isBlack && topCardData.isRed) || (bottomCardData.isRed && topCardData.isBlack)) && bottomCardData.faceValue == topCardData.faceValue - 1)
                {
                    foreach (GameObject card in stack)
                    {
                        card.transform.parent = column.transform;
                    }
                    return true;
                }
            }
        }
        return false;
    }
}
