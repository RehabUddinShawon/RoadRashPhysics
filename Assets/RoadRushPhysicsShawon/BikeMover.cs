using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMover : MonoBehaviour
{

    public Transform com;
    public float maxSpeed;
    public float engineForce;
    public float steerTorque;
    public float tiltCorrectionTorque;
    public float tiltCorrectionDampingTorque;

    [Header("TiltRelated")]
    
    public GameObject raycastingHolder;
    public RaycastFromToShawon leftRaycaster;
    public RaycastFromToShawon rightRaycaster;
    float angVelZlastFrame = 0;
    float tiltVelocity;

   [Range(-1,1)]
    public float targetTilt;
    public float maxTilt;
    public GameObject targetTiltVisual;
    public float tiltAngleDiff;
    float tiltAngleDiffPrevFrame;
    bool firstUpdate;
    Rigidbody rb;

    [Header("InputRelated")]
    public JoystickOnUiShawon joystick;


    public DetectCollisionWithALayer[] detectCollisions;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = Quaternion.ToEulerAngles(transform.rotation) * Mathf.Rad2Deg;
        raycastingHolder.transform.localRotation = Quaternion.Euler(0, 0, -rot.z);
    }
    void FixedUpdate()
    {
        int wheelsCollided = 0;
        for (int i = 0; i < detectCollisions.Length; i++)
        {
            if (detectCollisions[i].colliding)
            {
                wheelsCollided += 1;
            }
        }
        float forceApplyFraction = (float)wheelsCollided / (float)detectCollisions.Length;


        rb.centerOfMass = com.localPosition;

        targetTilt = joystick.GetJoystickOutputValues().x;
       
        //if (leftRaycaster.isHitting && rightRaycaster.isHitting)
        //{
        //    
        //
        //    Vector3 refDir = transform.right + (transform.up * Mathf.Sin( targetTilt*maxTilt * Mathf.Deg2Rad));
        //    targetTiltVisual.transform.position = refDir;
        //    tiltAngleDiff = Vector3.SignedAngle((rightRaycaster.hitPoint - leftRaycaster.hitPoint), refDir, transform.forward) ;
        //    //rb.AddTorque(-transform.forward * tiltAngleDiff * tiltCorrectionTorque, ForceMode.Force);
        //
        //
        //
        //    Vector3 angVelocityLocall = transform.InverseTransformVector(rb.angularVelocity);
        //    if (!firstUpdate)
        //    {
        //        firstUpdate = true;
        //        tiltAngleDiffPrevFrame = tiltAngleDiff;
        //        angVelZlastFrame = angVelocityLocall.z;
        //    }
        //    tiltVelocity =Mathf.Lerp(tiltVelocity,  tiltAngleDiff - tiltAngleDiffPrevFrame,Time.fixedDeltaTime*20);
        //    rb.AddTorque(-transform.forward * ((tiltVelocity * tiltCorrectionDampingTorque)+(tiltAngleDiff * tiltCorrectionTorque)), ForceMode.Force);
        //
        //    tiltAngleDiffPrevFrame = tiltAngleDiff;
        //
        //    
        //    //rb.AddTorque(-transform.forward * (angVelocityLocall.z- angVelZlastFrame)* tiltCorrectionDampingTorque, ForceMode.Force);
        //    angVelZlastFrame = angVelocityLocall.z;
        //}
        if (wheelsCollided < detectCollisions.Length)
        {
            return;
        }
        else
        {

        }




        Vector3 velocityLocal = transform.InverseTransformVector(rb.velocity);
        if (velocityLocal.z < maxSpeed)
        {
            rb.AddForce(transform.forward * engineForce* forceApplyFraction);
        }
        velocityLocal.x = velocityLocal.x* 0.1f * (1 - forceApplyFraction);
        rb.velocity = transform.forward * velocityLocal.z + transform.up * velocityLocal.y + transform.right* velocityLocal.x;
        
        
        Vector3 angVelocityLocal = transform.InverseTransformVector(rb.angularVelocity);
        float targetAngVelY = joystick.GetJoystickOutputValues().x*2f;
        rb.AddTorque(transform.up * (targetAngVelY- angVelocityLocal.y) *steerTorque* forceApplyFraction);
        //angVelocityLocal.y = angVelocityLocal.y*0.1f;
        
        //rb.angularVelocity= transform.forward * angVelocityLocal.z + transform.right * angVelocityLocal.x + transform.up* angVelocityLocal.y;
       

    }
}
