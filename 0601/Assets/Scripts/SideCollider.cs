using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideCollider : MonoBehaviour {
    public bool IsOnBlock = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Const.tn_BlackWall)
        {
            IsOnBlock = true;
        } else
        {
            IsOnBlock = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == Const.tn_BlackWall)
        {
            IsOnBlock = true;
        }
        else
        {
            IsOnBlock = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Const.tn_BlackWall)
        {
            IsOnBlock = false;
        }
    }
}
