using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Cinemachine;
using TMPro;

/// <summary>
/// Holds miscelaneous assets
/// </summary>
public class AssetHolder : MonoBehaviour
{
    public Material SpriteOutlineMaterial;
    public GameObject DamageAnimation;
    public CinemachineVirtualCamera VirtualCamera;
    public CinemachineTargetGroup TargetGroup;
    public TextMeshProUGUI ScoreText;

    public static AssetHolder Instance;
    void Awake()
    {
        Instance = this;
    }
}
