using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ryunm_Button_StartGame : MonoBehaviour
{
    public void ChooseDifficulties() {
        SceneManager.LoadScene(1);
    }
}
