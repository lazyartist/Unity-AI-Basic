using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public Renderer Renderer;

    public bool Closed = false;
    public bool IsBlock = false;

    public float fitness; // 적절성 : goal + heuristic
    public float goal; // 목표 : 시작에서 현재까지 거리
    public float heuristic; // 추정치 : 종료에서 현재까지  거리

    private Color _color;

    private void Start()
    {
        _color = Renderer.material.GetColor("_Color");
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

}
