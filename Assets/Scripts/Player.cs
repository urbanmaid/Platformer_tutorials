using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //public variables
    public float speed;
    public float jumppwr;

    //moving variables
    float hAxis;
    float vAxis;
    Vector3 moveVec;
    Vector3 dodgeVec;

    //action shift variables
    bool wDown;
    bool jDown;
    bool isJumping;
    bool isDodging;

    //interaction variables
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
        Dodge();
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

        if(isDodging)
            moveVec = dodgeVec;

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
        if (jDown && moveVec == Vector3.zero && !isJumping && !isDodging){
            rigid.AddForce(Vector3.up * jumppwr, ForceMode.Impulse);
            anim.SetBool("isJumping", true);
            anim.SetTrigger("doJumping");
            isJumping = true;
        }
    }

    void Dodge()
    {
        if (jDown && (moveVec != Vector3.zero) && !isJumping){ 
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodging");
            isDodging = true;

            Invoke("DodgeOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodging = false;
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "floor"){
            isJumping = false;
            anim.SetBool("isJumping", false);
        }
    }
}
