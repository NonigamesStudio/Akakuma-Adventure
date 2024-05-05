using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refs Components")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform t;
    [SerializeField] Camera cam;
    [Space(5)]
    [Header("Movement Variables")]
    [SerializeField] float speedWalk;
    [SerializeField] float speedRun;
    [SerializeField] LayerMask layerGround;
    [HideInInspector]public float speed;
    [SerializeField] float speedToReachGround = 3.0f;

    [SerializeField] float groundDetectionDistance = 0.5f;
    


    
    float offsetSpeed = 2;
    
    public void Walk()
    {
        Vector3 dir = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
        dir = matrix.MultiplyPoint3x4(dir);
        speed = dir.magnitude * speedRun * Time.deltaTime * offsetSpeed;
        rb.MovePosition (t.position + dir.normalized * speedWalk * Time.deltaTime * offsetSpeed); 
        
    }
    public void Run()
    {
        Vector3 dir = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
        dir = matrix.MultiplyPoint3x4(dir);
        speed = dir.magnitude * speedRun * Time.deltaTime * offsetSpeed;
        rb.MovePosition(t.position + dir.normalized * speedRun * Time.deltaTime * offsetSpeed);
        
    }
    public void Rotate()
    {

        //Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,-cam.transform.position.z));
        Ray rayMouse = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(rayMouse, out RaycastHit hit,100, layerGround))
        {
            Vector3 mousePos = new Vector3(hit.point.x, t.position.y, hit.point.z);
            Vector3 dir = (mousePos - t.position).normalized;

            float rotationSpeed = 5f;
            t.forward = Vector3.Slerp(t.forward, dir, rotationSpeed * Time.deltaTime);
        }
        
    }
   

    public void SlowDown(bool state)
    {
        if (state)
        {
            speedWalk = speedWalk / 2;
            speedRun = speedRun / 2;
        }
        else
        {
            speedWalk = speedWalk * 2;
            speedRun = speedRun * 2;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sticky"))
        {
            StartCoroutine(Slow());
        }
    }

    IEnumerator Slow()
    {
        speedWalk = speedWalk / 2;
        speedRun = speedRun / 2;
        yield return new WaitForSeconds(2);
        speedWalk = speedWalk * 2;
        speedRun = speedRun * 2;
    }
    void Update()
    {
        GravityFunction();
    }
    void GravityFunction()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, groundDetectionDistance, layerGround))
        {
            rb.velocity = Vector3.down * speedToReachGround;
        }
    }

    
}
