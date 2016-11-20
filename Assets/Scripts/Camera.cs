using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{

    private new UnityEngine.Camera camera;

    void Start()
    {
        camera = GetComponent("Camera") as UnityEngine.Camera;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) Dezoom();
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) Zoom();
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow)) MoveCamera(0, 0.3f);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) MoveCamera(0, -0.3f);
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow)) MoveCamera(-0.3f, 0);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) MoveCamera(0.3f, 0);
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();
    }

    public void Zoom()
    {
        if (camera.orthographicSize > 5) camera.orthographicSize--;
    }

    public void Dezoom()
    {
        camera.orthographicSize++;
    }

    public void MoveCamera(float x, float y)
    {
        camera.gameObject.transform.position =
            new Vector3(
                camera.gameObject.transform.position.x + x,
                camera.gameObject.transform.position.y + y,
                camera.gameObject.transform.position.z
                );
    }
}
