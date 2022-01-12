using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float x_speed = 5f;
    [SerializeField] float jump_force = 5f;
    [HideInInspector] public Animator animator;
    Rigidbody2D m_rigidbody;
    bool is_jump_pressed = false;
    bool is_attacking = false;
    [HideInInspector] public bool can_jump = true;

    public GameObject attack_collider_obj;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && can_jump)
        {
            is_jump_pressed = true;
            can_jump = false;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("Attack");
            is_attacking = true;

            // 遊戲設計為攻擊時不能跳，攻擊結束後可以(跳躍過程中可以攻擊)
            can_jump = false;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            animator.SetTrigger("Throw");
            is_attacking = true;
            can_jump = false;
        }
    }

    private void FixedUpdate()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        if (is_attacking)
        {
            horizontal = 0;
        }

        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (horizontal < 0)
        {
            // 不使用 SpriteRenderer 的 flipX，而是使用 localScale 來達到轉向的效果，因為可以連同 Collider 一起翻轉過去
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        animator.SetFloat("Run", Mathf.Abs(horizontal));

        if (is_jump_pressed)
        {
            // ForceMode2D.Impulse: 瞬間施力
            // ForceMode2D.Force: 持續施力
            m_rigidbody.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
            is_jump_pressed = false;
            animator.SetBool("Jump", true);
        }

        float x_move = horizontal * x_speed;
        m_rigidbody.velocity = new Vector2(x_move, m_rigidbody.velocity.y);
    }

    /// <summary>
    /// 攻擊動畫最後一個 frame 執行此函式，添加在 Player 的 Animation 的攻擊動畫影格當中
    /// TODO: 要在受傷的第一個 frame 呼叫此函式
    /// </summary>
    public void endAttacking()
    {
        is_attacking = false;
        can_jump = true;

        // 避免跳躍過程中重複按攻擊，落地後仍繼續執行
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Throw");
    }

    public void setAttackColliderOn()
    {
        attack_collider_obj.SetActive(true);
    }

    public void setAttackColliderOff()
    {
        attack_collider_obj.SetActive(false);
    }
}
