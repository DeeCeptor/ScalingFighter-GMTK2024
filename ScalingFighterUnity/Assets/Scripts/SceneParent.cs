using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneParent : MonoBehaviour
{
    public static SceneParent scene_parent;
    /// <summary>
    /// FALSE, user can't click on anything
    /// </summary>
    public static bool scene_is_interactable
    {
        get { return SceneParent.scene_parent.group.interactable; }// && CameraShake.camera_shake.shake <= 0f; }
    }
    public CanvasGroup group;
    public CanvasGroup ui_group_to_fade;
    public Canvas main_canvas;

    public GameObject click_blocker;
    public bool debug_override_ui_visible = true;
    public bool ui_visible = true;
    public bool scene_interactactable {
        get { return scene_is_interactable; }
    }
    float min_alpha = 0.001f;

    void Awake()
    {
        scene_parent = this;
        if (group == null)
            group = this.GetComponent<CanvasGroup>();
    }


    public static void SetSceneInteractable(bool interactable)
    {
        SceneParent.scene_parent.group.interactable = interactable;
        SceneParent.scene_parent.group.blocksRaycasts = interactable;
        if (scene_parent != null && scene_parent.click_blocker != null)
        {
            scene_parent.click_blocker.SetActive(!interactable);
        }
    }

    public void OverrideDebugToggleUI()
    {
        debug_override_ui_visible = !debug_override_ui_visible;
        ShowHideUI(debug_override_ui_visible);
    }
    public void ToggleUI()
    {
        ShowHideUI(!ui_visible);
    }
    public void ShowHideUI(bool show)
    {
        if (!debug_override_ui_visible)
            show = false;
        ui_visible = show;
        //Debug.Log("ShowHideUI " + show);
    }
    public void ShowUI()
    {
        ShowHideUI(true);
    }
    public void HideUI()
    {
        ShowHideUI(false);
    }


    void Update()
    {
        /*
        // Changing is SUPER LAGGY. Fix this!
        if (ui_visible && ui_group_to_fade.alpha != 1f)
        {
            ui_group_to_fade.alpha = 1f;
        }
        else if (!ui_visible && ui_group_to_fade.alpha != 0f)
        {
            ui_group_to_fade.alpha = 0f;
        }
        */
        if (ui_group_to_fade != null)
        {
            if (ui_visible && ui_group_to_fade.alpha != 1f)
            {
                ui_group_to_fade.alpha = Mathf.MoveTowards(ui_group_to_fade.alpha, 1f, Time.deltaTime);
            }
            else if (!ui_visible && ui_group_to_fade.alpha != 0f)
            {
                ui_group_to_fade.alpha = Mathf.MoveTowards(ui_group_to_fade.alpha, 0f, Time.deltaTime);
            }
        }
    }
}
