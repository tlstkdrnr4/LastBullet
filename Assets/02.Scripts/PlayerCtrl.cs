using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public Sprite IdleSprite;           // Idle.png
    public Sprite DashSprite;           // Dash.png
    public Sprite DeathSprite;          // Death.png
    public Sprite WalkSprite;           // Walk.png

    public Sprite foot0;
    public Sprite foot1;
    public Sprite foot2;
    public Sprite foot3;
    public Sprite foot4;

    public int health = 1;              // ĳ���� ü��
    private bool dead = false;          // ĳ���� ��� ����
    public float speed = 10.0f;         // ĳ���� �ӵ�
    public float Dash = 15.0f;          // ĳ���� �뽬 �ӵ�
    public Text DashCoolDownText;       // �뽬 ��Ÿ�� �ؽ�Ʈ

    public Transform body;
    public Transform foot;
    private SpriteRenderer bodyRenderer;
    private SpriteRenderer footRenderer;

    Vector2 moveV;                      // ĳ���� ����Ű
    Rigidbody2D rb;                     // ĳ���� ����



    // Start is called before the first frame update
    void Start()
    {
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        bodyRenderer.sprite = IdleSprite;

        footRenderer = transform.Find("foot").GetComponent<SpriteRenderer>();
        footRenderer.sprite = foot0;

        rb = GetComponent<Rigidbody2D>();
        dead = false;

        DashCoolDownText.text = "�뽬 : " + ((int)dashTimer).ToString();
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

    public float Walktime = 0.0f;

    void ObjMove()
    {
        //W, A, S, DŰ �� �����¿�Ű �̵� �Է¹ޱ�
        Vector2 Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Move.x > 0 || Move.x < 0 || Move.y < 0 || Move.y > 0)
        {
            Walktime += Time.deltaTime;

            if (Walktime > 0.0f)
            {
                footRenderer.sprite = foot1;
            }
            else if (Walktime > 0.15f)
            {
                footRenderer.sprite = foot2;
            }
            else if (Walktime > 0.25f)
            {
                footRenderer.sprite = foot3;
            }
            Walktime = 0.0f;
        }
        else
        {
            footRenderer.sprite = foot0;
            Walktime = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0f)
        {
            //Dash
            float dashDir = Input.GetAxisRaw("Horizontal"); // -1 (����), 0, 1 (������)

            if (dashDir != 0) // �¿� �Է� ���� ���� �뽬
            {
                rb.velocity = Move.normalized * speed * Dash * Time.deltaTime;
                /*moveV = Move.normalized * speed * Dash;*/
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
        DashCoolDownText.text = "�뽬 : " + ((int)dashTimer).ToString();
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
