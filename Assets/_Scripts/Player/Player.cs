using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float x_speed = 5f;
    [SerializeField] float jump_force = 5f;
    [HideInInspector] public Animator animator;
    Rigidbody2D m_rigidbody;
    SpriteRenderer sr;

    bool is_jump_pressed = false;
    bool is_attacking = false;
    bool is_hurt = false;
    bool can_be_hurt = true;
    [HideInInspector] public bool can_jump = true;
    int hp = 3;

    public GameObject attack_collider_obj;
    public GameObject kunai_prefab;
    float kunai_distance;

    public AudioClip hurt_clip;
    public AudioClip item_clip;
    public AudioClip kunai_clip;
    public AudioClip sword_clip;
    public AudioClip dead_clip;
    AudioSource audio_source;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audio_source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // 跳躍、攻擊、投擲 都不能在受傷狀態下執行
        if (!is_hurt)
        {
            // can_jump 確保玩家不在跳躍狀態中又再次按下跳躍，落地後才能再次跳躍
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

                // 遊戲設計為投擲時不能跳，投擲結束後可以(跳躍過程中可以投擲)
                can_jump = false;
            }
        }        
    }

    private void FixedUpdate()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");

        if (is_attacking || is_hurt)
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

        if (!is_hurt)
        {
            m_rigidbody.velocity = new Vector2(x_move, m_rigidbody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hurtByEnemy(collision);

        if (collision.tag.Equals("Item"))
        {
            audio_source.PlayOneShot(item_clip);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hurtByEnemy(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Equals("BoundBottom"))
        {
            hp = 0;
            is_hurt = true;
            audio_source.PlayOneShot(dead_clip);

            // TODO: 此種死亡發生在畫面外，是否還有播放動畫的必要？
            animator.SetBool("Dead", true);

            // 避免玩家死後仍可左右翻轉方向
            is_attacking = true;

            // 確保玩家死後直接停在原地
            m_rigidbody.velocity = Vector2.zero;
        }
    }

    void hurtByEnemy(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy") && !is_hurt && can_be_hurt)
        {
            hp--;
            is_hurt = true;

            if (hp < 1)
            {
                animator.SetBool("Dead", true);
                audio_source.PlayOneShot(dead_clip);

                // 避免玩家死後仍可左右翻轉方向
                is_attacking = true;

                // 確保玩家死後直接停在原地
                m_rigidbody.velocity = Vector2.zero;
            }
            else
            {                
                can_be_hurt = false;
                animator.SetBool("Hurt", true);
                audio_source.PlayOneShot(hurt_clip);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);

                if (transform.localScale.x == 1.0f)
                {
                    m_rigidbody.velocity = new Vector2(-2.5f, 10.0f);
                }
                else if (transform.localScale.x == -1.0f)
                {
                    m_rigidbody.velocity = new Vector2(2.5f, 10.0f);
                }

                StartCoroutine(endHurt());
            }
        }
    }

    IEnumerator endHurt()
    {
        yield return new WaitForSeconds(1.0f);

        #region 無敵時間
        is_hurt = false;
        animator.SetBool("Hurt", false); 
        #endregion

        yield return new WaitForSeconds(1.0f);
        can_be_hurt = true;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
    }

    /// <summary>
    /// 攻擊動畫最後一個 frame 執行此函式，添加在 Player 的 Animation 的攻擊動畫影格當中
    /// </summary>
    public void endAttacking()
    {
        is_attacking = false;

        // can_jump 確保玩家不在跳躍狀態中又再次按下跳躍，落地後才能再次跳躍
        can_jump = true;

        // 避免跳躍過程中重複按攻擊，落地後仍繼續執行
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Throw");
    }

    public void setAttackColliderOn()
    {
        attack_collider_obj.SetActive(true);
    }

    /// <summary>
    /// TODO: 要在受傷的第一個 frame 呼叫此函式
    /// </summary>
    public void setAttackColliderOff()
    {
        attack_collider_obj.SetActive(false);
    }

    public void createKunai()
    {
        if (transform.localScale.x == 1f)
        {
            kunai_distance = 1.0f;
        }
        else
        {
            kunai_distance = -1.0f;
        }

        Vector3 kunai_position = new Vector3(transform.position.x + kunai_distance, transform.position.y, transform.position.z);
        Instantiate(kunai_prefab, kunai_position, Quaternion.identity);
    }

    public void beHurt()
    {
        is_attacking = false;

        // 避免跳躍過程中重複按攻擊，落地後仍繼續執行
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Throw");

        attack_collider_obj.SetActive(false);
    }

    public void playSwordEffect()
    {
        audio_source.PlayOneShot(sword_clip);
    }

    public void playKunaiEffect()
    {
        audio_source.PlayOneShot(kunai_clip);
    }
}
