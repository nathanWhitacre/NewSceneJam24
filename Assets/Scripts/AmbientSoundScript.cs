using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AmbientSoundScript : MonoBehaviour
{
    public AudioClip[] Birds;
    public AudioClip[] Wind;
    private AudioSource Player;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AmbientBirds());
        StartCoroutine(AmbientWind());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator AmbientBirds()
    {
        while (true)
        {


            Player = GetComponent<AudioSource>();
            Player.clip = Birds[Random.Range(0, Birds.Length)];
            Player.PlayOneShot(Player.clip);
            yield return new WaitForSeconds(Random.Range(2f, 30f));
        }
    }
    private IEnumerator AmbientWind()
    {
        while (true)
        {


            Player = GetComponent<AudioSource>();
            Player.clip = Wind[Random.Range(0, Wind.Length)];
            Player.PlayOneShot(Player.clip);
            yield return new WaitForSeconds(Random.Range(30f, 60f));
        }
    }
}
