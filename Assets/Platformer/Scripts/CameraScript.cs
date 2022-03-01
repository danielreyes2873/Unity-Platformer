using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    // private ParticleSystem brickBreak;
    private string scoreFormat = "000000.##";
    private string timeFormat = "000.##";
    private string coinFormat = "00.##";
    private int score = 0;
    private int coins = 0;
    private float accumulatedTime;
    private float totalTime = 100f;
    private GameObject brick;
    private GameObject coin;
    public bool gameOver = false;
    public GameObject mario;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Mario\n" + score.ToString(scoreFormat);
        timerText.text = "Time\n" + totalTime.ToString(timeFormat);
        coinsText.text = "x" + coins.ToString(coinFormat);
        StartCoroutine(UpdatePickingRaycast());
    }

    // Update is called once per frame
    void Update()
    {
        if (mario != null)
        {
            var newX = mario.transform.position.x;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
        
        if (!gameOver)
        {
            UpdateTimer();
        }
    }

    IEnumerator UpdatePickingRaycast()
    {
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (hitInfo.collider.gameObject.name.Equals("Question(Clone)"))
                    {
                        score += 200;
                        coins++;
                        CoinAnimation(hitInfo.collider.gameObject);
                        // Destroy(hitInfo.collider.gameObject);
                        Debug.Log("hit object at " + Input.mousePosition);
                    }
                    else if (hitInfo.collider.gameObject.name.Equals("Brick(Clone)"))
                    {
                        
                        brick = hitInfo.collider.gameObject;
                        // Destroy(particle);
                        StartCoroutine(Break());
                        Debug.Log("destroyed object at " + Input.mousePosition);
                    }
                    //Debug.Log($"{hitInfo.collider.gameObject.name}");
                }
                
            }
            yield return null;
        }
    }

    IEnumerator Break()
    {
        var p = brick.GetComponent<ParticleSystem>();
        var m = brick.GetComponent<MeshRenderer>();
        p.Play();
        m.enabled = false;

        yield return new WaitForSeconds(p.main.startLifetime.constantMax);
        Destroy(brick);
    }

    public void CoinAnimation(GameObject passCoin)
    {
        var c = passCoin.GetComponent<Animator>();
        c.SetTrigger("getCoin");
    }

    private void UpdateTimer()
    {
        accumulatedTime += Time.deltaTime;

        if (accumulatedTime > 1f)
        {
            totalTime -= 1f;
            accumulatedTime = 0f;
        }
        scoreText.text = "Mario\n" + score.ToString(scoreFormat);
        timerText.text = "Time\n" + totalTime.ToString(timeFormat);
        coinsText.text = "x" + coins.ToString(coinFormat);
        if (totalTime == 0)
        {
            gameOver = true;
            Debug.Log("Time ran out. Game Over!");
        }
    }

    public void GameOverEvent()
    {
        gameOver = true;
        Debug.Log("Game Over");
        Destroy(mario);
    }

    public void GameWinEvent()
    {
        gameOver = true;
        Debug.Log("You Win!");
    }

    public void addCoins(GameObject passCoin)
    {
        CoinAnimation(passCoin);
        coins++;
        score += 100;
    }

    public void addBrick()
    {
        score += 100;
    }
}
