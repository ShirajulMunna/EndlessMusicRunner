using UnityEngine;

public class Note : Monster
{
    protected override void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 20f);
    }

    public override void SetDie()
    {
        base.SetDie();
 
    }
}