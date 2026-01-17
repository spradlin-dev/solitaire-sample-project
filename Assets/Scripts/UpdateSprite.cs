using System;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Selectable selectable;
    private Solitaire solitaire;
    private UserInput userInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        solitaire = FindFirstObjectByType<Solitaire>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectable = GetComponent<Selectable>();
        userInput = FindFirstObjectByType<UserInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (selectable.isFaceUp == true) {
             spriteRenderer.sprite = Resources.Load<Sprite>("png/face/" + this.name + "@3x");
    
        } else {
            spriteRenderer.sprite = Resources.Load<Sprite>("png/back/bicycle_blue@3x");
        }
        
        if (userInput && userInput.selectedCard && this.name == userInput.selectedCard.name)
        {
            spriteRenderer.color = Color.yellow;

        } else
        {
             spriteRenderer.color = Color.white;
        }
        
    }
}
