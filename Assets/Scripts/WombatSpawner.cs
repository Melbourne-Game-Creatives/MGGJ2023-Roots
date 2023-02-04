using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class WombatSpawner : MonoBehaviour
{
    private bool running = false;
    private WaitForSeconds waitForSeconds;

    public GameObject WombatPrefab;
    public AudioSource audioSource;
    public AudioClip[] Exclamations;
    public int SpawnDelay = 10;

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(SpawnDelay);
    }

    void Start()
    {
        running = true;
        StartCoroutine("DoSpawn");
    }

    private void OnDestroy()
    {
        running = false;
    }

    IEnumerator DoSpawn()
    {
        while (running)
        {
            SpawnWombat();
            yield return waitForSeconds;
        }
    }

    private void SpawnWombat()
    {
        PlayExclamation();
        Instantiate(WombatPrefab, new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f)),
            Quaternion.Euler(0, Random.Range(0f, 360f), 0));
    }
    
    public void PlayExclamation()
    {
        var soundIndex = Random.Range(0, Exclamations.Length);

        audioSource.clip = Exclamations[soundIndex];
        audioSource.Play();
    }
}
