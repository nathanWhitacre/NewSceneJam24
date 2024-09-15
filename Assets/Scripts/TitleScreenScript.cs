using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceenFadeScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TitleScreen());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator TitleScreen()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            SceneManager.LoadScene(1);
            yield return null;
        }
    }
}
