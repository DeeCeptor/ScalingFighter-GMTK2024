using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance;
    /// <summary>
    /// DEBUG, view in-editor which animation we're on (in case we get stuck)
    /// </summary>
    [TextArea]
    public string CurrentAnimationName;

    /// <summary>
    /// Animations are COROUTINES
    /// </summary>
    Tuple<IEnumerator, bool> current_animation;
    /// <summary>
    /// Animations WAITING to be played
    /// <COROUTINE, BLOCKS SCENE CONTROLS>
    /// </summary>
    public Queue<Tuple<IEnumerator, bool>> queued_animations = new Queue<Tuple<IEnumerator, bool>>();

    /// <summary>
    /// If game over, only accept certain new animations
    /// </summary>
    public bool accepting_normal_animations = true;



    public IEnumerator PlayAndFinishAnimation(Animator animator, string animName)
    {
        if (animator == null)
            yield break;
        else
        {
            animator.Play(animName);
            yield return null;
            while (animator.GetCurrentAnimatorStateInfo(0).IsName(animName))
            {
                yield return null;
            }
        }
    }


    /// <summary>
    /// Coroutine instance playing animations
    /// </summary>
    IEnumerator AnimationLoopCo;
    /// <summary>
    /// ALWAYS PLAYING, dequeues and plays animations
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayAnimationsLoop()
    {
        while (true)
        {
            while (queued_animations.Count > 0)// || VNSceneManager.current_conversation != null)
            {
                /*
                if (VNSceneManager.current_conversation != null)
                {
                    SceneParent.SetSceneInteractable(false);    // No interactivity during conversations
                }
                else
                {
                */
                current_animation = queued_animations.Dequeue();
                //  Set if scene is clickable or not
                SceneParent.SetSceneInteractable(!current_animation.Item2);
                //Debug.Log("Starting animation: " + current_animation);
                // Wait until animation finishes
                if (current_animation.Item1 != null)
                {
                    CurrentAnimationName = current_animation.Item1.ToString();
                    yield return StartCoroutine(current_animation.Item1);
                }
                else
                    Debug.LogError("null animation");
                CurrentAnimationName = "";
                current_animation = null;
                yield return null;
            }
            CurrentAnimationName = "";
            SceneParent.SetSceneInteractable(true);
            yield return null;
        }
    }

    public static void AddAnim(IEnumerator anim, bool override_dont_accept_animations = false, bool blocks_controls = true)
    {
        AnimationManager.Instance.AddAnimation(anim, override_dont_accept_animations: override_dont_accept_animations, blocks_controls: blocks_controls);
    }
    public void AddAnimation(IEnumerator anim, bool override_dont_accept_animations = false, bool blocks_controls = true)
    {
        if (!accepting_normal_animations && !override_dont_accept_animations)
            return;
        queued_animations.Enqueue(new Tuple<IEnumerator, bool>(anim, blocks_controls));
    }
    public static void ClearALLQueuedAnimations()
    {
        AnimationManager.Instance.queued_animations.Clear();
        // Turn OFF then ON this object to kill coroutine
        AnimationManager.Instance.RestartAnimationsLoop();
    }

    void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        RestartAnimationsLoop();
    }
    public void RestartAnimationsLoop()
    {
        if (AnimationLoopCo != null)
            StopCoroutine(AnimationLoopCo);
        AnimationLoopCo = PlayAnimationsLoop();
        StartCoroutine(AnimationLoopCo);
    }


    /// <summary>
    /// Add to animation queue for prompt
    /// </summary>
    /// <param name="cm"></param>
    /// <returns></returns>
    public static IEnumerator StartConversationPrompt(ConversationManager cm)
    {
        yield return null;
        if (cm != null)
            cm.Start_Conversation();
    }
}
