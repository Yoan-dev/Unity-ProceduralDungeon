using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    // GameManager
    public GameObject gameManager;

    // Scripts
    public GameManager gameManagerScript;

    // Camera
    new UnityEngine.Camera camera;
    
    void Start()
    {
        gameManagerScript = gameManager.GetComponent("GameManager") as GameManager;
        camera = GetComponent("Camera") as UnityEngine.Camera;
    }

    #region Actions;
    
    public void Zoom ()
    {
        camera.orthographicSize -= 10;
        if (camera.orthographicSize < 45) camera.orthographicSize = 45;
    }

    public void Dezoom ()
    {
        camera.orthographicSize += 10;
        if (camera.orthographicSize > 300) camera.orthographicSize = 300;
    }

    public void MoveCamera (float x, float y)
    {
        camera.gameObject.transform.position =
            new Vector3(
                camera.gameObject.transform.position.x + x,
                camera.gameObject.transform.position.y + y,
                camera.gameObject.transform.position.z
                );
    }

    public void Focus (GameObject focus)
    {
        transform.position = new Vector3(focus.transform.position.x + 1.0f, focus.transform.position.y, -1);
        camera.orthographicSize = 3.0f;
    }

    #endregion Actions;

}
