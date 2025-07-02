using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovingSpriteScript : MonoBehaviour
{
    private Vector3 target;
    public float radius = 5;
    public float speed = 1;
    private Vector3 direction;
    public float deadZone = 10;

    // Start is called before the first frame update
    void Start()
    {
        target = Vector3.zero + Random.onUnitSphere * radius;
        target.z = 0;
        direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + direction.normalized * speed * Time.deltaTime;

        if (transform.position.y >= deadZone || transform.position.x >= deadZone || transform.position.y <= -deadZone || transform.position.x <= -deadZone)
        {
            Destroy(gameObject);
        }
    }
}
