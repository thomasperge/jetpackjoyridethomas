using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseController : MonoBehaviour
{
    private float jetpackForce = 0.0f;
    private float forwardMovementSpeed = 0.0f;
    private Rigidbody2D playerRigidbody;
    private bool gameStarted = false;

    private float startTime;
    private float targetSpeed;
    public float maxSpeed = 10.0f;
    public float initialSpeed = 3.0f;
    public float speedIncreaseDuration = 90.0f;

    public Transform groundCheckTransform;
    private bool isGrounded;
    public LayerMask groundCheckLayerMask;
    private Animator mouseAnimator;

    public ParticleSystem jetpack;
    private bool isDead = false;

    private uint coins = 0;
    public Text coinsCollectedLabel;

    private uint metter = 0;
    public Text metterLabel;

    public Canvas scoreGameScene;
    public Canvas restartScene;
    public Canvas startScene;
    public Canvas pauseScene;
    public Canvas finishScene;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        mouseAnimator = GetComponent<Animator>();
        AdjustJetpack(false);

        restartScene.gameObject.SetActive(false);
        startScene.gameObject.SetActive(true);
        pauseScene.gameObject.SetActive(false);
        finishScene.gameObject.SetActive(false);

        jetpackForce = 0.0f;
        forwardMovementSpeed = 0.0f;
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        jetpackForce = 23.0f;
        forwardMovementSpeed = 5f;
        startTime = Time.time;
        targetSpeed = forwardMovementSpeed;

        scoreGameScene.gameObject.SetActive(true);
        startScene.gameObject.SetActive(false);
        pauseScene.gameObject.SetActive(false);
        finishScene.gameObject.SetActive(false);

        StartCoroutine(UpdateMetterLabel());
        gameStarted = true;
        Time.timeScale = 1f;
    }

    IEnumerator UpdateMetterLabel()
    {
        while (isDead == false)
        {
            yield return new WaitForSeconds(0.1f);
            metter += 1;

            metterLabel.text = Mathf.FloorToInt(metter).ToString("D4");

            if (Mathf.FloorToInt(metter) >= 1000)
            {
                finishScene.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("RocketMouse");
    }

    void FixedUpdate() 
    {
        if (gameStarted) {
            bool jetpackActive = Input.GetButton("Fire1");
            jetpackActive = jetpackActive && !isDead;

            if (jetpackActive)
            {
                playerRigidbody.AddForce(new Vector2(0, jetpackForce));
            }

            if (!isDead)
            {
                Vector2 newVelocity = playerRigidbody.velocity;
                newVelocity.x = forwardMovementSpeed;
                playerRigidbody.velocity = newVelocity;
            }

            UpdateGroundedStatus();
            AdjustJetpack(jetpackActive);

            if (isDead && isGrounded)
            {
                Transform coinsCollected = restartScene.transform.Find("Score/coin/coinsCollected");
                Transform meterCollected = restartScene.transform.Find("Score/meter/meterCollected");
                
                if (coinsCollected != null && meterCollected != null)
                {
                    Text coinsText = coinsCollected.GetComponent<Text>();
                    coinsText.text = Mathf.FloorToInt(coins).ToString("D3");

                    Text meterText = meterCollected.GetComponent<Text>();
                    meterText.text = Mathf.FloorToInt(metter).ToString("D4");
                }
                else
                {
                    Debug.LogError("Coins collected text not found!");
                }

                scoreGameScene.gameObject.SetActive(false);
                restartScene.gameObject.SetActive(true);
            }
        }
    }

    void UpdateGroundedStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        mouseAnimator.SetBool("isGrounded", isGrounded);
    }

    void AdjustJetpack(bool jetpackActive)
    {
        var jetpackEmission = jetpack.emission;
        jetpackEmission.enabled = !isGrounded;
        if (jetpackActive)
        {
            jetpackEmission.rateOverTime = 125.0f;
        }
        else
        {
            jetpackEmission.rateOverTime = 0.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Coins"))
        {
            CollectCoin(collider);
        }
        else
        {
            HitByLaser(collider);
        }
    }

    void HitByLaser(Collider2D laserCollider)
    {
        isDead = true;
        mouseAnimator.SetBool("isDead", true);
    }

    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        coinsCollectedLabel.text = Mathf.FloorToInt(coins).ToString("D3");
        Destroy(coinCollider.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.time - startTime;

        if (elapsedTime <= speedIncreaseDuration)
        {
            forwardMovementSpeed = Mathf.Lerp(initialSpeed, maxSpeed, elapsedTime / speedIncreaseDuration);
        }
        else
        {
            forwardMovementSpeed = maxSpeed;
        }
    }
}
