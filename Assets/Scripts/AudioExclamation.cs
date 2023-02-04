using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExclamation : MonoBehaviour
{
    public AudioClip[] Exclamations;
    public AudioSource audioSource;

    public void PlayExclamation()
    {
        var soundIndex = Random.Range(0, Exclamations.Length);

        audioSource.clip = Exclamations[soundIndex];
        audioSource.Play();
    }
}
