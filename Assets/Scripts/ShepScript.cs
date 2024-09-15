using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShepScript : MonoBehaviour
{
    public AudioClip[] Baahs;
    private AudioSource Sheep;
    // Start is called before the first frame update
    void Start()
    {
        // Code that lets the sheep baah randomly
        StartCoroutine(RealisticSheepBaah());

    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator RealisticSheepBaah()
    {
        while(true)
        {


            Sheep = GetComponent<AudioSource>();
            Sheep.clip = Baahs[Random.Range(0, Baahs.Length)];
            Sheep.PlayOneShot(Sheep.clip);
            Debug.Log(Baahs);
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }
}
