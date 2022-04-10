using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text InternetModeText;
    public Image InternetModeImg;
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(InternetModeCheckerRoutien());
    }

    IEnumerator InternetModeCheckerRoutien()
    {
      
        while (true)
        {
            if (NetworkChecker.CheckInternetConnection())
            {
                InternetModeText.text = "Online";
                InternetModeImg.color = Color.green;

            }
            else
            {
                InternetModeText.text = "Offline";
                InternetModeImg.color = Color.red;
            }
            yield return new WaitForSeconds(10);
        }


    }

}
