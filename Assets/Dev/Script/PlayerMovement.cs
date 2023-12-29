using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Refs Components")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform t;
    [SerializeField] Camera cam;
    [Space(5)]
    [Header("Movement Variables")]
    [SerializeField] float speedWalk;
    [SerializeField] float speedRun;
    [Space(5)]
    [Header("Dash Variables")]
    public float speedDash;
    [SerializeField] float distanceDash;
    float offsetSpeed = 100;
    
    public void Walk()
    {
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = dir.normalized * speedWalk * Time.deltaTime * offsetSpeed; ;
    }
    public void Run()
    {
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.velocity = dir.normalized * speedRun * Time.deltaTime * offsetSpeed; ;
    }
    public void Rotate()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,-cam.transform.position.z));
        Vector2 dir = (mousePos - t.position).normalized;
        t.up = dir;
    }
    public void DashMove()
    {
        t.gameObject.layer = 6;

        Vector2 posToMove = t.position + t.up * distanceDash;

        LeanTween.value(0, 1, speedDash).setOnUpdate((float value) => { 
            t.position = Vector2.MoveTowards(t.position, posToMove, value);
        }).setOnComplete(()=> { t.gameObject.layer = 0; });
    }

}
