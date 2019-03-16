using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour
{
    public IntTuple2 NodeGridSize;
    public FloatTuple2 NodeSpace;

    public IntTuple2 StartNodePosition;
    public IntTuple2 EndNodePosition;

    public Node BlockPrefab;

    public Node StartNode;
    public Node EndNode;

    private List<Node> _nodes = new List<Node>();

    private void Start()
    {
    }

    public void CreateNodes()
    {
        _nodes.Clear();

        // Create nodes
        for (int i = 0; i < NodeGridSize.x * NodeGridSize.y; i++)
        {
            int col = (i % NodeGridSize.x);
            int row = Mathf.FloorToInt(i / NodeGridSize.x);

            Node node = Instantiate<Node>(BlockPrefab);
            node.transform.SetParent(this.transform);
            node.transform.position = new Vector3(col * NodeSpace.x, 0, (row * -NodeSpace.y));

            node.name = i.ToString();
            node.Closed = false;

            if (StartNodePosition.x == col && StartNodePosition.y == row)
            {
                node.SetColor(Color.magenta);
                StartNode = node;
            }
            else if (EndNodePosition.x == col && EndNodePosition.y == row)
            {
                node.SetColor(Color.yellow);
                EndNode = node;
            }

            _nodes.Add(node);
        }
    }

    void Update()
    {
        // print info
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if(hit.collider.tag == "Block")
                {
                    Node node = hit.collider.GetComponent<Node>();
                    if (node != null)
                    {
                        node.Click();
                    }
                }
            }
        }
    }

    public Collider[] GetAroundBlocks(Node node)
    {
        Collider[] aroundColliders = Physics.OverlapSphere(node.transform.position, 1.5f);
        return aroundColliders;
    }
}
