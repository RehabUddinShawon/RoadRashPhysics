using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class RaycastFromToShawon : MonoBehaviour
{
    public bool runOnUnpdateInstead;
    public Transform fromPoint;
    public Transform toPoint;
    public float range;
    public float hitDistance;
    public LayerMask layerMask;
    //public UnityEvent eventOnRaycastHit;
    public Action<GameObject> actionOnRaycastHit;
    public GameObject hitObject;
    public Vector3 hitPoint;
    public bool isHitting;
    public bool visualShow;
    public GameObject visual;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(runOnUnpdateInstead)
        {
            hitObject = RaycastThrow();
        }
    }
    void FixedUpdate()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        //int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        //layerMask = ~layerMask;

        if (!runOnUnpdateInstead)
        {
            hitObject = RaycastThrow();
        }


    }
    public GameObject RaycastThrow()
    {
        
        GameObject hitObj = null;
        Vector3 fromPos = Vector3.zero;
        Vector3 direction = Vector3.zero;
        float _range = 0;

        fromPos = fromPoint.position;
        direction = toPoint.position - fromPoint.position;
        _range = range;



        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(fromPos, direction, out hit, _range, layerMask))
        {
            Debug.DrawRay(fromPos, direction * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            isHitting = true;
            hitDistance = hit.distance;
            hitObj = hit.transform.gameObject;
            actionOnRaycastHit?.Invoke(hitObj);
            hitPoint = hit.point;
            if(visualShow && visual!= null)
            {
                visual.SetActive(true);
                visual.transform.position = hitPoint;
            }
            //eventOnRaycastHit?.Invoke();
        }
        else
        {
            Debug.DrawRay(fromPos, direction * 100, Color.white);
            //Debug.Log("Did not Hit");
            isHitting = false;
            hitDistance = 0;
            hitObj = null;

            if (visual != null)
            {
                visual.SetActive(false);
            }
        }

        return hitObj;
    }
}
