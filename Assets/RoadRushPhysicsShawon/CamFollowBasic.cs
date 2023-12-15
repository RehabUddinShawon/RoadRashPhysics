using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowBasic : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * 30);
        Vector3 tarRot = Quaternion.ToEulerAngles(target.transform.rotation)*Mathf.Rad2Deg;
      
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3 (tarRot.x * 0.2f, tarRot.y,tarRot.z*0.2f)), Time.deltaTime * 2f);
    }

}
