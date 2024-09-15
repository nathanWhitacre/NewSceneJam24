using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManagerManager : MonoBehaviour
{
    public AudioSource Song1;
    public AudioSource Song2;
    public AudioSource Song3;
    public AudioSource Song5;
    [SerializeField] private AltarSheepController altarSheepController;
    public int SheepCount;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //ChangeSong(SheepCount);
        ChangeSong(altarSheepController.sheepCount);
    }
    public void ChangeSong(int i)
    {
        if(i == 1)
        {
            Song1.volume = 0.75f;
            Song2.volume = 0;
            Song3.volume = 0;
            Song5.volume = 0;
        }
        else if (i == 2 || i == 3)
        {
            Song1.volume = 0;
            Song2.volume = 0.75f;
            Song3.volume = 0;
            Song5.volume = 0;
        }
        else if (i == 4 || i == 5)
        {
            Song1.volume = 0;
            Song2.volume = 0;
            Song3.volume = 0.75f;
            Song5.volume = 0;
        }
        else if (i == 6)
        {
            Song1.volume = 0;
            Song2.volume = 0;
            Song3.volume = 0;
            Song5.volume = 0.75f;
        }
    }
}
