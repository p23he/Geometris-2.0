using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void OnMouseUp()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
