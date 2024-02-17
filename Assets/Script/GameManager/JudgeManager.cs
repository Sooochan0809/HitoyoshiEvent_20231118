using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JudgeManager : MonoBehaviour
{
    private List<Joycon> joycons;
    public int jc_ind = 0;

    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public Quaternion orientation;

    //����
    public bool isCorrectR = false;
    public bool isCorrectL = false;
    public bool isCorrectUp = false;
    //�s����
    public bool inCorrectR;
    public bool inCorrectL;
    public bool inCorrectUp;

    //1��̏����ōς܂�                      
    public bool isCalledOnce = false;

    public RawImage _Pointer;

    void Start()
    {
        joycons = JoyconManager.Instance.j;
        this.enabled = false;
    }
    public void Update()
    {
        var audio = gameObject.GetComponent<AudioList>();
        var gamemanager = gameObject.GetComponent<GameManager>();

        _Pointer = RawImage.FindAnyObjectByType<RawImage>();

        Joycon j = joycons[jc_ind];
        accel = j.GetAccel();
        orientation = j.GetVector();

        float valueX = gamemanager.startposX - _Pointer.rectTransform.anchoredPosition.x;
        float valueY = gamemanager.startposY - _Pointer.rectTransform.anchoredPosition.y;

        //�E
        //����ɒ����K�v
        if (!isCalledOnce && valueX < -100f && valueX > -300f || Input.GetKey(KeyCode.D))
        {
            isCalledOnce = true;
            Sound();

            //���딻��
            isCorrectR = (Random.Range(0, 2) == 0);

            if (isCorrectR)
            {
                gamemanager.isCorrectR = true;
                Debug.Log("���茋��: �E�̐���");
                StartCoroutine(DelayCoroutine());
            }
            else
            {
                Debug.Log("���茋��: �E�̕s����");
                //audio.InCorrect();
                inCorrectR = true;
            }
            this.enabled = false;
        }
        //��
        //����ɒ����K�v
        if (!isCalledOnce && valueX > 100f && valueX < 300f || Input.GetKey(KeyCode.A))
        {
            isCalledOnce = true;
            Sound();

            //���딻��
            isCorrectL = (Random.Range(0, 2) == 0);

            if (isCorrectL)
            {
                gamemanager.isCorrectL = true;
                Debug.Log("���茋��: ���̐���");
                StartCoroutine(DelayCoroutine());
            }
            else
            {
                Debug.Log("���茋��: ���̕s����");
                inCorrectL = true;
            }
            this.enabled = false;
        }
        //��
        if (!isCalledOnce && valueY < -50f && valueX > -200f && valueX < 100f || Input.GetKey(KeyCode.W))
        {
            isCalledOnce = true;
            Sound();

            //���딻��
            isCorrectUp = (Random.Range(0, 2) == 0);

            if (isCorrectUp)
            {
                gamemanager.isCorrectUp = true;
                Debug.Log("���茋��: ��̐���");
                StartCoroutine(DelayCoroutine());
            }
            else
            {
                Debug.Log("���茋��: ��̕s����");
                inCorrectUp = true;
            }
            this.enabled = false;
        }
        IEnumerator DelayCoroutine()
        {
            // 10�t���[���҂�
            for (var i = 0; i < 70; i++)
            {
                yield return null;
            }
            gamemanager.LoadRandomScene();
        }

        void Sound()
        {
            if (gamemanager.red)
            {
                audio.Red2();
            }
            else if (gamemanager.blue)
            {
                audio.Blue2();
            }
            else if (gamemanager.yellow)
            {
                audio.Yellow2();
            }
            else
            {
                audio.Debug2();
            }
        }
    }
}