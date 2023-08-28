using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{


    public string TGlink;
    public string MMLink;




    public void OpenTG()
    {
        Application.OpenURL(TGlink);
    }
    public void OpenMM()
    {
        Application.OpenURL(MMLink);
    }

}
