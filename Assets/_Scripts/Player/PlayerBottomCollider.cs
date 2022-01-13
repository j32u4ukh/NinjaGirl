using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomCollider : MonoBehaviour
{
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Ground"))
        {
            player.can_jump = true;
            player.animator.SetBool("Jump", false);
        }
    }
}
