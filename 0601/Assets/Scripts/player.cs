using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    const float turnRightAngle = -90;
    const float turnLeftAngle = 90;
    const float turnBackAngle = 180;

    public float moveSpeed = 5f;

    public SideCollider FrontCollider;
    public SideCollider LeftCollider;
    public SideCollider RightCollider;

    //private Vector3 _prevPosition;
    private bool _inBlackWall = false;

    // Use this for initialization
    void Start()
    {
        _inBlackWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    RotateTo(turnLeftAngle);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    RotateTo(turnRightAngle);
        //}

        if (FrontCollider.IsOnBlock)
        {
            CheckDirectionWhenBlock();
        }
        else if ((!LeftCollider.IsOnBlock || !RightCollider.IsOnBlock) && Random.Range(0, 100) > 93)
        {
            CheckDirectionWhenUnblock();
        }
        //else if ((!FrontCollider.IsOnBlock && !LeftCollider.IsOnBlock && !RightCollider.IsOnBlock) && Random.Range(0, 100) > 97)
        //{
        //    CheckDirection();
        //}

        if (_inBlackWall)
        {
            //transform.position = _prevPosition;
            _inBlackWall = false;
        }
        else
        {
            Move();
        }
    }

    void Move()
    {
        //_prevPosition = transform.position;

        // 바라보는 방향으로 이동한다.
        //transform.Translate((Vector3.up) * Time.deltaTime * moveSpeed);

        // 위의 코드를 수동으로 구현
        Vector3 angle = transform.rotation.eulerAngles;
        float zRadian = Mathf.PI * angle.z / 180 + (Mathf.PI / 2);
        float x = Mathf.Cos(zRadian);
        float y = Mathf.Sin(zRadian);
        Vector3 move = new Vector3(x, y, 0);
        transform.position = transform.position + (move * Time.deltaTime * moveSpeed);
        //////////////////////////
    }

    void RotateTo(float direction)
    {
        U.d("RotateTo", direction);

        transform.localRotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + direction);
        //transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 90);

        FrontCollider.IsOnBlock = false;
        LeftCollider.IsOnBlock = false;
        RightCollider.IsOnBlock = false;
    }

    void CheckDirectionWhenBlock()
    {
        U.d("CheckDirectionWhenBlock", FrontCollider.IsOnBlock, LeftCollider.IsOnBlock, RightCollider.IsOnBlock);

        // 앞 막, 양 뚤
        if (FrontCollider.IsOnBlock && !LeftCollider.IsOnBlock && !RightCollider.IsOnBlock)
        {
            RotateTo(Random.Range(0, 2) == 0 ? turnLeftAngle : turnRightAngle);
        }
        // 앞 양 막
        else if (FrontCollider.IsOnBlock && LeftCollider.IsOnBlock && RightCollider.IsOnBlock)
        {
            RotateTo(turnBackAngle);
        }
        // 왼 막
        else if (LeftCollider.IsOnBlock && !RightCollider.IsOnBlock)
        {
            RotateTo(turnRightAngle);
        }
        // 오른 막
        else if (!LeftCollider.IsOnBlock && RightCollider.IsOnBlock)
        {
            RotateTo(turnLeftAngle);
        }
        else
        {
        }
    }

    void CheckDirectionWhenUnblock()
    {
        U.d("CheckDirectionWhenUnblock", FrontCollider.IsOnBlock, LeftCollider.IsOnBlock, RightCollider.IsOnBlock);

        // 왼 막
        if (LeftCollider.IsOnBlock && !RightCollider.IsOnBlock)
        {
            RotateTo(turnRightAngle);
        }
        // 오른 막
        else if (!LeftCollider.IsOnBlock && RightCollider.IsOnBlock)
        {
            RotateTo(turnLeftAngle);
        }
        else
        {
            RotateTo(Random.Range(0, 2) == 0 ? turnLeftAngle : turnRightAngle);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(collision);

    //    if (collision.gameObject.tag == "BlackWall")
    //    {
    //        _inBlackWall = true;
    //        CheckDirection();
    //    }
    //}

}
