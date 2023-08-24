using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public Rigidbody2D rb;
    public bool inPlay;
    public Transform paddle;
    public float speed;
    public Transform explosion;
    public GameManager gm;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        switch(Info.diff) {
            // setting speed depending on difficulty chosen
            case 0:
                speed = 345;
                break;
            case 1:
                speed = 380;
                break;
            case 2:
                speed = 415;
                break;
            case 3:
                speed = 450;
                break;
            case 4:
                speed = 485;
                break;
        }
    }

    // Update is called once per frame
    void Update() {
        // behaviours when game is not in play
        if(!inPlay) {
            rb.velocity = Vector2.zero;
            transform.position = paddle.position + new Vector3(0,1,0);

        }
        if (gm.gameover) return;
        // starting the game
        if (!inPlay && Input.GetButtonDown("Jump")) {
            inPlay = true;
            rb.AddForce(Vector2.up * speed);
        }
    }

    // detect if ball left screen
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Bottom")) {
            inPlay = false;
            gm.updateLive(-1);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.CompareTag("Brick")) {
            Transform var = Instantiate(explosion, other.transform.position, 
                other.transform.rotation);
            // update the score and coins
            gm.updateScore(other.gameObject.GetComponent<Brick>().worth);
            gm.updateCoin(other.gameObject.GetComponent<Brick>().coin);
            // if is spawning brick, spawn some bricks
            if (other.gameObject.GetComponent<Brick>().spawning) {
                gm.spawn();
            }
            gm.brickHit();
            // destroy the objects
            Destroy(var.gameObject, 2);
            Destroy(other.gameObject);
        }
    }

    // for accesses beyond the script
    public void not_in_play() {
        inPlay = false;
    }
}
