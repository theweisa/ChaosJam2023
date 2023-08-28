using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEventRef : MonoBehaviour
{
    [field: Header("Impacts")]
    [field: SerializeField] public EventReference RatImpact {get; private set;}
    [field: SerializeField] public EventReference BoxDestruction {get; private set;}
    
    [field: Header("Whooshes")]
    [field: SerializeField] public EventReference RatFlying {get; private set;}
    [field: SerializeField] public EventReference RatSwinging {get; private set;}
    [field: SerializeField] public EventReference RatRelease {get; private set;}
    
    [field: Header("Music")]
    [field: SerializeField] public EventReference LevelMusic {get; private set;} 

    [field: Header ("Other SFX")]
    [field: SerializeField] public EventReference SewerAmbience {get; private set;}
    [field: SerializeField] public EventReference RatCollect {get; private set;}
    [field: SerializeField] public EventReference ClawMachine {get; private set;}
    [field: SerializeField] public EventReference ClawMachineLeave {get; private set;}
    [field: SerializeField] public EventReference ClawMachineEnter {get; private set;}

    public static FMODEventRef instance {get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Attempted to create second FMODEventRef instance.");
        }
        instance = this;
    }
}
