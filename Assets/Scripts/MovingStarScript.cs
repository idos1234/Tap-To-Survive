using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingStarScript : MonoBehaviour
{
    public float deadZone = -5;

    private float originalY;
    private int randomNumber;
    public float verticalAmplitude = 1; // how high it moves up/down
    public float verticalFrequency = 1; // how fast it moves up/down

    // Start is called before the first frame update
    void Start()
    {
        Vector3 rightEdgeWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.nearClipPlane));

        transform.position = new Vector3(
            rightEdgeWorldPos.x + 0.5f,
            transform.position.y,
            transform.position.z
        );

        originalY = transform.position.y;
        randomNumber = Random.Range(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        // Move left continuously
        transform.position += Vector3.left * Random.Range(1f, 1.5f) * Time.deltaTime;

        // Apply vertical sine wave movement
        if (randomNumber % 2 == 1)
        {
            float newY = originalY + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        } else
        {
            float newY = originalY + Mathf.Cos(Time.time * verticalFrequency) * verticalAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }


        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
