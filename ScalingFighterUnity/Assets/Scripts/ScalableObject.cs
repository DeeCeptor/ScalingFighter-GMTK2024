using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HOLDING left/right-click increases/decreases size
/// </summary>
public class ScalableObject : MonoBehaviour
{
    /// <summary>
    /// How fast we change scale
    /// </summary>
    public float ScaleChangeSpeed = 1f;
    /// <summary>
    /// Smallest scale allowed
    /// </summary>
    public float MinScale = 0.1f;
    /// <summary>
    /// Smallest scale allowed
    /// </summary>
    public float MaxScale = 10f;
    /// <summary>
    /// Left-click to grow
    /// </summary>
    public bool CanGrow = true;
    /// <summary>
    /// Right-click to shrink
    /// </summary>
    public bool CanShrink = true;
    /// <summary>
    /// Is mouse hovering inside object?
    /// </summary>
    public bool MouseInside;
    /// <summary>
    /// Extra actions to take if this does something special
    /// </summary>
    public Button.ButtonClickedEvent onMouseLeftDown;
    public Button.ButtonClickedEvent onMouseRightDown;
    public Button.ButtonClickedEvent onMouseMiddleDown;

    public void ChangeSize(float amount)
    {
        this.transform.localScale = new Vector3(
            Mathf.Clamp(this.transform.localScale.x + amount, MinScale, MaxScale)
            , Mathf.Clamp(this.transform.localScale.y + amount, MinScale, MaxScale)
            , Mathf.Clamp(this.transform.localScale.z + amount, MinScale, MaxScale));
    }

    void Update()
    {
        if (MouseInside)
        {
            // Clicking will change size if mouse inside
            if (Input.GetMouseButton(0))
            {
                if (onMouseLeftDown != null)
                    onMouseLeftDown.Invoke();
                this.ChangeSize(ScaleChangeSpeed * Time.deltaTime);
            }
            else if (Input.GetMouseButton(1))
            {
                if (onMouseRightDown != null)
                    onMouseRightDown.Invoke();
                this.ChangeSize(-ScaleChangeSpeed * Time.deltaTime);
            }
        }
    }



    public void OnMouseEnter()
    {
        MouseInside = true;
    }
    private void OnMouseOver()
    {
        // Left-click
        if (Input.GetMouseButtonDown(0))
        {
            onMouseLeftDown.Invoke();
        }
        // Right-click
        else if (Input.GetMouseButtonDown(1))
        {
            onMouseRightDown.Invoke();
        }
        // Middle-click
        else if (Input.GetMouseButtonDown(2))
        {
            onMouseMiddleDown.Invoke();
        }
    }
    public void OnMouseExit()
    {
        MouseInside = false;
    }
}
