using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float MoveSpeed = 6;
    public Rigidbody Rigidbody;
    public Camera Camera;

    private Vector3 velocity;

    void Start () {
	}
	
	void Update () {
        // 카메라 세팅
        // position y == 10, rotation x == 90
        // x, z 값이 평면 좌표가된다.

        // z-depth
        // 카메라에서 얼마나 깊이 있는지를 나타낸다.
        // 카메라의 위치와 이 깊이 값으로 world 상의 좌표를 계산하기 때문에 입력하지 않으면 0으로 입력되고 이는 카메라와 같은 위치로 계산된다.
        Vector3 mouseWorldPos = Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.transform.position.y/*z-depth*/));

        // Vector3.up * transform.position.y 더해주는 이유
        // 현재 카메라가 위(up)에서 바라보기 때문에 y 좌표가 높이가 되고 x, z 값이 실제 위치가되는데
        // y값이 0이 아니면 각도는 상관없지만 x, z 벡터 길이가 짧아질 수 있다.
        transform.LookAt(mouseWorldPos + Vector3.up * transform.position.y);

        // 이동값
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * MoveSpeed;
    }

    private void FixedUpdate()
    {
        Rigidbody.MovePosition(Rigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
