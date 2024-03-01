using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ryunm_Button_BackToStart : MonoBehaviour
{
    public void BackToStart() {
        SceneManager.LoadScene(0);
    }
}
