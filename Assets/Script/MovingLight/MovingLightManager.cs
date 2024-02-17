using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using System.Collections;

public class MovingLightManager : MonoBehaviour
{
    public bool LaunchTrigger;
    public GameObject gobj;
    public Slider slider1;
    public Slider slider2;
    public Slider slider3;
    public Slider slider4;
    public Slider slider5;
    public Slider slider6;
    private SerialPort serial;
    byte[] dmxData = new byte[513];

    private float maxvalue;
    private float minvalue;
    public bool _tag;
    public bool _tag1;
    public bool _tag2;
    public bool _tag3;

    void Start()
    {
        serial = new SerialPort("COM3", 250000);
        serial.DataBits = 8;
        serial.Parity = Parity.None;
        serial.StopBits = StopBits.Two;
        serial.Open();

        LaunchTrigger = false;

        Observable
           .Interval(System.TimeSpan.FromSeconds(1 / 30f))
           .ObserveOn(Scheduler.ThreadPool)
           .Subscribe(_ => SendDMX())
           .AddTo(this);

    }

    private void SendDMX()
    {
        serial.BreakState = true;
        MicroSecDelay(176);
        serial.BreakState = false;
        MicroSecDelay(16);
        dmxData[1] = (byte)(42.5f + slider1.value * 85f);//ch1に0-255の値を設定
        dmxData[2] = (byte)(slider2.value * 128f);//ch1に0-255の値を設定
        dmxData[3] = (byte)(slider3.value * 255f);//ch1に0-255の値を設定
        dmxData[4] = (byte)(slider4.value * 255f);//ch1に0-255の値を設定
        dmxData[5] = (byte)(slider5.value * 255f);//ch1に0-255の値を設定
        dmxData[6] = (byte)(slider6.value * 255f);//ch1に0-255の値を設定

        serial.Write(dmxData, 0, dmxData.Length);
    }

    private float launchTime = 0.0f;
    private void Update()
    {
        if (LaunchTrigger)
        {
            if (launchTime == 0.0f)
                slider6.value = 0.3f;
            launchTime += Time.deltaTime;
            if (launchTime > 5.0f)
            {
                LaunchTrigger = false;
                launchTime = 0.0f;
                slider6.value = 0.0f;
            }
            slider2.value = launchTime * launchTime / 25.0f;
        }
        try
        {
            slider1 = GameObject.Find("Canvas(MovingLightSlider)/Slider").GetComponent<Slider>();
            slider2 = GameObject.Find("Canvas(MovingLightSlider)/Slider (1)").GetComponent<Slider>();
            slider3 = GameObject.Find("Canvas(MovingLightSlider)/Slider (2)").GetComponent<Slider>();
            slider4 = GameObject.Find("Canvas(MovingLightSlider)/Slider (3)").GetComponent<Slider>();
            slider5 = GameObject.Find("Canvas(MovingLightSlider)/Slider (4)").GetComponent<Slider>();
            slider6 = GameObject.Find("Canvas(MovingLightSlider)/Slider (5)").GetComponent<Slider>();
        }
        catch
        {
            return;
        }

    }

    public void SetLightColor(string colName)
    {
        float val = colName switch
        {
            "red" => 0.05f,
            "yellow" => 0.10f,
            "green" => 0.17f,
            "orange" => 0.20f,
            "blue" => 0.30f,
            _ => 0.0f,
        };
        slider3.value = val;
    }

    private void MicroSecDelay(int time)
    {
        var sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        while (true)
        {
            sw.Stop();
            var elapsed = sw.ElapsedTicks * 1000 * 1000 / System.Diagnostics.Stopwatch.Frequency;
            if (elapsed > time)
            {
                break;
            }
            else
            {
                sw.Start();
            }
        }
    }
    private void OnDestroy()
    {
        serial?.Close();
        serial?.Dispose();
    }

    //スタンバイモード
    public void MLStandby()
    {
        slider1.value = 0.5f;
        slider2.value = 0.1f;
        slider3.value = 0f;
        slider4.value = 0f;
        slider5.value = 0f;
        LightPowerChanger();
    }
    public void MLStandbyRed()
    {
        slider1.value = 0.5f;
        slider2.value = 0.1f;
        SetLightColor("red");
        slider4.value = 0f;
        slider5.value = 0f;
        LightPowerChanger();
    }
    public void MLStandbyBlue()
    {
        slider1.value = 0.5f;
        slider2.value = 0.1f;
        SetLightColor("blue");
        slider4.value = 0f;
        slider5.value = 0f;
        LightPowerChanger();
    }
    public void MLStandbyYellow()
    {
        slider1.value = 0.5f;
        slider2.value = 0.1f;
        SetLightColor("yellow");
        slider4.value = 0f;
        slider5.value = 0f;
        LightPowerChanger();
    }
    void LightPowerChanger()
    {
        maxvalue = 1.0f;
        minvalue = 0f;
        float val = 0.025f;

        if (!_tag && slider6.value <= maxvalue)
        {
            slider6.value = slider6.value + val;
        }
        if (_tag && slider6.value >= minvalue)
        {
            slider6.value = slider6.value - val;
        }
        if (slider6.value == 1)
        {
            _tag = true;
        }
        else if (slider6.value == 0)
        {
            _tag = false;
        }
    }

    //正解
    public void MLIsCorrectR()
    {
        slider1.value = 0.25f;
        slider2.value = 0.1f;
        color();
        slider4.value = 0f;
        slider5.value = 0.8f;
        slider6.value = 1.0f;
    }
    public void MLIsCorrectL()
    {
        slider1.value = 0.75f;
        slider2.value = 0.1f;
        color();
        slider4.value = 0f;
        slider5.value = 0.8f;
        slider6.value = 1.0f;
    }
    public void MLIsCorrectUp()
    {
        slider1.value = 0.5f;
        slider2.value = 0.6f;
        color();
        slider4.value = 0f;
        slider5.value = 0.8f;
        slider6.value = 1.0f;
    }

    //不正解
    public void MLInCorrectR()
    {
        slider1.value = 0.75f;
        slider2.value = 0.1f;
        color();
        slider4.value = 0f;
        slider5.value = 0f;
        slider6.value = 1.0f;

        if (!_tag3)
        {
            _tag3 = true;
            StartCoroutine(DelayCoroutine());
        }
        IEnumerator DelayCoroutine()
        {
            // 10フレーム待つ
            for (var i = 0; i < 40; i++)
            {
                yield return null;
            }
            var audio = gameObject.GetComponent<AudioList>();
            audio.InCorrect();
        }
    }
    public void MLInCorrectL()
    {
        slider1.value = 0.25f;
        slider2.value = 0.1f;
        color();
        slider4.value = 0f;
        slider5.value = 0f;
        slider6.value = 1.0f;

        if (!_tag3)
        {
            _tag3 = true;
            StartCoroutine(DelayCoroutine());
        }
        IEnumerator DelayCoroutine()
        {
            // 10フレーム待つ
            for (var i = 0; i < 40; i++)
            {
                yield return null;
            }
            var audio = gameObject.GetComponent<AudioList>();
            audio.InCorrect();
        }
    }
    public void MLInCorrectUp()
    {
        slider1.value = 0.25f;
        slider2.value = 0.1f;
        color();
        slider4.value = 0f;
        slider5.value = 0f;
        slider6.value = 1.0f;
        if (!_tag3)
        {
            _tag3 = true;
            StartCoroutine(DelayCoroutine());
        }

        IEnumerator DelayCoroutine()
        {
            // 10フレーム待つ
            for (var i = 0; i < 40; i++)
            {
                yield return null;
            }
            var audio = gameObject.GetComponent<AudioList>();
            audio.InCorrect();
        }
    }

    public void MLInCorrectR2()
    {
        var audio = gameObject.GetComponent<AudioList>();
        color();

        if (!_tag1 && slider1.value < 0.8f)
        {
            slider1.value = slider1.value - 0.02f;//0,01
            slider5.value = 0.8f;
            Debug.Log(slider1.value);

        }
        if (slider1.value < 0.30f)//0.25
        {
            _tag1 = true;
        }
        if (_tag1 && slider1.value < 0.8f)
        {

            while (slider1.value <= 0.75f)
            {
                Thread.Sleep(5);
                slider1.value = slider1.value + 0.01f;
                slider5.value = 0f;
                Debug.Log(slider1.value);
            }

            if (!_tag2 && slider1.value > 0.75f)
            {
                _tag2 = true;
                audio.InCorrect();
            }
            slider2.value = 0.1f;
            slider4.value = 0f;
            slider6.value = 1.0f;
        }
    }

    public void MLInCorrectL2()
    {
        var audio = gameObject.GetComponent<AudioList>();

        color();

        if (!_tag1 && slider1.value < 0.8f)
        {
            slider1.value = slider1.value + 0.02f;//0.01
            slider5.value = 0.8f;
            Debug.Log(_tag1);
        }
        if (slider1.value >= 0.75f)//0,75
        {
            _tag1 = true;
        }
        if (_tag1 && slider1.value < 0.8f)
        {

            while (slider1.value > 0.25)
            {
                Thread.Sleep(5);
                slider1.value -= 0.01f;
                slider5.value = 0f;
                Debug.Log(slider1.value);
            }

            if (!_tag2 && slider1.value < 0.25f)
            {
                _tag2 = true;
                audio.InCorrect();
            }
        }
        slider2.value = 0.1f;
        slider4.value = 0f;
        slider6.value = 1.0f;
    }

    public void color()
    {
        var gamemanager = gameObject.GetComponent<GameManager>();

        if (gamemanager.red)
        {
            SetLightColor("red");
        }
        else if (gamemanager.blue)
        {
            SetLightColor("blue");
        }
        else if (gamemanager.yellow)
        {
            SetLightColor("yellow");
        }
        else
        {
            slider3.value = 0f;
        }
    }
}