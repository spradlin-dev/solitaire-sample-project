using UnityEngine;

public class LayoutColumn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RefreshColumnLayout();
    }

    void RefreshColumnLayout()
    {
        GameObject[] columnCards = new GameObject[] { };
        foreach (Transform card in this.transform)
        {
            columnCards = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Append(columnCards, card.gameObject));
        }
        for (int i = 0; i < columnCards.Length; i++)
        {
            Transform card = columnCards[i].transform;
            card.position = new Vector3(this.transform.position.x, this.transform.position.y - (0.3f * i), this.transform.position.z - (0.01f * (i + 1)));
        }
        if (columnCards.Length > 0)
        {
            // always make the top card face up
            Transform topCard = columnCards[columnCards.Length - 1].transform;
            topCard.GetComponent<Selectable>().isFaceUp = true;
        }
    }
}
