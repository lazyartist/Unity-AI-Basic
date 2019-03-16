using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Renderer Renderer;
    public Node ParentNode;
    public GameObject Arrow;

    public bool IsBlock = false;

    public float fitness; // 적절성 : goal + heuristic
    public float goal; // 목표 : 시작에서 현재까지 거리
    public float heuristic; // 추정치 : 종료에서 현재까지  거리

    private Color _color;

    private void Start()
    {
        _color = Renderer.material.GetColor("_Color");
        Arrow.SetActive(false);
    }

    public void SetColor(Color color)
    {
        _color = color;
        Renderer.material.SetColor("_Color", color);
    }

    public void Click()
    {
        U.d(name, fitness, goal, heuristic);
        StartCoroutine(Coroutine_Color());
    }

    IEnumerator Coroutine_Color()
    {
        Renderer.material.SetColor("_Color", Color.red);

        yield return new WaitForSeconds(1.0f);

        Renderer.material.SetColor("_Color", _color);
    }

    public void SetParent(Node parentNode)
    {
        ParentNode = parentNode;
        Arrow.SetActive(true);

        Vector3 dis = ParentNode.transform.position - this.transform.position;
        float radian = Mathf.Atan2(dis.z, dis.x);
        //float radian = Mathf.Acos(dis.x / dis.magnitude);
        //float radian = dis.x / dis.magnitude;
        float degree = radian * Mathf.Rad2Deg * -1;

        Arrow.transform.rotation = Quaternion.Euler(new Vector3(0, degree, 0));
    }

}
