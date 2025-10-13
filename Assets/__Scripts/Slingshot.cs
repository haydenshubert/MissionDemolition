using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    // fields set in the Unity Inspector pane
    [Header("Inscribed")]
    public GameObject       projectilePrefab;
    public float            velocityMult = 10f;
    public GameObject       projLinePrefab;

    [Header("Dynamic")]
    public GameObject       launchPoint;
    public Vector3          launchPos;
    public GameObject       projectile;
    public bool             aimingMode;
    public AudioClip        launchSound;

    private AudioSource audioSrc;
    public LineRenderer rubberBand1;
    public LineRenderer rubberBand2;
    public Transform bandFront1;
    public Transform bandFront2;
    public Transform bandBack1;
    public Transform bandBack2;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
        audioSrc = GetComponent<AudioSource>();
    }

    void OnMouseEnter()
    {
        // print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        // print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        // The player has pressed the mouse button while over Slingshot
        aimingMode = true;
        // Instantiate a Projectile
        projectile = Instantiate(projectilePrefab) as GameObject;
        // Start it at the launchPoint
        projectile.transform.position = launchPos;
        // Set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        // If Slingshot is not in aimingMode, don't run this code
        if (!aimingMode) return;

        // Get the current mouse position in 2D screen coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;
        // Limit mouseDelta to the radius of the Slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        // Move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        
        rubberBand1.enabled = true;
        rubberBand1.SetPosition(0, bandBack1.position);
        rubberBand1.SetPosition(1, projPos);

        rubberBand2.enabled = true;
        rubberBand2.SetPosition(0, bandBack2.position);
        rubberBand2.SetPosition(1, projPos);

        if (Input.GetMouseButtonUp(0))
        {
            audioSrc.PlayOneShot(launchSound);
            aimingMode = false;
            rubberBand1.enabled = false;
            rubberBand2.enabled = false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;

            // Switch to slingshot view immediately before setting POI
            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);

            FollowCam.POI = projectile; // Set the _MainCamera POI
            // Add a ProjectileLine to the Projectile
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }

}
