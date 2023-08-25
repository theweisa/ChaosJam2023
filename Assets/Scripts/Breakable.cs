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

    [Space(10)]
    [SerializeField] private bool autoSetPhysicsMaterial = true;

    [Space(10)]
    [SerializeField] private bool autoGenerateMass = true;
    public float mass = 1f;

    [Space(10)]
    [SerializeField] private bool autoGenerateBreakCoefficient = true;
    public float breakCoefficient = 50f;
    public float currentBreakHealth = 50f;

    [Space(10)]
    public bool isInvincible = true;

    [Space(8)]
    [Header("Misc")]
    [Space(1)]
    public UnityEvent OnBreakEvent;

    private void Start()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (autoGenerateMass)
        {
            CalculateMass();
            SetMass();
        }

        if (autoGenerateBreakCoefficient)
        {
            CalculateBreakForce();
        }

        if (autoSetPhysicsMaterial)
        {
            SetMaterial();
        }

        SetMass();

        currentBreakHealth = breakCoefficient;
        StartCoroutine(StartRoutine());

    }

    IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(0.3f);
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region EditorUpdate
#if UNITY_EDITOR

        if (Application.isPlaying)
        {
            return;
        }

        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (autoGenerateMass)
        {
            CalculateMass();
            SetMass();
        }

        if (autoGenerateBreakCoefficient)
        {
            CalculateBreakForce();
        }

        if (autoSetPhysicsMaterial)
        {
            SetMaterial();
        }

        SetMass();
        return;
#endif
        #endregion


    }



    void CalculateMass()
    {
        rb.useAutoMass = true;
        mass = rb.mass * (breakableMaterial != null ? breakableMaterial.massCoefficient : 1);
        rb.useAutoMass = false;
    }

    void CalculateBreakForce()
    {
        breakCoefficient = mass * (breakableMaterial != null ? breakableMaterial.breakCoefficient : 1);
        currentBreakHealth = breakCoefficient;
    }

    void SetMass()
    {
        if (rb && !rb.useAutoMass)
        {
            rb.mass = mass;
        }
    }

    void SetMaterial()
    {
        if(rb && breakableMaterial && breakableMaterial.physicsMaterial)
        {
            rb.sharedMaterial = breakableMaterial.physicsMaterial;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impulse = collision.contacts[0].normalImpulse;

        if(Mathf.Approximately(impulse, 0))
        {
            return;
        }

        Debug.Log("felt a collision with impulse: " + impulse);

        if (isInvincible)
        {
            return;
        }

        currentBreakHealth -= impulse;

        if(currentBreakHealth <= 0)
        {
            Debug.Log("I broke!");
            OnBreakEvent.Invoke();
            Destroy(gameObject);
        }



    }


}
