using UnityEngine;

public class FloatingReward : MonoBehaviour
{
    public float rotationSpeed;
    public float floatSpeed;
    public float floatHeight;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Rotate the object around its up-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

        // Float up and down
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
    }


}
