using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds miscelaneous assets
/// </summary>
public class AssetHolder : MonoBehaviour
{
    public Material SpriteOutlineMaterial;
    public GameObject DamageAnimation;

    public static AssetHolder Instance;
    void Awake()
    {
        Instance = this;
    }
}
