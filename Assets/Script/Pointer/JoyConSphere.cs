using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyConSphere : MonoBehaviour
{
    public GameObject _Cube;
    public GameObject _Plane;
    public GameObject _Sphere;

    // Update is called once per frame
    void Update()
    {
        var n = _Plane.transform.up;
        var x = _Plane.transform.position;
        var x0 = _Cube.transform.position;
        var m = _Cube.transform.forward;
        var h = Vector3.Dot(n, x);

        var intersectPoint = x0 + ((h - Vector3.Dot(n, x0)) / (Vector3.Dot(n, m))) * m;

        _Sphere.transform.position = new Vector3 (intersectPoint.x, intersectPoint.y,intersectPoint.z);
    }
}
