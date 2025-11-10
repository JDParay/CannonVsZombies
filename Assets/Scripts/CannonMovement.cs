using UnityEngine;

public class CannonMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Aiming Settings")]
    public Transform cannonBase;   // the part that rotates (e.g., barrel or shootPoint parent)
    public float aimSpeed = 30f;
    public float minAimAngle = -10f; // how far down it can tilt
    public float maxAimAngle = 45f;  // how far up it can tilt

    [Header("Shooting Settings")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootForce = 20f;

    private float currentAimAngle = 0f;

    void Update()
    {
        Move();
        Aim();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Move()
    {
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(Vector3.forward * move, Space.Self);
    }

    void Aim()
    {
        float aimInput = 0f;
        if (Input.GetKey(KeyCode.A))
            aimInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            aimInput = 1f;

        currentAimAngle += aimInput * aimSpeed * Time.deltaTime;
        currentAimAngle = Mathf.Clamp(currentAimAngle, minAimAngle, maxAimAngle);

        cannonBase.localRotation = Quaternion.Euler(0f, 0f, currentAimAngle);
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);
        Destroy(projectile, 5f);
    }
}
