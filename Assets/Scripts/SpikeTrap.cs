using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private GameObject spikesComponent;
    [SerializeField] private float timeIntervals = 2f;
    private bool active;
    private Collider2D spikeCollider;

    void Start()
    {
        spikesComponent.SetActive(false);
        active = false;
        spikeCollider = GetComponent<Collider2D>();
        StartCoroutine(ActivateSpikes());
    }

    private IEnumerator ActivateSpikes()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeIntervals);
            if (active)
            {
                spikeCollider.isTrigger = true;
                spikesComponent.SetActive(false);
                active = false;
            }
            else
            {
                spikeCollider.isTrigger = false;
                spikesComponent.SetActive(true);
                active = true;
            }
        }
    }
}
