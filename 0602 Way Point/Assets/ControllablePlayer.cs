using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllablePlayer : MonoBehaviour
{
    public float Speed;
    public float JumpSpeed;
    public float JumpDelay;

    public float Velocity = 0;
    public bool IsJump = false;

    public Animator Animator;
    public Rigidbody Rigidbody;

    public Vector3 jumpForce;
    public Vector3 _direction;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckKey();

        Move();

        Animator.SetFloat("Velocity", Velocity);
        Animator.SetBool("IsJump", IsJump);
    }

    void CheckKey()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            Velocity = Speed;
            _direction = Vector3.right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            Velocity = Speed;
            _direction = Vector3.left;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            Velocity = 0;
            _direction = Vector3.zero;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            Velocity = 0;
            _direction = Vector3.zero;
        }
        else
        {
            Velocity = 0;
            _direction = Vector3.zero;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            IsJump = true;
            StartCoroutine(Coroutine_Jump());
        }
    }

    IEnumerator Coroutine_Jump()
    {
        yield return new WaitForSeconds(JumpDelay);
        Rigidbody.AddForce(Vector3.up * JumpSpeed);
    }

    void Move()
    {
        //U.d(_direction);
        Rigidbody.AddForce(_direction * Velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        U.d("OnTriggerEnter", other);

        if (other.gameObject.tag == "Ground")
        {
            IsJump = false;
        }
    }
}
