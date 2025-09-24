using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCtrl : MonoBehaviour
{
    public Sprite IdleSprite;           // Idle.png
    public Sprite DashSprite;           // Dash.png
    public Sprite DeathSprite;          // Death.png
    public Sprite WalkSprite;           // Walk.png

    public int health = 1;              // ĳ���� ü��
    private bool dead = false;          // ĳ���� ��� ����
    public float speed = 10.0f;         // ĳ���� �ӵ�
    private float Dash = 5.0f;          // ĳ���� �뽬 �ӵ�

    public Transform body;
    private SpriteRenderer bodyRenderer;

    Vector2 moveV;                      // ĳ���� ����Ű
    Rigidbody2D rb;                     // ĳ���� ����



    // Start is called before the first frame update
    void Start()
    {
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        bodyRenderer.sprite = IdleSprite;
        rb = GetComponent<Rigidbody2D>();
        dead = false;
    }

    public float dashCoolDown = 5.0f;           // �뽬�� ����ϱ� ���� ��Ÿ��
    float dashTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (dead) return;                       // �׾����� �Է� ����

        ObjMove();

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        UpdateSprite();

        if (Input.GetKeyDown(KeyCode.F)){
            Dead();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position +  moveV * Time.fixedDeltaTime);
    }

    void ObjMove()
    {
        //W, A, S, DŰ �� �����¿�Ű �̵� �Է¹ޱ�
        Vector2 Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0f)
        {
            //Dash
            float dashDir = Input.GetAxisRaw("Horizontal"); // -1 (����), 0, 1 (������)

            if (dashDir != 0) // �¿� �Է� ���� ���� �뽬
            {
                rb.AddForce(Vector2.right * dashDir * Dash, ForceMode2D.Impulse);
                print("Dash!");
                bodyRenderer.sprite = DashSprite;
                StartCoroutine(ReturnIdle(0.2f));
                dashTimer = dashCoolDown;
            }
        }
        else
        {
            moveV = Move.normalized * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !dead)
        {
            health--;
            if(health <= 0)
            {
                dead = true;
                Dead();
            }
        }
    }

    void Dead()
    {
        dead = true;
        speed = 0.0f;
        Dash = 0.0f;
        bodyRenderer.sprite = DeathSprite;
    }

    void UpdateSprite()
    {
        if (dead) return;

        if(moveV.magnitude > 0.1f)
        {
            bodyRenderer.sprite = WalkSprite;
        }
        else
        {
            bodyRenderer.sprite = IdleSprite;
        }
    }

    IEnumerator ReturnIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!dead) bodyRenderer.sprite = IdleSprite;
    }
}
