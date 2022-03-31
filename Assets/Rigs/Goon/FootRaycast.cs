using UnityEngine;

public class FootRaycast : MonoBehaviour
{
    /// <summary> Length in Meters </summary>
    public float raycastLength = 2;
    /// <summary> Local-Space position of where the IK spawned </summary>
    private Vector3 startingPosition;
    /// <summary> Local-Space Rotation of where the IK spawned </summary>
    private Quaternion startingRotation;
    /// <summary>   The world-space position  of the ground above/below the foot IK. </summary>
    private Vector3 groundPosition;
    /// <summary> The world-space rotation for the foot to be aligned to </summary>
    private Quaternion groundRotation;
    /// <summary>
    /// Local-Space position to ease towards.
    /// </summary>
    private Vector3 targetPosition;

    private Vector3 footSeparateDir;

    void Start()
    {
        // Set starting position and rotation
        startingRotation = transform.localRotation;
        startingPosition = transform.localPosition;

        footSeparateDir = (startingPosition.x > 0) ? Vector3.right : Vector3.left;
    }

    void Update()
    {
        // Ease Twards Target Position
        transform.localPosition = AnimMath.Ease(transform.localPosition, targetPosition, .01f);
    }

    /// <summary>
    /// Wapper function?
    /// </summary>
    /// <param name="p"> This is the position of the foot?</param>
    public void SetPositionLocal (Vector3 p) {
        targetPosition = p;
    }

    public void SetPositionHome(){
        targetPosition = startingPosition;
    }

    public void SetPositionOffset(Vector3 p, float separateAmount = 0) {
        targetPosition = startingPosition + p + separateAmount * footSeparateDir;
    }

    private void FindGround() // Why isn't this being called?
    {
        Vector3 origin = transform.position + Vector3.up * raycastLength / 2;
        Vector3 direction = Vector3.down;


        // Draw ray
        Debug.DrawRay(origin, direction * raycastLength, Color.blue);

        // check for collision with ray:
        if (Physics.Raycast(origin, direction, out RaycastHit hitInfo, raycastLength))
        {

            // Find ground position
            groundPosition = hitInfo.point + Vector3.up * startingPosition.y;

            // Convert starting rotation into world-space
            Quaternion worldNeutral = transform.parent.rotation * startingRotation;

            groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal) * worldNeutral;
        }
    } // Control Foot/Hip Movement relative to the ground.
}
