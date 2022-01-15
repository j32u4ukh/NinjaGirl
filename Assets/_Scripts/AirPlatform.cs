using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlatform : MonoBehaviour
{
    public Vector3 turn_point;
    Vector3 origin, target;
    public float speed;

    private void Awake()
    {
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == origin)
        {
            target = turn_point;
        }
        else if (transform.position == turn_point)
        {
            target = origin;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
