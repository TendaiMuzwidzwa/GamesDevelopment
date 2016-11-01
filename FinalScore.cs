using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FinalScore : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<Text>().text = PlayerPrefs.GetFloat("Duration").ToString();


    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
