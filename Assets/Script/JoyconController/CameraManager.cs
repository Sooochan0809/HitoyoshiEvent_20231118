using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{

    private List<Joycon> joycons;

    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;
    private Quaternion StartVector;
    private Vector3 StartAccel;
    private Vector3 StartEuler;

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

    // Update is called once per frame
    void Update()
    {
        // make sure the Joycon only gets checked if attached
        if (joycons.Count > 0)
        {
            Joycon j = joycons[jc_ind];

            stick = j.GetStick();
            gyro = j.GetGyro();
            accel = j.GetAccel();
            orientation = j.GetVector();

            EulerRotation eulerrotation = FindFirstObjectByType<EulerRotation>();
            var _EulerRotation = eulerrotation.eulerrotation;

            if (j.GetButtonDown(Joycon.Button.DPAD_RIGHT))
            {
                StartVector = j.GetVector();
                StartAccel = j.GetAccel();
                StartEuler = _EulerRotation;
                //Debug.Log("GetVector" + _j.GetVector);
                //Debug.Log("オイラー" + _EulerRotation);
            }

            //X
            /*
            float value_accel_z = StartVector.z - orientation.z;
            float min_z_joycon = -0.3f;
            float max_z_joycon = 0.3f;
            float min_x_unity = -3f;
            float max_x_unity = 3f;

            var valueToLimit_x = value_accel_z * ((min_z_joycon * max_x_unity) / (max_z_joycon * min_x_unity));//うごかす値
            if (orientation.z > -0.5f && orientation.z < 1.0f || orientation.z > 0.5f && orientation.z < 1.0f)
            {
                valueToLimit_x = value_accel_z * ((min_z_joycon * max_x_unity) / (max_z_joycon * min_x_unity));
            }
            if (valueToLimit_x < min_x_unity)
            {
                valueToLimit_x = min_x_unity; // 最小値未満の場合、最小値に設定
            }
            else if (valueToLimit_x > max_x_unity)
            {
                valueToLimit_x = max_x_unity; // 最大値を超える場合、最大値に設定
            }
            gameObject.transform.position = new Vector3(valueToLimit_x, 0, -5);
            */

            //Y→もう触らない
            float value_accel_x = StartAccel.x - accel.x;
            float min_x_joycon = -1f;
            float max_x_joycon = 1f;
            float min_y_unity = -3f;
            float max_y_unity = 3f;

            var valueToLimit_y = value_accel_x * ((min_x_joycon * max_y_unity) / (max_x_joycon * min_y_unity));//うごかす値

            if (valueToLimit_y < min_y_unity)
            {
                valueToLimit_y = min_y_unity; // 最小値未満の場合、最小値に設定
            }
            else if (valueToLimit_y > max_y_unity)
            {
                valueToLimit_y = max_y_unity; // 最大値を超える場合、最大値に設定
            }
            gameObject.transform.position = new Vector3(0, -valueToLimit_y, -5);
            gameObject.transform.rotation = new Quaternion(0f, 0f, orientation.z, orientation.w);

            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                SceneManager.LoadScene("MainEvent");
            }
        }
    }

}
