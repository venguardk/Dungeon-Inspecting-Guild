using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    // This script is used on the Flamethrower prefab to shoot fireballs at the player
    // Other scripts this script interacts with: Fireball
    private List<GameObject> fireballList = new List<GameObject>();
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform parent, shootPoint;
    [SerializeField] private float shootSpeed = 2.5f;
    [SerializeField] private float shootDelay = 0.25f;
    [SerializeField] private AudioSource shootAudio;
    private bool isShooting;

    private void Start()
    {
        // Initialize the fireball prefab pool with 10 inactive fireballs
        Vector3 spawnPoint = Vector3.zero;
        for (int i = 0; i < 10; i++)
        {
            GameObject fireball = Instantiate(fireballPrefab, spawnPoint, Quaternion.Euler(0, 0, 0), parent);
            fireball.SetActive(false);
            fireballList.Add(fireball);
        }
        isShooting = false;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.IsLevelEditorMode() == false)
        {
            // If in play mode, proceed to fire
            if (isShooting == false)
            {
                isShooting = true;
                StartCoroutine(ShootFire());
                shootAudio.Play(); // Play the shooting sound
            }
        }
        else
        {
            // If in level editor mode, stop shooting
            isShooting = false;
            shootAudio.Stop();
            StopAllCoroutines();
        }
    }

    private IEnumerator ShootFire()
    {
        while (true)
        {
            // Continuously shoot fireballs
            yield return new WaitForSeconds(shootDelay);
            GameObject chosenFireball = GetFireball();
            if (chosenFireball != null)
            {
                chosenFireball.transform.position = shootPoint.position;
                chosenFireball.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
                chosenFireball.SetActive(true);
                chosenFireball.GetComponent<Rigidbody2D>().linearVelocity = shootPoint.up * shootSpeed;
                chosenFireball.GetComponent<Fireball>().StartDistanceCount();
            }
        }
    }

    private GameObject GetFireball()
    {
        foreach (GameObject fireball in fireballList)
        {
            if (!fireball.activeInHierarchy)
            {
                return fireball;
            }
        }
        return null;
    }
}
