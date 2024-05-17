using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    public Transform CenterPoint;
    public Transform StableCenterPoint;

    public GameObject player;
    private Vector2 playerVector;
    private Vector2 centerVector;
    public Image fullScreenFade;
    public Image DynamicFullScreenFade;
    private Color fadeColor;
    private Color DynamicFadeColor;
    public float fadeStartDistance;
    public float fadeEndDistance;
    private float fadeRange;
    private bool movingCenter;

    private bool warping;

    //movement Ability variables
    public PlayerMovement controller;
    public PlayerCam cameraController;

    private bool slowfallActivated;
    private bool slowfallInUse;
    public Image slowfallMask;
    private Color slowfallMaskColor;
    private float slowfallDelay;

    private bool highjumpActivated;
    private bool highjumpInUse;
    public Image highjumpMask;
    private Color highjumpColor;
    [SerializeField] private Image LightMask;

    public Vector3 playerSpawnPoint;
    [SerializeField] private Transform MainCenter;
    

    // game progress markers
    public bool hasYellowPower;
    public bool hasBluePower;
    public bool hasPurplePower;
    public bool hasRedPower;
    private int numPowersFound;

    // pause menu & other ui
    public bool paused;
    [SerializeField] private GameObject pausemenu;
    [SerializeField] private GameObject optionsmenu;
    
    [SerializeField] private popupText popup;



    //Debugging toggle
    [SerializeField] private bool Debugging;
    // public Transform targetSphere;
    // Start is called before the first frame update
    void Start()
    {

        playerSpawnPoint = new Vector3(0, 2, 0);
        player = GameObject.FindWithTag("Player");
        playerVector = new Vector2(player.transform.position.x, player.transform.position.z);
        
        warping = false;

        findNewCenter();

        fadeColor = fullScreenFade.color;
        DynamicFadeColor = DynamicFullScreenFade.color;
        DynamicFadeColor.a = 0.0f;
        fadeColor.a = 0.0f;
        
        controller = player.GetComponent<PlayerMovement>();
        cameraController = GameObject.FindWithTag("MainCamera").GetComponent<PlayerCam>();
        slowfallActivated = false;
        slowfallInUse = false;
        slowfallMaskColor = slowfallMask.color;
        slowfallMaskColor.a = 0.0f;
        slowfallDelay = 0.5f;

        highjumpActivated = false;
        highjumpInUse = false;
        highjumpColor = highjumpMask.color;
        highjumpColor.a = 0.0f;


        if (!Debugging) {
            hasYellowPower = false;
            hasBluePower = false;
            hasPurplePower = false;
            hasRedPower = false;
            numPowersFound = 0;
        
            readSaveData();
        }
        
        
        paused = false;
        pausemenu.SetActive(false);
        optionsmenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pausePlay();
        }

        // if (Input.GetKeyDown("r")) {
        //     Debug.Log("rrr");
        //     turnTowardsTarget(targetSphere);
        // }

        // Debug.Log(CenterPoint.gameObject.name);

        if (CenterPoint == null & !warping) {
            panicFindCenter();
        }
        if (movingCenter) {
            centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);
        }
        playerVector = new Vector2(player.transform.position.x, player.transform.position.z);
        
        float distance = Vector2.Distance(centerVector, playerVector);

        if (distance >= fadeStartDistance) {
            float x = (((distance - fadeStartDistance)/fadeRange));
            fadeColor.a = Mathf.Min(x, 1.0f);
            if (x > 1.5f & CenterPoint != null & !warping) {
                // Debug.Log("returnn");
                StartCoroutine(oobReturn());
            }
        } else {
            fadeColor.a = 0.0f;
        }
        fullScreenFade.color = fadeColor;
    }

    


    private void panicFindCenter() {
        
        Debug.Log("panic find new center");
        GameObject[] centers = GameObject.FindGameObjectsWithTag("centerpoint");

        foreach(GameObject c in centers) {
            Vector2 currCenterVector = new Vector2(c.transform.position.x, c.transform.position.z);
            float tempdistance = Vector2.Distance(currCenterVector, playerVector);
            
            if (tempdistance <= c.GetComponent<CenterPointControl>().endDistance) {
                CenterPoint = c.transform;
                fadeStartDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().startDistance;
                fadeEndDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().endDistance;
                fadeRange = fadeEndDistance - fadeStartDistance;

                centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);
                movingCenter = CenterPoint.gameObject.GetComponent<CenterPointControl>().isMoving;
                if (!movingCenter) {
                    StableCenterPoint = CenterPoint;
                }
                RenderSettings.fogDensity = 1.2f/fadeStartDistance;
            }
        }

        if (CenterPoint == null & StableCenterPoint != null) {
            StartCoroutine(oobReturn());
            CenterPoint = StableCenterPoint;
        }

        Debug.Log("new center " + CenterPoint.gameObject.name);
    }

    private IEnumerator oobReturn() {
        
        // controller.haltWalk = true;
        if (CenterPoint == StableCenterPoint) {
            // controller.flipped = !(controller.flipped);
            // turnTowardsTarget(CenterPoint);
            cameraController.yOverride += 180;
            player.transform.position = Vector3.MoveTowards(player.transform.position, CenterPoint.position, fadeRange/2);
        } else {
            Vector3 stablePos = StableCenterPoint.position;
            player.transform.position = stablePos + StableCenterPoint.GetComponent<CenterPointControl>().relativeSpawnPos;
        }
        
        yield return new WaitForSeconds(0.05f);
        // controller.haltWalk = false;
    }

    // figures out what center point player in rn so can figure out what should be able to do
    // called at start() but also when enter or leave centers
    public void findNewCenter() {
        GameObject[] centers = GameObject.FindGameObjectsWithTag("centerpoint");
        GameObject[] activeCenters = {null, null};

        // for each center checks if player within range adds to activeCenters array
        // player should never be in more than two centers 
        foreach(GameObject c in centers) {
            Vector2 currCenterVector = new Vector2(c.transform.position.x, c.transform.position.z);
            float distance = Vector2.Distance(currCenterVector, playerVector);
            
            if (distance <= c.GetComponent<CenterPointControl>().startDistance) {
                if(activeCenters[0] == null) {
                    activeCenters[0] = c;
                } else {
                    activeCenters[1] = c;
                }
            }
        }

        if (activeCenters[0] == null) {
            return;
        } else if (activeCenters[1] == null) {
            CenterPoint = activeCenters[0].transform;
        } else {
            float c1range = activeCenters[0].GetComponent<CenterPointControl>().startDistance;
            float c2range = activeCenters[1].GetComponent<CenterPointControl>().startDistance;

            // if in two overlapping always puts you in bigger one
            if (c1range > c2range) {
                CenterPoint = activeCenters[0].transform;
            } else {
                CenterPoint = activeCenters[1].transform;
            }
        }

        fadeStartDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().startDistance;
        fadeEndDistance = CenterPoint.gameObject.GetComponent<CenterPointControl>().endDistance;
        fadeRange = fadeEndDistance - fadeStartDistance;

        centerVector = new Vector2(CenterPoint.position.x, CenterPoint.position.z);
        movingCenter = CenterPoint.gameObject.GetComponent<CenterPointControl>().isMoving;
        if (!movingCenter) {
            StableCenterPoint = CenterPoint;
        } else {
            StartCoroutine(teleportToDestination());
        }
        RenderSettings.fogDensity = 1.2f/fadeStartDistance;
        Debug.Log("new center " + CenterPoint.gameObject.name);
    }
    
    // when the player is in a moving light
    public IEnumerator teleportToDestination() {
        // Debug.Log("in a mover");
        yield return new WaitForSeconds(3.0f);
        if (CenterPoint.gameObject.GetComponent<CenterPointControl>().isMoving & !warping) {
            float distance = Vector2.Distance(centerVector, playerVector);
            
            //if the player is still in range after 3 seconds, start the teleport
            if(fadeEndDistance >= distance) {
                warping = true;

                Vector3 destination = CenterPoint.parent.GetComponent<ControlledLightMove>().getDestination();
                
                Color LightMaskColor = LightMask.color;
                while (LightMask.color.a < 1f) {
                    LightMaskColor.a += Time.deltaTime * 1.5f;
                    LightMask.color = LightMaskColor;
                    yield return null;
                }

                StartCoroutine(warpPlayer(destination));
                yield return new WaitForSeconds(0.2f);
                warping = false;

                turnTowardsTarget(CenterPoint);
                
                while (LightMask.color.a > 0f) {
                    LightMaskColor.a -= Time.deltaTime * 1.5f;
                    LightMask.color = LightMaskColor;
                    yield return null;
                }
                foreach (Transform child in transform) {
                    Destroy(child.gameObject);
                }
            }
        }

    }

    private void turnTowardsTarget(Transform target) {
        
        Vector3 directionToTarget = target.position - player.transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        float yRotationDifference = targetRotation.eulerAngles.y - player.transform.GetChild(1).rotation.eulerAngles.y;
        // float xRotationDifference = targetRotation.eulerAngles.x - cameraController.transform.rotation.eulerAngles.x;
        // Debug.Log("target: " + targetRotation.eulerAngles.y + "current: " + player.transform.rotation.eulerAngles.y + "difference: " + rotationDifference);
    
        cameraController.yOverride += yRotationDifference;
        // cameraController.xOverride += xRotationDifference;
    }

    public void checkFade() {
        if (fadeColor.a > 0.0f) {
            StartCoroutine(manualFade(fadeColor.a));
            // Debug.Log("Howdy");
        }
    }

    private IEnumerator manualFade(float startingAlpha) {
        float currAlpha = startingAlpha;
        while (currAlpha > 0.0f) {
            // Debug.Log(currAlpha);
            DynamicFadeColor.a = currAlpha;
            DynamicFullScreenFade.color = DynamicFadeColor;
            currAlpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        DynamicFadeColor.a = 0.0f;
    }

    public IEnumerator manualFadeIN() {
        float currAlpha = 0;
        while (currAlpha < 1.0f) {
            DynamicFadeColor.a = currAlpha;
            DynamicFullScreenFade.color = DynamicFadeColor;
            currAlpha += Time.deltaTime;
            yield return null;
        }
        DynamicFadeColor.a = 1.0f;
        // Debug.Log("huh???");
        // controller.haltWalk = true;
        yield return new WaitForSeconds(1f);
        player.transform.position = MainCenter.position + new Vector3(0, 1.2f, 0);
        findNewCenter();
        yield return new WaitForSeconds(0.1f);
        // controller.haltWalk = false;
    
    }
    // -------------------------------------------------------------------------------------------
    // PLAYER POWERS
    // PLAYER POWERS
    // PLAYER POWERS
    // PLAYER POWERS
    // -------------------------------------------------------------------------------------------

    public void slowfall() {
        // Debug.Log("Slowly falling");
        slowfallActivated = true;
        StartCoroutine(fadeinSlowfall());
    }

    public void highjump() {
        // Debug.Log("high jump");
        highjumpActivated = true;
        controller.jumpForce = 22;
        slowfallDelay = 0.9f;

        StartCoroutine(fadeinHighJump());

    }


    public void playerJumped() 
    {
        // Debug.Log("jumped");
        if (slowfallActivated) {
            slowfallInUse = true;
            StartCoroutine(changeGrav());
        }
        if (highjumpActivated) {
            StartCoroutine(fadeoutHighJump());
            highjumpActivated = false;
            highjumpInUse = true;
        }
    }
    
    public void playerLanded() 
    {
        // Debug.Log("landed");
        if (slowfallInUse) {
            slowfallActivated = false;
            slowfallInUse = false;

            StartCoroutine(fadeoutSlowfall());
        }

        if (highjumpInUse) {
            highjumpInUse = false;
            controller.jumpForce = 15;
            slowfallDelay = 0.5f;
        }
    }

    private IEnumerator fadeinSlowfall() {
        while (slowfallMask.color.a < 0.10f) {
            slowfallMaskColor.a += Time.deltaTime;
            slowfallMask.color = slowfallMaskColor;
            yield return null;
        }
    }

    private IEnumerator fadeinHighJump() {
        while (highjumpMask.color.a < 0.25f) {
            highjumpColor.a += Time.deltaTime;
            highjumpMask.color = highjumpColor;
            yield return null;
        }
    }

    private IEnumerator fadeoutSlowfall() {
        controller.gravityMultiplier = 2;
        while (slowfallMask.color.a > 0) {
            slowfallMaskColor.a -= Time.deltaTime;
            slowfallMask.color = slowfallMaskColor;
            yield return null;
        }
    }

    private IEnumerator fadeoutHighJump() {
        while (highjumpMask.color.a > 0) {
            highjumpColor.a -= Time.deltaTime/2;
            highjumpMask.color = highjumpColor;
            yield return null;
        }
    }

    private IEnumerator changeGrav() {
        yield return new WaitForSeconds(slowfallDelay);
        if (slowfallInUse) {
            controller.gravityMultiplier = 0.2f;
        }
    }


    public IEnumerator falling() {
        
        Color LightMaskColor = LightMask.color;
        while (LightMask.color.a < 1f) {
            LightMaskColor.a += Time.deltaTime * 2;
            LightMask.color = LightMaskColor;
            yield return null;
        }

        StartCoroutine(warpPlayer(playerSpawnPoint));

        yield return new WaitForSeconds(0.2f);

        while (LightMask.color.a > 0f) {
            LightMaskColor.a -= Time.deltaTime * 1.5f;
            LightMask.color = LightMaskColor;
            yield return null;
        }
        
    }

    


    public void newPowerFound() {
        numPowersFound++;
        if (numPowersFound - 2 >= 0) {
            MainCenter.GetChild(numPowersFound - 2).gameObject.SetActive(false);
        }
        MainCenter.GetChild(numPowersFound - 1).gameObject.SetActive(true);

        switch (numPowersFound) {
            case 1:
                Debug.Log("Got Yellow");
                StartCoroutine(popup.CenterPopupAppear("NEW POWER UNLOCKED","Throw Yellow Lights to Interact With Objects" , 3));
                hasYellowPower = true;
                break;
            case 2:
                Debug.Log("Got Blue");
                StartCoroutine(popup.CenterPopupAppear("NEW POWER UNLOCKED", "Absorb Blue Lights to Slow Your Fall", 3));
                hasBluePower = true;
                break;
            case 3:
                Debug.Log("Got Purple");
                StartCoroutine(popup.CenterPopupAppear("NEW POWER UNLOCKED", "Absorb Purple Lights to Jump Higher", 3));
                hasPurplePower = true;
                break;
            case 4:
                Debug.Log("Got Red");
                StartCoroutine(popup.CenterPopupAppear("NEW POWER UNLOCKED", "Absorb Red Lights to Do Something", 3));
                hasRedPower = true;
                break;
        }
    }

    // -------------------------------------------------------------------------------------------
    // PAUSE MENU FUNCTIONS
    // PAUSE MENU FUNCTIONS
    // PAUSE MENU FUNCTIONS
    // -------------------------------------------------------------------------------------------

    private void pausePlay() {
        if (!paused) {
            pausemenu.SetActive(true);
            Time.timeScale = 0;
            paused = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

        } else {
            resume();
        }
    }

    public void resume() {
        pausemenu.SetActive(false);
        Time.timeScale = 1;
        paused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void options() {
        pausemenu.SetActive(false);
        optionsmenu.SetActive(true);
    }

    public void options_back() {
        optionsmenu.SetActive(false);
        pausemenu.SetActive(true);
    }

    public void exit() {
        Time.timeScale = 1;

        Vector3 savePos = StableCenterPoint.position + StableCenterPoint.GetComponent<CenterPointControl>().relativeSpawnPos + new Vector3(0, 1, 0);
        foreach (Transform child in StableCenterPoint.GetChild(4))
        {
            if (child.tag == "Save Point") {
                savePos = playerSpawnPoint;
                break;
            }
        }
        // Debug.Log(savePos);
        writeSaveData(savePos);
        SceneManager.LoadScene("Main Menu");
    }



    private void readSaveData() {
        if (!SaveData.isNewGame) 
        {
            hasYellowPower = SaveData.hasYellowPower;
            hasBluePower = SaveData.hasBluePower;
            hasPurplePower = SaveData.hasPurplePower;
            hasRedPower = SaveData.hasRedPower;

            if (hasRedPower) {
                numPowersFound = 4;
            } else if (hasPurplePower) {
                numPowersFound = 3;
            } else if (hasBluePower) {
                numPowersFound = 2;
            } else if (hasYellowPower) {
                numPowersFound = 1;
            } else {
                numPowersFound = 0;
            }

            if (numPowersFound > 0) {
                MainCenter.GetChild(numPowersFound - 1).gameObject.SetActive(true);
            }
        } else {
            SaveData.hasYellowPower = false;
            SaveData.hasBluePower = false;
            SaveData.hasPurplePower = false;
            SaveData.hasRedPower = false;
        }
        // Debug.Log(SaveData.spawnPoint);
        StartCoroutine(warpPlayer(SaveData.spawnPoint));
    }

    //teleport the player by overriding the walk controller
    private IEnumerator warpPlayer(Vector3 pos) {
        // controller.haltWalk = true;
        yield return new WaitForSeconds(0.1f);
        
        player.transform.position = pos;
        findNewCenter();
        Debug.Log(player.transform.position);
        yield return new WaitForSeconds(0.1f);
        
        // controller.haltWalk = false;
        
    }

    private void writeSaveData(Vector3 position) {
        SaveData.hasYellowPower = hasYellowPower;
        SaveData.hasBluePower = hasBluePower;
        SaveData.hasPurplePower = hasPurplePower;
        SaveData.hasRedPower = hasRedPower;
        SaveData.spawnPoint = position;
    }

}