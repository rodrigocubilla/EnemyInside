using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private LayerMask solidLayers;
    [SerializeField] private LayerMask pusheableLayers;

    private bool canMoveRight;
    private bool canMoveLeft;
    private bool canMoveUp;
    private bool canMoveDown;

    private bool canPushRight;
    private bool canPushLeft;
    private bool canPushUp;
    private bool canPushDown;

    private RaycastHit2D hitPushableRight;
    private RaycastHit2D hitPushableLeft;
    private RaycastHit2D hitPushableUp;
    private RaycastHit2D hitPushableDown;

    [SerializeField] private PlayerController player;
    private bool lastMonsterMode;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void FixedUpdate() 
    {
        if (player.monsterMode != lastMonsterMode)
        {
            lastMonsterMode = player.monsterMode;
            if (player.monsterMode)
            {
                gameObject.layer = LayerMask.NameToLayer("Solids");
                //Debug.Log("Cambiado a layer Solids");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Pushable");
                //Debug.Log("Cambiado a layer Pushable");
            }
        }
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, .5f, solidLayers); 
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, .5f, solidLayers); 
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, .5f, solidLayers); 
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, .5f, solidLayers); 

        hitPushableRight = Physics2D.Raycast(transform.position + Vector3.right * transform.localScale.x/1.9f, Vector2.right, .5f, pusheableLayers); 
        hitPushableLeft = Physics2D.Raycast(transform.position + Vector3.left * transform.localScale.x/1.9f, Vector2.left, .5f, pusheableLayers); 
        hitPushableUp = Physics2D.Raycast(transform.position + Vector3.up * transform.localScale.x/1.9f, Vector2.up, .5f, pusheableLayers); 
        hitPushableDown = Physics2D.Raycast(transform.position + Vector3.down * transform.localScale.x/1.9f, Vector2.down, .5f, pusheableLayers); 

        Debug.DrawRay(transform.position + Vector3.right * transform.localScale.x/1.9f, Vector2.right * .5f);
        Debug.DrawRay(transform.position + Vector3.left * transform.localScale.x/1.9f, Vector2.left * .5f);
        Debug.DrawRay(transform.position + Vector3.up * transform.localScale.x/1.9f, Vector2.up * .5f);
        Debug.DrawRay(transform.position + Vector3.down * transform.localScale.x/1.9f, Vector2.down * .5f);

        canMoveRight = hitRight ? false : true;
        canMoveLeft = hitLeft ? false : true;
        canMoveUp = hitUp ? false : true;
        canMoveDown = hitDown ? false : true;

        canPushRight = hitPushableRight ? true : false;
        canPushLeft = hitPushableLeft ? true : false;
        canPushUp = hitPushableUp ? true : false;
        canPushDown = hitPushableDown ? true : false;
    }

    public bool PushRight()
    {
        if (canMoveRight)
        {
            if (canPushRight && hitPushableRight.collider.gameObject != gameObject)
            {
                var lava = hitPushableRight.collider.GetComponent<Lava>();
                if (lava != null)
                {
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    lava.enabled = false;
                    LeanTween.alpha(hitPushableRight.collider.gameObject, 0.1f, movementSpeed);
                    LeanTween.alpha(gameObject, 0.1f, movementSpeed);
                    LeanTween.move(gameObject, hitPushableRight.collider.transform.position - new Vector3(0f, 0f, hitPushableRight.collider.transform.position.z), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                        this.enabled = false;
                    });
                }
                return false;
            }
            LeanTween.move(gameObject, gameObject.transform.position + new Vector3(1, 0, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(1, .8f, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
            });
            return true;
        }
        else
        {
            if (hitPushableRight.collider.GetComponent<Lava>() != null)
            {
                LeanTween.move(gameObject, hitPushableRight.collider.transform.position, movementSpeed).setEaseOutQuad();
                this.enabled = false;
            }
        }
        return false;
    }

    public bool PushLeft()
    {
        if (canMoveLeft)
        {
            if (canPushLeft && hitPushableLeft.collider.gameObject != gameObject)
            {
                var lava = hitPushableLeft.collider.GetComponent<Lava>();
                if (lava != null)
                {
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    lava.enabled = false;
                    LeanTween.alpha(hitPushableLeft.collider.gameObject, 0.1f, movementSpeed);
                    LeanTween.alpha(gameObject, 0.1f, movementSpeed);
                    LeanTween.move(gameObject, hitPushableLeft.collider.transform.position - new Vector3(0f, 0f, hitPushableLeft.collider.transform.position.z), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                        this.enabled = false;
                    });
                }
                return false;
            }
            LeanTween.move(gameObject, gameObject.transform.position - new Vector3(1, 0, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(1, .8f, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
            });
            return true;
        }
        return false;
    }

    public bool PushUp()
    {
        if (canMoveUp)
        {
            if (canPushUp && hitPushableUp.collider.gameObject != gameObject)
            {
                var lava = hitPushableUp.collider.GetComponent<Lava>();
                if (lava != null)
                {
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    lava.enabled = false;
                    LeanTween.alpha(hitPushableUp.collider.gameObject, 0.1f, movementSpeed);
                    LeanTween.alpha(gameObject, 0.1f, movementSpeed);
                    LeanTween.move(gameObject, hitPushableUp.collider.transform.position - new Vector3(0f, 0f, hitPushableUp.collider.transform.position.z), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                        this.enabled = false;
                    });
                }
                return false;
            }
            LeanTween.move(gameObject, gameObject.transform.position + new Vector3(0, 1, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(.8f, 1, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
            });
            return true;
        }
        return false;
    }

    public bool PushDown()
    {
        if (canMoveDown)
        {
            if (canPushDown && hitPushableDown.collider.gameObject != gameObject)
            {
                var lava = hitPushableDown.collider.GetComponent<Lava>();
                if (lava != null)
                {
                    this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    lava.enabled = false;
                    LeanTween.alpha(hitPushableDown.collider.gameObject, 0.1f, movementSpeed);
                    LeanTween.alpha(gameObject, 0.1f, movementSpeed);
                    LeanTween.move(gameObject, hitPushableDown.collider.transform.position - new Vector3(0f, 0f, hitPushableDown.collider.transform.position.z), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                        this.enabled = false;
                    });
                }
                return false;
            }
            LeanTween.move(gameObject, gameObject.transform.position - new Vector3(0, 1, 0), movementSpeed).setEaseOutQuad();
            LeanTween.scale(gameObject, new Vector3(.8f, 1, .8f), movementSpeed).setEaseOutQuad().setOnComplete(() => {
                LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.02f).setEaseOutQuad();
            });
            return true;
        }
        return false;
    }
}