using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ryunm_PlayerHeroChoice : MonoBehaviour {
    public void ChooseBallistic() {
        PlayerPrefs.SetString("PlayerChoice", "Ballistic");
    }
    public void ChooseOctane() {
        PlayerPrefs.SetString("PlayerChoice", "Octane");
    }
    public void ChooseLoto() {
        PlayerPrefs.SetString("PlayerChoice", "Loto");
    }
    public void SelectDifficuties() {
        SceneManager.LoadScene(2);
    }
}
