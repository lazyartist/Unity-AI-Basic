using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour
{
    //public int NodeCount;
    public IntTuple2 NodeGridSize; // x, y
    public float NodeSpace;
    //public FloatTuple2 NodeSpace;

    //public IntTuple2 StartNodePosition;
    //public IntTuple2 EndNodePosition;

    //public IntTuple2[] BlockNodes;
    public List<int> BlockNodePositionIndices;

    public Node NodePrefab;

    public Node StartNode;
    public Node EndNode;

    public int StartNodePositionIndex;
    public int EndNodePositionIndex;

    public Color NodeColor_Normal = new Color(204.0f / 255.0f, 204.0f / 255.0f, 204.0f / 255.0f);
    public Color NodeColor_Start = Color.magenta;
    public Color NodeColor_End = Color.yellow;
    public Color NodeColor_Block = Color.black;

    private List<Node> _nodes = new List<Node>();

    private void Start()
    {
    }

    public void CreateNodes()
    {
        // remove all children
        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }

        _nodes.Clear();

        // Create nodes
        //for (int i = 0; i < NodeCount; i++)
        for (int i = 0; i < NodeGridSize.x * NodeGridSize.y; i++)
        {
            int col = (i % NodeGridSize.x);
            int row = Mathf.FloorToInt(i / NodeGridSize.x);

            Node node = Instantiate<Node>(NodePrefab);
            node.transform.SetParent(this.transform);
            node.transform.position = new Vector3(col * NodeSpace, 0, (row * -NodeSpace));
            //node.transform.position = new Vector3(col * NodeSpace.x, 0, (row * -NodeSpace.y));

            node.name = i.ToString();
            node.PositionIndex = i;

            if (i == StartNodePositionIndex)
            //if (StartNodePosition.x == col && StartNodePosition.y == row)
            {
                node.SetColor(NodeColor_Start);
                StartNode = node;
            }
            else if (i == EndNodePositionIndex)
            //else if (EndNodePosition.x == col && EndNodePosition.y == row)
            {
                node.SetColor(NodeColor_End);
                EndNode = node;
            }
            else
            {
                for (int j = 0; j < BlockNodePositionIndices.Count; j++)
                {
                    int BlockNodePositionIndex = BlockNodePositionIndices[j];
                    if (i == BlockNodePositionIndex)
                    {
                        node.IsBlock = true;
                    }
                }

                if (node.IsBlock)
                {
                    node.SetColor(NodeColor_Block);
                } else
                {
                    Color NodeColor_Normal = new Color(204.0f / 255.0f, 204.0f / 255.0f, 204.0f / 255.0f);

                    node.SetColor(NodeColor_Normal);
                }
                //for (int j = 0; j < BlockNodes.Length; j++)
                //{
                //    IntTuple2 blockNode = BlockNodes[j];
                //    if (blockNode.x == col && blockNode.y == row)
                //    {
                //        node.SetColor(Color.black);
                //        node.IsBlock = true;
                //    }
                //}
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
                if(hit.collider.tag == "Node")
                {
                    Node node = hit.collider.GetComponent<Node>();
                    if (node != null)
                    {
                        node.Click();
                    }
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // right
        {
            // 클릭된 노드를 End 노드로 만들기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Node node = hit.collider.GetComponent<Node>();
                if (node != null)
                {
                    EndNode = node;
                    EndNodePositionIndex = node.PositionIndex;
                }
            }

            EndNode.Click();
            EndNode.SetColor(NodeColor_End);
        }
        else if (Input.GetMouseButtonDown(2)) // middle
        {
            // 클릭된 노드를 Block 노드로 만들기
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Node node = hit.collider.GetComponent<Node>();
                if (node != null)
                {
                    if (node.IsBlock)
                    {
                        node.IsBlock = false;
                        BlockNodePositionIndices.Remove(node.PositionIndex);
                        node.SetColor(NodeColor_Normal);
                    } else
                    {
                        node.IsBlock = true;
                        BlockNodePositionIndices.Add(node.PositionIndex);
                        node.Click();
                        node.SetColor(NodeColor_Block);
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
