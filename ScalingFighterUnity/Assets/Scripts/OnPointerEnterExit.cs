using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnPointerEnterExit : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Button.ButtonClickedEvent on_pointer_enter;
    public Button.ButtonClickedEvent on_pointer_exit;
    public Button.ButtonClickedEvent on_pointer_click;



    public void OnPointerEnter (PointerEventData eventData) 
    {
        Debug.Log("OnPointerEnter", this.gameObject);
        on_pointer_enter.Invoke();
    }

    public void OnPointerExit (PointerEventData eventData) 
    {
        Debug.Log("OnPointerExit", this.gameObject);
        on_pointer_exit.Invoke();
    }

    public void OnPointerClick (PointerEventData eventData) 
    {
        Debug.Log("OnPointerClick", this.gameObject);
        on_pointer_click.Invoke();
    }
}
