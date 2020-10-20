using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator HandsAnimator;
    public Animator CameraAnimator;

    HeroController hero;

    private void Start()
    {
        hero = FindObjectOfType<HeroController>();
    }

    private void Update()
    {
        float magnitude = new Vector2(hero.JoystickMove.Horizontal, hero.JoystickMove.Vertical).magnitude;
        HandsAnimator.SetFloat("BlendState", magnitude);
        CameraAnimator.SetFloat("State", magnitude);
    }
}
