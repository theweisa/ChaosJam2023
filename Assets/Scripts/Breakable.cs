using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody2D))]
public class Breakable : MonoBehaviour
{
    //base class for all physics breakables

    [Header("Refs")]
    [Space(1)]
    public Rigidbody2D rb;

    [Space(4)]

    [Header("Stats")]
    [Space(1)]

    [Expandable]
    public BreakableMaterial breakableMaterial;
    [SerializeField] private bool autoGenerateMass = true;
    [DisableIf("autoGenerateMass")]
    public float mass = 1f;

    [SerializeField] private bool autoGenerateBreakCoefficient = true;
    [DisableIf("autoGenerateBreakCoefficient")]
    public float breakCoefficient = 50f;

    [Space(4)]

    [Header("Misc")]
    [Space(1)]
    public UnityEvent OnBreakEvent;


    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR

        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (autoGenerateMass)
        {
            CalculateMass();
        }

        if (autoGenerateBreakCoefficient)
        {
            CalculateBreakForce();
        }

        return;
#endif


    }

    void CalculateMass()
    {
        rb.useAutoMass = true;
        mass = rb.mass * breakableMaterial.massCoefficient;
        rb.useAutoMass = false;
        SetMass();
    }

    void CalculateBreakForce()
    {
        breakCoefficient = mass * breakableMaterial.breakCoefficient;
    }

    void SetMass()
    {
        if (rb && !rb.useAutoMass)
        {
            rb.mass = mass;
        }
    }


    #region Editor

    #if UNITY_EDITOR

    private void OnValidate()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (autoGenerateMass)
        {
            CalculateMass();
        }

        if (autoGenerateBreakCoefficient)
        {
            CalculateBreakForce();
        }
    }
    #endif
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impulse = collision.contacts[0].normalImpulse;
        Debug.Log("felt a collision with impulse: " + impulse);

        if(impulse >= breakCoefficient)
        {
            Debug.Log("I broke!");
            OnBreakEvent.Invoke();
            Destroy(gameObject);
        }



    }


}
