using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ExitGrateController : MonoBehaviour
{
    public SpriteRenderer sr;
    public float requiredRatPercentage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RatController rat = collision.transform.GetComponent<RatController>();

        if (rat && rat.isAttached)
        {
            int totalRats = FindObjectsOfType<RatController>().Length;
            Debug.Log(totalRats);

            if (RatController.connectedRats.Count > requiredRatPercentage * totalRats)
            {
                RuntimeManager.PlayOneShot(FMODEventRef.instance.SewerGrate);
                Debug.Log("sewer grate destroyed");
                Destroy(gameObject);
                GameManager.Instance.WinGame();
            }
        }
    }
}
