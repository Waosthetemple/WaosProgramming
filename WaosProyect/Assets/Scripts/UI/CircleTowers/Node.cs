using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Node : MonoBehaviour
{
    private Animator anim;
    private bool isSelected = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            TowerRequestManager.instance.OnOpenRequestPanel();
        }
        else
        {
            TowerRequestManager.instance.OnCloseRequestPanel();
        }
        anim.SetBool("IsSelected", isSelected);
    }
}
