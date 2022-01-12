using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float x_move = Input.GetAxis("Horizontal") * Time.deltaTime * speed;

        if(x_move > 0)
        {
            //transform.localScale = new Vector3(1f, 1f, 1f);
            sr.flipX = false;
        }
        else if(x_move < 0)
        {
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            sr.flipX = true;
        }

        transform.position = new Vector3(transform.position.x + x_move, 
                                         transform.position.y, 
                                         transform.position.z);
    }
}
