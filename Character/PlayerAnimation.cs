using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Player player;
    public Animator animator;

    private void Awake()
    {
        player = GetComponent<Player>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        animator.Play("sleep");
        animator.SetBool("isSleep", true);
    }

    public void PlayerTurnBeginAnimator()
    {
        animator.SetBool("isSleep", false);
        animator.SetBool("isParry", false);
         
    }

    public void PlayerTurnEndAnimator()
    {
        if (player.defense.currentValue > 0)
        {
            animator.SetBool("isParry", true);
        }
        else
        {
            animator.SetBool("isSleep", true);
            animator.SetBool("isParry", false);
        }
    }

    public void OnPlayerCardEvent(object obj)
    {
        Card card = (Card)obj;

        switch (card.cardData.cardType)
        {
            case CardType.Attack:
                animator.SetTrigger("attack");
                break;
            case CardType.Defense:
            case CardType.Abilities:
                animator.SetTrigger("skill");
                break;
        }
    }

    public void SetSleepAnimator()
    {
        animator.Play("death");
    }
}
