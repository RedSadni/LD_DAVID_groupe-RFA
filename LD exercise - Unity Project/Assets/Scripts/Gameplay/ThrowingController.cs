using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThrowingController : MonoBehaviour
{
    public float maxThrowDistance = 7f;
    public float throwForwardForce = 20f;
    public float throwUpwardForce = 20f;
    public Transform throwTransform;
    public LayerMask layersForCollision;
    public float soundRadius = 8f;

    [Space]
    public GameObject[] curveDots;

    [HideInInspector] public Vector3 lastPoint;

    private Transform cameraTransform;
    private StarterAssetsInputs inputs;
    private bool isAiming;
    private bool canHitSomething;
    private GameManager gameManager;

    // Initial trajectory position
    private Vector3 startPosition;
    // Initial trajectory velocity
    private Vector3 startVelocity;
    // Step distance for the trajectory
    [SerializeField]
    private float trajectoryVertDist = 0.25f;


    private void Start()
    {
        cameraTransform = FindObjectOfType<Cinemachine.CinemachineBrain>().transform;
        inputs = GetComponent<StarterAssetsInputs>();
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (inputs.aim)
        {
            DrawTrajectory();
            isAiming = true;
        }
        else if (isAiming)
        {
            isAiming = false;
            ClearTrajectory();

            if (canHitSomething)
                gameManager.EmitSound(lastPoint, soundRadius, false);
        }
    }

    private void DrawTrajectory()
    {
        startPosition = throwTransform.position;
        startVelocity = (cameraTransform.forward * throwForwardForce) + (Vector3.up * throwUpwardForce);


        // Create a list of trajectory points
        var curvePoints = new List<Vector3>();
        curvePoints.Add(startPosition);

        // Initial values for trajectory
        var currentPosition = startPosition;
        var currentVelocity = startVelocity;
        // Init physics variables
        RaycastHit hit;
        Ray ray = new Ray(currentPosition, currentVelocity.normalized);
        // Loop until hit something or distance is too great
        while (!Physics.Raycast(ray, out hit, trajectoryVertDist, layersForCollision)
               && Vector3.Distance(startPosition, currentPosition) < maxThrowDistance)
        {
            // Time to travel distance of trajectoryVertDist
            var t = trajectoryVertDist / currentVelocity.magnitude;
            // Update position and velocity
            currentVelocity = currentVelocity + t * Physics.gravity;
            currentPosition = currentPosition + t * currentVelocity;
            // Add point to the trajectory
            curvePoints.Add(currentPosition);
            // Create new ray
            ray = new Ray(currentPosition, currentVelocity.normalized);
        }
        // If something was hit, add last point there
        if (hit.transform)
        {
            curvePoints.Add(hit.point);
            lastPoint = hit.point;
            canHitSomething = true;
        }
        else
        {
            ClearTrajectory();
            canHitSomething = false;
            return;
        }
        // Display line with all points
        for (int i = 0; i< curveDots.Length; i++)
        {
            if (i < curvePoints.Count)
            {
                curveDots[i].transform.position = curvePoints[i];
                curveDots[i].transform.localScale = Vector3.one * 0.015f + Vector3.one * (curvePoints[i] - throwTransform.position).magnitude * 0.003f;
            }
            else
            {
                curveDots[i].transform.position = Vector3.down;
            }
        }

    }

    private void ClearTrajectory()
    {
        foreach (GameObject dot in curveDots)
        {
            dot.transform.position = Vector3.down;
        }
    }
}
