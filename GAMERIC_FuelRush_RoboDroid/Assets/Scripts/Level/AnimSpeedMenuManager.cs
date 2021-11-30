using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimSpeedMenuManager : MonoBehaviour
{
    public static float animSpeedMultiplier;
    private Animator anim;

    [SerializeField] private GameObject normalSpeedButton;
    [SerializeField] private GameObject doubleSpeedButton;

    private Color normalColor;
    private Color hoveredColor;
    private Color clickedColor;

    private void Awake()
    {
        animSpeedMultiplier = 1f;
        normalColor = Color.white;
        hoveredColor = new Color(192, 192, 192);
        clickedColor = new Color(140, 140, 140);
    }

    private void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        anim.SetFloat("AnimSpeedMultiplier", 1f);
    }

    public void ChangeToTwoTimesSpeed()
    {
        doubleSpeedButton.SetActive(false);
        normalSpeedButton.SetActive(true);

        animSpeedMultiplier = 2f;
        anim.SetFloat("AnimSpeedMultiplier", 1.5f);
    }

    public void ChangeToNormalSpeed()
    {
        doubleSpeedButton.SetActive(true);
        normalSpeedButton.SetActive(false);

        animSpeedMultiplier = 1f;
        anim.SetFloat("AnimSpeedMultiplier", 1f);
    }
}
