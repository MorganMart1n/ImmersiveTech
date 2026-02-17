using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsPullPlayer : MonoBehaviour
{
    // Pull configuration
    public float waitOnPickup = 0.2f;
    public float breakForce = 35f;
    public float pullForce = 30f;        // Strength of pull applied to the player
    public float stopDistance = 1f;      // Distance to stop pulling at
    public float maxPullSpeed = 10f;     // <= 0 means no cap

    [HideInInspector] public bool pickedUp = false;
    [HideInInspector] public PlayerInteractions playerInteractions;

    // Internal pulling state
    private Rigidbody playerRigidbody;
    private bool isPulling = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (pickedUp)
        {
            if (collision.relativeVelocity.magnitude > breakForce)
            {
                playerInteractions?.BreakConnection();
            }
        }
    }

    // Public API: start pulling the player (pass the player's Rigidbody)
    public void StartPull(Rigidbody playerRb, PlayerInteractions interactions = null)
    {
        if (playerRb == null) return;
        playerRigidbody = playerRb;
        if (interactions != null) playerInteractions = interactions;

        // Start the pickup protection so an initial collision doesn't immediately break
        StartCoroutine(PickUp());

        isPulling = true;
    }

    // Public API: stop pulling the player
    public void StopPull()
    {
        isPulling = false;
        playerRigidbody = null;
    }

    private void FixedUpdate()
    {
        if (!isPulling || playerRigidbody == null) return;

        Vector3 toTarget = transform.position - playerRigidbody.position;
        float distance = toTarget.magnitude;

        // If close enough, stop pulling
        if (distance <= stopDistance)
        {
            StopPull();
            return;
        }

        Vector3 direction = toTarget.normalized;

        // Apply acceleration toward the object
        // Using ForceMode.Acceleration makes force independent of the player's mass
        playerRigidbody.AddForce(direction * pullForce, ForceMode.Acceleration);

        // Optionally cap velocity to keep behavior stable
        if (maxPullSpeed > 0f)
        {
            Vector3 vel = playerRigidbody.velocity;
            float speed = vel.magnitude;
            if (speed > maxPullSpeed)
            {
                playerRigidbody.velocity = vel.normalized * maxPullSpeed;
            }
        }
    }

    // this is used to prevent the connection from breaking when you just picked up the object
    // as it sometimes fires a collision with the ground or whatever it is touching
    public IEnumerator PickUp()
    {
        yield return new WaitForSecondsRealtime(waitOnPickup);
        pickedUp = true;
    }
}