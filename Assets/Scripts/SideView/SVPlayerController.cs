using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SVPlayerController : MonoBehaviour
{
    
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 100f;

    private Rigidbody2D rigid;

    private Vector3 moveDirection = Vector3.zero;

    private bool isOnGround = false;
    private bool isJumped = false;

    [SerializeField] private int maxJumpCount = 1;
    private int leftJumpCount = 0;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        leftJumpCount = maxJumpCount;
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        if (leftJumpCount > 0)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJumped = true;
                Debug.Log("Jump Key Input");
                rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                --leftJumpCount;
            }
        }

        transform.Translate(translation, 0, 0);
    }

    //private void OnCollisionStay2D(Collision2D coll)
    //{
    //    isJumped = false;
    //    isOnGround = true;
    //    Debug.Log("isJumped: " + isJumped);
    //}

    //private void OnCollisionExit2D(Collision2D coll)
    //{
    //    isOnGround = false;
    //}

    private void OnTriggerStay2D(Collider2D other)
    {
        isJumped = false;
        isOnGround = true;
        leftJumpCount = maxJumpCount;
        Debug.Log("isJumped: " + isJumped);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnGround = false;
    }

    public bool IsOnGround {
        get { return isOnGround; }
        set { isOnGround = value; }
    }
}
