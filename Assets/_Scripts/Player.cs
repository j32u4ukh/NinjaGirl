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

            // �C���]�p�������ɤ�����A����������i�H(���D�L�{���i�H����)
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
            // ���ϥ� SpriteRenderer �� flipX�A�ӬO�ϥ� localScale �ӹF����V���ĪG�A�]���i�H�s�P Collider �@�_½��L�h
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        animator.SetFloat("Run", Mathf.Abs(horizontal));

        if (is_jump_pressed)
        {
            // ForceMode2D.Impulse: �����I�O
            // ForceMode2D.Force: ����I�O
            m_rigidbody.AddForce(Vector2.up * jump_force, ForceMode2D.Impulse);
            is_jump_pressed = false;
            animator.SetBool("Jump", true);
        }

        float x_move = horizontal * x_speed;
        m_rigidbody.velocity = new Vector2(x_move, m_rigidbody.velocity.y);
    }

    /// <summary>
    /// �����ʵe�̫�@�� frame ���榹�禡�A�K�[�b Player �� Animation �������ʵe�v���
    /// TODO: �n�b���˪��Ĥ@�� frame �I�s���禡
    /// </summary>
    public void endAttacking()
    {
        is_attacking = false;
        can_jump = true;

        // �קK���D�L�{�����ƫ������A���a�ᤴ�~�����
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
