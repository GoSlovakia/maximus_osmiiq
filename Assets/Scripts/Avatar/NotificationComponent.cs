using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationComponent : MonoBehaviour
{
    [SerializeField]
    private Image sign;

    private bool _showNotification;
    public bool showNotification
    {
        get => _showNotification;
        set
        {
            _showNotification = value;
            sign.gameObject.SetActive(value);
        }
    }
}
