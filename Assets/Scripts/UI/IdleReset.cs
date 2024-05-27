using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class IdleReset : MonoBehaviour
{
    public bool QuioskeMode = true;
    [SerializeField]
    private float Timer = 20;

    private float currentTime = 0;

    private Vector2 LastMousePos;

    // Start is called before the first frame update
    private void Update()
    {
        if (QuioskeMode)
        {
            if (LastMousePos == null || LastMousePos != Mouse.current.position.ReadValue() || Mouse.current.leftButton.isPressed)
            {
                LastMousePos = Mouse.current.position.ReadValue();
                currentTime = 0;
            }
            else
            {
                currentTime += Time.deltaTime;
                if (currentTime >= Timer)
                {
                    currentTime = 0;
                    AvatarReader.LoadDefaultAvatar();
                }
            }
           // Debug.Log("Timer " + currentTime);
        }
    }


}
