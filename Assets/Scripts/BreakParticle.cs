using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem pSys;

    private void Start()
    {
        Destroy(gameObject, pSys.main.duration);
    }
}
