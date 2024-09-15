using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class ChainSounds : MonoBehaviour
{
    [SerializeField] private AudioSource breathing;
    [SerializeField] private AudioSource[] randoms;
    [SerializeField] private AudioSource deathSound;
    private float baseBreathVolume;

    // Start is called before the first frame update
    void Start()
    {
        baseBreathVolume = breathing.volume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomSound() {
        int rand = Random.Range(0, randoms.Length);
        randoms[rand].Play();
    }

    public void PlayBreathing() {
        breathing.volume = 1f;
        deathSound.Play();
        breathing.Play();
    }

    public void StopBreathing() {
        breathing.volume = baseBreathVolume;
        breathing.Stop();
        deathSound.Stop();
    }
}
