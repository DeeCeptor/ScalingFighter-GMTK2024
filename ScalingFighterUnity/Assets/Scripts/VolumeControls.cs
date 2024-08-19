using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I don't know C# or have any experience with unity, so bear with me.
public class VolumeControls : MonoBehaviour
{
    AudioSource m_MyAudioSource;

    void Start()
    {
        m_MyAudioSource = GetComponent<AudioSource>();
        m_MyAudioSource.Play();
        m_MyAudioSource.volume = 0.5f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            m_MyAudioSource.volume += 0.05f;
        }
        else if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            m_MyAudioSource.volume -= 0.05f;
        }
        
    }
}
