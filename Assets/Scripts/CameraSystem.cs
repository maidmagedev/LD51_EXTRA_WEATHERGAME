using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.Editor;
using Cinemachine.PostFX;
using Cinemachine.Utility;

// https://www.youtube.com/watch?v=pJQndtJ2rk0&ab_channel=CodeMonkey

public class CameraSystem : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase cinemachine;
    [SerializeField] GameObject player;
    [SerializeField] GameObject cameraSys;
    [SerializeField] MousePosition3D mousePosScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CamSwap"))
        {
            //Debug.Log("Moving to player");
            //cameraSys.transform.position = player.transform.position;
            mousePosScript.cycleCam();
        }

        Vector3 inputDir = new (0, 0, 0);

        if (Input.GetKey(KeyCode.W)) inputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = +1f;

        //Input.mouse

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        float rotateSpeed = 100f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);

        
    }
}
