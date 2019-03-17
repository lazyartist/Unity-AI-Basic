using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public List<Node> Path;
    public GameObject Player;

    public float Speed = 1;
    public float TurnSpeed = 1;

    private Node _targetNode;
    private int _targetNodeIndex;
    private bool _moving = false;

	void Start () {
        _targetNode = null;
        _targetNodeIndex = -1;
    }
	
	void Update () {
        if (_moving)
        {
            // 방향
            Player.transform.LookAt(_targetNode.transform.localPosition + new Vector3(0, this.transform.localPosition.y, 0));
            //Vector3 dir = _targetNode.transform.localPosition - Player.transform.localPosition;
            //Vector3 newDir = Vector3.RotateTowards(Vector3.forward, dir, TurnSpeed, 0.0f);
            //Player.transform.rotation = Quaternion.LookRotation(newDir);

            Player.transform.Translate(Vector3.forward * Speed * Time.deltaTime);

            if(Vector3.Distance(Player.transform.localPosition, _targetNode.transform.localPosition) < 0.1f)
            {
                if(GetNextNode() == false)
                {
                    // 목적지 도착
                    _moving = false;
                    Player.GetComponentInChildren<Animator>().SetBool("Attack", true);
                    Player.GetComponentInChildren<Animator>().SetBool("Run", false);
                    U.d("Move Complete!!!");
                }
            }
        }
    }

    public void Move()
    {
        Player.transform.localPosition = new Vector3(-3f, 0, 0);
        Player.GetComponentInChildren<Animator>().SetBool("Attack", false);
        Player.GetComponentInChildren<Animator>().SetBool("Run", true);

        _moving = true;
        _targetNodeIndex = -1;
        GetNextNode();
    }

    bool GetNextNode()
    {
        ++_targetNodeIndex;
        if (_targetNodeIndex >= Path.Count)
        {
            return false;
        }

        _targetNode = Path[_targetNodeIndex];
        return true;
    }
}
