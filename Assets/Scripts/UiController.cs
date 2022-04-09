using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController: MonoBehaviour
{
    [SerializeField]  GameObject escapeMenu;
    [SerializeField]  Button exitButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button newGameButton;

    // Start is called before the first frame update
    void OnEnable()
    {
        escapeMenu.gameObject.SetActive(false);
        exitButton.onClick.AddListener(delegate () { Application.Quit(); });
        resumeButton.onClick.AddListener(delegate () { ToggleMenu(false); });
        newGameButton.onClick.AddListener(delegate () { /* TODO: Add game restart functionality. */ });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            ToggleMenu(!escapeMenu.gameObject.activeInHierarchy);
        }
    }
    void ToggleMenu(bool visible)
    {
        escapeMenu.gameObject.SetActive(visible);
    }

}
