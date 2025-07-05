using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public float amplitude = 0.5f;  // Height of the float
    public float frequency = 1f;    // Speed of the float
    private GameLogic _GameLogic;

    private Vector3 startPos;

    void Start()
    {
        _GameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameLogic>();
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        if(_GameLogic.gameIsStarted == false)
        {
            Destroy(gameObject);
        }
    }
}
