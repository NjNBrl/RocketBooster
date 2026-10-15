using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Rover : MonoBehaviour
{
    private Rigidbody rb;
    private bool isEndReached = false;
    private Vector3 startPosition ;
    private Vector3 endPosition;

    [SerializeField] Transform rover;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = rover.transform.position;
        endPosition = startPosition + new Vector3(0f, 0f, -25f);
    }

    void Start()
    {
        transform.position = startPosition;
    }

    void FixedUpdate()
    {
        Move();
        Check();
        
    }
    private void Move() {
        transform.position = Vector3.MoveTowards(rover.transform.position, endPosition, moveSpeed * Time.fixedDeltaTime);

    }
    private void Check()
    {
        if (rover.transform.position == endPosition)
        {
            // In FixedUpdate
            endPosition = startPosition;
            startPosition = rover.transform.position;
            StartCoroutine(Delay());
           
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
    }
}
