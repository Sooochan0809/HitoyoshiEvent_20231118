using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerRotation : MonoBehaviour
{
    private List<Joycon> m_joycons;
    private Joycon m_joyconR;
    private Quaternion initialRotation;
    //���̃X�N���v�g��deltaRotation���g����悤�ɂ���
    public Vector3 eulerrotation;

    // Start is called before the first frame update
    void Start()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconR = m_joycons.Find(c => !c.isLeft);
        Quaternion Vector = m_joyconR.GetVector();

        initialRotation = Vector;
        // Joy-Con�̏����������擾
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion Vector = m_joyconR.GetVector();

        // Joy-Con�̌������擾
        Quaternion currentRotation = Vector;

        // Joy-Con�̌����̕ω����v�Z
        Quaternion deltaRotation = Quaternion.Inverse(initialRotation) * currentRotation;
        eulerrotation = deltaRotation.eulerAngles;

        //�f�o�b�N
        if (m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            //Debug.Log("Euler��" + eulerrotation.eulerAngles);
        }
        // �����̕ω����g���ĉ������s�����Ƃ��ł��܂�
        // �Ⴆ�΁AdeltaRotation.eulerAngles�ŉ�]�̃I�C���[�p���擾�ł��܂�
    }
}
