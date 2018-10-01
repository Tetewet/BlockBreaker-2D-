using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveBall : MonoBehaviour {

    [SerializeField] private float dir = 150f;
    [SerializeField] private Rigidbody2D bodyBall;
    private bool onlyOnce = false;
    private Transform myParent;
    private Vector3 initPos;
    private GameManager code;
    public GameObject deathEffect;
    [SerializeField] private float paddleDirBlindspot = 0.2f;
    [SerializeField] private float vForceMin = 0.6f;
    [SerializeField] private float vForceMinMulti = 2f;

    // Use this for initialization
    void Start () {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        bodyBall.simulated = false;
        initPos = transform.localPosition;
        myParent = transform.parent;
	}

    public void Init()
    {
        transform.parent = myParent;
        transform.localPosition = initPos;
        bodyBall.simulated = false;
        bodyBall.velocity = new Vector2(0, 0);
        onlyOnce = false;
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetButtonUp("Jump") && !onlyOnce)
        {
            onlyOnce = true;
            bodyBall.simulated = true;
            bodyBall.transform.parent = null;
            bodyBall.AddForce(new Vector2(dir, dir));
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource rebondAudio = GetComponent<AudioSource>();
        AudioSource boomAudio = deathEffect.GetComponent<AudioSource>();
        ParticleSystem explodeEffect = deathEffect.GetComponent<ParticleSystem>();
        if (collision.gameObject.tag == "death") 
        {
            boomAudio.Play();
            deathEffect.transform.position = transform.position;
            transform.position = new Vector3(-100, -100, -100);
            Time.timeScale = 1;
            explodeEffect.Play();
            code.Death();
        }
        if (collision.gameObject.tag == "paddle")
        {
            rebondAudio.Play();
            float diffX = transform.position.x - collision.transform.position.x;
            if (diffX < -paddleDirBlindspot) 
            {
                //je viens de toucher le côté gauche de ma paddle
                bodyBall.velocity = new Vector2(0, 0);
                bodyBall.AddForce(new Vector2(-dir, dir));
            }
            else if (diffX > paddleDirBlindspot)
            {
                //je viens de toucher le côté droit de ma paddle
                bodyBall.velocity = new Vector2(0, 0);
                bodyBall.AddForce(new Vector2(dir, dir));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Mathf.Abs(bodyBall.velocity.y) < vForceMin)
        {
            float vectorX = bodyBall.velocity.x;
            if (bodyBall.velocity.y < 0)
            {
                bodyBall.velocity = new Vector2(vectorX, -vForceMin * vForceMinMulti);
            }
            else
            {
                bodyBall.velocity = new Vector2(vectorX, vForceMin * vForceMinMulti);
            }
            Debug.Log("Fail safe Vertical");
        }
    }
}
