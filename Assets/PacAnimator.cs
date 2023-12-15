using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] LeftChomp;
    [SerializeField] private Sprite[] RightChomp;
    [SerializeField] private Sprite[] UpChomp;
    [SerializeField] private Sprite[] DownChomp;
    [SerializeField] private Sprite[] Die;

    private SpriteRenderer sr;

    private Sprite[] currentAnim;
    private int index;
    private int framesPerSprite = 2;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        currentAnim = LeftChomp;
        index = 0;
    }

    public void ChangeDir(Vector2 dir)
    {
        if (Equals(dir, Vector2.left))
        {
            currentAnim = LeftChomp;
            index = 0;
        }
        else if (Equals(dir, Vector2.right))
        {
            currentAnim = RightChomp;
            index = 0;
        }
        else if (Equals(dir, Vector2.up))
        {
            currentAnim = UpChomp;
            index = 0;
        }
        else if (Equals(dir, Vector2.down))
        {
            currentAnim = DownChomp;
            index = 0;
        }
    }

    public void UpdateAnimation()
    {
        sr.sprite = currentAnim[index];

        if (index == currentAnim.Length)
        {

        }
    }
}
