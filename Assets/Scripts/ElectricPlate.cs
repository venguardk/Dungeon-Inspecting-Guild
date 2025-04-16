using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricPlate : MonoBehaviour
{
    // This script is attached to the ElectricPLate prefab; The script controls the spikes that are activated and deactivated
    [SerializeField] private GameObject zapComponent;
    [SerializeField] private float timeIntervals = 2f;
    [SerializeField] private AudioSource audioSrc;
    private bool active;
    private bool isZapping;
    private Collider2D zapCollider;

    void Start()
    {
        zapComponent.SetActive(false);
        isZapping = false;
        active = false;
        zapCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.IsLevelEditorMode() == false)
        {
            // If in play mode, start the spikes
            if (isZapping == false)
            {
                zapCollider.enabled = false;
                isZapping = true;
                StartCoroutine(ActivateZap());
            }
        }
        else
        {
            // If in level editor mode, stop the spikes
            isZapping = false;
            StopAllCoroutines();
            if (active) //If the spikes were active when swapping back to level editor, deactivate them
            {
                zapCollider.enabled = true;
                zapComponent.SetActive(false);
                active = false;
            }
        }
    }

    private IEnumerator ActivateZap()
    {
        while (true)
        {
            // Activate and deactivate the spikes at regular intervals
            yield return new WaitForSeconds(timeIntervals);
            if (active)
            {
                // If the spikes are active, deactivate them
                zapCollider.enabled = false;
                zapComponent.SetActive(false);
                active = false;
            }
            else
            {
                // If the spikes are inactive, activate them
                zapCollider.enabled = true;
                zapComponent.SetActive(true);
                audioSrc.Play();
                active = true;
            }
        }
    }
}
