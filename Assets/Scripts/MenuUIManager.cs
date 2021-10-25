using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    private TMP_InputField NameInputField;

    public void SetInitData(string userName)
    {
        NameInputField.text = userName;
    }

    void Awake()
    {
        NameInputField = GameObject.Find("NameInputField").GetComponent<TMP_InputField>();
    }

    public void LoadMainScene()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene("main");
    }

    // �C�x���g�n���h���[
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        // main�V�[���̃Z�b�g�A�b�v
        if (nextScene.name == "main")
        {
            var userName = NameInputField.text;

            var mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
            mainManager.SetInitData(userName);
        }

        SceneManager.sceneLoaded -= SceneLoaded;
    }
}
