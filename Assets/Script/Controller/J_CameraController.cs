using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class J_CameraController : MonoBehaviour
{
    private List<Joycon> joycons;

    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Joycon j = joycons[jc_ind];

        orientation = j.GetVector();

        var rotationX = Mathf.Abs(orientation.x);
        var rotationY = Mathf.Abs(orientation.y);
        var rotationz = Mathf.Abs(orientation.z);
        var rotationw = Mathf.Abs(orientation.w);

        this.transform.rotation = new Quaternion(orientation.x, orientation.y, orientation.z, orientation.w);

        if (SceneManager.GetActiveScene().name == "Start" && j.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            SceneManager.LoadScene("MainEvent");
        }
    }
}
