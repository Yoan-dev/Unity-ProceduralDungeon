using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    // GameManager
    public GameObject gameManager;
    public GameManager gameManagerScript;

    // Camera
    public GameObject cameraObject;
    private new UnityEngine.Camera camera;
    
    void Start()
    {
        gameManagerScript = gameManager.GetComponent("GameManager") as GameManager;
    }

    void Update()
    {
        // Camera
        Camera camera = cameraObject.GetComponent<Camera>() as Camera;
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) camera.Dezoom();
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) camera.Zoom();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) camera.MoveCamera(0, 1.5f);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) camera.MoveCamera(0, -1.5f);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) camera.MoveCamera(-1.5f, 0);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) camera.MoveCamera(1.5f, 0);
        if (Input.GetKey(KeyCode.Q)) Application.Quit();
    }

}
