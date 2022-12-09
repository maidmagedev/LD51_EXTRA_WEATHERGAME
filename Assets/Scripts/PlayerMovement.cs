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

        if (!isMoving)
        {
            startPosition = transform.position;
            endPosition = startPosition + new Vector3(1, 0, 0);
            elapsedTime = 0;
            Debug.Log("MovePlayer called");

            StartCoroutine(MovePlayerCoroutine(1, 0, 0));
        }
    }

    public void movePlayerToCell(int x, int y)
    {

    }

    private IEnumerator MovePlayerCoroutine(float x, float y, float z)
    {
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
    }
}
