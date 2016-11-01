using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour
{

    public float speed;
    public Vector3 look;
    public Vector3 lastSighting;
    public Vector3 targetPosition;
    public Vector3 outsidePosition;
    public bool playerInSight;
    public float runningSpeed = 1f;
    public Transform player;
    public float fieldOfView;
    public bool movingToPlayer;
    public bool playerOutside;
    public bool chaseOutside; //global boolean to solve the problem of the ai not chasing the player once it was outside.
    public float closeto = 1;

    public int index = 0;
    public int hallIndex = 0;
    public int livingroomIndex = 0;
    public int kitchenIndex = 0;
    public int laundryroomIndex = 0;
    public int garageIndex;
    public int bedroomIndex = 0;
    public int bedroom2Index = 0;
    public int masterbedroomIndex = 0;
    public int outsideIndex = 0;

    //Copied over from the Third Person Character Script from Unity Standard Assets
    float m_GroundCheckDistance = 0.1f;
    float m_StationaryTurnSpeed = 180;
    float m_JumpPower = 12f;
    float m_RunCycleLegOffset = 0.2f;
    float m_AnimSpeedMultiplier = 1f;
    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;
    bool m_Crouching;

    Ray ray = new Ray();
    RaycastHit hit = new RaycastHit();


    public Actions state;

    // Use this for initialization
    void Start()
    {
        state = Actions.patrolHall;

        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movingToPlayer)
        {
            switch (state)
            {
                case Actions.patrolHall:
                    patrolHall();
                    break;
                default:
                    break;
                case Actions.patrolLivingroom:
                    patrolLivingroom();
                    break;
                case Actions.patrolKitchen:
                    patrolKitchen();
                    break;
                case Actions.patrolLaundryRoom:
                    patrolLaundryRoom();
                    break;
                case Actions.patrolGarage:
                    patrolGarage();
                    break;
                case Actions.patrolBedroom:
                    patrolBedroom();
                    break;
                case Actions.patrolBedroom2:
                    patrolBedroom2();
                    break;
                case Actions.patrolMasterBedroom:
                    patrolMasterBedroom();
                    break;
                case Actions.goOutside:
                    goOutside();
                    break;
                case Actions.retreat:
                    break;

            }
        }



        // looks towards where it wants to go 
        look = (targetPosition - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(look);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);

        if(!chaseOutside)
        {

            // Ray will be shot here  // out hit means raycast hit is an output variable,
            //if it hits anything it will contain data on what it hit
            if (playerInSight)
            {
                if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        lastSighting.x = player.position.x;
                        lastSighting.y = transform.position.y;
                        lastSighting.z = player.position.z;
                        targetPosition = lastSighting;
                        movingToPlayer = true;
                    }

                }

            }
            if (movingToPlayer)
            {
                transform.position = Vector3.MoveTowards(transform.position, lastSighting, runningSpeed);
                //C hecking to see how close the ai is to the player
                //Going to the last place it saw the player and if it doesnt hit the player it starts patrolling again
                if (Vector3.Distance(transform.position, lastSighting) <= 0.5f)
                {
                    movingToPlayer = false;
                }
            }

            if (playerOutside)
            {
                transform.position = Vector3.MoveTowards(transform.position, outsidePosition, runningSpeed);
                if (Vector3.Distance(transform.position, lastSighting) <= 0.5f)
                {
                    movingToPlayer = true;
                }
            }
        }

 //       UpdateAnimator(targetPosition);
    }

    public List<Vector3> hall = new List<Vector3>();
    public List<Vector3> livingroom = new List<Vector3>();
    public List<Vector3> kitchen = new List<Vector3>();
    public List<Vector3> laundryroom = new List<Vector3>();
    public List<Vector3> garage = new List<Vector3>();
    public List<Vector3> bedroom = new List<Vector3>();
    public List<Vector3> bedroom2 = new List<Vector3>();
    public List<Vector3> masterbedroom = new List<Vector3>();
    public List<Vector3> outside = new List<Vector3>(); //contains 4 different points for the sides of the house
    public List<Vector3> spawnoutside = new List<Vector3>();// spawns and then moves to be a specific place 

    public Vector3 outsidelivingroom;
    public Vector3 outsidekitchen;
    public Vector3 outsidelaundryroom;
    public Vector3 outsidegarage;
    public Vector3 outsidebedroom;
    public Vector3 outsidebedroom2;
    public Vector3 outsidemasterbedroom;
    public Vector3 endofhallway;

    void patrolHall()
    {
        targetPosition = hall[hallIndex];
        transform.position = Vector3.MoveTowards(transform.position, hall[hallIndex], speed);

        if (Vector3.Distance(transform.position, hall[hallIndex]) <= closeto)
        {
            hallIndex++;
            if (hall[hallIndex - 1] == outsidelivingroom)
            {

                state = Actions.patrolLivingroom;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (hallIndex >= hall.Count)
        {
            hallIndex = 0;
        }

    }
    void patrolLivingroom()
    {
        targetPosition = livingroom[livingroomIndex];
        transform.position = Vector3.MoveTowards(transform.position, livingroom[livingroomIndex], speed);

        if (Vector3.Distance(transform.position, livingroom[livingroomIndex]) <= closeto)
        {
            livingroomIndex++;
            if (livingroom[livingroomIndex - 1] == outsidekitchen)
            {

                state = Actions.patrolKitchen;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (livingroomIndex >= livingroom.Count)
        {
            livingroomIndex = 0;
        }

    }
    void patrolKitchen()

    {
        targetPosition = kitchen[kitchenIndex];
        transform.position = Vector3.MoveTowards(transform.position, kitchen[kitchenIndex], speed);

        if (Vector3.Distance(transform.position, kitchen[kitchenIndex]) <= closeto)
        {
            kitchenIndex++;
            if (kitchen[kitchenIndex - 1] == outsidelaundryroom)
            {

                state = Actions.patrolLaundryRoom;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (kitchenIndex >= kitchen.Count)
        {
            kitchenIndex = 0;
        }

    }

    void patrolLaundryRoom()
    {
        targetPosition = laundryroom[laundryroomIndex];
        transform.position = Vector3.MoveTowards(transform.position, laundryroom[laundryroomIndex], speed);

        if (Vector3.Distance(transform.position, laundryroom[laundryroomIndex]) <= closeto)
        {
            laundryroomIndex++;
            if (laundryroom[laundryroomIndex - 1] == outsidegarage)
            {

                state = Actions.patrolGarage;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (laundryroomIndex >= laundryroom.Count)
        {
            laundryroomIndex = 0;
        }

    }
    void patrolGarage()
    {
        targetPosition = garage[garageIndex];
        transform.position = Vector3.MoveTowards(transform.position, garage[garageIndex], speed);

        if (Vector3.Distance(transform.position, garage[garageIndex]) <= closeto)
        {
            garageIndex++;
            if (garage[garageIndex - 1] == outsidebedroom)
            {

                state = Actions.patrolBedroom;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (garageIndex >= garage.Count)
        {
            garageIndex = 0;
        }

    }

    void patrolBedroom()
    {
        targetPosition = bedroom[bedroomIndex];
        transform.position = Vector3.MoveTowards(transform.position, bedroom[bedroomIndex], speed);

        if (Vector3.Distance(transform.position, bedroom[bedroomIndex]) <= closeto)
        {
            bedroomIndex++;
            if (bedroom[bedroomIndex - 1] == outsidebedroom2)
            {

                state = Actions.patrolBedroom2;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (bedroomIndex >= bedroom.Count)
        {
            bedroomIndex = 0;
        }

    }
    void patrolBedroom2()
    {
        targetPosition = bedroom2[bedroom2Index];
        transform.position = Vector3.MoveTowards(transform.position, bedroom2[bedroom2Index], speed);

        if (Vector3.Distance(transform.position, bedroom2[bedroom2Index]) <= closeto)
        {
            bedroomIndex++;
            if (bedroom2[bedroom2Index - 1] == outsidemasterbedroom)
            {

                state = Actions.patrolMasterBedroom;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (bedroom2Index >= bedroom2.Count)
        {
            bedroom2Index = 0;
        }

    }

    void patrolMasterBedroom()
    {
        targetPosition = masterbedroom[masterbedroomIndex];
        transform.position = Vector3.MoveTowards(transform.position, masterbedroom[masterbedroomIndex], speed);

        if (Vector3.Distance(transform.position, masterbedroom[masterbedroomIndex]) <= closeto)
        {
            masterbedroomIndex++;
            if (masterbedroom[masterbedroomIndex - 1] == endofhallway)
            {

                state = Actions.patrolHall;

            }
        }

        // If run through all the positions of that list it resets the patrol index
        if (masterbedroomIndex >= masterbedroom.Count)
        {
            masterbedroomIndex = 0;
        }

    }
    void goOutside()
    {

        // Noticed a problem that is the ai was within of 0.5f of the outside outside index position then it would target the player
        //but then it would move outside of that 0.5f of that position and as a result 
        // constantly move towards and away from the players position constantly and that it why it was not following the player

        if (Vector3.Distance(transform.position, outside[outsideIndex]) <= closeto)
        {
            chaseOutside = true;
        }
        // This checks to see if it has gone around the corer and if it has it sets it to just chase the player 
        if (chaseOutside)
        {
            targetPosition = player.transform.position;  
        }
        else
        {
        targetPosition = outside[outsideIndex];
        }

        
        transform.position = Vector3.MoveTowards(transform.position,targetPosition, runningSpeed);

        /*
        Needed to create a global boolean for the script (bool chaseOuside)
        */
    }
    void OnTriggerEnter(Collider coll)
    {
        
        if (coll.gameObject.tag == "NeighbourTerrain")
        {
            state = Actions.retreat;
        }
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (m_IsGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // don't use that while airborne
            m_Animator.speed = 1;
        }
    }
}
public enum Actions
{
    patrolHall,
    patrolLivingroom,
    patrolKitchen,
    patrolLaundryRoom,
    patrolGarage,
    patrolBedroom,
    patrolBedroom2,
    patrolMasterBedroom,
    goOutside,
    retreat, // action to stop the ai from moving 
}
