using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSounds : MonoBehaviour
{
    [SerializeField] private AudioSource breathing;
    [SerializeField] private AudioSource[] randoms;

    // Start is called before the first frame update
    void Start()
    {
        
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
        breathing.Play();
    }

    public void StopBreathing() {
        breathing.Stop();
    }
}
