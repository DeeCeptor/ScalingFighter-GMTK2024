using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mouse enter/exit changes material of spriterenderer
/// </summary>
public class OutlineOnMouseOver : MonoBehaviour
{
    /// <summary>
    /// Change material of spriterenderer
    /// </summary>
    public SpriteRenderer Sprite;
    public Material DefaultMaterial;

    public void ShowOutline(bool show)
    {
        if (Sprite == null)
        {
            Sprite = this.GetComponentInChildren<SpriteRenderer>();
        }
        if (Sprite == null)
            return;

        if (show && DefaultMaterial == null)
        {
            DefaultMaterial = Sprite.material;
        }

        if (show)
            Sprite.material = AssetHolder.Instance.SpriteOutlineMaterial;
        else
            Sprite.material = DefaultMaterial;
    }


    private void OnMouseEnter()
    {
        ShowOutline(true);
    }
    private void OnMouseExit()
    {
        ShowOutline(false);
    }
}
