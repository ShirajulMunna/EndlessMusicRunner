using UnityEngine;

public class ToolEffect : MonoBehaviour, IToolEffect
{
    [SerializeField] GameObject G_Effect;
    [SerializeField] Transform Tr_Craete;

    public void CreateEffect()
    {
        var effect = Instantiate(G_Effect, Tr_Craete);
        Destroy(effect, 2f);
    }
}

interface IToolEffect
{
    void CreateEffect();
}