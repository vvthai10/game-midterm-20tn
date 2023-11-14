using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightborneController : MonoBehaviour
{
    private BossGeneral boss;
    private BossHealth health;
    private Animator animator;

    public TeleportController teleportController;
    public NightborneFightController fightController; // used to start coroutines when this object is inactive

    private void Awake()
    {
        boss = GetComponent<BossGeneral>();
        health = GetComponent<BossHealth>();
        animator = GetComponent<Animator>();
        animator.keepAnimatorStateOnDisable = true;
    }
    
    public void ShowBoss()
    {
        boss.LookAtPlayer();
        boss.Show();
        health.IntroHealthBar();
        StartCoroutine(StartAttackingAfter(3));
    }

    IEnumerator StartAttackingAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        animator.SetTrigger("start");
    }

    public void BeginTeleportChain()
    {
        animator.SetBool("teleporting", true); // teleport animation on Nightborne
        teleportController.PlayDisappearAnimation(); // disappear animation on Nightborne/teleport --> TeleportController.OnDisappeared
                                                 // --> this.OnDisappear --> timeout --> this.EndTeleportChain -->
    }

    public void EndTeleportChain()
    {
        this.MoveBehindPlayer();
        boss.LookAtPlayer();
        teleportController.PlayAppearAnimation(); // appear animation on Nightborne/teleport --> TeleportController.OnAppear
                                              // --> this.OnReappear -->
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void MoveBehindPlayer(float offsetValue = 2)
    {
        // get player's facing direction
        // assuming rotation = 0 <=>  facing right
        bool playerFacingRight = boss.targetedPlayer.transform.localEulerAngles.y == 0;
        float offset = offsetValue * (playerFacingRight ? -1 : 1);
        gameObject.transform.position = new Vector3(boss.targetedPlayer.position.x + offset, transform.position.y, transform.position.z);
        Debug.Log("player X: " + boss.targetedPlayer.position.x.ToString());
        Debug.Log("offset: " + offset.ToString());
        Debug.Log("after: " + gameObject.transform.position.x.ToString());
    }

    public void OnDisappear()
    {
        this.Hide();
        fightController.StartCoroutine(ReappearAfter(teleportController.teleportDelay));
    }

    public void OnReappear()
    {
        this.Show();
        animator.SetBool("teleporting", false);
        //animator.Play("walk");
    }

    IEnumerator ReappearAfter(float seconds = 0)
    {
        yield return new WaitForSeconds(seconds);
        EndTeleportChain();
    }
}
