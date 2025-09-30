using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class ShotGun : MonoBehaviour
{
    public GameObject ReloadImage;          // ������ �̹���
    public GameObject[] BulletPrefab;       // ź
    public GameObject[] EmptyPrefab;        // ź��
    public Transform FirePoint;             // �ѱ� ��ġ
    public Transform EmptyBullet;           // ź�� ����
    public Text BulletCount;                // ź ����
    private int MaxBulletCount = 6;         // �ִ�ġ ź ����
    public int NowBulletCount = 0;          // ���� ź ����
    public float BulletSpeed = 10.0f;       // ź �ӵ�

    // Start is called before the first frame update
    void Start()
    {
        BulletCount.text = NowBulletCount.ToString()+ " "+ "/" + " " + MaxBulletCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Conmand();
        BulletCount.text = NowBulletCount.ToString() + " " + "/" + " " + MaxBulletCount.ToString();
    }

    public float EmptyBulletSpeed = 0.0f;
    void Conmand()
    {
        //ź�� �߻�
        if (Input.GetMouseButtonDown(0))
        {
            print("Left Click");
            if(NowBulletCount >= 1) 
            {
                int BulletNumber = Random.Range(3, 9);
                //print(BulletNumber);
                if (BulletPrefab != null)
                {
                    for (int i = 0; i < BulletNumber; i++)
                    {
                        float BulletSpread = Random.Range(-15f, 15f);
                        Quaternion bulletRot = FirePoint.rotation * Quaternion.Euler(0, 0, BulletSpread);

                        GameObject Bullet = Instantiate(BulletPrefab[0], FirePoint.position, bulletRot);

                        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();
                        rb.velocity = bulletRot * Vector3.right * BulletSpeed;
                        Destroy(Bullet, 2.0f);
                    }
                    EmptyBulletSpeed += Time.deltaTime;
                    Quaternion EBrot = EmptyBullet.rotation * Quaternion.Euler(0, 0, -EmptyBulletSpeed);
                    GameObject EB = Instantiate(EmptyPrefab[0], EmptyBullet.position, EBrot);
                    Destroy(EB, 2.0f);
                    NowBulletCount--;
                }
            }
            else if(NowBulletCount <= 0)
            {
                print("Bullet Empty");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            print("Reload");
        }
    }

    public float ReloadTime = 3.0f;     // QTE�� ����Ǵ� ���� ������ �ð�
    bool isReloading = false;           // ������������
    /// </summary>
    public int qteMCount = 3;           // QTE �ִ� Ƚ��

    void Reload()
    {
        if (isReloading) return;

        // �� ź�� ������ ���� ź�� ���� �� ������ ź�� ���� �������Ѵ�
        int reloadBullet = MaxBulletCount - NowBulletCount;
        StartCoroutine(ReloadC(reloadBullet));
    }

    IEnumerator ReloadC(int reloadbullet)
    {
        isReloading = true;
        int qte = 0;            // QTE Ƚ��

        for(int i = 0; i < reloadbullet; i++)
        {
            // QTE �߻� ���� Ȯ���� QTE Ƚ���� �ִ뺸�� ���� ��
            bool DoQte = (Random.value < 0.5f) && (qte < qteMCount);

            if (DoQte)
            {
                qte++;
                KeyCode getkey = (KeyCode)Random.Range(65, 91);     // A~Z ����
                print((char)getkey);

                ReloadImage.SetActive(true);

                float time = ReloadTime;
                bool IsSuccess = false;

                while(time > 0f)
                {
                    if (Input.GetKeyDown(getkey))
                    {
                        print("Reload!");
                        NowBulletCount++;
                        IsSuccess = true;
                        break;
                    }
                    time -= Time.unscaledDeltaTime;
                    yield return null;
                }

                if (!IsSuccess)
                {
                    print("Reload Fail!");
                }

                ReloadImage.SetActive(false);
            }
            else
            {
                NowBulletCount++;
                yield return new WaitForSeconds(0.1f);
            }
        }
        isReloading = false;

    }
}

