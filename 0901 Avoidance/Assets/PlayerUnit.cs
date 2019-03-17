using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit {
    public float AvoidWeight;
    public float Speed;
    public Unit Enemy;

    void Start () {
	}
	
	void Update () {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
	}

    void OnUpdate()
    {
        Vector3 collidedAhead = Vector3.zero;
        float avoidWeight = 0;
        if (Enemy.ColliderRadius > Vector3.Distance(transform.position + ahead, Enemy.transform.position))
        {
            collidedAhead = ahead;
            avoidWeight = AvoidWeight;
        }

        if (Enemy.ColliderRadius > Vector3.Distance(transform.position + aheadHalf, Enemy.transform.position))
        {
            collidedAhead = aheadHalf;
            avoidWeight = AvoidWeight * 1.0f;
        }

        if (collidedAhead != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + collidedAhead, Enemy.transform.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, transform.localScale.x);

            // 내적으로 적이 왼쪽(-)인지 오른쪽(+)인지 판단할 수 없다. 
            // 내적은 cos(Theta)이므로 1(0)~0(90)~-1(-0) 값이 나오고 앞뒤는 구별할 수 있지만 왼/오른은 구별할 수 없다.
            // 왼 오른을 구별하기 위해서는 두 벡터의 외적과 좌표계 up과 내적했을 때 0이상이면 오른쪽, 0이하이면 왼쪽이다.
            // 내적값이 90을 넘지 않는다.
            Vector3 cross = Vector3.Cross(transform.position + collidedAhead, Enemy.transform.position);
            //Vector3 cross = Vector3.Cross(collidedAhead.normalized, Enemy.transform.position.normalized);
            float dot = Vector3.Dot(Vector3.up, cross);
            float enemyDirection = dot / Mathf.Abs(dot); // +오른쪽, -왼족

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.right * enemyDirection);

            // 외적 벡터 - 시야에 90 벡터를 구하기 위해 외적을 구한다. transform.left, right 해도 나올 듯.
            // 오른쪽 벡터를 구하기 위해 z, y 순으로 외적 계산한다.
            // 유니티는 왼손 좌표계이디.
            Vector3 collidedAheadCrossProduct = Vector3.Cross(collidedAhead.normalized, transform.up * -1) * avoidWeight;
            collidedAheadCrossProduct *= enemyDirection * -1; // 방향을 반대로 곱해준다. 두 벡터 사이각을 그대로 회전각도로 더해주기 위함.

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + collidedAhead, transform.position + collidedAhead + collidedAheadCrossProduct);
            Gizmos.DrawLine(transform.position, transform.position + collidedAhead + collidedAheadCrossProduct);
            Gizmos.DrawLine(transform.position + Vector3.zero, transform.position + transform.up);

            /*
             각도 구하는 방법
                 1. 내적을 이용하여 Acos으로 각도구하는 법 ( 0~ 180도)
                 2. Atan2를 이용하여 각도를 구하는 법(-180 ~ 180도)
                 3. Vector3.SignedAngle 360각도(-180 ~ 180도)
                 4. Quaternion.FromToRotation 360 각도(0~360)
             */

            // 두 벡터 360각도(-180 ~ 180도)
            //float rotation = Vector3.SignedAngle(collidedAhead, collidedAhead + collidedAheadCrossProduct, Vector3.up);
            //transform.rotation = Quaternion.Euler(0, rotation, 0);

            // 두 벡터 Quaternion 각도 
            //Quaternion rotation = Quaternion.FromToRotation(collidedAhead + collidedAheadCrossProduct, collidedAhead);
            Quaternion rotation = Quaternion.FromToRotation(collidedAhead, collidedAhead + collidedAheadCrossProduct);
            //transform.Rotate(rotation.eulerAngles); // 각도 더하기
            Debug.Log(rotation.eulerAngles);
        }
    }

    protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        OnUpdate();
    }
}
