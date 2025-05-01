using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public float detectRange = 8f;
    public float detectMelee = 4f;

    public Vector3 raycastForward = Vector3.forward;
    public Vector3 raycastBackwards = Vector3.back;
    public Vector3 raycastLeft = Vector3.left;
    public Vector3 raycastRight = Vector3.right;
    //Can Attack
    public bool canAttack = false;

    [Header("Objects that the enemy sees (hits)")]
    public GameObject front;
    public GameObject left;
    public GameObject right;
    public GameObject back;


    public float playerDist;

    public bool ForwardRay()
    {
        // Cast a ray from the player's position
        Ray Forward = new Ray(transform.position, transform.TransformDirection(raycastForward));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(raycastForward) * detectMelee, Color.blue);
        // Perform the raycast
        if (Physics.Raycast(Forward, out hit, detectMelee))
        {
            front = hit.collider.gameObject;
        }
        else
        {
            front = null;
        }

        // Perform the raycast
        if (Physics.Raycast(Forward, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(raycastForward) * detectRange, Color.blue);

                playerDist = hit.distance;
                Debug.Log("Player detected: Can Attack");
                return true;
            }
            else
            {
                playerDist = 0;
                return false;
            }
        }
        else
        {
            playerDist = 0;
            return false;
        }
    }
    public bool BackwardRay()
    {
        // Cast a ray from the player's position
        Ray Backward = new Ray(transform.position, transform.TransformDirection(raycastBackwards));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(raycastBackwards) * detectMelee, Color.green);
        // Perform the raycast
        if (Physics.Raycast(Backward, out hit, detectMelee))
        {
            back = hit.collider.gameObject;
        }
        else
        {
            back = null;
        }

        // Perform the raycast
        if (Physics.Raycast(Backward, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(raycastBackwards) * detectRange, Color.green);

                Debug.Log("Player detected Behind");
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool LeftRay()
    {
        // Cast a ray from the player's position
        Ray Left = new Ray(transform.position, transform.TransformDirection(raycastLeft));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(raycastLeft) * detectMelee, Color.yellow);
        // Perform the raycast
        if (Physics.Raycast(Left, out hit, detectMelee))
        {
            left = hit.collider.gameObject;
        }
        else
        {
            left = null;
        }
        // Perform the raycast
        if (Physics.Raycast(Left, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(raycastLeft) * detectRange, Color.yellow);

                Debug.Log("Player detected Left");
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    public bool RightRay()
    {
        // Cast a ray from the player's position
        Ray Right = new Ray(transform.position, transform.TransformDirection(raycastRight));
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(raycastRight) * detectMelee, Color.red);
        // Perform the raycast
        if (Physics.Raycast(Right, out hit, detectMelee))
        {
            right = hit.collider.gameObject;
        }
        else
        {
            right = null;
        }
        // Perform the raycast
        if (Physics.Raycast(Right, out hit, detectRange))
        {
            // Check if the object hit is tagged as "Player"
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(raycastRight) * detectRange, Color.red);

                Debug.Log("Player detected Right");
                return true;
            }
            else
            {

                return false;
            }
        }
        return false;
    }
}