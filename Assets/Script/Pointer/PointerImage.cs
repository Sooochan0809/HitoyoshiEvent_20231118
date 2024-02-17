using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerImage : MonoBehaviour
{
    private List<Joycon> joycons;
    public int jc_ind = 0;

    public Transform SphereTransfrom;
    private RectTransform rectTransform;
   
    private float scale = 8.0f; // ”CˆÓ‚Ì”{—¦A‘å‚«‚­‚·‚é‚Æ¬‚³‚¢“®‚«‚Å‘å‚«‚­“®‚­

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        joycons = JoyconManager.Instance.j;
    }

    void Update()
    {
        float x = SphereTransfrom.position.x * scale;
        float y = SphereTransfrom.position.z * -scale;
        rectTransform.anchoredPosition = new Vector3(x, y, 0);
    }
}