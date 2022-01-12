using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    SpriteRenderer sr;
    [SerializeField] float x_speed = 5f;
    Animator animator;
    Rigidbody2D rig;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0)
        {
            //transform.localScale = new Vector3(1f, 1f, 1f);
            sr.flipX = false;
        }
        else if (horizontal < 0)
        {
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            sr.flipX = true;
        }

        if (Mathf.Abs(horizontal) > 0.1f && vertical == 0f)
        {
            animator.SetFloat("Run", Mathf.Abs(horizontal));
        }
        else if (Mathf.Abs(vertical) > 0.1f && horizontal == 0f)
        {
            animator.SetFloat("Run", Mathf.Abs(vertical));
        }
        else if (Mathf.Abs(horizontal) > 0.1f && Mathf.Abs(vertical) > 0.1f)
        {
            animator.SetFloat("Run", Mathf.Abs(vertical));
        }
        else
        {
            animator.SetFloat("Run", 0f);
        }

        // Rigidbody2D 和 transform 的位置計算不同，因此添加了 Rigidbody2D 之後也應利用 Rigidbody2D 來取的得位置
        // 也因使用了 Rigidbody2D，因此應置於 FixedUpdate 當中來做更新，而非 Update
        float x_move = rig.position.x + horizontal * Time.fixedDeltaTime * x_speed;
        float y_move = rig.position.y + vertical * Time.fixedDeltaTime * x_speed;
        rig.position = new Vector2(x_move, y_move);
    }
}
