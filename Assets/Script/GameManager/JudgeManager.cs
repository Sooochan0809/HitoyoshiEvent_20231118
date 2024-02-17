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

    //正解
    public bool isCorrectR = false;
    public bool isCorrectL = false;
    public bool isCorrectUp = false;
    //不正解
    public bool inCorrectR;
    public bool inCorrectL;
    public bool inCorrectUp;

    //1回の処理で済ます                      
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

        //右
        //判定に調整必要
        if (!isCalledOnce && valueX < -100f && valueX > -300f || Input.GetKey(KeyCode.D))
        {
            isCalledOnce = true;
            Sound();

            //正誤判定
            isCorrectR = (Random.Range(0, 2) == 0);

            if (isCorrectR)
            {
                gamemanager.isCorrectR = true;
                Debug.Log("判定結果: 右の正解");
                StartCoroutine(DelayCoroutine());
            }
            else
            {
                Debug.Log("判定結果: 右の不正解");
                //audio.InCorrect();
                inCorrectR = true;
            }
            this.enabled = false;
        }
        //左
        //判定に調整必要
        if (!isCalledOnce && valueX > 100f && valueX < 300f || Input.GetKey(KeyCode.A))
        {
            isCalledOnce = true;
            Sound();

            //正誤判定
            isCorrectL = (Random.Range(0, 2) == 0);

            if (isCorrectL)
            {
                gamemanager.isCorrectL = true;
                Debug.Log("判定結果: 左の正解");
                StartCoroutine(DelayCoroutine());
            }
            else
            {
                Debug.Log("判定結果: 左の不正解");
                inCorrectL = true;
            }
            this.enabled = false;
        }
        //上
        if (!isCalledOnce && valueY < -50f && valueX > -200f && valueX < 100f || Input.GetKey(KeyCode.W))
        {
            isCalledOnce = true;
            Sound();

            //正誤判定
            isCorrectUp = (Random.Range(0, 2) == 0);

            if (isCorrectUp)
            {
                gamemanager.isCorrectUp = true;
                Debug.Log("判定結果: 上の正解");
                StartCoroutine(DelayCoroutine());
            }
            else
            {
                Debug.Log("判定結果: 上の不正解");
                inCorrectUp = true;
            }
            this.enabled = false;
        }
        IEnumerator DelayCoroutine()
        {
            // 10フレーム待つ
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