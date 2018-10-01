using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickBehavior : MonoBehaviour {

    private GameManager code;

    // Use this for initialization
    void Start () {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        code.AddBlock();
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        code.AddPoints();
        Destroy(gameObject);
    }
}
