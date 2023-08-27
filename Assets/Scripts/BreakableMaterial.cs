using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Custom/BreakableMaterial")]
public class BreakableMaterial : ScriptableObject
{
    public enum MaterialType { Cardboard, Glass, Scrap, Chain };
    public MaterialType id;
    public float massCoefficient = 1;
    public float breakCoefficient = 1;
    public PhysicsMaterial2D physicsMaterial;
    //idk put some sound stuff here or something?

}
