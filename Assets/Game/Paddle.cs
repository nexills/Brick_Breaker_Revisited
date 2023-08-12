using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    public float speed; // the speed of how fast the paddle moves
    public float rightLimit;
    public float leftLimit;
    public GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // get the key that is pressed
        if (gm.gameover) return;
        float direction = Input.GetAxis("Horizontal");
        transform.Translate(Vector2.right * direction * Time.deltaTime * speed);
        if (transform.position.x < leftLimit)
        {
            transform.position = new Vector2(leftLimit, transform.position.y);
        }
        if (transform.position.x > rightLimit)
        {
            transform.position = new Vector2(rightLimit, transform.position.y);
        }
    }
}
