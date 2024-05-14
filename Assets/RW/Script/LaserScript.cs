using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;

    public float toggleInterval = 0.5f;
    public float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;

    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;

    // Start is called before the first frame update
    void Start()
    {
        timeUntilNextToggle = toggleInterval;
        laserCollider = gameObject.GetComponent<Collider2D>();
        laserRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //1
        timeUntilNextToggle -= Time.deltaTime; 
        //2
        if (timeUntilNextToggle <= 0)
        {
            //3
            isLaserOn = !isLaserOn;
            //4
            laserCollider.enabled = isLaserOn;
            //5
            if (isLaserOn)
            {
                laserRenderer.sprite = laserOnSprite;
            }
            else
            {
                laserRenderer.sprite = laserOffSprite;
            }
            //6
            timeUntilNextToggle = toggleInterval;
        }
        //7
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
