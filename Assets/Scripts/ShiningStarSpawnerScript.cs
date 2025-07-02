using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiningStarSpawnerScript : MonoBehaviour
{
    public GameObject Star;
    public float spawnRate;
    private float timer = 0;
    public float heightOffset = 5;

    // Start is called before the first frame update
    void Start()
    {
        spawnStar();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnStar();
            timer = 0;
        }

    }

    void spawnStar()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float heighestPoint = transform.position.y + heightOffset;
        Instantiate(Star, new Vector3(transform.position.x, Random.Range(lowestPoint, heighestPoint), 1), transform.rotation);
    }
}
