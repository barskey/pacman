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
    [SerializeField] public Pacman player;
    [SerializeField] public Transform scatterTarget;
    [SerializeField] private BoardController board;
    [SerializeField] private TileController startTile;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 startDir;

    private PacAnimator anim;
    private GhostStateMachine stateMachine;

    private TileController currentTile;
    private Queue<Vector2> nextExits = new Queue<Vector2>();

    [SerializeField] private float speed;
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
        speed = 0.75f * Constants.MaxSpeedInPPS;
        transform.position = startPosition;
        movePos = startPosition;
        currentTile = startTile;
        currentTarget = scatterTarget;
        moveDir = startDir;
        nextExits.Enqueue(moveDir);

        currentMode = GhostMode.Scatter;

        stateMachine.Initialize(scatterState);
    }

    private void FixedUpdate()
    {
        UpdateMoveDir();

        UpdatePosition();

        anim.UpdateAnimation();
    }

    private void UpdatePosition()
    {
        var pixelsToMove = speed * Time.deltaTime;
        movePos += moveDir * pixelsToMove;

        transform.position = new Vector2(Mathf.RoundToInt(movePos.x), Mathf.RoundToInt(movePos.y));
    }

    private void UpdateMoveDir()
    {
        if (Equals(transform.position, currentTile.transform.position))
        {
            moveDir = nextExits.Dequeue();
            anim.ChangeDir(moveDir);
            //Debug.Log($"Dequeued {moveDir} Queue len:{nextExits.Count}");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)  // entered new tile
    {
        Debug.Log($"Entered {collision.name}");
        var newTile = collision.GetComponent<TileController>();

        ChooseNextExit(newTile);

        currentTile = newTile;

        UpdateTarget();
    }

    private void ChooseNextExit(TileController _checkTile)
    {
        var nextTile = board.GetTileAtExit(_checkTile.coord, nextExits.Peek());

        float dist = Mathf.Infinity;
        Vector2 bestChoice = nextTile.Exits[0];

        foreach (Vector2 exit in nextTile.Exits)
        {
            if (Equals(exit, nextExits.Peek() * -1))
            {
                continue;
            }

            if (Equals(exit, Vector2.up) && !nextTile.ghostCanExitUp)
            {
                continue;
            }

            TileController exitTile = board.GetTileAtExit(nextTile.coord, exit);
            Debug.Assert(exitTile != null);

            var distanceToTarget = Vector2.Distance(exitTile.transform.position, currentTarget.position);
            if (distanceToTarget < dist) {
                bestChoice = exit;
                dist = distanceToTarget;
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
