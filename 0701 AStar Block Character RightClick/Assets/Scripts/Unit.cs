using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Animator Animator;
    public GameObject Model;

    void Start()
    {
        Animate(ConstVar.AniType.Idle);
    }

    public void Animate(ConstVar.AniType aniType)
    {
        Model.transform.localPosition = Vector3.zero;

        Animator.SetBool("Attack", false);
        Animator.SetBool("Run", false);
        Animator.SetBool("Idle", false);

        switch (aniType)
        {
            case ConstVar.AniType.Idle:
                //Animator.SetBool("Idle", true);
                break;
            case ConstVar.AniType.Run:
                Animator.SetBool("Run", true);
                break;
            case ConstVar.AniType.Attack:
                Animator.SetBool("Attack", true);
                break;
            default:
                break;
        }
    }
}
