using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TowerRequestManager : MonoBehaviour
{
    private Animator anim;
    public static TowerRequestManager instance;
    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(instance);
            anim = GetComponent<Animator>();
    }
    public void OnOpenRequestPanel()
    {
        anim.SetBool("IsOpen", true);
    }
    public void OnCloseRequestPanel()
    {
        anim.SetBool("IsOpen", false);
    }
}
