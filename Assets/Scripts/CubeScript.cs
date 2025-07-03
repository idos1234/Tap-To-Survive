using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    private Vector3 Offset;
    private Camera Cam;
    private bool isDragging = false;
    private GameLogic _GameLogic;
    public SpriteRenderer squareSpriteRenderer;
    public List<Sprite> sprites;
    public GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        string spriteName = PlayerPrefs.GetString("Skin");
        foreach(var s in sprites)
        {
            if(s.name == spriteName)
            {
                squareSpriteRenderer.sprite = s;
                break;
            }
        }

        Cam = Camera.main;
        _GameLogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_GameLogic.gameIsStarted)
        {
            #if UNITY_EDITOR
                HandleMouseInput(); // for testing in editor
            #else
                HandleTouchInput();
            #endif
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 touchPos = Cam.ScreenToWorldPoint(Input.mousePosition);
            if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
            {
                Offset = transform.position - (Vector3)touchPos;
                isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 touchPos = Cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = touchPos + (Vector2)Offset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Cam.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (GetComponent<Collider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        Offset = transform.position - (Vector3)touchPos;
                        isDragging = true;
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isDragging)
                    {
                        transform.position = touchPos + (Vector2)Offset;
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "MovingSprite(Clone)")
        {
            Debug.Log("Game Over!");
            _GameLogic.gameOver();

            Destroy(collision.gameObject);
        } else if (collision.gameObject.name == "Coin(Clone)")
        {
            Debug.Log("Got Coin");

            StartCoroutine(SpawnWithDelay(1));

            Destroy(collision.gameObject);
        }
    }

    public IEnumerator SpawnWithDelay(float delay)
    {
        int screenWidth = Screen.width;
        int screenHeight = Screen.height;

        float randomX = Random.Range(0.5f, screenWidth - 0.5f);
        float randomY = Random.Range(0.5f, screenHeight - 0.5f);

        Vector3 worldPos = Cam.ScreenToWorldPoint(new Vector3(randomX, randomY, 0));
        worldPos.z = 0;

        // Wait for 2 seconds
        yield return new WaitForSeconds(delay);
        Instantiate(coin, worldPos, Quaternion.identity);
    }
}
