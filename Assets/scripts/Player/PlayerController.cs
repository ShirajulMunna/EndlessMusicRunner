using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.UI;

public class PlayerController : Entity
{
    private Rigidbody2D playerRb;
    public SkeletonAnimation playerSkeletonAnimation;
    public float jumpForce;
 
    public bool isJump=false;
    public float gravityModifier;
     
    [SpineAnimation]
    public string runAnimation;

    [SpineAnimation]
    public string flyAnimation;

    [SpineAnimation]
    public string kickAnimation;

    public float runningTimeScale;
    public GameObject movingEffect;
   



    protected override void Start()
    {
        MoveAnimation();
        playerRb = GetComponent<Rigidbody2D>();
        StartCoroutine(RunAnimation());
        Physics.gravity*=gravityModifier;

        currentHealth = maxHealth;

        //playerSkeletonAnimation.AnimationState.SetAnimation(0, runAnimation, true).TimeScale = runningTimeScale;

    }   

    protected override void FixedUpdate()
    {
       if(isOnGround)
            movingEffect.SetActive(true);

        if (Input.GetKeyDown(KeyCode.F) && isOnGround) 
        {
          
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);


            GameManager.instance.AnimationController(flyAnimation);

            GameObject.Find("AttackPoint_Up").GetComponent<Collider2D>().enabled = true;
            Invoke("UpperColliderDeactivate", 0.5f);
            movingEffect.SetActive(false);
            isOnGround = false;

        

        }

        if (Input.GetKeyDown(KeyCode.J)) 
        {
            
            GameManager.instance.AnimationController(kickAnimation);          
            GameObject.Find("AttackPoint_Down").GetComponent<Collider2D>().enabled = true;
            Invoke("LowerColliderDeactivate", 0.5f);
           

        }

                   
    }
 

    public void ColliderDeactivate() 
    {
        GameObject.Find("AttackChecked").GetComponent<Collider2D>().enabled = false;

    }
    public void UpperColliderDeactivate()
    {
        GameObject.Find("AttackPoint_Up").GetComponent<Collider2D>().enabled = false;

    }
    public void LowerColliderDeactivate()
    {
        GameObject.Find("AttackPoint_Down").GetComponent<Collider2D>().enabled = false;

    }


    IEnumerator RunAnimation() 
    {
        while (true) 
        {
            if (isOnGround)
            {
                playerSkeletonAnimation.AnimationState.SetAnimation(0, runAnimation, true).TimeScale= runningTimeScale;
            }
            yield return new WaitForSeconds(0.5f);

        }
    
    }

    public void MoveAnimation() 
    {
        transform.DOMoveX(-56, 2).SetEase(Ease.Flash).OnComplete(() =>
        {
            StartCoroutine(WatingTime());
           
        });
    }

    IEnumerator WatingTime() 
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.ActivatPanel(true);
        UIManager.Instance.attackPoints.SetActive(true);

    }

   






}
