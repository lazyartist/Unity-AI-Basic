using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AStar AStar;
    public NodeContainer NodeContainer;

    public List<Node> Path;
    public Player Player;

    public float Speed = 1;
    public float TurnSpeed = 1;

    private Node _targetNode;
    private int _targetNodeIndex;
    private bool _moving = false;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        _targetNode = null;
        _targetNodeIndex = -1;
        _moving = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // mouse right
        {
            Reset();

            // 클릭된 노드를 끝 노드로 지정
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Node node = hit.collider.GetComponent<Node>();
                if (node != null && !node.IsBlock)
                {
                    NodeContainer.EndNodePositionIndex = node.PositionIndex;
                    break;
                }
            }

            // 캐릭터에서 가장 가까운 노드를 시작 노드로 지정
            Node startNode = NodeContainer.StartNode;
            Collider[] colliders = Physics.OverlapSphere(Player.transform.position, 1.5f);
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                if (collider.gameObject.tag == "Node")
                {
                    Node node = collider.gameObject.GetComponent<Node>();
                    if (node != null)
                    {
                        if (startNode == null)
                        {
                            startNode = node;
                        }
                        else if (
                            node.IsBlock == false &&
                          (Vector3.Distance(Player.transform.position, startNode.transform.position) >
                          Vector3.Distance(Player.transform.position, node.transform.position))
                          )
                        {
                            startNode = node;
                        }
                    }
                }
            }
            NodeContainer.StartNodePositionIndex = startNode.PositionIndex;

            // 맵 재구성
            NodeContainer.CreateNodes();

            // 경로 탐색
            AStar.Init();
            AStar.AutoStartToSearch = true;
        }

        if (_moving)
        {
            // 방향
            Player.transform.LookAt(_targetNode.transform.localPosition + new Vector3(0, this.transform.localPosition.y, 0));
            //Vector3 dir = _targetNode.transform.localPosition - Player.transform.localPosition;
            //Vector3 newDir = Vector3.RotateTowards(Vector3.forward, dir, TurnSpeed, 0.0f);
            //Player.transform.rotation = Quaternion.LookRotation(newDir);

            Player.transform.Translate(Vector3.forward * Speed * Time.deltaTime);

            if (Vector3.Distance(Player.transform.localPosition, _targetNode.transform.localPosition) < 0.3f)
            {
                if (GetNextNode() == false)
                {
                    // 목적지 도착
                    Reset();
                    Player.Animate(ConstVar.AniType.Attack);
                    U.d("Move Complete!!!");
                }
            }
        }
    }

    public void Move()
    {
        Player.Animate(ConstVar.AniType.Run);

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
