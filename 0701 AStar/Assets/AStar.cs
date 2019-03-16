using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    public NodeContainer NodeContainer;

    public bool AutoStartToSearch = false;

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

        curNode.Closed = true;

        Collider[] colliders = NodeContainer.GetAroundBlocks(curNode);

        if (colliders.Length == 0) return;

        Node minFitnessNode = colliders[0].GetComponent<Node>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Node node = colliders[i].gameObject.GetComponent<Node>();
            if (node != null)
            {
                if (curNode == node || node.Closed == true) continue;

                var disFromCur = Vector3.Distance(curNode.transform.position, node.transform.position);
                var disToEnd = Vector3.Distance(NodeContainer.EndNode.transform.position, node.transform.position);

                node.goal = curNode.fitness + disFromCur;
                node.heuristic = disToEnd;
                node.fitness = node.goal + node.heuristic;

                if (minFitnessNode.fitness > node.fitness)
                {
                    minFitnessNode = node;
                }

                node.SetColor(Color.gray);
            }
        }

        minFitnessNode.Closed = true;
        minFitnessNode.SetColor(Color.blue);
        _curNode = minFitnessNode;

        if (_curNode == NodeContainer.EndNode)
        {
            _curNode.SetColor(Color.green);
            _curNode = null;
            return;
        }
    }
}
