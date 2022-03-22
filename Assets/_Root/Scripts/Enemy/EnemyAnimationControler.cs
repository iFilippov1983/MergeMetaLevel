using UnityEngine;

public class EnemyAnimationControler : MonoBehaviour
{
    public bool appearAnimationFinished;
    public bool deathAnimationFinished;

    private void Awake()
    {
        appearAnimationFinished = false;
        deathAnimationFinished = false;
    }

    public void AppearAnimationFinished() => appearAnimationFinished = true;
    public void DeathAnimationFinished() => deathAnimationFinished = true;

}
