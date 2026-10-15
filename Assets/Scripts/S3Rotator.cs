using UnityEngine;
using UnityEngine.UIElements;

public class S3Rotator : MonoBehaviour
{
    private float rotateSpeed = 50;

    private Vector3 startPositionFighter;
    private Vector3 endPositionFighter;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private float moveSpeed = 3f;

    [SerializeField]private Transform fighter;
    [SerializeField] private Transform rover;
    void Start()
    {
        
    }

    private void Awake()
    {

        startPositionFighter = fighter.transform.position;
        endPositionFighter = startPositionFighter + new Vector3(0f, 20f, 0f);
        startPosition = rover.transform.position;
        endPosition = startPosition + new Vector3(0f, 7f, -25f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
        Check();
        CheckRover();
    }

    private void Rotate()
    {
        if (gameObject.tag == "VerticalRotator") {
            transform.Rotate(0f, 0f, rotateSpeed * Time.fixedDeltaTime);
        }
        if (gameObject.tag == "Fighter")
        {
            Move();
        }
        if (gameObject.tag == "Horizontal")
        {
            MoveRover();
        }


    }


    private void Move()
    {
        transform.position = Vector3.MoveTowards(fighter.transform.position, endPositionFighter, moveSpeed * Time.fixedDeltaTime);

    }
    private void Check()
    {
        if (fighter.transform.position == endPositionFighter)
        {
            // In FixedUpdate
            endPositionFighter= startPositionFighter;
            startPositionFighter = fighter.transform.position;
       

        }
    }
    private void MoveRover()
    {
        transform.position = Vector3.MoveTowards(rover.transform.position, endPosition, moveSpeed * Time.fixedDeltaTime);

    }
    private void CheckRover()
    {
        if (rover.transform.position == endPosition)
        {
            // In FixedUpdate
            endPosition = startPosition;
            startPosition = rover.transform.position;
        }
    }

}
