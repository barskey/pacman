using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    private PacAnimator anim;

    public Vector2 facingDir { get; private set; }

    public TileController currentTile { get; private set; }
    private Vector2 movePos;
    private Vector2 target;
    [SerializeField] private float speed;

    private bool moving;
    private bool chomp;

    private void Awake()
    {
        anim = GetComponent<PacAnimator>();
    }

    private void Start()
    {
        speed = 0.8f * Constants.MaxSpeedInPPS;
        movePos = transform.position;
        facingDir = Vector2.left;
    }

    private void FixedUpdate()
    {
        Vector2 dir = GetInput();

        if (!Equals(dir, Vector2.zero) && CanMoveInDirection(dir))
        {
            facingDir = dir;
            anim.ChangeDir(dir);
        }

        UpdatePosition();

        anim.UpdateAnimation();

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

    private void UpdatePosition()
    {
        var pixelsPerFrame = speed * Time.deltaTime;
        movePos += facingDir * pixelsPerFrame;

        transform.position = new Vector2(Mathf.RoundToInt(movePos.x), Mathf.RoundToInt(movePos.y));
    }

    private bool CanMoveInDirection(Vector2 dir)
    {
        //Debug.Log($"{Equals(transform.position, currentTile.transform.position)}");
        if (currentTile.Exits.Contains(dir))
        {
            return true;
        }
        else if (!Equals(transform.position, currentTile.transform.position))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentTile = collision.GetComponent<TileController>();
    }
}
