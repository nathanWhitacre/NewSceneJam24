using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneChange : MonoBehaviour
{
    [SerializeField] private int nextScene;
    private bool stillWaiting;
    // Start is called before the first frame update
    void Start()
    {
        stillWaiting = true;
        StartCoroutine(ExitScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            stillWaiting = false;
        }
    }

    bool isWaiting() {
        return stillWaiting;
    }



    IEnumerator ExitScene() {
        yield return new WaitForSeconds(2f);
        yield return new WaitWhile(isWaiting);
        if (nextScene == -1) {
            Debug.Log("exiting");
            Application.Quit();
        }
        else {
            Debug.Log("changing scene");
            SceneManager.LoadScene(nextScene);
        }
        yield return null;
    }
}
