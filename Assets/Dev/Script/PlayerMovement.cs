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
    [Space(5)]
    [Header("Dash Variables")]
    public float speedDash;
    [SerializeField] float distanceDash;
    float offsetSpeed = 2;
    
    public void Walk()
    {
        Vector3 dir = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
        dir = matrix.MultiplyPoint3x4(dir);

        rb.MovePosition (t.position + dir.normalized * speedWalk * Time.deltaTime * offsetSpeed); 
    }
    public void Run()
    {
        Vector3 dir = new Vector3(-Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));

        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Matrix4x4 matrix = Matrix4x4.Rotate(rotation);
        dir = matrix.MultiplyPoint3x4(dir);

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
            t.forward = dir;
        }
        
    }
    public void DashMove()
    {
        t.gameObject.layer = 6;

        Vector3 posToMove = t.position + t.forward * distanceDash;

        LeanTween.value(0, 1, speedDash).setOnUpdate((float value) => { 
            t.position = Vector3.MoveTowards(t.position, posToMove, value);
        }).setOnComplete(()=> { t.gameObject.layer = 0; });
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

}
