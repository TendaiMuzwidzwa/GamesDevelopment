using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DurationDisplay : MonoBehaviour {

    public Transform player;

    public Player p;
	// Use this for initialization
	void Start () {

        p = player.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = p.duration.ToString();
	}
}
