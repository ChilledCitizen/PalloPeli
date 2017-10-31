using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour {

    public GameObject player;
    Collider2D col;
    public float offset = 0.5f;

	// Use this for initialization
	void Start () {

        col = GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if (player.transform.position.y > transform.position.y + offset)
        {
            col.isTrigger = false;
        }

        else if (player.transform.position.y < transform.position.y - offset)
        {
            col.isTrigger = true;
        }
		
	}
}
