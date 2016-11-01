using UnityEngine;
using System.Collections;

public class EmergencyExit : MonoBehaviour {

    public Transform AI;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Emergency Exit Trigger")
        {
            //Stop player input 
            //Make theplayer fall over 
            //Display some kind of game over feedback ( You lasted hh:mm:ss)

            AI.transform.position = AI.gameObject.GetComponent<AI>().spawnoutside[4];
            AI.gameObject.GetComponent<AI>().state = Actions.goOutside;
            AI.gameObject.GetComponent<AI>().outsideIndex = 4;


        }
        
    }

}
