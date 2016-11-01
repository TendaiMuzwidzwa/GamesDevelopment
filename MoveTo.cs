using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveTo : MonoBehaviour {

  //  public Transform goal;
    public Transform target;
  //  public Transform myTransform;
    public float rotationSpeed = 5;
    public Vector3 directionOfPlayer;
    public bool inSight = false;

    public float speed = 0.1F;
    public GameObject playerPos;
    //  public static reference ;

    // Use this for initialization
    void Awake()
    {
  //      myTransform = target;
    }

    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player");
        if (!playerPos)
        {
            Debug.Log("Make sure your player is tagged!!");
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            //   agent.destination = goal.position;
            agent.destination = target.position;

            target = GameObject.Find("Player").transform;
            //   }
        }
    }

    // Update is called once per frame
    void Update () {
           GetComponent<NavMeshAgent>().destination = playerPos.transform.position;

 /*       myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
        Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
        myTransform.position += myTransform.forward * speed * Time.deltaTime;
        transform.position += (target.position - transform.position).normalized * Time.deltaTime;
        */
/*       if (inSight)
        {
            directionOfPlayer = target.transform.position - transform.position;
            directionOfPlayer = directionOfPlayer.normalized;
            transform.Translate(directionOfPlayer * speed, Space.World);
        }*/
    }
 /*   voidOnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inSight = true;
        }
    }*/

    
}

/*
if (EnemyAlerted) {
			nav.speed = 6f;
			NavMoveScript.moveToPath = false;
			gameObject.GetComponent<Animation> ().CrossFade ("run");
			//run towards player
			//transform.LookAt (player.transform);
			//transform.position += transform.forward * nav.speed * Time.deltaTime;
//or use navmesh to chase to player...
Nav.SetDestination(player.transform);
		} else {
nav.speed = 1f;
gameObject.GetComponent<Animation> ().CrossFade ("walk");
   NavMoveScript.moveToPath = false;
}



    */
