using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartShooter : MonoBehaviour
{
    // This script is used to control the DartShooter prefab and its ability to shoot darts
    private List<GameObject> dartList = new List<GameObject>();
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private Transform parent, shootPoint;
    [SerializeField] private float shootSpeed = 1f;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private AudioSource shootAudio;
    private bool isShooting;

    private void Start()
    {
        // Initialize the dart prefab pool with 3 inactive darts
        Vector3 spawnPoint = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            GameObject dart = Instantiate(dartPrefab, spawnPoint, Quaternion.Euler(0, 0, 0), parent);
            dart.SetActive(false);
            dartList.Add(dart);
        }
        isShooting = false;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.IsLevelEditorMode() == false) //If in play mode, proceed to fire
        {
            if (isShooting == false)
            {
                isShooting = true;
                StartCoroutine(ShootDart());
            }
        }
        else
        {
            // If in level editor mode, stop shooting
            isShooting = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator ShootDart()
    {
        while (true) // Continuously shoot darts
        {
            yield return new WaitForSeconds(shootDelay);
            GameObject chosenDart = GetDart();
            if (chosenDart != null)
            {
                shootAudio.Play(); // Play the shooting sound
                chosenDart.transform.position = shootPoint.position; // Set the position of the dart to the shoot point
                chosenDart.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
                chosenDart.SetActive(true);
                chosenDart.GetComponent<Rigidbody2D>().linearVelocity = shootPoint.up * shootSpeed; //Fire dart in direction of this object
            }
        }
    }

    private GameObject GetDart() //Acquire the next inactive dart from the pool and return it for use
    {
        foreach (GameObject dart in dartList)
        {
            if (!dart.activeInHierarchy)
            {
                return dart;
            }
        }
        return null;
    }
}
