using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    const float moveSpeed = 3f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = transform.position + (Vector3.up * Time.deltaTime * moveSpeed);
        transform.Translate((Vector3.up) * Time.deltaTime * moveSpeed); // 위와 같은 코드
    }

    //void Move() {
    //    transform.position 
    //}

    void RotateRight()
    {
        var a = transform.rotation.eulerAngles.z + 90;
        Quaternion.Euler(new Vector3(0, 0, a));
    }
}
