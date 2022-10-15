using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// https://www.youtube.com/watch?v=EfSImxUqmO0&ab_channel=Mtir%27sCode
public class GridClick : MonoBehaviour
{
    public UnityEvent unityEvent = new UnityEvent();
    public GameObject gameObj;


    // Start is called before the first frame update
    void Start()
    {
        gameObj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickMeIfPointerOnMe();
        }
    }

    private void ClickMeIfPointerOnMe()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            unityEvent.Invoke();
        }

    }
}
