using UnityEngine;

public class Chase : MonoBehaviour
{
    public Transform target;
    public float speed = 15f;
    public float minDistance = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 15f);
        if (target == null)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                target = GameObject.FindWithTag("Player").GetComponent<Transform>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Debug.Log("Target is not assigned");
            return;
        }
        transform.LookAt(target);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > minDistance)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
    }
}
