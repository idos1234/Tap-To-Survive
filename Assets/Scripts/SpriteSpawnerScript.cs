using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpriteSpawnerScript : MonoBehaviour
{
    public GameObject Sprite;
    public float radius = 6;
    private float timer = 0;
    public float spawnRate;
    private GameLogic GameLogic;
    private SoundScript sound;
    public AudioSource spriteSound;

    // Start is called before the first frame update
    private void Start()
    {
        GameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameLogic>();
        sound = GameObject.FindGameObjectWithTag("Sound").GetComponent<SoundScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLogic.gameIsStarted)
        {
            if (timer < spawnRate)
            {
                timer = timer + Time.deltaTime;
            }
            else
            {
                spawnSprite();
                timer = 0;
            }
        }
    }

    private void spawnSprite()
    {
        Vector2 direction = Random.onUnitSphere.normalized; // random point on circle edge
        Vector3 spawnPos = transform.position + (Vector3)(direction * radius);
        if (!(spawnPos.x < 3.5 && spawnPos.x > -3.5 && spawnPos.y < 5 && spawnPos.y > -5))
        {
            StartCoroutine(spawnWithDelay(spawnPos));
        }
    }

    private IEnumerator spawnWithDelay(Vector3 spawnPos)
    {
        Instantiate(Sprite, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        if (sound.isSoundOn)
        {
            spriteSound.PlayOneShot(spriteSound.clip);
        }

    }
}
