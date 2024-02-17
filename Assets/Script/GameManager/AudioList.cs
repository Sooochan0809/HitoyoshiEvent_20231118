using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    public AudioClip _Red1;
    public AudioClip _Red2;
    public AudioClip _Yellow1;
    public AudioClip _Yellow2;
    public AudioClip _Blue1;
    public AudioClip _Blue2;

    public AudioClip _RightNav;
    public AudioClip _ForwardNav;
    public AudioClip _UpNav;

    public AudioClip _IsCorrect;
    public AudioClip _InCorrect;

    public AudioClip _Input;

    public AudioClip _FindTarget1;
    public AudioClip _FindTarget2;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //MainEvebt
    public void Debug1()
    {
        audioSource.PlayOneShot(_Red1);
    }
    public void Debug2()
    {
        audioSource.PlayOneShot(_Red2);
    }
    public void Red1()
    {
        audioSource.PlayOneShot(_Red1);
    }
    public void Red2()
    {
        audioSource.PlayOneShot(_Red2);
    }
    public void Blue1()
    {
        audioSource.PlayOneShot(_Blue1);
    }
    public void Blue2()
    {
        audioSource.PlayOneShot(_Blue2);
    }
    public void Yellow1()
    {
        audioSource.PlayOneShot(_Yellow1);
    }
    public void Yellow2()
    {
        audioSource.PlayOneShot(_Yellow2);
    }


    public void RightNav()
    {
        audioSource.PlayOneShot(_RightNav);
    }
    public void FrontNav()
    {
        audioSource.PlayOneShot(_ForwardNav);
    }
    public void UpNav()
    {
        audioSource.PlayOneShot(_UpNav);

    }

    public void IsCorrect()
    {
        audioSource.PlayOneShot(_IsCorrect);
    }
    public void InCorrect()
    {
        audioSource.PlayOneShot(_InCorrect);
    }
    public void Input()
    {
        audioSource.PlayOneShot(_Input);
    }
    public void FindTarget1()
    {
        audioSource.PlayOneShot(_FindTarget1);
    }
    public void FindTarget2()
    {
        audioSource.PlayOneShot(_FindTarget2);
    }
}
