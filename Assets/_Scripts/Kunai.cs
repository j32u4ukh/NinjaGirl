using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    GameObject player_obj;
    Rigidbody2D m_rigidbody;
    public float speed = 3.0f;

    private void Awake()
    {
        player_obj = GameObject.Find("Player");
        m_rigidbody = GetComponent<Rigidbody2D>();

        if (player_obj.transform.localScale.x == 1.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            m_rigidbody.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        }
        else if (player_obj.transform.localScale.x == -1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            m_rigidbody.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
        }

        Destroy(gameObject, 5.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collision.tag.Equals("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
