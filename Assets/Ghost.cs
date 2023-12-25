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
    [SerializeField] public LevelManager levelManager;
    [SerializeField] public Transform scatterTarget;

    [SerializeField] private BoardController board;
    [SerializeField] private Pacman player;
    [SerializeField] private TileController startTile;

    [SerializeField] private Vector2 startPosition = new Vector2(108, 168);
    [SerializeField] private Vector2 startDir = Vector2.left;

    private PacAnimator anim;
    private GhostStateMachine stateMachine;

    private TileController currentTile;
    private Queue<Vector2> nextExits = new Queue<Vector2>();

    [SerializeField] private float speed;
    private Vector2 moveDir;
    private Vector2 movePos;

    protected Transform currentTarget;

    #region States
    public State scatterState { get; private set; }
    public State chaseState { get; private set; }
    public State frightenedState { get; private set; }
    #endregion

    private void Awake()
    {
        levelManager.WaveChanged += OnWaveChanged;

        anim = GetComponent<PacAnimator>();

        stateMachine = new GhostStateMachine();

        scatterState = new ScatterState(stateMachine, this, player);
        chaseState = new ChaseState(stateMachine, this, player);
        frightenedState = new FrightenedState(stateMachine, this, player);
    }

    private void Start()
    {
        transform.position = startPosition;
        movePos = startPosition;
        currentTile = startTile;
        currentTarget = scatterTarget;
        moveDir = startDir;
        nextExits.Enqueue(moveDir);

        stateMachine.Initialize(scatterState);
    }

    private void FixedUpdate()
    {
        if (UpdatePosition())
            UpdateMoveDir();

        anim.UpdateAnimation();
        //Debug.Log($"CurrentTile:{currentTile.coord} MoveDir:{moveDir} Exits:{nextExits.Count}");
    }

    private bool UpdatePosition()
    {
        var pixelsToMove = speed * Time.deltaTime;
        movePos += moveDir * pixelsToMove;

        var lastPos = transform.position;
        transform.position = new Vector2(Mathf.RoundToInt(movePos.x), Mathf.RoundToInt(movePos.y));

        return !Equals(transform.position, lastPos);
    }

    private void UpdateMoveDir()
    {
        if (Equals(transform.position, currentTile.transform.position)) // if we are at center pixel of tile
        {
            //Debug.Log("Center Pixel");
            moveDir = nextExits.Dequeue(); // get next turn
            anim.ChangeDir(nextExits.Peek());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)  // entered new tile
    {
        //Debug.Log($"Entered {collision.name}");
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
            if (Equals(exit, nextExits.Peek() * -1)) // can't exit in the direction we entered
                continue;

            if (Equals(exit, Vector2.up) && !nextTile.ghostCanExitUp) // can't turn up from special zones
                continue;

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

    private void OnWaveChanged(GhostMode _newWave)
    {
        switch (_newWave)
        {
            case GhostMode.Chase:
                stateMachine.ChangeState(chaseState);
                break;
            case GhostMode.Scatter:
                stateMachine.ChangeState(scatterState);
                break;
            case GhostMode.Frightened:
                stateMachine.ChangeState(frightenedState);
                break;
        }
    }

    private void UpdateTarget()
    {
    }

    public void SetTarget(Transform _newTarget)
    {
        currentTarget = _newTarget;
    }

    public void SetSpeed(float _speedPercent)
    {
        speed = _speedPercent * Constants.MaxSpeedInPPS;
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
