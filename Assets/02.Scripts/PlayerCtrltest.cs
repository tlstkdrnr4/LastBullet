using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCtrltest : MonoBehaviour
{
    public int health = 1;              // ĳ���� ü��
    private bool dead = false;          // ĳ���� ��� ����
    public float speed = 10.0f;         // ĳ���� �ӵ�
    private float Dash = 5.0f;          // ĳ���� �뽬 �ӵ�
    Vector2 moveV;                      // ĳ���� ����Ű
    Rigidbody2D rb;                     // ĳ���� ����

    Animation anim;                     // ĳ���� �ִϸ��̼�


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dead = false;
    }

    public float dashCoolDown = 5.0f;           // �뽬�� ����ϱ� ���� ��Ÿ��
    float dashTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        ObjMove();
        RotateToMouse();

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
    }

    // ĳ���Ͱ� ���콺�� �ٶ󺸰� �ϴ� �ڵ�
    void RotateToMouse()
    {
        // ���� ��ǥ���� ���콺 ��ġ ���ϱ�
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���� ����
        Vector2 dir = (mousePos - transform.position).normalized;

        // ���� ���
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // ĳ���� ȸ��
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveV * Time.fixedDeltaTime);
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
        if (collision.CompareTag("Enemy") && !dead)
        {
            health--;
            if (health <= 0)
            {
                dead = true;
                Dead();
            }
        }
    }

    void Dead()
    {
        speed = 0.0f;
        Dash = 0.0f;
    }
}
