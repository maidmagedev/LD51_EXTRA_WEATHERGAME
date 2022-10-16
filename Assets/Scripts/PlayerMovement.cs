using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Usage of Lerp based on https://www.youtube.com/watch?v=MyVY-y_jK1I&t=206s&ab_channel=KetraGames

public class PlayerMovement : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition = new Vector3(3, 1.86f, 13);
    private float desiredDuration = 1f;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / desiredDuration;

        transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));
    }
}
