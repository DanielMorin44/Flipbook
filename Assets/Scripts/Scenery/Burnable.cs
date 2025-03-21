using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Burnable : MonoBehaviour
{
    public Sprite burned;

    private SpriteRenderer display;

    public void Start()
    {
        display = GetComponent<SpriteRenderer>();
    }

    public void Burn()
    {
        display.sprite = burned;
    }
}
