using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public NodeContainer NodeContainer;

    public bool AutoStartToSearch = false;

    public List<Node> Path = new List<Node>();

    private List<Node> _openNodes = new List<Node>();
    private List<Node> _closedNodes = new List<Node>();

    private Node _curNode;

    private void Start()
    {
        NodeContainer.CreateNodes();

        _curNode = NodeContainer.StartNode;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == "Next" && _curNode != null)
                {
                    SearchPath(_curNode);
                }
                else if (hit.collider.tag == "Auto" && _curNode != null)
                {
                    AutoStartToSearch = true;
                }
            }
        }

        if (AutoStartToSearch && _curNode != null)
        {
            SearchPath(_curNode);
        }
    }

    void SearchPath(Node curNode)
    {
        U.d("SearchPath", curNode);

        _openNodes.Remove(_curNode);
        _closedNodes.Add(_curNode);

        // 주변 노드를 찾아온다.
        Collider[] colliders = NodeContainer.GetAroundBlocks(curNode);
        if (colliders.Length == 0) return;

        for (int i = 0; i < colliders.Length; i++)
        {
            Node node = colliders[i].gameObject.GetComponent<Node>();
            if (node != null)
            {
                // 현재 노드, 블록이라면 무시
                if (curNode == node || node.IsBlock)
                {
                    continue;
                }

                // 이미 탐색 완료된 노드라면 무시
                bool isClosedNode = _closedNodes.Any<Node>((eleNode) => node == eleNode);
                if (isClosedNode)
                {
                    continue;
                }

                // 이미 오픈리스트에 있는 노드라면
                bool isOpenNode = _openNodes.Any<Node>((eleNode) => node == eleNode);
                if (isOpenNode)
                {
                    // 누적 경로 비용을 현재 노드를 부모라 가정하고 다시 계산해서
                    // 비용이 더 낮다면 부모를 현재 노드로 변경하고 비용을 전부 갱신
                    var disFromCur = Vector3.Distance(curNode.transform.position, node.transform.position);
                    var disToEnd = Vector3.Distance(NodeContainer.EndNode.transform.position, node.transform.position);

                    if (node.goal > curNode.goal + disFromCur)
                    {
                        node.goal = curNode.goal + disFromCur;
                        node.heuristic = disToEnd;
                        node.fitness = node.goal + node.heuristic;

                        node.SetParent(curNode);
                    }

                    continue;
                }

                // 처음 발견한 노드라면 
                {
                    // 비용 계산
                    var disFromCur = Vector3.Distance(curNode.transform.position, node.transform.position);
                    var disToEnd = Vector3.Distance(NodeContainer.EndNode.transform.position, node.transform.position);

                    node.goal = curNode.goal + disFromCur;
                    node.heuristic = disToEnd;
                    node.fitness = node.goal + node.heuristic;

                    // 부모 설정
                    node.SetParent(curNode);
                    // 오픈 리스트에 추가
                    _openNodes.Add(node);

                    node.SetColor(Color.gray);
                }
            }
        }

        // 오픈 리스트에서 최적화 노드 찾기
        Node fitnessNode = null;
        foreach (Node node in _openNodes)
        {
            if (fitnessNode == null)
            {
                fitnessNode = node;
            }
            else if (fitnessNode.fitness > node.fitness)
            {
                fitnessNode = node;
            }
        }

        if (fitnessNode == curNode)
        {
            // 오픈 노드가 없다. 종료
            return;
        }

        fitnessNode.SetColor(Color.blue);
        _curNode = fitnessNode;

        if (_curNode == NodeContainer.EndNode)
        {
            // 목표 지점에 도착했다.
            _curNode.SetColor(Color.green);
            _curNode = null;

            // 목표지점에서 역으로 부모노드를 찾아가며 최종 경로 저장
            SavePath();
            return;
        }
    }

    void SavePath()
    {
        Path.Clear();

        Node node = NodeContainer.EndNode;
        while (node != NodeContainer.StartNode)
        {
            Path.Insert(0, node);

            if(node != NodeContainer.EndNode && node != NodeContainer.StartNode)
            {
                node.SetColor(Color.yellow);
            }

            node = node.ParentNode;
        }

        U.d("Fitness Path Found!!!");
    }
}
