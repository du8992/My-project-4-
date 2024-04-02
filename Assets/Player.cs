using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    
    public bool isTorchTop;
    public bool isTorchBottom;
    public bool isTorchRight;
    public bool isTorchLeft;

    public float speed;
    public float power;
    public float maxShotDelay;
    public float curShotDelay;

    public GameObject bulletobjA;
    public GameObject bulletobjB;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Fire();
        Reload();
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTorchRight && h == 1) || (isTorchLeft && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((isTorchTop && v == 1) || (isTorchBottom && v == -1))
            v = 0;

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;


        if(Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
            return;

        if (curShotDelay < maxShotDelay)
            return;

        switch (power)
        {
            case 1:
                GameObject bullet = Instantiate(bulletobjA, transform.position, transform.rotation);
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

            case 2:
                GameObject bulletR = Instantiate(bulletobjA, transform.position + Vector3.right * 0.1f, transform.rotation);
                GameObject bulletL = Instantiate(bulletobjA, transform.position + Vector3.left * 0.1f, transform.rotation);
                
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                
                break;

            case 3:
                GameObject bulletRR = Instantiate(bulletobjA, transform.position + Vector3.right * 0.25f, transform.rotation);
                GameObject bulletCC = Instantiate(bulletobjB, transform.position, transform.rotation);
                GameObject bulletLL = Instantiate(bulletobjA, transform.position + Vector3.left * 0.25f, transform.rotation);

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

        }
        

        curShotDelay = 0;
    }

    void Reload() 
    {
        curShotDelay += Time.deltaTime;
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTorchTop = true;
                    break;

                case "Bottom":
                    isTorchBottom = true;
                    break;

                case "Right":
                    isTorchRight = true;
                    break;

                case "Left":
                    isTorchLeft = true;
                    break;
            }

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTorchTop = false;
                    break;

                case "Bottom":
                    isTorchBottom = false;
                    break;

                case "Right":
                    isTorchRight = false;
                    break;

                case "Left":
                    isTorchLeft = false;
                    break;
            }
        }
    }
}
