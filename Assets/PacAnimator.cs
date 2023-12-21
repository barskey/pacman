using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] LeftAnim;
    [SerializeField] private Sprite[] RightAnim;
    [SerializeField] private Sprite[] UpAnim;
    [SerializeField] private Sprite[] DownAnim;
    [SerializeField] private Sprite[] DieAnim;

    private SpriteRenderer sr;

    private Sprite[] currentAnim;
    [SerializeField] private int frameIndex;
    [SerializeField] private int framesPerSprite = 2;

    [SerializeField] private int frameCounter;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentAnim = LeftAnim;
        frameIndex = 0;
        frameCounter = 0;
    }

    public void ChangeDir(Vector2 dir)
    {
        if (Equals(dir, Vector2.left))
        {
            currentAnim = LeftAnim;
        }
        else if (Equals(dir, Vector2.right))
        {
            currentAnim = RightAnim;
        }
        else if (Equals(dir, Vector2.up))
        {
            currentAnim = UpAnim;
        }
        else if (Equals(dir, Vector2.down))
        {
            currentAnim = DownAnim;
        }
    }

    public void UpdateAnimation()
    {
        if (frameCounter >= framesPerSprite)
        {
            frameIndex++;
            frameCounter = 0;
        }

        if (frameIndex == currentAnim.Length)
        {
            frameIndex = 0;
        }

        sr.sprite = currentAnim[frameIndex];

        frameCounter++;
    }
}
