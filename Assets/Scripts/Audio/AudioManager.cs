using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set; }
  
    private List<EventInstance> eventInstances;
    public bool musicStarted = false;
    public float bpm = 170;
    float beatTimer = 0;
    float secondsPerBeat;
    [HideInInspector] public bool onBeat = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Attempted to create second Audio Manager instance.");
        }
        instance = this;
        eventInstances = new List<EventInstance>();
        secondsPerBeat = 1f/(bpm/60f);
    }

    void Update() {
        onBeat = false;
        if (musicStarted) {
            beatTimer += Time.deltaTime;
            if (beatTimer >= secondsPerBeat) {
                beatTimer -= secondsPerBeat;
                onBeat = true;
            }
        }
        else {
            beatTimer = 0f;
        }
        
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
    
    private void OnDestroy()
    {
        CleanUp();
    }
}
