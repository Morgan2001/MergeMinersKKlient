using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour
{
    public Sprite InactiveSprite;
    public Sprite ActiveSprite;
    private Sprite originalSprite;

    private Image image;
    
    public void Construct()
    {
        image = GetComponent<Image>();

        originalSprite = image.sprite;
    }

    public void SetOriginalSprite()
    {
        image.sprite = originalSprite;
    }
    public void SetInactiveSprite()
    {
        image.sprite = InactiveSprite;
    }
    public void SetActiveSprite()
    {
        image.sprite = ActiveSprite;
    }
}
