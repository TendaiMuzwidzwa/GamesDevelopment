using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Transform enemy;
    public bool fallingOver;

    public bool playerWins;

    public Transform playerCamera;

    public Transform parent;

    public Transform winDisplay;

    public Vector3 outsidePosition;

    public float duration;

    public float gameOverWait;

    public Image fadeImg;

    public float fadeSpeed = 1.5f;

    FirstPersonController playerController;

 //   MouseLook playerLook;

 //   MouseLook cameraLook;
    

    AI _ai;
    // Use this for initialization
    void Start()
    {
        _ai = enemy.gameObject.GetComponent<AI>();

        //sets the player controller variable to be the firstpersoncontroller for the arent game object which is in this case the player 

        playerController = gameObject.GetComponentInParent<FirstPersonController>();

        // Sets the player look to be the mouse look in the parent gameobject which is the player 

        //       playerLook = gameObject.GetComponentInParent<MouseLook>();


        //Gets the mouse look for the player camera 
        //      cameraLook = playerCamera.GetComponent<MouseLook>();
        duration = 0;
            }

    // Update is called once per frame
    void Update()
    {

        if (fallingOver)
        {
            gameOverWait += Time.deltaTime;

            if(gameOverWait > 4)
            {
                //Sets duration in player prefs and shows the players score 
                PlayerPrefs.SetFloat("Duration",duration);
                GameOver();
            }
            // Declares the Quaternion for the rotation
            Quaternion fallRotation = Quaternion.LookRotation(new Vector3(1,1,0).normalized);


            //Slows down the rotation time. Lets the rotation get done smoothly not immediately 
            parent.transform.rotation = Quaternion.Slerp(parent.transform.rotation, fallRotation, Time.deltaTime * 4);

            playerController.enabled = false;
            //        playerLook.enabled = false;

            //        cameraLook.enabled = false;
            // Lerp the colour of the image between itself and black.
            fadeImg.color = Color.Lerp(fadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
        }

        else
        {
        //records time as a score
               duration += Time.deltaTime;
        }

        if (playerWins)
        {
            winDisplay.gameObject.SetActive(true);

            gameOverWait += Time.deltaTime;
            if (gameOverWait > 6)
            {
                PlayerPrefs.SetFloat("Duration", duration);
                GameOver();
            }

            // Lerp the colour of the image between itself and black.
            fadeImg.color = Color.Lerp(fadeImg.color, Color.black, fadeSpeed * Time.deltaTime);

        }

        
    }

    void OnWillRenderObject()
    {
        if (Camera.current.tag == "EnemyView")
        {
            _ai.playerInSight = true;
        }
    }

    void OnBecameInvisible()
    {
        _ai.playerInSight = false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag=="AI")
        {
            //Stop player input 
            //Make theplayer fall over 
            //Display some kind of game over feedback ( You lasted hh:mm:ss)

            fallingOver = true;


        }
        if (coll.gameObject.tag == "NeighbourTerrain")
        {
            playerWins = true;
        }
    }

   

    void GameOver()
    {
        SceneManager.LoadScene("GameOver_HighScores");
    }
}