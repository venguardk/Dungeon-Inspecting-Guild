using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    // This script is attached to the SpikeTrap prefab; The script controls the spikes that are activated and deactivated
    [SerializeField] private GameObject spikesComponent;
    [SerializeField] private float timeIntervals = 2f;
    [SerializeField] private AudioSource audioSrc;
    private bool active;
    private bool isTrapping;
    private Collider2D spikeCollider;

    void Start()
    {
        spikesComponent.SetActive(false);
        isTrapping = false;
        active = false;
        spikeCollider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.IsLevelEditorMode() == false)
        {
            // If in play mode, start the spikes
            if (isTrapping == false)
            {
                spikeCollider.enabled = false;
                isTrapping = true;
                StartCoroutine(ActivateSpikes());
            }
        }
        else
        {
            // If in level editor mode, stop the spikes
            isTrapping = false;
            StopAllCoroutines();
            if (active) //If the spikes were active when swapping back to level editor, deactivate them
            {
                spikeCollider.enabled = true;
                spikesComponent.SetActive(false);
                active = false;
            }
        }
    }

    private IEnumerator ActivateSpikes()
    {
        while (true)
        {
            // Activate and deactivate the spikes at regular intervals
            yield return new WaitForSeconds(timeIntervals);
            if (active)
            {
                // If the spikes are active, deactivate them
                spikeCollider.enabled = false;
                spikesComponent.SetActive(false);
                active = false;
            }
            else
            {
                // If the spikes are inactive, activate them
                spikeCollider.enabled = true;
                spikesComponent.SetActive(true);
                audioSrc.Play();
                active = true;
            }
        }
    }
}
