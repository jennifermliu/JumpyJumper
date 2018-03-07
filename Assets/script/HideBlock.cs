using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBlock : MonoBehaviour
{

    public int index;
    public bool prev = false;
    public int reward;

    private Renderer renderer;

    private Color startColor;
    private Color endColor;

    private bool disappear = false;
    private float startTime = 0f;
    private float targetTime = 10f;
    private float progress = 0f;

    private GameObject player;
    private LineRenderer playerLineRenderer;
    private MainCharacter playerController;

    private float revertTime = 5f;


    void Start()
    {

        renderer = gameObject.GetComponent<Renderer>();

        startColor = renderer.material.color;
        endColor = startColor;
        endColor.a = 0;

        player = GameObject.Find("MainCharacter");
        playerController = player.GetComponent<MainCharacter>();
        playerLineRenderer = player.GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        if (disappear)
        {
            progress = Time.time - startTime;
            renderer.material.color = Color.Lerp(startColor, endColor, progress / targetTime);
        }

        if (progress >= targetTime)
        {
            revert();
            Destroy(gameObject);
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            startTime = Time.time;
            disappear = true;
            renderer.material.color = endColor;

            playerLineRenderer.enabled = false;
            playerController.canSeeLine = false;

            Invoke("revert", revertTime);

        }
    }

    void revert()
    {
        playerLineRenderer.enabled = true;
        playerController.canSeeLine = true;
    }

}
