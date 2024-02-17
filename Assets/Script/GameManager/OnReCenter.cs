using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class OnReCenter : MonoBehaviour
{
    public GameObject _ReCenter;
    private void Awake()
    {
        _ReCenter.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        // 2F後に呼び出す
        Observable.TimerFrame(30)
            .Subscribe(_ => DelayMethod())
            .AddTo(this);

        // 2F後のFixedUpdateで実行する
        Observable.TimerFrame(30, FrameCountType.Update)
            .Subscribe(_ => DelayMethod())
            .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void DelayMethod()
    {
        _ReCenter.SetActive(true);
    }
}
