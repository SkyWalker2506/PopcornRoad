using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [HideInInspector]
    public bool DoMove;
    [Header("Movement")]

    public float ForwardSpeed = 10;
    public float SideSpeed = 1;
    Vector3 lastPosition = new Vector3();
    float deltaX;
    [Tooltip("It limits the maximum position change per frame. It is needed for more stable movement. Otherwise some anomalies are happening.")]
    [Range(1, 100)]
    [SerializeField]
    float maxDeltaX = 50;
    bool isDraging;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        SetMouseState();
        if (DoMove)
        {
            MoveForward();
            MoveSide();
        }
    }

    void MoveForward()
    {
        transform.position += transform.forward * ForwardSpeed * Time.deltaTime;
    }

    void MoveSide()
    {
        if (isDraging)
        // transform.Translate(-deltaX * SideSpeed * Time.deltaTime, 0, 0);
        {
            var limitedDeltaX = Mathf.Sign(deltaX) * Mathf.Min(Mathf.Abs(deltaX), maxDeltaX);
            rb.velocity = new Vector3(-limitedDeltaX * SideSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }


    void SetMouseState()
    {
        if (Input.GetMouseButtonDown(0))
            MouseIsDown();
        if (Input.GetMouseButtonUp(0))
            MouseIsUp();
        if (Input.GetMouseButton(0))
            MouseIsDraging();
    }


    private void MouseIsDown()
    {
        lastPosition = Input.mousePosition;
        isDraging = true;
    }

    private void MouseIsDraging()
    {
        isDraging = true;
        deltaX = lastPosition.x - Input.mousePosition.x;
        lastPosition = Input.mousePosition;
    }

    private void MouseIsUp()
    {
        isDraging = false;
    }

}
