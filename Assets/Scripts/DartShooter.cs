using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartShooter : MonoBehaviour
{
    private List<GameObject> dartList = new List<GameObject>();
    [SerializeField] private GameObject dartPrefab;
    [SerializeField] private Transform parent, shootPoint;
    [SerializeField] private float shootSpeed = 1f;
    [SerializeField] private float shootDelay = 1f;
    [SerializeField] private AudioSource shootAudio;
    private bool isShooting;

    private void Start()
    {
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
        if (GameManager.instance.IsLevelEditorMode() == false)
        {
            if (isShooting == false)
            {
                isShooting = true;
                StartCoroutine(ShootDart());
            }
        }
        else
        {
            isShooting = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator ShootDart()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelay);
            GameObject chosenDart = GetDart();
            if (chosenDart != null)
            {
                shootAudio.Play();
                chosenDart.transform.position = shootPoint.position;
                chosenDart.transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
                chosenDart.SetActive(true);
                chosenDart.GetComponent<Rigidbody2D>().linearVelocity = shootPoint.up * shootSpeed;
            }
        }
    }

    private GameObject GetDart()
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
