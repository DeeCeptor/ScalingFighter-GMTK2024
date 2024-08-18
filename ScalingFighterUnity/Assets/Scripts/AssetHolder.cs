using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Cinemachine;

/// <summary>
/// Holds miscelaneous assets
/// </summary>
public class AssetHolder : MonoBehaviour
{
    public Material SpriteOutlineMaterial;
    public GameObject DamageAnimation;
    public CinemachineVirtualCamera VirtualCamera;
    public CinemachineTargetGroup TargetGroup;

    public static AssetHolder Instance;
    void Awake()
    {
        Instance = this;
    }
}
