using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Drawer : MonoBehaviour
{
    //drawer state
    [SerializeField] private Animator m_drawerAnimator;

    public void CloseDrawer()
    {
        if (m_drawerAnimator.GetBool("drawerState") == false)
        {
            m_drawerAnimator.SetBool("drawerState", true);
        }
        else
        {
            m_drawerAnimator.SetBool("drawerState", false);
        }
    }
}
