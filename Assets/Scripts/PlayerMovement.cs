using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Usage of Lerp based on https://www.youtube.com/watch?v=MyVY-y_jK1I&t=206s&ab_channel=KetraGames

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Used by the Lerp in MovePlayerCoroutine() ------------------
    private Vector3 startPosition; 
    private Vector3 endPosition;
    private float desiredDuration = 1f;
    private float elapsedTime = 0;
    bool moving = false;
    // ------------------------------------------------------------

    public AStarGridCell playerCurrentCell;
    private SGrid grid;
    [Header("SET THIS UP FOR EACH LEVEL!")]
    public int startingX;
    public int startingY;

    public bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<SGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCurrentCell == null)
        {
            playerCurrentCell = grid.gridArray[startingX, startingY].GetComponent<AStarGridCell>();
        }

        /*
        if (!isMoving)
        {
            startPosition = transform.position;
            endPosition = startPosition + new Vector3(1, 0, 0);
            elapsedTime = 0;
            Debug.Log("MovePlayer called");

            StartCoroutine(MovePlayerCoroutine(1, 0, 0));
        }
        */
    }

    public void MovePlayerDirectional(int tileCount, directions dir)
    {
        if (tileCount == 0)
        {
            return;
        }
        Vector3 newPostion = Vector3.zero;

        switch(dir)
        {
            case directions.north:
                newPostion = new Vector3(0, 0, tileCount);
                break;
            case directions.south:
                newPostion = new Vector3(0, 0, tileCount);
                break;
            case directions.west:
                newPostion = new Vector3(tileCount, 0, 0);
                break;
            case directions.east:
                newPostion = new Vector3(tileCount, 0, 0);
                break;
         
        }
        startPosition = transform.position;
        endPosition = transform.position + newPostion;
        StartCoroutine(MovePlayerCoroutine());
    }

    public enum directions
    {
        north,
        south,
        east,
        west
    }

    private IEnumerator MovePlayerCoroutine()
    {
        bool completedMovement = false;
        while (!completedMovement)
        {
            if (!moving)
            {
                moving = true;
                animator.SetTrigger("walk");
                Debug.Log("MovePlayer Started");
                isMoving = true;
                while (elapsedTime < desiredDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float percentageComplete = elapsedTime / desiredDuration;
                    transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, percentageComplete));
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                isMoving = false;
                animator.SetTrigger("idle");

                Debug.Log("MovePlayer End");
                moving = false;
                completedMovement = true;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
