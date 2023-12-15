using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeMover2 : MonoBehaviour
{
    public Transform com;
    public float maxSpeed;
    public float engineForce;
    public float sideFrictionForce;
    public float steerTorque;
    public float tiltCorrectionTorque;
    public float tiltCorrectionDampingTorque;

    [Range(-1, 1)]
    public float targetTilt;
    public float maxTilt;
    public GameObject targetTiltVisual;
    public float tiltAngleDiff;
    float tiltAngleDiffPrevFrame;
    bool firstUpdate;
    public Transform uprightLocator;
    public Transform uprightLocatorHolder;
    Rigidbody rb;
    float angVelZlastFrame = 0;
    float tiltVelocity;

    [Header("InputRelated")]
    public JoystickOnUiShawon joystick;

    public DetectCollisionWithALayer[] detectCollisions;
    float forceApplyFraction;


    [Header("TiltCorrectionRelated")]
    public GameObject globalLocatorHolder;
    public GameObject globalLocator;
    public GameObject localLocatorHolder;
    public GameObject localLocator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
        //float uprightAngleDiff = Quaternion. -Vector3.SignedAngle(Vector3.up, transform, transform.forward);
        //Vector3 rotV = transform.(rot);
        //Vector3 localRot = Quaternion.ToEulerAngles(uprightLocator.transform.localRotation) * Mathf.Rad2Deg;
        //uprightLocator.transform.localRotation = Quaternion.Euler(Mathf.Clamp(-rot.z, -80, 80), Mathf.Clamp(-rot.z, -80, 80), Mathf.Clamp(-rot.z,  -80,80));

        //uprightLocator.transform.rotation = Quaternion.Euler(Mathf.Clamp(rot.x, -70, 70), (rot.y), 0);
        //uprightLocator.transform.rotation = Quaternion.Euler(rot.x,  (rot.y),0);
        //Debug.Log("rot= " + rotV);

        // uprightLocator.transform.rotation = transform.rotation;
        //uprightLocator.transform.RotateAround(uprightLocator.transform.position, uprightLocator.transform.forward, -angle);

    }


    private void FixedUpdate()
    {

        targetTilt = joystick.GetJoystickOutputValues().x;
        globalLocatorHolder.transform.rotation = Quaternion.identity;
        Vector3 offsetFromGlobalRot = localLocator.transform.InverseTransformPoint(globalLocator.transform.position);

        float angleToCorrectInLocalZ = Mathf.Asin(offsetFromGlobalRot.x / 1) * Mathf.Rad2Deg;
        Debug.Log("off= " + offsetFromGlobalRot + " angle " + angleToCorrectInLocalZ);

        uprightLocator.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Clamp(-angleToCorrectInLocalZ, -60, 60) );



        int wheelsCollided = 0;
        for (int i = 0; i < detectCollisions.Length; i++)
        {
            if (detectCollisions[i].colliding)
            {
                wheelsCollided += 1;
            }
        }
        
        if (wheelsCollided == 0)
        {
            forceApplyFraction = Mathf.MoveTowards(forceApplyFraction, 0, Time.fixedDeltaTime * 2f);
        }
        else
        {
            forceApplyFraction = Mathf.MoveTowards(forceApplyFraction, 1, Time.fixedDeltaTime * 5f);
        }

        KeepUpright();
        BikeMove();
    }
    void KeepUpright()
    {
        Vector3 refDir = transform.up + (-transform.right * Mathf.Sin(targetTilt * maxTilt * forceApplyFraction * Mathf.Deg2Rad));
        
        tiltAngleDiff = -Vector3.SignedAngle(refDir, uprightLocator.transform.up, transform.forward);
        //Debug.Log("tiltDiff = "+tiltAngleDiff);
        //rb.AddTorque(-transform.forward * tiltAngleDiff * tiltCorrectionTorque, ForceMode.Force);



        //Vector3 angVelocityLocall = transform.InverseTransformVector(rb.angularVelocity);
        if (!firstUpdate)
        {
            firstUpdate = true;
            tiltAngleDiffPrevFrame = tiltAngleDiff;
            //angVelZlastFrame = angVelocityLocall.z;
        }
        tiltVelocity = Mathf.Lerp(tiltVelocity, tiltAngleDiff - tiltAngleDiffPrevFrame, Time.fixedDeltaTime * 20);
        rb.AddTorque(-transform.forward * ((tiltVelocity * tiltCorrectionDampingTorque) + (tiltAngleDiff * tiltCorrectionTorque))* forceApplyFraction, ForceMode.Force);

        tiltAngleDiffPrevFrame = tiltAngleDiff;


        
        //angVelZlastFrame = angVelocityLocall.z;
    }

    void BikeMove()
    {
        float engineOn = 0;
        
        Vector3 velocityLocal = uprightLocator. transform.InverseTransformVector(rb.velocity);
        if (Input.GetMouseButton(0))
        {
            engineOn = 1;
            

        }
        else
        {
            engineOn = 0;
            rb.AddForce(uprightLocator.transform.forward * velocityLocal.z* -200f);
            //rb.AddTorque(uprightLocator.transform.right * 500);

        }
        if (velocityLocal.z < maxSpeed)
        {
            rb.AddForce(uprightLocator.transform.forward * engineForce* engineOn* forceApplyFraction);
        }
        rb.AddForce(-uprightLocator.transform.right* velocityLocal.x * sideFrictionForce* forceApplyFraction);

        Vector3 angVelocityLocal = uprightLocator.transform.InverseTransformVector(rb.angularVelocity);
        float targetAngVelY = joystick.GetJoystickOutputValues().x * 1f;
        rb.AddTorque(uprightLocator.transform.up * (targetAngVelY - angVelocityLocal.y) * steerTorque * forceApplyFraction);
        //velocityLocal.x = velocityLocal.x * 0.1f ;
        //rb.velocity = transform.forward * velocityLocal.z + transform.up * velocityLocal.y + transform.right * velocityLocal.x;
    }
}
