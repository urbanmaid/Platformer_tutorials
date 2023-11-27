using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumppwr;

    float hAxis;
    float vAxis;
    Vector3 moveVec;
    bool wDown;
    bool jDown;
    bool isJumping;

    Rigidbody rigid;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");
        jDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        transform.position += moveVec * speed * Time.deltaTime * (wDown ? 0.3f : 1f);

        anim.SetBool("isRunning", moveVec != Vector3.zero);
        anim.SetBool("isWalking", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }

    void Jump()
    {
        if (jDown && !isJumping){
            rigid.AddForce(Vector3.up * jumppwr, ForceMode.Impulse);
            isJumping = true;
            anim.SetBool("isJumping", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "floor"){
            isJumping = false;
            anim.SetBool("isJumping", false);
        }
    }
}
