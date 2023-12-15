using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostMode
{
    Chase,
    Scatter,
    Frightened
}

public class Ghost : MonoBehaviour
{
    public float speedInPPS;

    [SerializeField] public Pacman player;
    [SerializeField] public Transform scatterTarget;
    [SerializeField] private BoardController board;

    private PacAnimator anim;
    private GhostStateMachine stateMachine;

    private TileController currentTile;
    private Queue<Vector2> nextExits;

    private Vector2 moveDir;
    private Vector2 movePos;

    private Transform currentTarget;
    private GhostMode currentMode;

    #region States
    public State scatterState { get; private set; }
    #endregion

    private void Awake()
    {
        anim = GetComponent<PacAnimator>();

        stateMachine = new GhostStateMachine();

        scatterState = new ScatterState(stateMachine, this, player);
    }

    private void Start()
    {
        movePos = transform.position;

        moveDir = Vector2.left;
        currentMode = GhostMode.Scatter;

        stateMachine.Initialize(scatterState);
    }

    private void FixedUpdate()
    {
        //if (!Equals(dir, Vector2.zero))
        //{
        //    facingDir = dir;
        //    anim.ChangeDir(dir);
        //}

        var pixelsToMove = speedInPPS * Time.deltaTime;
        movePos += moveDir * pixelsToMove;

        transform.position = new Vector2(Mathf.RoundToInt(movePos.x), Mathf.RoundToInt(movePos.y));

    }

    private void OnTriggerEnter2D(Collider2D collision)  // entered new tile
    {
        var newTile = collision.GetComponent<TileController>();

        ChooseNextExit(newTile);

        currentTile = newTile;

        UpdateTarget();
    }

    private void ChooseNextExit(TileController _checkTile)
    {
        var nextTile = board.GetTileAt(_checkTile.coord + nextExits.Peek());

        float dist = 0;
        Vector2 bestChoice = nextExits.Peek();

        foreach (var exit in nextTile.Exits)
        {
            if (!Equals(exit, nextExits.Peek() * -1) && Vector2.Distance(nextTile.coord, currentTarget.position) > dist){
                bestChoice = exit;
                dist = Vector2.Distance(nextTile.coord, currentTarget.position);
            }
        }

        nextExits.Enqueue(bestChoice);
    }

    private void UpdateTarget()
    {

    }

    public void SetTarget(Transform _newTarget)
    {
        currentTarget = _newTarget;
    }

    private void OnDrawGizmos()
    {
        if (currentTarget != null)
        {
            Gizmos.color = new Color(255, 0, 0, 120);
            Gizmos.DrawCube(currentTarget.position, new Vector3(6f, 6f, 0));
        }
    }
}
