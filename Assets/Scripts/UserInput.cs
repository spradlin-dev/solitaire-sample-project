using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class UserInput : MonoBehaviour
{
    public GameObject selectedCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
                    GameObject parentObject = clickedObject.transform.parent.gameObject;
                    Debug.Log("You clicked on " + clickedObject.name);
                    
                    Debug.Log("parent: " + parentObject.name);
                    if (clickedObject.name == "Stock Slot")
                    {
                        HandleStockSlotClick(clickedObject);
                    }
                    else if (parentObject.name == "Stock Slot")
                    {
                        HandleStockClick(clickedObject);
                    }
                    else if (parentObject.name.Contains("Column"))
                    {
                        HandleTableauClick(clickedObject, parentObject);
                    }
                    else if (parentObject.name == "Waste Slot")
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
        this.gameObject.GetComponent<Solitaire>().Deal3ToWaste();
    }
    void HandleStockSlotClick(GameObject stockSlot)
    {
        // To be implemented
        print("Stock Slot clicked: " + stockSlot.name);
        this.selectedCard = null;
        this.gameObject.GetComponent<Solitaire>().ResetStockFromWaste();
    }
    void HandleTableauClick(GameObject card, GameObject column)
    {
        // To be implemented
        print("Tableau clicked: " + card.name + " in " + column.name);
        // if the clicked card is not the top card in the column, do nothing

        // if the clicked card is face up
        // select the card and all cards below it
        if (card.GetComponent<Selectable>().isFaceUp == false)
        {
            print("Card is face down, do nothing");
            this.selectedCard = null;
            return;
        }
        this.selectedCard = card;
        print("IS TOP CARD?? " + card.GetComponent<Selectable>().isTopCardInContainer);
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
        this.selectedCard = card;
    }

}
