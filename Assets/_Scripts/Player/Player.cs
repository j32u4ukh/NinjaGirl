using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


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
    LevelCanvas level_canvas;
    int hp;
    int n_kunai;
    int n_stone;

    public GameObject attack_collider_obj;
    public GameObject kunai_prefab;
    float kunai_distance;

    public AudioClip hurt_clip;
    public AudioClip item_clip;
    public AudioClip kunai_clip;
    public AudioClip sword_clip;
    public AudioClip dead_clip;
    AudioSource audio_source;

    InputAction player_move;
    InputAction player_jump;
    InputAction player_attack;
    InputAction player_kunai;

    private void Awake()
    {        
        animator = GetComponent<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audio_source = GetComponent<AudioSource>();
        level_canvas = GameObject.Find("/Canvas").GetComponent<LevelCanvas>();

        hp = PlayerPrefs.GetInt("Hp", 5);
        n_kunai = PlayerPrefs.GetInt("Kunai", 2);
        n_stone = PlayerPrefs.GetInt("Stone", 0);

        PlayerInput pi = GetComponent<PlayerInput>();
        player_move = pi.currentActionMap["Move"];
        player_jump = pi.currentActionMap["Jump"];
        player_attack = pi.currentActionMap["Attack"];
        player_kunai = pi.currentActionMap["Kunai"];
    }

    private void Update()
    {
        // ���D�B�����B���Y ������b���˪��A�U����
        if (!is_hurt)
        {
            // can_jump �T�O���a���b���D���A���S�A�����U���D�A���a��~��A�����D
            if (player_jump.triggered && can_jump)
            {
                is_jump_pressed = true;
                can_jump = false;
            }

            if (player_attack.triggered)
            {
                animator.SetTrigger("Attack");
                is_attacking = true;

                // �C���]�p�������ɤ�����A����������i�H(���D�L�{���i�H����)
                can_jump = false;
            }

            // animator �קK����ʵe���P�ɰ����U��{��
            if (player_kunai.triggered && (n_kunai > 0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Throw") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                updateKunai(n_kunai - 1);
                animator.SetTrigger("Throw");
                is_attacking = true;

                // �C���]�p�����Y�ɤ�����A���Y������i�H(���D�L�{���i�H���Y)
                can_jump = false;
            }
        }        
    }

    private void FixedUpdate()
    {
        //float horizontal = Input.GetAxisRaw("Horizontal");
        float horizontal = player_move.ReadValue<Vector2>().x;

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
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        hurtByEnemy(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name.Equals("BottomBound"))
        {
            //hp = 0;
            //PlayerPrefs.SetInt("Hp", hp);
            //level_canvas.updateHp();
            updateHp(value: 0);
            is_hurt = true;
            playerDead();
        }
    }

    void hurtByEnemy(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy") && !is_hurt && can_be_hurt)
        {
            //hp--;
            //PlayerPrefs.SetInt("Hp", hp);
            //level_canvas.updateHp();
            updateHp(hp - 1);

            is_hurt = true;

            if (hp < 1)
            {
                playerDead();
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

        #region �L�Įɶ�
        is_hurt = false;
        animator.SetBool("Hurt", false); 
        #endregion

        yield return new WaitForSeconds(1.0f);
        can_be_hurt = true;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1.0f);
    }

    /// <summary>
    /// �����ʵe�̫�@�� frame ���榹�禡�A�K�[�b Player �� Animation �������ʵe�v���
    /// </summary>
    public void endAttacking()
    {
        is_attacking = false;

        // can_jump �T�O���a���b���D���A���S�A�����U���D�A���a��~��A�����D
        can_jump = true;

        // �קK���D�L�{�����ƫ������A���a�ᤴ�~�����
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Throw");
    }

    public void setAttackColliderOn()
    {
        attack_collider_obj.SetActive(true);
    }

    /// <summary>
    /// TODO: �n�b���˪��Ĥ@�� frame �I�s���禡
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

        // �קK���D�L�{�����ƫ������A���a�ᤴ�~�����
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

    void playerDead()
    {
        animator.SetBool("Dead", true);
        audio_source.PlayOneShot(dead_clip);

        // �קK���a���ᤴ�i���k½���V
        is_attacking = true;

        // �T�O���a���᪽�����b��a
        m_rigidbody.velocity = Vector2.zero;
        PlayerPrefs.SetInt("Hp", 5);
        FadeInOut.instance.sceneFadeInOut("LevelSelect");
    }

    public void updateHp(int value)
    {
        hp = value;
        level_canvas.updateHp(value: value);
    }

    public void updateKunai(int value)
    {
        n_kunai = value;
        level_canvas.updateKunai(value: value);
    }

    public void updateStone(int value = -1)
    {
        n_stone = value;
        level_canvas.updateStone(value: value);
    }
}
