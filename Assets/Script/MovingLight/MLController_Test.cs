using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.UI;
using UniRx;

public class MLController_Test : MonoBehaviour
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
        /*var hValue = agl.y;
        var vValue = agl.x;
        
        dmxData[1] = (byte)(Map(hValue, 0f, 180f, 0f, 127));
        dmxData[2] = (byte)(Map(vValue, 0f, 90f, 0f, 127));
        */
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

        SetLightColor("yellow");
        var _slider = slider1.value = 0.5f;
        var _slider1 = slider2.value = 0.1f;
        //var _slider2 = slider3.value;
        var _slider3 = slider4.value = 0f;
        var _slider4 = slider5.value = 0f;
        var _slider5 = slider6.value = 1.0f;

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
}