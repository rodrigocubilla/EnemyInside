using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask solidLayers;
    [SerializeField] private LayerMask pusheableLayers;
    [SerializeField] private PathFinding pathFinding;
    [SerializeField] private Transform block;
    [SerializeField] private ParticleSystem deadParticles;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private int lives;
    
    public int moves = 10;

    private bool OnMoveRight => Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
    private bool OnMoveLeft => Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
    private bool OnMoveUp => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
    private bool OnMoveDown => Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
    private bool OnReset => Input.GetKeyDown(KeyCode.R);

    private bool canMoveRight;
    private bool canMoveLeft;
    private bool canMoveUp;
    private bool canMoveDown;

    private bool canPushRight;
    private bool canPushLeft;
    private bool canPushUp;
    private bool canPushDown;

    private bool canMove = true;
    public bool monsterMode = false;
    private int startMoves;

    public bool isDead = false;

    private RaycastHit2D hitPushableRight;
    private RaycastHit2D hitPushableLeft;
    private RaycastHit2D hitPushableUp;
    private RaycastHit2D hitPushableDown;

    private void Start()
    {
        startMoves = moves;

        pathFinding.OnPathFinded += (path) => {
            StartCoroutine(DoPath(path));
        };

        pathFinding.OnCantReachPosition += () => {
            HUDEventReciever.OnChangeMode.Invoke(PlayerMode.Human);
            moves = startMoves;
            MaskReveal.OnRevealMask.Invoke();
            monsterMode = false;
        };

        HUDLives.OnLifeChange?.Invoke(lives);
    }

    private void FixedUpdate() 
    {
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, .5f, solidLayers); 
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, .5f, solidLayers); 
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, .5f, solidLayers); 
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, .5f, solidLayers); 

        hitPushableRight = Physics2D.Raycast(transform.position, Vector2.right, .5f, pusheableLayers); 
        hitPushableLeft = Physics2D.Raycast(transform.position, Vector2.left, .5f, pusheableLayers); 
        hitPushableUp = Physics2D.Raycast(transform.position, Vector2.up, .5f, pusheableLayers); 
        hitPushableDown = Physics2D.Raycast(transform.position, Vector2.down, .5f, pusheableLayers); 

        canMoveRight = hitRight ? false : true;
        canMoveLeft = hitLeft ? false : true;
        canMoveUp = hitUp ? false : true;
        canMoveDown = hitDown ? false : true;

        canPushRight = hitPushableRight ? true : false;
        canPushLeft = hitPushableLeft ? true : false;
        canPushUp = hitPushableUp ? true : false;
        canPushDown = hitPushableDown ? true : false;
    }

    private void GoToRandomKillZone()
    {
        KillZone[] killzones = FindObjectsOfType<KillZone>();
        Lava[] lavas = FindObjectsOfType<Lava>();

        if (killzones.Length == 0 && lavas.Length == 0) return;
        
        KillZone killZone = null;

        foreach (KillZone kill in killzones)
        {
            if (kill.blocked == false)
            {
                killZone = kill;
                break;
            }
        }

        Lava lava = null;

        foreach (Lava lav in lavas)
        {
            if (lav.blocked == false && lav.enabled)
            {
                lava = lav;
                break;
            }
        }

        if (killZone == null && lava == null)
        {
            HUDEventReciever.OnChangeMode.Invoke(PlayerMode.Human);
            moves = startMoves;
            MaskReveal.OnRevealMask.Invoke();
            monsterMode = false;
            return;
        }
        
        if (lava != null)
            pathFinding.PathFindToPoint(new Vector2Int((int)lava.gameObject.transform.position.x, (int)lava.gameObject.transform.position.y));
        else
            pathFinding.PathFindToPoint(new Vector2Int((int)killZone.gameObject.transform.position.x, (int)killZone.gameObject.transform.position.y));
    }

    private void Update()
    {
        if (OnReset)
        {
            monsterMode = false;
            HUDEventReciever.OnChangeMode.Invoke(PlayerMode.Human);
            MaskReveal.OnHideMask.Invoke();
            lives = 5;
            HUDLives.OnLifeChange.Invoke(lives);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
        }

        if (lives <= 0)
        {
            lives = 5;
            HUDLives.OnLifeChange.Invoke(lives);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (moves <= 0)
        {
            moves = startMoves;
            HUDEventReciever.OnPlayerMove(moves);
            MaskReveal.OnRevealMask.Invoke();
            monsterMode = !monsterMode;
            if (monsterMode)
            {
                HUDEventReciever.OnChangeMode.Invoke(PlayerMode.Monster);

                StartCoroutine(WaitFor(() => {
                    GoToRandomKillZone();
                }, 1f));
                //GoToRandomKillZone();
            }
            else
            {
                HUDEventReciever.OnChangeMode.Invoke(PlayerMode.Human);
            }
        }

        if (monsterMode) return;

        if (isDead) return;

        if (!canMove) return;

        if (OnMoveRight && canMoveRight)
        {
            if (canPushRight)
            {
                if (hitPushableRight.collider.gameObject.GetComponent<Box>().PushRight() == false)
                {
                    return;
                }
            }
            canMove = false;
            LeanTween.move(gameObject, gameObject.transform.position + new Vector3(1, 0, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(1, .8f, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
                canMove = true;
                moves--;
                HUDEventReciever.OnPlayerMove(moves);
            });
        }
        else if (OnMoveLeft && canMoveLeft)
        {
            if (canPushLeft)
            {
                if (hitPushableLeft.collider.gameObject.GetComponent<Box>().PushLeft() == false)
                {
                    return;
                }
            }
            canMove = false;
            LeanTween.move(gameObject, gameObject.transform.position - new Vector3(1, 0, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(1, .8f, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
                canMove = true;
                moves--;
                HUDEventReciever.OnPlayerMove(moves);
            });
        }
        else if (OnMoveUp && canMoveUp)
        {
            if (canPushUp)
            {
                if (hitPushableUp.collider.gameObject.GetComponent<Box>().PushUp() == false)
                {
                    return;
                }
            }
            canMove = false;
            LeanTween.move(gameObject, gameObject.transform.position + new Vector3(0, 1, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(.8f, 1, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
                canMove = true;
                moves--;
                HUDEventReciever.OnPlayerMove(moves);
            });
        }
        else if (OnMoveDown && canMoveDown)
        {
            if (canPushDown)
            {
                if (hitPushableDown.collider.gameObject.GetComponent<Box>().PushDown() == false)
                {
                    return;
                }
            }
            canMove = false;
            LeanTween.move(gameObject, gameObject.transform.position - new Vector3(0, 1, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(.8f, 1, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
                canMove = true;
                moves--;
                HUDEventReciever.OnPlayerMove(moves);
            });
        }
    }

    private IEnumerator DoPath(Vector2Int[] path)
    {
        var waypoints = path;

        for (int i = 0; i < waypoints.Length; i++)
        {   
            yield return new WaitUntil( () => canMove );

            canMove = false;
            LeanTween.move(gameObject, new Vector3(waypoints[i].x, waypoints[i].y, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(.8f, 1, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
                canMove = true;
            });
        }
    }

    private IEnumerator WaitFor(Action func, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        func.Invoke();
    }

    public void Kill()
    {
        isDead = true;
        deadParticles.transform.position = transform.position;
        deadParticles.Play();
        StartCoroutine(RespawnAfterSeconds(1f));
    }

    private IEnumerator RespawnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        trail.Clear();
        gameObject.transform.position = spawnPoint.position;
        trail.Clear();
        yield return new WaitForSeconds(.1f);
        isDead = false;

        moves = startMoves;
        MaskReveal.OnHideMask.Invoke();
        monsterMode = false;
        HUDEventReciever.OnChangeMode.Invoke(PlayerMode.Human);
        lives--;
        HUDLives.OnLifeChange.Invoke(lives);
    }
}
