using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBlock : MonoBehaviour {

    public int index;
    public bool prev = false;
    public int reward;

    private Color startColor;
    private Color endColor;

    private Renderer renderer;

    private bool disappear = false;
    private float startTime = 0f;
    private float targetTime = 10f;
    private float progress = 0f;

    private GameObject player;
    private GameObject playerSphere;
    private Renderer playerRenderer;
    private Renderer playerRendererSphere;
    private MainCharacter playerController;

    private float revertTime = 5f;

    public Color freezeColor = new Color(1F, 1F, 1F, 1F);
    private Color originalColor;

    void Start()
    {

        renderer = gameObject.GetComponent<Renderer>();

        startColor = renderer.material.color;
        endColor = startColor;
        endColor.a = 0;

        player = GameObject.Find("MainCharacter");
        playerSphere = GameObject.Find("Sphere");
        playerController = player.GetComponent<MainCharacter>();
        playerRenderer = player.GetComponent<Renderer>();
        playerRendererSphere = playerSphere.GetComponent<Renderer>();

        originalColor = playerRenderer.material.color;

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

            playerRenderer.material.color = freezeColor;
            playerRendererSphere.material.color = freezeColor;
            playerController.canMove = false;

            Invoke("revert", revertTime);

        }
    }

    void revert()
    {
        playerController.canMove = true;
        playerRenderer.material.color = originalColor;
        playerRendererSphere.material.color = originalColor;
    }

}
