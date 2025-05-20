using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float upForce = 6;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(Vector3.up * upForce, ForceMode.Impulse);
            }
        }
    }
}
