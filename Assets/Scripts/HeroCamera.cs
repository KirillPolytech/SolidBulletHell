using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCamera : MonoBehaviour
{
    public float Sensivity = 5f;

    float XRotation = 0f;
    float MouseX, MouseY;
    Vector3 Direction;
    GameObject PlayerBody;
    private void Start()
    {
        PlayerBody = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        CameraMovement();
    }
    void CameraMovement()
    {
        MouseX = Input.GetAxis("Mouse X") * Sensivity;
        MouseY = Input.GetAxis("Mouse Y") * Sensivity;

        XRotation -= MouseY;
        XRotation = Mathf.Clamp(XRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(XRotation, 0f, 0f);
        PlayerBody.transform.Rotate(Vector3.up * MouseX);
    }
}
