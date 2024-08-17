using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Calls events when Awake, Start, OnEnable, OnDisable, OnDestroy are called
public class OnEnableDisable : MonoBehaviour
{
	// Assigned actions to events like a UI button
    public Button.ButtonClickedEvent on_enable;
    public Button.ButtonClickedEvent on_disable;
	public Button.ButtonClickedEvent on_destroy;
    public Button.ButtonClickedEvent on_awake;
    public Button.ButtonClickedEvent on_start;


    public void OnEnable()
    {
        on_enable.Invoke();
    }
    public void OnDisable()
    {
        on_disable.Invoke();
    }
	public void OnDestroy()
	{
		on_destroy.Invoke();
	}
	
	
	void Awake()
	{
		on_awake.Invoke();
	}
	void Start()
	{
		on_start.Invoke();
	}
}
