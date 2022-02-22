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
    private float totalTime = 400f;
    private GameObject brick;
    private GameObject coin;
    
    
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
        accumulatedTime += Time.deltaTime;

        if (accumulatedTime > 1f)
        {
            totalTime -= 1f;
            accumulatedTime = 0f;
        }
        scoreText.text = "Mario\n" + score.ToString(scoreFormat);
        timerText.text = "Time\n" + totalTime.ToString(timeFormat);
        coinsText.text = "x" + coins.ToString(coinFormat);
        //StartCoroutine(UpdatePickingRaycast());
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
                        coin = hitInfo.collider.gameObject;
                        CoinAnimation();
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

    private void CoinAnimation()
    {
        var c = coin.GetComponent<Animator>();
        c.SetTrigger("getCoin");
    }
}
