using UnityEngine;

public class LayoutAcePile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Layout();
    }

    void Layout()
    {
        GameObject[] pileCards = new GameObject[] { };
        foreach (Transform card in this.transform)
        {
            pileCards = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Append(pileCards, card.gameObject));
        }
        for (int i = 0; i < pileCards.Length; i++)
        {
            Transform card = pileCards[i].transform;
            card.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - (0.01f * (i + 1)));
            card.GetComponent<Selectable>().isFaceUp = true;
        }
    }
}
