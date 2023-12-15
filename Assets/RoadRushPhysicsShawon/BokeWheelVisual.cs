using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BokeWheelVisual : MonoBehaviour
{
    public Transform refWheel;
    public Transform visualWheel;
    public bool isSteer;

    public Transform steerHolder;
    public Transform wheelRotor;
    public Rigidbody rb;
    JoystickOnUiShawon joystick;
    // Start is called before the first frame update
    void Start()
    {
        joystick = FindObjectOfType<JoystickOnUiShawon>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float velForward = transform.InverseTransformVector(rb.velocity).z;

        wheelRotor.transform.Rotate(velForward * Time.deltaTime*60f, 0, 0);
        visualWheel.transform.localPosition = refWheel.transform.localPosition;

        if (isSteer)
        {
            if (joystick)
            {

                steerHolder.transform.localRotation = Quaternion.Euler(0,30* joystick.GetJoystickOutputValues().x,0);
            }
        }
    }
}
