using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class HomeMenuController : MonoBehaviour
{
    public Image fadeInCurtain;
    private bool isRunning;
    public float fadeInSpeed = 2f;

    private void Awake()
    {
        fadeInCurtain.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(BranchOutAnimation());
    }
    
    IEnumerator BranchOutAnimation()
    {
        if (isRunning)
        {
            yield break;
        }

        isRunning = true;
        Color c = fadeInCurtain.color;
        while (true)
        {
            c.a -= fadeInSpeed * Time.deltaTime;
            fadeInCurtain.color = c;
            if (c.a <= 0)
            {
                c.a = 0;
                fadeInCurtain.color = c;
                Debug.Log("Faded In.");
                break;
            }

            yield return null;
        }

        isRunning = false;
    }
}