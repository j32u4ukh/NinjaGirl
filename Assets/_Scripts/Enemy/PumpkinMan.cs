using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinMan : MonoBehaviour
{
   
    bool is_alive = true;
    bool is_idle = true;
    bool jump_attack = false;
    bool slide_attack = false;
    bool is_jump_up = true;
    bool is_hurt = false;
    bool can_be_hurt = true;

    public float attack_distance = 2.0f;
    public float jump_height = 3.5f;
    public float ground_height = -2.85f;
    public float jump_attack_speed = 15.0f;
    public float slide_attack_speed = 8.0f;
    public float fall_down_speed = 10.0f;
    Vector3 slide_target;
    public int hp;

    Animator animator;
    SpriteRenderer sr;
    AudioSource audio_source;

    /* Stand offset: (-0.2404826, -0.1482427)
     * Stand size: (1.001012, 2.00966)
     * Slide offset: (-0.2404826, -0.4719939)
     * Slide size: (1.001012, 1.362157)
     */
    BoxCollider2D m_collider;
    readonly Vector2 STAND_OFFSET = new Vector2(-0.2404826f, -0.1482427f);
    readonly Vector2 STAND_SIZE = new Vector2(1.001012f, 2.00966f);
    readonly Vector2 SLIDE_OFFSET = new Vector2(-0.2404826f, -0.4719939f);
    readonly Vector2 SLIDE_SIZE = new Vector2(1.001012f, 1.362157f);
    
    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audio_source = GetComponent<AudioSource>();
        m_collider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (is_alive)
        {
            if (is_idle)
            {
                lookAtPlayer();

                // 玩家距離近的時候，滑行攻擊
                if (Vector3.Distance(player.transform.position, transform.position) <= attack_distance)
                {
                    is_idle = false;
                    StartCoroutine(beforeSlide());
                }

                // 玩家距離遠的時候，跳躍攻擊
                else
                {
                    is_idle = false;
                    StartCoroutine(beforeJumpUp());
                }
            }

            // 玩家距離遠的時候，跳躍攻擊
            else if (jump_attack)
            {
                lookAtPlayer();

                // 正在往上跳
                if (is_jump_up)
                {
                    Vector3 target = new Vector3(player.transform.position.x, jump_height, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, target, jump_attack_speed * Time.deltaTime);
                    animator.SetBool("JumpUp", true);
                }

                // 正在往下掉
                else
                {
                    animator.SetBool("JumpUp", false);
                    animator.SetBool("JumpDown", true);

                    Vector3 target = new Vector3(transform.position.x, ground_height, transform.position.z);
                    transform.position = Vector3.MoveTowards(transform.position, target, jump_attack_speed * Time.deltaTime);                }

                // 已跳到最高點
                if (transform.position.y == jump_height)
                {
                    is_jump_up = false;
                }

                // 已掉到最低點
                else if (transform.position.y == ground_height)
                {
                    jump_attack = false;
                    StartCoroutine(afterJumpDown());
                }
            }
            else if (slide_attack)
            {
                animator.SetBool("Slide", true);
                transform.position = Vector3.MoveTowards(transform.position, slide_target, slide_attack_speed * Time.deltaTime);

                if(transform.position == slide_target)
                {
                    m_collider.offset = STAND_OFFSET;
                    m_collider.size = STAND_SIZE;

                    animator.SetBool("Slide", false);
                    slide_attack = false;
                    is_idle = true;
                }
            }
            else if (is_hurt)
            {
                Vector3 target = new Vector3(transform.position.x, ground_height, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, target, fall_down_speed * Time.deltaTime);
            }
        }
        else
        {


            Vector3 target = new Vector3(transform.position.x, -2.85f, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, fall_down_speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("PlayerAttack"))
        {
            if (can_be_hurt)
            {
                hp--;
                can_be_hurt = false;
                audio_source.PlayOneShot(audio_source.clip);

                if (hp >= 1)
                {
                    is_idle = false;
                    jump_attack = false;
                    slide_attack = false;
                    is_hurt = true;

                    StopCoroutine(beforeSlide());
                    StopCoroutine(beforeJumpUp());
                    StopCoroutine(afterJumpDown());

                    animator.SetBool("Hurt", true);
                    StartCoroutine(endHurt());
                }
                else
                {
                    is_alive = false;
                    m_collider.enabled = false;
                    StopAllCoroutines();
                    animator.SetBool("Hurt", false);
                    animator.SetBool("Dead", true);
                    Time.timeScale = 0.5f;
                    StartCoroutine(afterDead());
                }
            }
        }
    }

    void lookAtPlayer()
    {
        if (transform.position.x < player.transform.position.x)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    IEnumerator beforeSlide()
    {
        yield return new WaitForSeconds(1.0f);
        m_collider.offset = SLIDE_OFFSET;
        m_collider.size = SLIDE_SIZE;
        lookAtPlayer();
        slide_target = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        slide_attack = true;
    }

    IEnumerator beforeJumpUp()
    {
        yield return new WaitForSeconds(1.0f);
        jump_attack = true;
    }

    IEnumerator afterJumpDown()
    {
        yield return new WaitForSeconds(0.5f);
        is_jump_up = true;
        is_idle = true;

        animator.SetBool("JumpUp", false);
        animator.SetBool("JumpDown", false);
    }

    IEnumerator endHurt()
    {
        yield return new WaitForSeconds(1.0f);
        animator.SetBool("Hurt", false);
        animator.SetBool("JumpUp", false);
        animator.SetBool("JumpDown", false);
        animator.SetBool("Slide", false);

        m_collider.offset = STAND_OFFSET;
        m_collider.size = STAND_SIZE;
        sr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);

        slide_attack = false;
        is_hurt = false;
        is_idle = true;
        is_jump_up = true;

        yield return new WaitForSeconds(2.0f);
        sr.material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        can_be_hurt = true;
    }

    IEnumerator afterDead()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        FadeInOut.instance.sceneFadeInOut("LevelSelect");
    }
}
