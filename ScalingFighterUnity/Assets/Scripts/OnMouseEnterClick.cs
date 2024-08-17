using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
/// Requires only a collider, invokes events
/// </summary>
public class OnMouseEnterClick : MonoBehaviour
{
    public Button.ButtonClickedEvent onMouseEnter;
    public Button.ButtonClickedEvent onMouseDown;
    public Button.ButtonClickedEvent onMouseLeftDown;
    public Button.ButtonClickedEvent onMouseRightDown;
    public Button.ButtonClickedEvent onMouseMiddleDown;
    public Button.ButtonClickedEvent onMouseExit;


    private void OnMouseEnter()
    {
        Debug.Log("OnMouseEnter", this.gameObject);
        if (onMouseEnter != null)
            onMouseEnter.Invoke();
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
            onMouseLeftDown.Invoke();
        }
        // Middle-click
        else if (Input.GetMouseButtonDown(2))
        {
            onMouseLeftDown.Invoke();
        }
    }
    /// <summary>
    /// LEFT-CLICK only (right-click doesn't work)
    /// </summary>
    private void OnMouseDown()
    {
        Debug.Log("OnMouseDown", this.gameObject);
        if (onMouseDown != null)
            onMouseDown.Invoke();
    }
    private void OnMouseExit()
    {
        Debug.Log("OnMouseExit", this.gameObject);
        if (onMouseExit != null)
            onMouseExit.Invoke();
    }
}
