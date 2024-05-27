using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GyroController : MonoBehaviour
{
    private float maxRotationY = 10f;
    private float maxRotationX = 4f;
    private Quaternion InicialRot;
    private Quaternion RotationReset;
    private float rreuler;
    public GameObject card;


    private void Start()
    {
        //InicialRot = card.transform.localRotation
        //    //* Quaternion.AngleAxis(-180, Vector3.right)
        //    ;
        //InputSystem.EnableDevice(AttitudeSensor.current);
        //InputSystem.EnableDevice(Accelerometer.current);
        //RotationReset = AttitudeSensor.current.attitude.ReadValue();


    }

    public void OnRotate(InputValue value)
    {
        Vector2 res = value.Get<Vector2>();
        res -= new Vector2(Screen.width / 2, Screen.height / 2);

        Debug.Log(res + " " + Screen.height / 2);

        card.transform.eulerAngles = new Vector3(-Extensions.Remap(res.y, -Screen.height / 2, Screen.height / 2, -maxRotationY, maxRotationY),
            Extensions.Remap(res.x, -Screen.width / 2, Screen.width / 2, -maxRotationX, maxRotationX)
            , 0);

    }





    private void Update()
    {
        // Debug.Log(AttitudeSensor.current.attitude.ReadValue() + " " + AttitudeSensor.current.enabled);
        //Quaternion res = AttitudeSensor.current.attitude.ReadValue();
        //res *= new Quaternion(0f, 0f, 1f, 0f);
        //if (res.x > 0)
        //{
        //    res = new Quaternion(Mathf.Min(res.x, maxRotation), res.y, res.z, res.w);
        //}
        //else
        //{
        //    res = new Quaternion(Mathf.Min(res.x, -maxRotation), res.y, res.z, res.w);
        //}

        //if (res.y > 0)
        //{
        //    res = new Quaternion(res.x, Mathf.Min(res.y, maxRotation), res.z, res.w);
        //}
        //else
        //{
        //    res = new Quaternion(res.x, Mathf.Min(res.y, -maxRotation), res.z, res.w);
        //}
        //RotationReset = Quaternion.Lerp(RotationReset, res, 0.2f);
        //rreuler = Mathf.Lerp(rreuler, Accelerometer.current.acceleration.ReadValue().z * 360f, 0.2f);
        //card.transform.localRotation = res * Quaternion.Inverse(RotationReset) * InicialRot;
        //card.transform.localEulerAngles = new Vector3(InicialRot.eulerAngles.x + ((Accelerometer.current.acceleration.ReadValue().z - rreuler) * 360f), card.transform.localEulerAngles.z, card.transform.localEulerAngles.y);

        //Debug.Log(Accelerometer.current.acceleration.ReadValue());
    }


}

public static class Extensions
{
    public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = from - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}

