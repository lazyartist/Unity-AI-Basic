using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public Player PlayerPrefab;

    public Transform WaypointContainer;

    private List<Transform> Waypoints = new List<Transform>();

    // Use this for initialization
    void Start () {
        Waypoints.Clear();
        for (int i = 0; i < WaypointContainer.childCount; i++)
        {
            Waypoints.Add(WaypointContainer.GetChild(i));
        }

        StartCoroutine(Coroutine_CreatePlayer());
    }

    IEnumerator Coroutine_CreatePlayer()
    {
        int i = 0;
        while (i++ < 100)
        {
            Player player = GameObject.Instantiate<Player>(PlayerPrefab);
            player.PlayerManager = this;

            player.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0, -6f);
            //player.transform.position = new Vector3(Random.Range(-4.0f, 4.0f), 0, Random.Range(-5f, -2f));

            //yield return new WaitForSeconds(0.2f);
            yield return new WaitForSeconds(Random.Range(.5f, 1.0f));
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    public Transform GetWaypoint(int curIndex)
    {
        if (curIndex >= Waypoints.Count) return null;
        return Waypoints[curIndex];
    }
}
