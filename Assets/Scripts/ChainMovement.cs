using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChainMovement : MonoBehaviour
{
    private Transform thisTrans;
    private Rigidbody thisBody;
    [SerializeField] private Transform neutralTrans;
    [SerializeField] private float groundCheckDist;
    [SerializeField] private float followDist;
    private Camera mainCamera;
    private Renderer thisRenderer;
    private Transform cableTrans;
    private int sheepCount;
    private int enemyLayerMask;
    [SerializeField] private float baseFollowKillTime;
    [SerializeField] private float killDistance;
    [SerializeField] private float baseDetectedKillTime;
    [SerializeField] private float observeDist;
    private float followKillTime;
    private float detectedKillTime;
    private ChainSounds sounds;
    public bool isActive;
    private bool inKill;

    void Start()
    {
        inKill = false;
        sheepCount = 1;
        enemyLayerMask = LayerMask.NameToLayer("chain");
        thisTrans = this.gameObject.GetComponent<Transform>();
        thisBody = this.gameObject.GetComponent<Rigidbody>();
        thisRenderer = this.gameObject.GetComponent<MeshRenderer>();
        mainCamera = Camera.main;
        cableTrans = GameObject.FindWithTag("Player").GetComponent<Transform>();
        sounds = this.gameObject.GetComponent<ChainSounds>();
        StartCoroutine(ChangeState(0));
        ChangeSheepCount(sheepCount);
    }

    void Update()
    {
        if (!inKill) {
            FacePlayer();
            thisBody.angularVelocity = Vector3.zero;
            if (Vector3.Distance(thisTrans.position, cableTrans.position) < 2f) {
                StopAllCoroutines();
                StartCoroutine(ChangeState(3));
            }
        }
    }

    public void ChangeSheepCount(int i) {
        sheepCount = i;
        followKillTime = baseFollowKillTime - sheepCount;
        detectedKillTime = baseDetectedKillTime - sheepCount;
    }

    float NeutralWaitTime() {
        float baseTime = 32f;
        float waitTime = baseTime - (sheepCount * 4);
        return waitTime + Random.Range(-4, 4);
    }

    void FacePlayer() {
        thisTrans.rotation = Quaternion.LookRotation(cableTrans.position - thisTrans.position, new Vector3(0, 1, 0));
        thisTrans.eulerAngles = new Vector3(0, thisTrans.eulerAngles.y, 0);
    }

    bool VisibleToPlayer() {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        return GeometryUtility.TestPlanesAABB(planes, thisRenderer.bounds);
    }

    Vector3 GroundPosition(Vector3 target) {
        RaycastHit hit;
        Vector3 retVect;
        if (Physics.Raycast(target + new Vector3(0, 20, 0), new Vector3(0, -1, 0), out hit, groundCheckDist, enemyLayerMask)) {
            retVect = hit.point;
            retVect.y = retVect.y + (thisTrans.localScale.y * 0.5f);
        }
        else {
            retVect = target;
        }
        return retVect;
    }

    void ResetCoroutines() {
        StopAllCoroutines();
        StartCoroutine(ChangeState(0));
    }

    IEnumerator ChangeState(int i) {
        switch (i) {
            case 0:
            StartCoroutine(State0());
            break;

            case 1:
            StartCoroutine(State1());
            break;

            case 2:
            StartCoroutine(State2());
            break;

            case 3:
            StartCoroutine(State3());
            break;

            case 4:
            StartCoroutine(State4());
            break;

            case 5:
            StartCoroutine(State5());
            break;

            case 6:
            StartCoroutine(State6());
            break;

            default:
            StartCoroutine(State0());
            break;
        }
        yield return null;
    }

    IEnumerator State0() {  //neutral
        if (isActive) {
            thisTrans.position = GroundPosition(neutralTrans.position);
            yield return new WaitForSeconds(NeutralWaitTime());
            float rando = Random.Range(0f, 1f);
            if (rando < 0.7f) {
                StartCoroutine(ChangeState(1));
            }
            else {
                StartCoroutine(ChangeState(5));
            }
        }
        yield return null;
    }

    IEnumerator PlayRandomSound() {
        yield return new WaitForSeconds(Random.Range(0.5f, detectedKillTime / 3));
        sounds.PlayRandomSound();
        yield return null;
    }

    IEnumerator State1() {  //following undetected
        StartCoroutine(PlayRandomSound());
        float tempDist = followDist + Random.Range(-3f, 3f);
        thisTrans.position = GroundPosition(cableTrans.position + (cableTrans.forward * -1 * tempDist));
        Coroutine followCor = StartCoroutine(Follow(tempDist));
        float followStartTime = Time.time;
        while (Time.time - followStartTime < followKillTime) {
            if (VisibleToPlayer()) {  //check if player looking
                StopCoroutine(followCor);
                StartCoroutine(ChangeState(2));
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        //kill if the player takes too long to look
        StopCoroutine(followCor);
        StartCoroutine(ChangeState(4));
        yield break;
    }

    IEnumerator Follow(float tempDist) {
        while (true) {
            yield return new WaitForFixedUpdate();
            if (Vector3.Distance(cableTrans.position, thisTrans.position) > tempDist) {
                thisTrans.position = GroundPosition(thisTrans.position + (thisTrans.forward * 1));
            }
        }
    }

    IEnumerator State2() {  //close range detected
        float detectedStartTime = Time.time;
        Coroutine randEvent = StartCoroutine(RandomCloseEvent());
        bool noNeedDist = Random.Range(0f, 1f) < 0.5f;  //if this is true, the player doesn't need to get away from the chain
        float distBias = 0f;
        if (noNeedDist == false) {
            distBias = 6f;
        }
        while ((Time.time - detectedStartTime) < (detectedKillTime + distBias)) {
            yield return new WaitForEndOfFrame();
            if (VisibleToPlayer() == false && (noNeedDist || Vector3.Distance(cableTrans.position, thisTrans.position) > killDistance)) {
                StopCoroutine(randEvent);
                StartCoroutine(ChangeState(0));
                yield break;
            }
        }
        StopCoroutine(randEvent);
        StartCoroutine(ChangeState(3));
        yield return null;
    }

    IEnumerator RandomCloseEvent() {
        float randTime = Random.Range(1f, detectedKillTime - 2f);
        yield return new WaitForSeconds(randTime);
        float randOption = Random.Range(0f, 1f);
        if (randOption < 0.4f) {
            thisTrans.position = GroundPosition(thisTrans.position + (thisTrans.forward * Random.Range(0.4f, 1.6f)));
        }
        else if (randOption < 0.6f) {
            ResetCoroutines();
        }
        while (true) {
            yield return new WaitForEndOfFrame();
        }
    }
    
    IEnumerator State3() {  //frontal attack
        if (Vector3.Distance(thisTrans.position, cableTrans.position) < killDistance) {
            this.StartFrontKillRoutine();
        } else {
            StartCoroutine(ChangeState(0));
        }
        yield return null;
    }

    IEnumerator State4() {  //sneak attack
        if (Vector3.Distance(thisTrans.position, cableTrans.position) < killDistance) {
            this.StartBehindKillRoutine();
        } else {
            StartCoroutine(ChangeState(0));
        }        
        yield return null;
    }

    IEnumerator State5() {  //far undetected
        StartCoroutine(PlayRandomSound());
        float tempDist = observeDist + Random.Range(-3f, 3f);
        thisTrans.position = GroundPosition(cableTrans.position + (cableTrans.forward * -1 * tempDist));
        float observeEventTime = Random.Range(5f, 30f);
        float observeStartTime = Time.time;
        while (Time.time - observeStartTime < observeEventTime) {
            if (VisibleToPlayer()) {  //check if player looking
                StartCoroutine(ChangeState(6));
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }        
        float randObserveEvent = Random.Range(0f, 1f);
        if (randObserveEvent < 0.3f) {
            StartCoroutine(ChangeState(1));  //follow closer
        } else {
            StartCoroutine(ChangeState(0));  //reset
        }
        yield return null;
    }

    IEnumerator State6() {  //far detected
        float farEventTime = Random.Range(2f, 10f);
        float farTimeStart = Time.time;
        while (Time.time - farTimeStart < farEventTime) {
            yield return new WaitForEndOfFrame();
            if (VisibleToPlayer() == false) {
                break;
            }
        }
        float randomFarEvent = Random.Range(0f, 1f);
        if (randomFarEvent < 0.4f) {
            StartCoroutine(ChangeState(1));
        } else if (randomFarEvent < 0.6f) {
            thisTrans.position = GroundPosition(cableTrans.position + (cableTrans.forward * Random.Range(5f, 10f)));
            StartCoroutine(ChangeState(2));
        } else {
            StartCoroutine(ChangeState(0));
        }
        yield return null;
    }

    void StartBehindKillRoutine() {
        StopAllCoroutines();
        StartCoroutine(KillPlayerFromBehind());
    }

    IEnumerator KillPlayerFromBehind() {
        inKill = true;
        sounds.PlayBreathing();
        //lock player camera
        cableTrans.gameObject.GetComponent<PlayerMovement>().enabled = false;
        Camera playerCamera = Camera.main;
        playerCamera.gameObject.GetComponent<CameraLook>().enabled = false;
        Vector3 currAngles = playerCamera.transform.localEulerAngles;
        playerCamera.transform.localEulerAngles = new Vector3(0, currAngles.y, currAngles.z);
        //move chain to player rear
        thisTrans.position = GroundPosition(cableTrans.position + (cableTrans.forward * -1f));
        thisTrans.rotation = Quaternion.LookRotation(cableTrans.position - thisTrans.position, thisTrans.up);
        yield return new WaitForSeconds(0.5f);  //kill
        float dropStart = Time.time;
        Vector3 cameraPos = playerCamera.transform.position;
        float startY = cameraPos.y;
        while (Time.time - dropStart < 0.4f) {
            currAngles = playerCamera.transform.localEulerAngles;
            playerCamera.transform.localEulerAngles = Vector3.Lerp(new Vector3(0, currAngles.y, currAngles.z), new Vector3(-90, currAngles.y, currAngles.z), (Time.time - dropStart)/0.4f);
            cameraPos = playerCamera.transform.position;
            cameraPos.y = Mathf.Lerp(startY, 0.2f, (Time.time - dropStart)/0.4f);
            playerCamera.transform.position = cameraPos;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);  //death scene
        yield return null;
    }

    void StartFrontKillRoutine() {
        StopAllCoroutines();
        StartCoroutine(KillPlayerFromFront());
    }

    IEnumerator KillPlayerFromFront() {
        inKill = true;
        sounds.PlayBreathing();
        //lock player camera
        cableTrans.gameObject.GetComponent<PlayerMovement>().enabled = false;
        cableTrans.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Camera playerCamera = Camera.main;
        playerCamera.gameObject.GetComponent<CameraLook>().enabled = false;
        Vector3 currAngles = playerCamera.transform.localEulerAngles;
        playerCamera.transform.localEulerAngles = new Vector3(0, currAngles.y, currAngles.z);
        //move chain to player front
        thisTrans.position = GroundPosition(cableTrans.position + (cableTrans.forward * 0.6f)) + (Vector3.up * -1.25f);
        thisTrans.rotation = Quaternion.LookRotation(cableTrans.position - thisTrans.position, Vector3.up);
        thisTrans.eulerAngles = new Vector3(0, thisTrans.eulerAngles.y, 0);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
        yield return null;
    }

}
