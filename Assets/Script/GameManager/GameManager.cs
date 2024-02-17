using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<Joycon> joycons;
    public int jc_ind = 0;

    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public Quaternion orientation;

    //正解
    public bool isCorrectR;
    public bool isCorrectL;
    public bool isCorrectUp;
    //不正解
    public bool inCorrectR;
    public bool inCorrectL;
    public bool inCorrectUp;

    public int consecutiveTrueCount;
    public int SceneCounter;
    public int CorrectCounter;

    public List<string> sceneOptions = new List<string> { "Right", "Front", "Up" };
    private string lastSelectedScene;

    //1回の処理で済ます                      
    public bool isCalledOnce;
    public bool isCalledOnce2;
    public bool stoper;

    public bool EffectChose;

    public GameObject _Model1;
    public GameObject _Model2;
    public GameObject _Model3;

    public bool red;
    public bool blue;
    public bool yellow;

    public RawImage _Pointer;

    public float startposX;
    public float startposY;

    public int randomNumber;

    public GameObject _RayCastManager;

    //ロードするシーンのリスト
    public List<string> scenesToLoad;

    void Start()
    {
        joycons = JoyconManager.Instance.j;
    }
    public void Update()
    {
        var movingright = gameObject.GetComponent<MovingLightManager>();
        var judge = gameObject.GetComponent<JudgeManager>();
        var audio = gameObject.GetComponent<AudioList>();
        _Pointer = RawImage.FindAnyObjectByType<RawImage>();

        if (SceneManager.GetActiveScene().name == "MainEvent")
        {
            Joycon j = joycons[jc_ind];

            if (!stoper && j.GetButtonDown(Joycon.Button.SHOULDER_2) || Input.GetKey(KeyCode.Space))
            {
                stoper = true;

                if (red)
                {
                    audio.Red1();
                    blue = false;
                    yellow = false;
                }
                else if (blue)
                {
                    audio.Blue1();
                    red = false;
                    yellow = false;
                }
                else if (yellow)
                {
                    audio.Yellow1();
                    blue = false;
                    red = false;
                }
                else
                {
                    audio.Debug1();
                }

                j.Recenter();

                movingright._tag = false;
                movingright._tag1 = false;
                movingright._tag2 = false;
                movingright._tag3 = false;

                EffectChose = false;

                isCalledOnce = false;
                isCalledOnce2 = false;

                //判定のリセット
                isCorrectR = false;
                isCorrectL = false;
                isCorrectUp = false;

                judge.isCorrectR = false;
                judge.isCorrectL = false;
                judge.isCorrectUp = false;

                judge.inCorrectR = false;
                judge.inCorrectL = false;
                judge.inCorrectUp = false;

                judge.isCalledOnce = false;

                StartCoroutine(DelayCoroutine());
            }

            IEnumerator DelayCoroutine()
            {
                // 10フレーム待つ
                for (var i = 0; i < 10; i++)
                {
                    yield return null;
                }

                //判定を起動
                judge.enabled = true;
                //初期値設定
                startposX = _Pointer.rectTransform.anchoredPosition.x;
                startposY = _Pointer.rectTransform.anchoredPosition.y;
            }

            if (!isCalledOnce && judge.isCorrectR || judge.isCorrectL || judge.isCorrectUp)
            {
                audio.IsCorrect();
                judge.isCorrectR = false;
                judge.isCorrectL = false;
                judge.isCorrectUp = false;
                consecutiveTrueCount++;
                SceneCounter++;
                CorrectCounter++;
                if (CorrectCounter == 11)
                {
                    CorrectCounter = 0;
                }
            }
            else if (judge.inCorrectR || judge.inCorrectL || judge.inCorrectUp)
            {
                consecutiveTrueCount = 0;
            }

            if (isCorrectR)
            {
                movingright.MLIsCorrectR();
            }
            else if (isCorrectL)
            {
                movingright.MLIsCorrectL();
            }
            else if (isCorrectUp)
            {
                movingright.MLIsCorrectUp();
            }

            else if (judge.inCorrectR)
            {
                if (!isCalledOnce)
                {
                    EffectChose = (Random.Range(0, 2) == 0);
                    isCalledOnce = true;
                }
                if (EffectChose)
                {
                    movingright.MLInCorrectR2();
                }
                else
                {
                    movingright.MLInCorrectR();
                }

                StartCoroutine(DelayCoroutine2());
            }
            else if (judge.inCorrectL)
            {

                if (!isCalledOnce)
                {
                    EffectChose = (Random.Range(0, 2) == 0);
                    isCalledOnce = true;
                }
                if (EffectChose)
                {
                    movingright.MLInCorrectL2();
                }
                else
                {
                    movingright.MLInCorrectL();
                }

                StartCoroutine(DelayCoroutine2());
            }
            else if (judge.inCorrectUp)
            {
                if (!isCalledOnce)
                {
                    EffectChose = (Random.Range(0, 2) == 0);
                    isCalledOnce = true;
                }
                if (EffectChose)
                {
                    movingright.MLInCorrectUp();
                }
                else
                {
                    movingright.MLInCorrectR();
                }

                StartCoroutine(DelayCoroutine2());
            }
            else
            {
                Stanby();
            }
            IEnumerator DelayCoroutine2()
            {
                // 10フレーム待つ
                for (var i = 0; i < 50; i++)
                {
                    yield return null;
                }
                //Stanby();
                stoper = false;
            }
        }

        if (SceneManager.GetActiveScene().name == "Right" || SceneManager.GetActiveScene().name == "Up" || SceneManager.GetActiveScene().name == "Front")
        {
            isCorrectR = false;
            isCorrectL = false;
            isCorrectUp = false;
            inCorrectR = false;
            inCorrectL = false;
            inCorrectUp = false;
            stoper = false;

            Stanby();

            if (!isCalledOnce2)
            {
                isCalledOnce2 = true;
                randomNumber = Random.Range(1, 4);
            }
        }
    }
    public void LoadRandomScene()
    {
        // 選択可能なシーンが残っているか確認
        if (sceneOptions.Count > 0)
        {
            // 前回選ばれたシーン以外をランダムに選ぶ
            string nextScene = lastSelectedScene;
            while (nextScene == lastSelectedScene)
            {
                nextScene = sceneOptions[Random.Range(0, sceneOptions.Count)];
            }
            // 選ばれたシーンをリストから削除
            sceneOptions.Remove(nextScene);
            SceneManager.LoadSceneAsync(nextScene);
            lastSelectedScene = nextScene;
        }
    }
    public void Stanby()
    {
        var movingright = gameObject.GetComponent<MovingLightManager>();

        if (red)
        {
            movingright.MLStandbyRed();
            blue = false;
            yellow = false;

        }
        else if (blue)
        {
            movingright.MLStandbyBlue();
            red = false;
            yellow = false;
        }
        else if (yellow)
        {
            movingright.MLStandbyYellow();
            blue = false;
            red = false;
        }
        else
        {
            movingright.MLStandby();
        }
    }
}