using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class RayCastManager : MonoBehaviour
{
    public GameObject _Model1;
    public GameObject _Model2;
    public GameObject _Model3;

    private GameObject _GameManager;

    public bool isCalledOnce = false;
    public bool isCalledOnce2 = false;

    private List<Joycon> joycons;
    public int jc_ind = 0;

    // Start is called before the first frame update
    void Start()
    {
        joycons = JoyconManager.Instance.j;
        _GameManager = GameObject.Find("GameManager");
        SoundController();
    }

    // Update is called once per frame
    void Update()
    {
        // 2F後に呼び出す
        Observable.TimerFrame(20)
            .Subscribe(_ => DelayMethod())
            .AddTo(this);

        // 2F後のFixedUpdateで実行する
        Observable.TimerFrame(20, FrameCountType.Update)
            .Subscribe(_ => DelayMethod())
            .AddTo(this);

        _GameManager = GameObject.Find("GameManager");
        var gamemanager = _GameManager.GetComponent<GameManager>();
        var movingright = _GameManager.GetComponent<MovingLightManager>();

        SceneTransition();

        StartCoroutine(DelayCoroutine());

    }
    private void DelayMethod()
    {
        _GameManager = GameObject.Find("GameManager");
        var audio = _GameManager.GetComponent<AudioList>();

        // Rayを発射する位置と方向を決定
        Vector3 origin = transform.position; // GameObjectの位置を起点に設定
        Vector3 direction = transform.forward; // GameObjectの前方方向を設定
                                               // Rayを可視化（線を引く）
        Debug.DrawRay(origin, direction * 20f, Color.red);
        try
        {
            _GameManager = GameObject.Find("GameManager");
            var gamemanager = _GameManager.GetComponent<GameManager>();

            var anime1 = _Model1.GetComponent<Animator>();
            var anime2 = _Model2.GetComponent<Animator>();
            var anime3 = _Model3.GetComponent<Animator>();

            // Rayを発射
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;
            // レイが何かにヒットした場合の処理
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                //もしhitのタグが"Target"と一致していた場合．．．の処理内容
                if (!isCalledOnce && hit.collider.CompareTag("Target"))
                {
                    isCalledOnce = true;

                    if (gamemanager.CorrectCounter < 10)
                    {
                        audio.FindTarget1();
                    }
                    else if (gamemanager.CorrectCounter == 10)
                    {
                        audio.FindTarget2();
                    }

                    anime1.SetTrigger("Animation1");
                    anime2.SetTrigger("Animation1");
                    anime3.SetTrigger("Animation1");
                }
            }
        }
        catch
        {
            return;
        }
    }
    private void SceneTransition()
    {
        try
        {
            _GameManager = GameObject.Find("GameManager");
            var gamemanager = _GameManager.GetComponent<GameManager>();
            var audio = _GameManager.GetComponent<AudioList>();

            var anime1 = _Model1.GetComponent<Animator>();
            var anime2 = _Model2.GetComponent<Animator>();
            var anime3 = _Model3.GetComponent<Animator>();

            if (anime1.GetCurrentAnimatorStateInfo(0).normalizedTime > 7f
                            || anime2.GetCurrentAnimatorStateInfo(0).normalizedTime > 7f
                            || anime3.GetCurrentAnimatorStateInfo(0).normalizedTime > 7f)
            {
                //10回繰り返したら
                if (!isCalledOnce2)
                {
                    isCalledOnce2 = true;
                    //ここで鬼の情報を渡す
                    if (gamemanager.randomNumber == 1)
                    {
                        audio.Input();
                        gamemanager.red = true;
                        gamemanager.blue = false;
                        gamemanager.yellow = false;
                    }
                    else if (gamemanager.randomNumber == 2)
                    {
                        audio.Input();
                        gamemanager.blue = true;
                        gamemanager.red = false;
                        gamemanager.yellow = false;
                    }
                    else if (gamemanager.randomNumber == 3)
                    {
                        audio.Input();
                        gamemanager.yellow = true;
                        gamemanager.red = false;
                        gamemanager.blue = false;
                    }
                    StartCoroutine(DelayCoroutine());
                }
            }
        }
        catch
        {
            return;
        }
        IEnumerator DelayCoroutine()
        {
            // 10フレーム待つ
            for (var i = 0; i < 10; i++)
            {
                yield return null;
            }

            LoadMainEvent();
        }

        void LoadMainEvent()
        {
            _GameManager = GameObject.Find("GameManager");
            var gamemanager = _GameManager.GetComponent<GameManager>();

            int count = gamemanager.SceneCounter;

            if (count == 3)
            {
                SceneManager.LoadSceneAsync("MainEvent");
                gamemanager.SceneCounter = 0;
                gamemanager.sceneOptions = new List<string> { "Right", "Front", "Up" };
            }
            else
            {
                SceneManager.LoadSceneAsync("MainEvent");
            }
        }
    }
    private void SoundController()
    {
      
        _GameManager = GameObject.Find("GameManager");
        var audio = _GameManager.GetComponent<AudioList>();

        if (SceneManager.GetActiveScene().name == "Right")
        {
            audio.RightNav();
        }
        if (SceneManager.GetActiveScene().name == "Front")
        {
            audio.FrontNav();
        }
        if (SceneManager.GetActiveScene().name == "Up")
        {
            audio.UpNav();
        }
    }

    void ModelChange()
    {
        _GameManager = GameObject.Find("GameManager");
        var gamemanager = _GameManager.GetComponent<GameManager>();

        if (gamemanager.randomNumber == 1)
        {
            _Model1.SetActive(true);
        }
        else if (gamemanager.randomNumber == 2)
        {
            _Model2.SetActive(true);
        }
        else if (gamemanager.randomNumber == 3)
        {
            _Model3.SetActive(true);
        }

    }
    IEnumerator DelayCoroutine()
    {
        // 10フレーム待つ
        for (var i = 0; i < 80; i++)
        {
            yield return null;
        }
        ModelChange();
    }
}