using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
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
            if (isTrapping == false)
            {
                spikeCollider.enabled = false;
                isTrapping = true;
                StartCoroutine(ActivateSpikes());
            }
        }
        else
        {
            isTrapping = false;
            StopAllCoroutines();
            if (active)
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
            yield return new WaitForSeconds(timeIntervals);
            if (active)
            {
                spikeCollider.enabled = false;
                spikesComponent.SetActive(false);
                active = false;
            }
            else
            {
                spikeCollider.enabled = true;
                spikesComponent.SetActive(true);
                audioSrc.Play();
                active = true;
            }
        }
    }
}
