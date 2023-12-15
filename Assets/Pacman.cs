using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    public float speedInPPS;

    private PacAnimator anim;

    public Vector2 facingDir { get; private set; }

    public Transform currentTile { get; private set; }
    private Vector2 movePos;
    private Vector2 target;

    private bool moving;
    private bool chomp;

    private void Awake()
    {
        anim = GetComponent<PacAnimator>();
    }

    private void Start()
    {
        movePos = transform.position;
    }

    private void FixedUpdate()
    {
        Vector2 dir = GetInput();

        if (!Equals(dir, Vector2.zero))
        {
            facingDir = dir;
            anim.ChangeDir(dir);
        }

        if (CanMove())
        {
            var pixelsPerFrame = speedInPPS * Time.deltaTime;
            movePos += facingDir * pixelsPerFrame;
        }

        transform.position = new Vector2(Mathf.RoundToInt(movePos.x), Mathf.RoundToInt(movePos.y));

    }

    private Vector2 GetInput()
    {
        var dir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            dir = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            dir = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            dir = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            dir = Vector2.right;
        }

        return dir;
    }

    private bool CanMove()
    {
        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentTile = collision.transform;
    }
}
