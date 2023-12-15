using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using ShawonGameTools;

public class JoystickOnUiShawon : MonoBehaviour
{
    public Image joystickBase;
    public Image joysticKnob;
    public Vector3 initialPos;
    public Vector3 currentPos;
    [SerializeField]
    bool joystickEnabled;
    
    public bool joystickMasterDisabled;

    public float boundaryLimit;
    public float sensitivity = 1;
    [SerializeField]
    private Vector2 output;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float outputMul = 1;
    public Action actionOnJoystickEnable;
    public Action actionOnJoystickDisable;
    public Action actionOnJoystickMouseIn;
    public Action<Vector2> actionOnJoystickUpdate;
    public bool followTouchDrag;
    [Header("SnapRelated")]
    [SerializeField]
    float snapToZeroSpeed = 1;
    float snapToZeroTimer;
    float snapToZeroDuration = 1;
    bool snappingToZero;
    Vector3 snapInitialPos;



    //Vector2 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        DisableJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            MasterDisableJoystick();
            
        }
        if (Input.GetMouseButtonDown(0))
        {
            JoystickInitialization(Input.mousePosition);
            EnableJoystick();
            
        }
        
        if (joystickEnabled)
        {
            currentPos = Input.mousePosition;
            joysticKnob.transform.position = currentPos;
            velocity = (((currentPos-initialPos) / Screen.height) * 50 * sensitivity) / Time.deltaTime;
            initialPos = currentPos;
        
            if(Vector3.Magnitude(joysticKnob.transform.localPosition) >= (boundaryLimit))
            {
                Vector3 increment = joysticKnob.transform.localPosition - joysticKnob.transform.localPosition.normalized;
                joysticKnob.transform.localPosition = joysticKnob.transform.localPosition.normalized * boundaryLimit;
        
                
                joystickBase.transform.localPosition += (increment/Screen.height)*50*sensitivity;
            }
            
        }
        if(snappingToZero)
        {
            //snapToZeroTimer += Time.deltaTime;
            //if(snapToZeroTimer >= snapToZeroDuration)
            //{
            //    snapToZeroTimer = snapToZeroDuration;
            //}
            //joysticKnob.transform.localPosition = Vector3.Lerp(snapInitialPos, Vector3.zero, snapToZeroTimer/snapToZeroDuration);
            joysticKnob.transform.localPosition =
                Vector3.MoveTowards(joysticKnob.transform.localPosition, Vector3.zero, Time.deltaTime*Screen.height*0.5f* snapToZeroSpeed);

            if (joysticKnob.transform.localPosition == Vector3.zero)
            {
                DisableJoystick();
            }
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            StartToSnapToZeroPos();
        }
        GetJoystickOutputValues();
    }

    public void EnableJoystick()
    {

        joystickEnabled = true;
        snappingToZero = false;
        joystickBase.gameObject.SetActive(true);
        actionOnJoystickEnable?.Invoke();
    }
    public void StartToSnapToZeroPos()
    {
        joystickEnabled = false;
        snappingToZero = true;
        snapToZeroTimer = 0;
        snapInitialPos = joysticKnob.transform.localPosition;
    }
    public void DisableJoystick()
    {

        joystickEnabled = false;
        snappingToZero = false;
        joystickBase.gameObject.SetActive(false);
        ResetJoystick();
        actionOnJoystickDisable?.Invoke();
    }
    public void ResetOnSpotScreenTapPos()
    {
        
        currentPos = Input.mousePosition;
        initialPos = currentPos;
        joystickBase.transform.position = currentPos;
       joysticKnob.transform.position = currentPos;
        velocity = (((currentPos - initialPos) / Screen.height) * 50 * sensitivity) / Time.deltaTime;
        initialPos = currentPos;

       
    }
    void JoystickInitialization(Vector2 mousepos)
    {
        initialPos = mousepos;
        currentPos = initialPos;
        joystickBase.transform.position = initialPos;
        //boundaryLimit = joystickBase.GetCompone
    }
    public Vector2 GetJoystickOutputValues()
    {
        if (joysticKnob.transform == null) Debug.Log(this.gameObject.name);
        output = (joysticKnob.transform.localPosition / (boundaryLimit))*outputMul;
        output.x = Mathf.Clamp(output.x, -1, 1);
        output.y = Mathf.Clamp(output.y, -1, 1);
        return output;
    }
    public Vector2 GetJoystickVelocity()
    {
        
        return velocity;
    }
    public void ResetJoystick()
    {
       
        joysticKnob.transform.localPosition = Vector3.zero;
        initialPos = Vector3.zero;
        currentPos = Vector3.zero;
        output = Vector2.zero;
    }
    public bool JoystickEnabledOrNot()
    {
        return joystickEnabled;
    }
    public void FunctionOnMouseDown(Vector2 mousepos)
    {
        if (joystickMasterDisabled)
        {
            return;
        }
        if (joystickEnabled )
        {
            return;
        }
        actionOnJoystickMouseIn?.Invoke();
        JoystickInitialization(mousepos);
        EnableJoystick();
    }
    public void FunctionOnMouseUp()
    {
        DisableJoystick();
    }
    public void FunctionOnMouseHold(Vector2 touchPos)
    {
        if (joystickMasterDisabled)
        {
            return;
        }
        if (joystickEnabled)
        {
            currentPos = touchPos;
            joysticKnob.transform.position = currentPos;


            if (Vector3.Magnitude(joysticKnob.transform.localPosition) >= (boundaryLimit))
            {
                Vector3 increment = joysticKnob.transform.localPosition - joysticKnob.transform.localPosition.normalized;
                joysticKnob.transform.localPosition = joysticKnob.transform.localPosition.normalized * boundaryLimit;

                if (followTouchDrag)
                {
                    joystickBase.transform.position += (increment / Screen.height) * 50 * sensitivity;
                }
                
            }

        }
    }
    public void MasterDisableJoystick()
    {
        joystickMasterDisabled = true;
        FunctionOnMouseUp();
    }
    public void MasterDisableJoystickOff()
    {
        joystickMasterDisabled = false;
    }
    
}
