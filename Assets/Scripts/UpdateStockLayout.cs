using UnityEngine;

public class UpdateStockLayout : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshStockLayout();
    }

    void RefreshStockLayout()
    {
        GameObject[] stockCards = new GameObject[] { };
        foreach (Transform card in this.transform)
        {
            stockCards = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Append(stockCards, card.gameObject));
        }
        for (int i = 0; i < stockCards.Length; i++)
        {
            Transform card = stockCards[i].transform;
            card.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - (0.01f * (i + 1)));
            card.GetComponent<Selectable>().isFaceUp = false;
        }
    }
}
