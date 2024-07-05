using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class O_BattleCharacterActor : MonoBehaviour
{
    private O_BattleCharacter battleCharacter;
    public SpriteRenderer render;
    public Animator animator;
    //TODO:
    //Write an event which corresponds with stuff like OnHurt to do animations


    public void SetCharacter(ref O_BattleCharacter battleCharacter) {
        this.battleCharacter = battleCharacter;
        this.battleCharacter.playAnimation += PlayAnimation;
        this.battleCharacter.onDefeat += OnDefeat;
        this.battleCharacter.onHurt += OnHurt;
    }

    public void PlayAnimation(string anim)
    {
        animator.Play(anim);
    }

    public void FreeCharacter()
    {
        battleCharacter.playAnimation -= PlayAnimation;
        battleCharacter.onDefeat -= OnDefeat;
        battleCharacter.onHurt -= OnHurt;
        battleCharacter = null;
    }

    public void OnDefeat()
    {
        StartCoroutine(DefeatAnimation());
        if (!battleCharacter.revivable) {
            FreeCharacter();
        }
    }
    public void OnHurt()
    {
        StartCoroutine(HurtAnimation());
    }
    private IEnumerator DefeatAnimation()
    {
        yield return PlayFadeCharacter(Color.black, Color.clear);
    }

    private IEnumerator HurtAnimation()
    {
        Vector3 characterPos = transform.position;
        for (int i = 0; i < 2; i++)
        {
            transform.position = characterPos + new Vector3(5, 0);
            yield return new WaitForSeconds(0.02f);
            transform.position = characterPos;
            yield return new WaitForSeconds(0.02f);
            transform.position = characterPos + new Vector3(-5, 0);
            yield return new WaitForSeconds(0.02f);
            transform.position = characterPos;
            yield return new WaitForSeconds(0.02f);
        }
        transform.position = characterPos;
        yield return new WaitForSeconds(0.01f);
    }
    public IEnumerator PlayFadeCharacter(Color from, Color to)
    {
        render.color = from;
        float dt = 0;
        while (render.color != to)
        {
            dt += Time.deltaTime * 1.4f;
            render.color = Color.Lerp(from, to, dt);
            yield return new WaitForSeconds(Time.deltaTime);
        };
    }

    void Update()
    {
        if (battleCharacter != null)
        {
            battleCharacter.position = transform.position;
            if(animator.runtimeAnimatorController != null)
                battleCharacter.getAnimHandlerState = animator.GetCurrentAnimatorStateInfo(0).length;
        }
    }
}
