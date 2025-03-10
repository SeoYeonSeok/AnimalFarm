using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class LogInProcess : MonoBehaviour
{
    public GameObject panel_LogIn;
    public TMP_InputField inf_Username;
    public TMP_InputField inf_PW;

    public void PressLogInBtn()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }

    public void PressSignInBtn()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name);
    }
}
