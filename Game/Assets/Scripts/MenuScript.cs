using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour 
{
    public Image textbox;
    public Text textDisplay;

    public void LoadScene(string sceneName)
	{
        /*if (textDisplay.text == "")
        {
            textbox.color = new Color(255, 77, 77);
            //ColorBlock cb = textbox.color;
            //cb.normalColor = new Color(255, 120, 120);
            //textbox.colors = cb;
        }
        else
        {*/
            if (sceneName == null)
            {
                Debug.Log("<color=orange>" + gameObject.name + ": No Scene Name Was given for LoadScene function!</color>");
            }
            SceneManager.LoadScene(sceneName); //load a scene
        //}
    }
}
