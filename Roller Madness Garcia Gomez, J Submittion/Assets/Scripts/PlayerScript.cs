using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using Unity.Mathematics;
//using System.Numerics;

public class PlayerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody rb;

    InputSystem_Actions inputSystem_Actions;
    Vector3 movement = Vector3.zero;
    public float moveSpeed = 15f;
    public TMP_Text scoreText;
    int score = 0;
    public GameObject explosionPlayer;
    public GameObject explosionCoin;
    public int moveForce;
    public float maxSpeed;
    public GameObject explosionPrefab;
    void Start()
    {
        inputSystem_Actions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveX, 0, moveZ).normalized;

        if (moveDirection != Vector3.zero)
        {
            rb.AddForce(moveDirection * moveForce, ForceMode.Acceleration);
        }

        // Optional: Limit max speed to prevent endless acceleration
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Coin")
        {
            // GetComponent<Renderer>().material.color = Color.blue;
            Instantiate(explosionCoin, transform.position, Quaternion.identity);


            Destroy(collision.gameObject);
            GameManager.gm.add_score(1);
            GetComponent<AudioSource>().Play();

        }
        if (collision.gameObject.tag == "EnemyTag")
        {
            GameManager.gm.decHealth(); 
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        if (collision.gameObject.tag == "Gem")
        {
            Instantiate(explosionCoin, transform.position, Quaternion.identity);

            GameManager.gm.add_score(100);
            GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);
        }




    }


    void OnMove(InputValue inputValue)
    {
        if (GameManager.gm.pauseGame)
        {
            movement = Vector3.zero;
            return;
        }
        Vector2 b = inputValue.Get<Vector2>();
        movement = new Vector3(b.x, 0, b.y) * moveSpeed;
        rb.AddForce(movement, ForceMode.Acceleration);

    }

    void OnPause()
    {
        GameManager.gm.pauseGame = !(GameManager.gm.pauseGame);
        GameManager.gm.pause();
    }


    void FixedUpdate()
    {
        rb.linearVelocity = movement;
    }



}