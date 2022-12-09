using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MousePosition3D : MonoBehaviour
{
    //[SerializeField] public Camera mainCamera; // cinemachine cam
    //[SerializeField] public Camera orthoCamera; // topcam
    public Camera activeCam;
    public List<Camera> cameras;
    private int cameraIndex;
    [SerializeField] private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        if (activeCam == null)
        {
            activeCam = cameras.ElementAt(0);
            cameraIndex = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            transform.position = raycastHit.point;
        }
    }

    public void cycleCam()
    {
        cameras.ElementAt(1).gameObject.SetActive(false);
        cameras.ElementAt(2).gameObject.SetActive(false);
        if (cameraIndex + 1 < cameras.Count)
        {
            cameraIndex++;
            

            switch (cameraIndex)
            {
                case 1:
                    // Enable Ortho Cam Object
                    cameras.ElementAt(1).gameObject.SetActive(true);
                    break;
                case 2:
                    cameras.ElementAt(2).gameObject.SetActive(true);

                    break;

            }
        } else
        {
            cameraIndex = 0;
        }
        activeCam = cameras.ElementAt(cameraIndex);

    }
}
