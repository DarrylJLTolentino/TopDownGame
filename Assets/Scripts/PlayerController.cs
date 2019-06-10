using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector3 moveInput;
    private Rigidbody rb;
    private Vector3 moveVelocity;
    private Camera mainCamera;
    private GameObject trapPrefab;
    private GameObject player;
    private GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        //Need to get Rigidbody of player and find the main camera within Unity
        rb = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        player = GameObject.FindWithTag("Player");
        trapPrefab = GameObject.FindWithTag("Trap");
        plane = GameObject.FindWithTag("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        //Since game is on 3D plane, movement input must be a 3D vector (x, y, z)
        moveInput = new Vector3 (Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * moveSpeed;

        //Ray is used to create a line from the camera to position of mouse
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(cameraRay, out rayLength)) 
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (Input.GetKeyDown("space"))
        {
            DropTrap();
        }
    }

    void FixedUpdate() {
        rb.velocity = moveVelocity;
    }

    private void DropTrap()
    {
        if (trapPrefab) {
            trapPrefab.transform.rotation = Quaternion.Euler(0, 0, 0);
            Instantiate(trapPrefab, new Vector3(Mathf.RoundToInt(player.transform.position.x), plane.transform.position.y, Mathf.RoundToInt(player.transform.position.z)), trapPrefab.transform.rotation);
        }
    }
}
