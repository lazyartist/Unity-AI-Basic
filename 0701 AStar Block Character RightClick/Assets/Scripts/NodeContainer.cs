using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeContainer : MonoBehaviour
{
    public AStar AStar;

    public IntTuple2 NodeGridSize; // x, y
    public float NodeSpace;

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

    public List<Node> Nodes = new List<Node>();

    private void Start()
    {
        // 정의와 함께 초기화가 안된다. 값이 0, 0, 0으로 들어간다.
        Color NodeColor_Normal = new Color(204.0f / 255.0f, 204.0f / 255.0f, 204.0f / 255.0f);
    }
    
    public void CreateNodes()
    {
        // remove all children
        for (int i = this.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(this.transform.GetChild(i).gameObject);
        }

        Nodes.Clear();

        // Create nodes
        for (int i = 0; i < NodeGridSize.x * NodeGridSize.y; i++)
        {
            int col = (i % NodeGridSize.x);
            int row = Mathf.FloorToInt(i / NodeGridSize.x);

            Node node = Instantiate<Node>(NodePrefab);
            node.transform.SetParent(this.transform);
            node.transform.position = new Vector3(col * NodeSpace, 0, (row * -NodeSpace));

            node.name = i.ToString();
            node.PositionIndex = i;
            Nodes.Add(node);

            // 시작, 끝 노드 
            if (i == StartNodePositionIndex || i == EndNodePositionIndex)
            {
                if (i == StartNodePositionIndex)
                {
                    node.SetColor(NodeColor_Start);
                    StartNode = node;
                }

                if (i == EndNodePositionIndex)
                {
                    node.SetColor(NodeColor_End);
                    EndNode = node;
                }

                continue;
            }

            // 블록 노드 찾기
            for (int j = 0; j < BlockNodePositionIndices.Count; j++)
            {
                int BlockNodePositionIndex = BlockNodePositionIndices[j];
                if (i == BlockNodePositionIndex)
                {
                    node.IsBlock = true;
                }
            }

            // 블록 노드
            if (node.IsBlock)
            {
                node.SetColor(NodeColor_Block);
            }
            // 일반 노드
            else
            {
                node.SetColor(NodeColor_Normal);
            }
        }
    }

    void Update()
    {
        // print info
        if (Input.GetMouseButtonDown(0)) // mouse left
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
        else if (Input.GetMouseButtonDown(2)) // mouse middle
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
