using UnityEngine;

public class GazePull : MonoBehaviour
{
    public Camera playerCamera;
    public int interactableLayerIndex = 9; // Matches your User Layer 9
    public float pullSpeed = 15f;
    public float maxDistance = 500f;

    private CharacterController controller;

    void Start()
    {
        // Find the controller on the parent Player_FPS object
        controller = transform.root.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetMouseButton(1)) // Right Click
        {
            ExecutePull();
        }
    }

    void ExecutePull()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        int layerMask = 1 << interactableLayerIndex;

        if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
        {
            // Calculate direction from player to the object
            Vector3 dir = (hit.point - transform.root.position).normalized;
            Vector3 velocity = dir * pullSpeed;

            if (controller != null)
            {
                // Move the physics-friendly way
                controller.Move(velocity * Time.deltaTime);
            }
            else
            {
                // Fallback if no controller exists
                transform.root.position += velocity * Time.deltaTime;
            }

            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
    }
}