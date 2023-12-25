using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    private PacAnimator anim;

    public TileController currentTile { get; private set; }
    private Vector2 movePos;

    [SerializeField] private Vector2 joystick = Vector2.zero;
    [SerializeField] private Vector2 facingDir;
    [SerializeField] private float speed;

    private void Awake()
    {
        anim = GetComponent<PacAnimator>();
    }

    private void Start()
    {
        speed = 0.8f * Constants.MaxSpeedInPPS;
        facingDir = Vector2.left;
        movePos = transform.position;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        if (!Equals(joystick, Vector2.zero) && CanTurn(joystick))
        {
            Turn();
        }

        if (currentTile != null && CanMove())
        {
            UpdatePosition();

            anim.UpdateAnimation();
        }
    }

    private void GetInput()
    {
        joystick = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            joystick = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            joystick = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            joystick = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            joystick = Vector2.right;
        }
    }

    private void Turn()
    {
        facingDir = joystick;
        anim.ChangeDir(joystick);
    }

    private void UpdatePosition()
    {
        Vector2 moveDirThisFrame;
        Vector2 cornerDir = Vector2.zero;

        if (facingDir == Vector2.up || facingDir == Vector2.down)
        {
            if (transform.position.x > currentTile.transform.position.x)
            {
                cornerDir = Vector2.left;
            }
            else if (transform.position.x < currentTile.transform.position.x)
            {
                cornerDir = Vector2.right;
            }
        }
        else if (facingDir == Vector2.left || facingDir == Vector2.right)
        {
            if (transform.position.y > currentTile.transform.position.y)
            {
                cornerDir = Vector2.down;
            }
            else if (transform.position.y < currentTile.transform.position.y)
            {
                cornerDir = Vector2.up;
            }
        }

        moveDirThisFrame = cornerDir + facingDir;

        var pixelsThisFrame = speed * Time.deltaTime;
        movePos += moveDirThisFrame * pixelsThisFrame;

        transform.position = new Vector2(Mathf.RoundToInt(movePos.x), Mathf.RoundToInt(movePos.y));
    }

    private bool CanTurn(Vector2 dir)
    {
        return currentTile.Exits.Contains(dir);
    }

    private bool CanMove()
    {
        if (currentTile.Exits.Contains(facingDir))
        {
            return true;
        }
        else if (!Equals(transform.position, currentTile.transform.position))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentTile = collision.GetComponent<TileController>();
    }
}
