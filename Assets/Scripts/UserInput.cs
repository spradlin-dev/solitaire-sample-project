using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class UserInput : MonoBehaviour
{
    public GameObject selectedCard;
    private Solitaire solitaire;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solitaire = this.gameObject.GetComponent<Solitaire>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMouseClick();
    }

    void GetMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10)    );
           
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit)
            {

                if (hit.collider != null)
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    GameObject parentObject = clickedObject.transform.parent?.gameObject;
                    Debug.Log("You clicked on " + clickedObject.name);
                    
                    Debug.Log("parent: " + parentObject?.name);
                    if (clickedObject.name == "Stock Slot")
                    {
                        HandleStockSlotClick(clickedObject);
                    }
                    else if (parentObject && parentObject.name == "Stock Slot")
                    {
                        HandleStockClick(clickedObject);
                    }
                    else if (parentObject && parentObject.name.Contains("Ace"))
                    {
                        HandleAcesClick(clickedObject);
                    }
                    else if (parentObject && parentObject.name.Contains("Column"))
                    {
                        HandleTableauClick(clickedObject, parentObject);
                    }
                    else if (parentObject && parentObject.name == "Waste Slot")
                    {
                        HandleWasteClick(clickedObject);
                    }
                }
            }
        }
    }

    void HandleStockClick(GameObject card)
    {
        // To be implemented
        print("Stock clicked: " + card.name);
        this.selectedCard = null;
        solitaire.Deal3ToWaste();
    }
    void HandleStockSlotClick(GameObject stockSlot)
    {
        // To be implemented
        print("Stock Slot clicked: " + stockSlot.name);
        this.selectedCard = null;
        solitaire.ResetStockFromWaste();
    }
    void HandleAcesClick(GameObject card)
    {
        // To be implemented
        print("Aces clicked: " + card.name);
        if (!card.GetComponent<Selectable>().isTopCardInContainer)
        {
            print("Not top card in ace pile, do nothing");
            this.selectedCard = null;
            return;
        }
        if (this.selectedCard == card)
        {
            this.selectedCard = null;
            this.solitaire.AttemptToPlayAcePileCard(card);
            return;
        }
        this.selectedCard = card;
    }
    void HandleTableauClick(GameObject card, GameObject column)
    {
        // To be implemented
        print("Tableau clicked: " + card.name + " in " + column.name);

        if (card.GetComponent<Selectable>().isFaceUp == false)
        {
            print("Card is face down, do nothing");
            this.selectedCard = null;
            return;
        }
        if (this.selectedCard == card && card.GetComponent<Selectable>().isTopCardInContainer)
        {
            this.selectedCard = null;
            // TODO implement moving the card to a valid location
            // Destroy(card);
            this.solitaire.AttemptToPlayCard(card);
            return;
        } else if (this.selectedCard == card && !card.GetComponent<Selectable>().isTopCardInContainer)
        {
            this.solitaire.AttemptToPlayCardStack(card);
            return;
        }
        this.selectedCard = card;
    }

    void HandleWasteClick(GameObject card)
    {
        // To be implemented
        print("Waste clicked: " + card.name);
        if (!card.GetComponent<Selectable>().isTopCardInContainer)
        {
            print("Not top card in waste pile, do nothing");
            this.selectedCard = null;
            return;
        }
        if (this.selectedCard == card)
        {
            this.selectedCard = null;
            this.solitaire.AttemptToPlayCard(card);
            return;
        }
        this.selectedCard = card;
    }

}
