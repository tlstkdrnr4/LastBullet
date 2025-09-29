using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        RotateToMouse();
    }

    // ĳ���Ͱ� ���콺�� �ٶ󺸰� �ϴ� �ڵ�
    // MOUSE
    void RotateToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float yrot = 0.0f;
        float zrot = 0.0f;
        float zrotspd = 11.0f;

        if(mousePos.x < transform.position.x)
        {
            yrot = 180.0f;
        }
        else
        {
            yrot = 0.0f;
        }

        if(mousePos.y == 0)
        {
            zrot = 0.0f;
        }
        else
        {
            if(mousePos.y > 4.5f)
            {
                zrot = 50.0f;
            }
            else if(mousePos.y < -4.5f)
            {
                zrot = -50.0f;
            }
            else
            {
                zrot = mousePos.y * zrotspd;
            }
        }

        transform.rotation = Quaternion.Euler(0, yrot, zrot);
    }
}
