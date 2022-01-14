using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleZonbie : MonoBehaviour
{
    public Vector3 target;
    protected Vector3 origin, turn_point;

    protected BoxCollider2D m_collider;
    protected SpriteRenderer sr;

    [SerializeField] protected AudioClip hurt_clip;
    [SerializeField] protected AudioClip attack_clip;
    protected AudioSource audio_source;

    public GameObject attack_collider_obj;
    protected float attack_distance = 1.3f;
    public int hp;

    public float speed;
    protected Animator animator;
    protected bool is_first_idle = true;
    protected bool is_attacked = false;
    protected bool is_alive = true;

    protected GameObject player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        origin = transform.position;
        player = GameObject.Find("Player");
        m_collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        audio_source = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackAndMove();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("PlayerAttack"))
        {
            hp--;
            audio_source.PlayOneShot(hurt_clip);
            audio_source.volume = 1.0f;

            if (hp < 1)
            {
                animator.SetTrigger("Dead");
                m_collider.enabled = false;
                is_alive = false;
                StartCoroutine(afterDead());
            }
            else
            {
                animator.SetTrigger("Hurt");
            }
        }
    }

    protected void attackAndMove()
    {
        if (is_alive)
        {
            attack();
            move();
        }
    }

    protected virtual void attack()
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

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("AttackWait"))
            {
                return;
            }

            audio_source.PlayOneShot(attack_clip);
            audio_source.volume = 1.0f;
            animator.SetTrigger("Attack");
            is_attacked = true;
            return;
        }
        else if (is_attacked)
        {
            if (turn_point == target)
            {
                //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                StartCoroutine(turnBack(new Vector3(-1.0f, 1.0f, 1.0f)));
            }
            else if (turn_point == origin)
            {
                //transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                StartCoroutine(turnBack(new Vector3(1.0f, 1.0f, 1.0f)));
            }

            is_attacked = false;
        }
    }

    protected virtual void move()
    {
        if (transform.position.x == target.x)
        {
            animator.SetTrigger("Idle");
            turn_point = origin;
            is_first_idle = false;
            StartCoroutine(turnBack(new Vector3(1.0f, 1.0f, 1.0f)));
        }
        else if (transform.position.x == origin.x)
        {
            if (!is_first_idle)
            {
                animator.SetTrigger("Idle");
            }

            turn_point = target;
            //transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            StartCoroutine(turnBack(new Vector3(-1.0f, 1.0f, 1.0f)));
        }

        // 檢查當前播放動畫是否為 Walk
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            transform.position = Vector3.MoveTowards(transform.position, turn_point, speed * Time.deltaTime);
        }
    }

    protected IEnumerator turnBack(Vector3 scale)
    {
        yield return new WaitForSeconds(2.0f);
        transform.localScale = scale;
    }

    public void setAttackColliderOn()
    {
        attack_collider_obj.SetActive(true);
    }

    public void setAttackColliderOff()
    {
        attack_collider_obj.SetActive(false);
    }

    protected IEnumerator afterDead()
    {
        yield return new WaitForSeconds(1.0f);
        //sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
        // 當有利用 Animation 來修改 color 時，就不能單純利用 sr.color 來修改
        sr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        
        yield return new WaitForSeconds(1.0f);
        sr.material.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}
