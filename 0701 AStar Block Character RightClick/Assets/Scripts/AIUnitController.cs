using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitController : MonoBehaviour {
    public AStar AStar;
    public NodeContainer NodeContainer;

    public List<Node> Path;
    public Unit AIUnit;

    public float Speed = 1;
    public float TurnSpeed = 1;

    private Node _targetNode;
    private int _targetNodeIndex;
    private bool _moving = false;

    void Start()
    {
        NodeContainer.CreateNodes();

        Reset();
    }

    public void Reset()
    {
        _targetNode = null;
        _targetNodeIndex = -1;
        _moving = false;

        SearchPath();
    }

    void Update()
    {
        if (_moving)
        {
            // 방향
            AIUnit.transform.LookAt(_targetNode.transform.localPosition + new Vector3(0, this.transform.localPosition.y, 0));
            AIUnit.transform.Translate(Vector3.forward * Speed * Time.deltaTime);

            if (Vector3.Distance(AIUnit.transform.localPosition, _targetNode.transform.localPosition) < 0.3f)
            {
                if (GetNextNode() == false)
                {
                    // 목적지 도착
                    Reset();
                    AIUnit.Animate(ConstVar.AniType.Attack);
                    U.d("Move Complete!!!");
                }
            }
        }
    }

    public void SearchPath()
    {
        // 이동 가능한 노드중 랜덤하게 하나를 골라 끝 노드로 설정
        Node endNode = null;
        while (endNode == null)
        {
            int nodeCount = NodeContainer.Nodes.Count;
            int randomNodeIndex = Random.Range(0, nodeCount);

            Node tmpNode = NodeContainer.Nodes[randomNodeIndex];
            if (!tmpNode.IsBlock && NodeContainer.EndNode != tmpNode)
            {
                endNode = tmpNode;
            }
        }
        NodeContainer.EndNodePositionIndex = endNode.PositionIndex;
        //NodeContainer.EndNode = endNode;


        // 캐릭터에서 가장 가까운 노드를 시작 노드로 지정
        Node startNode = NodeContainer.StartNode;
        Collider[] colliders = Physics.OverlapSphere(AIUnit.transform.position, 1.5f);
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
                      (Vector3.Distance(AIUnit.transform.position, startNode.transform.position) >
                      Vector3.Distance(AIUnit.transform.position, node.transform.position))
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

    public void Move()
    {
        AIUnit.Animate(ConstVar.AniType.Run);

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



