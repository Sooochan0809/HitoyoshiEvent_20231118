using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JoyconLog : MonoBehaviour
{
    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    private Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonR;

    private void Start()
    {
        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconR = m_joycons.Find(c => !c.isLeft);
    }

    private void Update()
    {
        m_pressedButtonR = null;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        foreach (var button in m_buttons)
        {
            if (m_joyconR.GetButton(button))
            {
                m_pressedButtonR = button;
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            m_joyconR.SetRumble(160, 320, 0.6f, 200);
        }
    }

    private void OnGUI()
    {
        var style = GUI.skin.GetStyle("label");
        style.fontSize = 20;

        if (m_joycons == null || m_joycons.Count <= 0)
        {
            ShowCenteredLabel("Joy-Con が接続されていません");
            return;
        }

        if (!m_joycons.Any(c => !c.isLeft))
        {
            ShowCenteredLabel("Joy-Con (R) が接続されていません");
            return;
        }

        foreach (var joycon in m_joycons)
        {
            if (!joycon.isLeft)
            {
                //入力Class
                var name = "Joy-Con (R)";
                var button = m_pressedButtonR;
                var stick = joycon.GetStick();
                var gyro = joycon.GetGyro();
                var accel = joycon.GetAccel();
                var orientation = joycon.GetVector();
                EulerRotation eulerrotation= FindFirstObjectByType<EulerRotation>();
                var euler_rotation = eulerrotation.eulerrotation;
                //入力Class

                var centeredX = (Screen.width + 100) / 2; // Centered horizontally
                var centeredY = (Screen.height - 70) / 2; // Centered vertically

                GUILayout.BeginArea(new Rect(centeredX, centeredY, 960, 240));
                GUILayout.Label(name);
                GUILayout.Label("押されているボタン：" + button);
                GUILayout.Label("ジャイロ：" + gyro.ToString("F1"));
                GUILayout.Label("加速度：" + accel.ToString("F1"));
                GUILayout.Label("傾き：" + orientation.ToString("F1"));
                GUILayout.Label("オイラー角：" + euler_rotation.ToString("F1"));
                GUILayout.EndArea();
            }
        }
    }

    private void ShowCenteredLabel(string text)
    {
        var centeredX = (Screen.width + 270) / 2; // Centered horizontally
        var centeredY = (Screen.height + 24) / 2;  // Centered vertically
        GUI.Label(new Rect(centeredX, centeredY, 960, 24), text);
    }


}