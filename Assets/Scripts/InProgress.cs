using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InProgress : MonoBehaviour
{
    private Solitaire solitaire;
    private Image image;
    void Start()
    {
        solitaire = FindFirstObjectByType<Solitaire>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (solitaire.solverCount > 0) 
        { 
            image.color = Color.red;
        } 
        else
        {
            image.color = Color.springGreen;
        }
    }
}
