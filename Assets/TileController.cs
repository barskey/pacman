using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private Sprite dotSprite;
    [SerializeField] private Sprite powerDotSprite;
    [SerializeField] private Sprite blankSprite;

    public Vector2 coord;
    public List<Vector2> Exits = new List<Vector2>();

    [SerializeField] private bool hasDot;
    [SerializeField] private bool isPowerPellet;
    [SerializeField] private bool canMoveInto;
    public bool ghostCanExitUp = true;
        
    [Header("Debug")]
    [SerializeField] private bool drawOutlines;
    [SerializeField] private bool drawMovePath;
    [SerializeField] private bool drawDots;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        sr.sprite = hasDot ? (isPowerPellet ? powerDotSprite : dotSprite) : blankSprite;
    }

    public void Setup(Vector2 _coord, bool _hasDot, bool _isPowerPellet, bool _canMoveInto, TileExits _exits)
    {
        coord = _coord;
        hasDot = _hasDot;
        isPowerPellet = _isPowerPellet;
        canMoveInto = _canMoveInto;

        if (_exits.HasFlag(TileExits.Up))
            Exits.Add(Vector2.up);

        if (_exits.HasFlag(TileExits.Left))
            Exits.Add(Vector2.left);

        if (_exits.HasFlag(TileExits.Down))
            Exits.Add(Vector2.down);

        if (_exits.HasFlag(TileExits.Right))
            Exits.Add(Vector2.right);
    }

    public void GetExits()
    {
        // up, left, down, right
        List<Vector2> exits = new List<Vector2>();


    }

    //private bool IsExit()
    //{
    //    Physics2D.Linecast(new Vector2(transform.position.x + 1, transform.position.y);
    //}

    private void OnDrawGizmos()
    {
        if (drawOutlines)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(transform.position, new Vector2(8, 8));
        }

        if (drawMovePath && canMoveInto)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawCube(transform.position, new Vector2(8, 8));
        }

        if (drawDots)
        {
            Gizmos.color = Color.yellow;

            if (hasDot)
                Gizmos.DrawSphere(transform.position, 1f);

            if (isPowerPellet)
                Gizmos.DrawSphere(transform.position, 3f);
        }
    }
}
