using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerRotation : MonoBehaviour
{
    private List<Joycon> m_joycons;
    private Joycon m_joyconR;
    private Quaternion initialRotation;
    //他のスクリプトでdeltaRotationを使えるようにする
    public Vector3 eulerrotation;

    // Start is called before the first frame update
    void Start()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconR = m_joycons.Find(c => !c.isLeft);
        Quaternion Vector = m_joyconR.GetVector();

        initialRotation = Vector;
        // Joy-Conの初期向きを取得
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion Vector = m_joyconR.GetVector();

        // Joy-Conの向きを取得
        Quaternion currentRotation = Vector;

        // Joy-Conの向きの変化を計算
        Quaternion deltaRotation = Quaternion.Inverse(initialRotation) * currentRotation;
        eulerrotation = deltaRotation.eulerAngles;

        //デバック
        if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            //Debug.Log("Euler＝" + eulerrotation.eulerAngles);
        }
        // 向きの変化を使って何かを行うことができます
        // 例えば、deltaRotation.eulerAnglesで回転のオイラー角を取得できます
    }
}
