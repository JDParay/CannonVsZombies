using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 2f;
    public float sideSpeed = 2f;
    public float sideRange = 2f;

    [Header("Pulse Settings")]
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.2f;

    [Header("Penalty Settings")]
    public GameObject baseObject;
    public int penaltyPoints = 10;

    private Vector3 originalScale;
    private Vector3 forwardDir;
    private float sideOffset;
    private bool movingRight = true;

    void Start()
    {
        if (baseObject != null)
        {
            forwardDir = (baseObject.transform.position - transform.position).normalized;
            forwardDir.y = 0;
            transform.rotation = Quaternion.LookRotation(forwardDir) * Quaternion.Euler(0, 90, 0);
        }

        originalScale = transform.localScale;
        sideOffset = Random.Range(0f, Mathf.PI * 2);
    }

    void Update()
    {
        Move();
        Pulse();
    }

    void Move()
    {
        if (baseObject == null) return;

        transform.position += forwardDir * forwardSpeed * Time.deltaTime;

        Vector3 rightDir = Quaternion.Euler(0, 90, 0) * forwardDir;
        float sideMotion = Mathf.Sin(Time.time * sideSpeed + sideOffset) * sideRange * Time.deltaTime;
        transform.position += rightDir * sideMotion;
    }

    void Pulse()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = originalScale * scale;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Bounce off walls
        if (collision.gameObject.CompareTag("Wall"))
        {
            forwardDir = Vector3.Reflect(forwardDir, collision.contacts[0].normal);
            transform.rotation = Quaternion.LookRotation(forwardDir);
        }

        // Hit by projectile
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
            ScoreManager.instance.AddScore(10);
        }

        // Reached base
        if (baseObject != null && collision.gameObject == baseObject)
        {
            ScoreManager.instance.AddScore(-penaltyPoints);
            Destroy(gameObject);
        }
    }
}
