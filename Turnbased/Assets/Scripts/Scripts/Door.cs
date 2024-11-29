using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator animator;
    public bool IsOpen;
    public GameObject key;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        IsOpen = false;
        animator.SetBool("IsOpen", IsOpen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (key.activeInHierarchy == true)
        {
            IsOpen = !IsOpen;
            animator.SetBool("IsOpen", IsOpen);
        }
    }
}
