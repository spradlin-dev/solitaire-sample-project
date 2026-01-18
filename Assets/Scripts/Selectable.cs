using System.Linq;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isFaceUp = false;

    public bool isHeart
    {
       get => this.name[1] == 'H';
    }
    public bool isClub
    {
       get => this.name[1] == 'C';
    }   
    public bool isDiamond
    {
       get => this.name[1] == 'D';
    }
    public bool isSpade
    {
       get => this.name[1] == 'S';
    }
    public bool isRed
    {
       get => isHeart || isDiamond;
    }
    public bool isBlack
    {
       get => isClub || isSpade;
    }

    public char suit
    {
        get 
        {
            return this.name[1];
        }
    }

    public int faceValue
    {
        get 
        {
            char faceChar = this.name[0];
            string faces = "A23456789TJQK";
            return faces.IndexOf(faceChar) + 1;
        }
    }

    public bool isTopCardInContainer
    {
        get
        {
            Transform parent = this.transform.parent;
            
            int siblingIndex = this.transform.GetSiblingIndex();
            return siblingIndex == parent.childCount - 1;
          
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
