using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayer : MonoBehaviour
{
    public float Speed = 0;
    public float Accelation = 0;
    public float Rotation = 0;
    public float GroundFriction = 0;

    private const float LeftRotation = -1;
    private const float RightRotation = 1;

    // 값 확인을 위해 인스펙터에 출력한다.
    [SerializeField]
    private float _accelation = 0;
    [SerializeField]
    private float _velocity = 0;
    [SerializeField]
    private float _rotation = 0;

    void Start()
    {
        _accelation = 0;
        _velocity = 0;
        _rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Accelation
        if (Input.GetKey(KeyCode.W))
        {
            _accelation = Accelation;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            _accelation = 0;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _accelation = Accelation * -1;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            _accelation = 0;
        }

        // Rotation
        if (Input.GetKey(KeyCode.A))
        {
            _rotation = Rotation * LeftRotation;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            _rotation = 0;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _rotation = Rotation * RightRotation;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            _rotation = 0;
        }

        Move();
    }

    void Move()
    {
        float velocity = GetVelocity();

        // 속도가 있을 때만 회전할 수 있게 한다.
        if (velocity != 0)
        {
            float newRotation = _rotation * (Mathf.Abs(velocity) / Speed); // 속도에 비례한 회전 각도
            transform.Rotate((new Vector3(0, newRotation, 0)));
        }

        // 현재 방향
        Vector3 rotation = transform.eulerAngles;
        float radian = rotation.y * Mathf.Deg2Rad * -1;
        Vector3 dir = new Vector3(Mathf.Cos(radian), 0, Mathf.Sin(radian));

        // 현재 방향으로 힘을 준다.
        Vector3 newPos = transform.position + dir * velocity;
        transform.position = newPos;
    }

    float GetVelocity()
    {
        // 엑셀의 힘을 속도에 적용
        _velocity += _accelation;

        // 속도의 반대로 마찰을 주기 위해 속도의 반대 부호를 계산
        float frictionDirection = (_velocity == 0) ? 0 : _velocity / -Mathf.Abs(_velocity);

        // 속도의 반대 방향인 마찰력을 속도에 더한다.
        _velocity += GroundFriction * frictionDirection;

        if (_velocity >= Speed)
        {
            _velocity = Speed;
        }
        else if (_velocity < -Speed)
        {
            _velocity = -Speed;
        }
        // 속도가 정확히 0이 되지 않기 때문에 0으로 만들어준다.
        else if (Mathf.Abs(_velocity) < 0.0001)
        {
            _velocity = 0;
        };

        return _velocity;
    }
}
