using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShepScript : MonoBehaviour
{
    public AudioClip[] Baahs;
    private AudioSource Sheep;
    //public int SheepTotal;
    // Start is called before the first frame update
    void Start()
    {
        // Code that lets the sheep baah randomly
        StartCoroutine(RealisticSheepBaah());
        //SheepTotal = 0;
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
            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().hasSheep = true;
            Destroy(gameObject);
            //SheepTotal = SheepTotal + 1;
        }
    }
}
