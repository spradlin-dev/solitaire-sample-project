using System;
using UnityEngine;

public class UpdateWasteLayout : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReformatWastePile();
    }

    public void ReformatWastePile()
    {
        GameObject[] wasteCards = new GameObject[] { };
        foreach (Transform card in this.transform)
        {
            wasteCards = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Append(wasteCards, card.gameObject));
        }
        for (int i = 0; i < wasteCards.Length; i++)
        {
            Transform card = wasteCards[i].transform;
            if (i < wasteCards.Length - 3)
            {
                card.position = new Vector3(this.transform.position.x + -0.6f, this.transform.position.y, this.transform.position.z - (0.01f * (i + 1)));
            }
            else
            {
                // make the last three cards spread out
                card.position = new Vector3(this.transform.position.x + -0.6f + (i - (wasteCards.Length - 3)) * 0.3f, this.transform.position.y, this.transform.position.z - (0.01f * (i + 1)));
                
            }
            card.GetComponent<Selectable>().isFaceUp = true;
        }
    }
}
