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
    private bool didWaveChange;
    private GhostMode nextWave;
    public bool reverseNextTile;

    private TileController currentTile;
    private Queue<Vector2> nextExits = new Queue<Vector2>();

    [SerializeField] private float speed;
    private Vector2 facingDir;
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
        //currentTile = startTile;
        currentTarget = scatterTarget;
        facingDir = startDir;
        nextExits.Enqueue(facingDir);

        stateMachine.Initialize(scatterState);
        nextWave = GhostMode.Scatter;
    }

    private void FixedUpdate()
    {
        if (didWaveChange)
            HandleWaveChanged();

        // Check if entered new tile
        if (board.GetTileAtPos(transform.position) != currentTile)
            EnterTile(board.GetTileAtPos(transform.position));

        // Update position, and update facing direction if movement happened
        if (UpdatePosition())
            UpdateMoveDir();

        anim.UpdateAnimation();
        //Debug.Log($"CurrentTile:{currentTile.coord} MoveDir:{moveDir} Exits:{nextExits.Count}");
    }

    private bool UpdatePosition()
    {
        var pixelsToMove = speed * Time.deltaTime;
        movePos += facingDir * pixelsToMove;

        var lastPos = transform.position;
        transform.position = new Vector2(Mathf.RoundToInt(movePos.x), Mathf.RoundToInt(movePos.y));

        return transform.position != lastPos;
    }

    private void UpdateMoveDir()
    {
        // if we are at center pixel of tile
        if (transform.position == currentTile.transform.position)
        {
            // Debug.Log("Center Pixel");
            facingDir = nextExits.Dequeue(); // get next exit
            // Debug.Log($"Dequeued {facingDir}");

            anim.ChangeDir(nextExits.Peek());
        }
    }

    private void EnterTile(TileController newTile) // entered new tile
    {
        currentTile = newTile;
        Debug.Log($"Entered {newTile}");

        stateMachine.currentState.OnEnterTile();

        if (reverseNextTile)
        {
            ReverseDirection();
            reverseNextTile = false;
        }

        ChooseNextExit(newTile);

        foreach (var exit in nextExits)
        {
            Debug.Log($"Exit:{exit}");
        }
    }

    private void ReverseDirection()
    {
        nextExits.Enqueue(facingDir * -1);
        _ = nextExits.Dequeue();
        // Debug.Log($"Enqueued {facingDir * -1}");
    }

    private void ChooseNextExit(TileController _checkTile)
    {
        TileController nextTile = board.GetTileAtExit(_checkTile.coord, nextExits.Peek());

        float dist = Mathf.Infinity;
        Vector2 bestChoice = Vector2.zero;

        foreach (Vector2 exit in nextTile.Exits)
        {
            // can't exit in the direction we entered
            if (exit == nextExits.Peek() * -1)
                continue;

            // can't turn up from special zones
            if (exit == Vector2.up && !nextTile.ghostCanExitUp)
                continue;

            TileController exitTile = board.GetTileAtExit(nextTile.coord, exit);

            var distanceToTarget = Vector2.Distance(exitTile.transform.position, currentTarget.position);
            if (distanceToTarget < dist)
            {
                bestChoice = exit;
                dist = distanceToTarget;
            }
        }

        Debug.Assert(bestChoice != Vector2.zero);
        nextExits.Enqueue(bestChoice);
        // Debug.Log($"Enqueued {bestChoice}");
    }

    private void OnWaveChanged(GhostMode _newWave)
    {
        nextWave = _newWave;
        didWaveChange = true;
    }

    private void HandleWaveChanged()
    {
        switch (nextWave)
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

        didWaveChange = false;
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
