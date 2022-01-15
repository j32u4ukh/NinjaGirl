using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FemaleZonbie : MaleZonbie
{
    public float attack_speed = 2.0f;
    bool is_battle_mode = true;

    protected override void Awake()
    {
        base.Awake();
        attack_distance = 4.0f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.tag.Equals("StopPoint"))
        {
            is_battle_mode = false;
        }
        else if (collision.tag.Equals("PlayerAttack"))
        {
            is_battle_mode = true;
        }
    }

    protected override void attackAndMove()
    {
        if (is_alive)
        {
            attack();

            if (is_battle_mode)
            {
                move();
            } 
        }
    }

    protected override void attack()
    {
        if (is_battle_mode)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < attack_distance)
            {
                if (player.transform.position.x <= transform.position.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }

                Vector3 target = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
                {
                    transform.position = Vector3.MoveTowards(transform.position, target, attack_speed * Time.deltaTime);
                }

                is_attacked = true;
                return;
            }
            else if (is_attacked)
            {
                if (turn_point.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                }
                else if (transform.position.x < turn_point.x)
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
                else if (turn_point == target)
                {
                    StartCoroutine(turnBack(new Vector3(-1.0f, 1.0f, 1.0f)));
                }
                else if (turn_point == origin)
                {
                    StartCoroutine(turnBack(new Vector3(1.0f, 1.0f, 1.0f)));
                }

                is_attacked = false;
            }
        }
        else
        {
            if (turn_point.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else if (transform.position.x < turn_point.x)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            // 只在走路動畫期間移動殭屍
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                transform.position = Vector3.MoveTowards(transform.position, turn_point, speed * Time.deltaTime);
            }

            if (transform.position == turn_point)
            {
                is_battle_mode = true;
            }
        }
    }

    protected override void move()
    {
        base.move();
    }
}
