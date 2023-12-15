using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionWithALayer : MonoBehaviour
{
    public bool colliding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }

    // Gets called during the collision
    void OnCollisionStay(Collision collision)
    {
        colliding = true;
    }

    // Gets called when the object exits the collision
    void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }
}
