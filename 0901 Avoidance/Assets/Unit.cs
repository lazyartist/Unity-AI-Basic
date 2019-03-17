using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public float AheadSize;
    public float ColliderRadius;

    protected Vector3 ahead;
    protected Vector3 aheadHalf;

	void Start () {
    }
	
	void Update () {
    }

    protected void OnDrawGizmos()
    {
        // forwar 방향을 바라봐야하기 때문에 매프레임 다시 계산한다.
        ahead = transform.forward * AheadSize;
        aheadHalf = ahead * 0.5f;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + ahead);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + aheadHalf);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ColliderRadius);
    }
}
