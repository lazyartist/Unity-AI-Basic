using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 1;
    public float TurnSpeed = 0.2f;
    public PlayerManager PlayerManager;
    public Animator Animator;

    private int _curWaypointIndex = -1;
    private bool _isGoal;
    private int _countToDelete = 0;


    private Transform TargetWaypoint;

    // Use this for initialization
    void Start()
    {
        _curWaypointIndex = -1;
        _isGoal = false;
        _countToDelete = 0;

        GetNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGoal)
        {
            ++_countToDelete;

            float amount = ((float)_countToDelete / 50f);

            // 점점 커지게
            //float scale = 1f + amount;
            //this.transform.localScale = new Vector3(scale, scale, scale);

            // 점점 흐려지게
            //MeshRenderer mr = this.GetComponent<MeshRenderer>();
            //Color color = mr.material.color;
            //color.a = 1 - amount;
            //mr.material.SetColor("_Color", color);

            // 그림자 끄기
            //mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            
            if (_countToDelete >= 50)
            {
                Destroy(this.gameObject);
            }

            Animator.SetBool("IsJump", true);
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);

            return;
        }

        MoveToWaypoint();

        if (ArrivedWaypoint())
        {
            U.d("ArrivedWaypoint");
            GetNextWaypoint();

            if (TargetWaypoint == null)
            {
                U.d("Goal");
                _isGoal = true;
            }
        }
    }

    void MoveToWaypoint()
    {
        if (TargetWaypoint == null) return;

        //transform.LookAt(TargetWaypoint);

        Vector3 dir = TargetWaypoint.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, dir, TurnSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);

        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        Animator.SetBool("IsRun", true);

        //U.d(dist, dist.normalized, dist.magnitude);
    }

    bool ArrivedWaypoint()
    {
        return (TargetWaypoint.transform.position - transform.position).magnitude < 0.1;
    }

    void GetNextWaypoint()
    {
        TargetWaypoint = PlayerManager.GetWaypoint(++_curWaypointIndex);
    }



}
