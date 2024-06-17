using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinMonster : Monster
{
    const string Name = "Monster_{0}";

    private bool isAttacking = false;
    protected override void Start()
    {
        DestoryX = -25;
        //Speed = 20f;
    }


    protected override void Update()
    {
        SetMove();
    }
    public override void SetMove()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
        var values = DestoryX;

        if (transform.position.x < player.transform.position.x && !isAttacking)
        {
            player.SetHp(-damageAmount);
            ScoreManager.instance.SetBestCombo_Reset();
            isAttacking = true;
            GameObject opsFx = Instantiate(damageFx, transform.position, Quaternion.identity);
            Destroy(opsFx, 0.2f);
        }
        if(transform.position.x < DestoryX)
        {
            Destroy(this.gameObject);
        }
    }
    public override void SetDie()
    {
        if(CurHp <=0)
        {
            Destroy(gameObject);
        }
    }
}
