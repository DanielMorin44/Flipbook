using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookwyrmController : MonoBehaviour
{
    public SplitScreenPlayerController player;
    public SpriteRenderer topHit;
    public SpriteRenderer bottomHit;
    public SpriteRenderer topFlame;
    public SpriteRenderer bottomFlame;
    public Transform topPosition;
    public Transform bottomPosition;
    public Transform bookWyrm;
    public GameObject sideAForeground;
    public GameObject sideBForeground;
    public float moveSpeed;

    private float indicatorBaseAlpha;
    private Dictionary<Side, bool> damageFlags = new Dictionary<Side, bool>();
    private Transform destination;
    private Vector3 velocity = Vector3.zero;

    private Burnable[] sideABurnables;
    private Burnable[] sideBBurnables;

    private void Start()
    {
        indicatorBaseAlpha = topHit.color.a;
        damageFlags[Side.A] = false;
        damageFlags[Side.B] = false;
        bookWyrm.position = topPosition.position;
        destination = topPosition;
        sideABurnables = sideAForeground.GetComponentsInChildren<Burnable>();
        sideBBurnables = sideBForeground.GetComponentsInChildren<Burnable>();
    }

    private void Update()
    {
        foreach (KeyValuePair<Side, bool> entry in damageFlags)
        {
            if(entry.Value && entry.Key == player.GetSide())
            {
                Debug.Log("Player Hit");
            }
        }
        bookWyrm.position = Vector3.SmoothDamp(bookWyrm.position, destination.position, ref velocity, moveSpeed);
    }

    private IEnumerator FadeOut(SpriteRenderer sprite, float speed)
    {
        sprite.enabled = true;
        float alphaVal = sprite.color.a;
        Color tmp = sprite.color;

        while (sprite.color.a > 0)
        {
            alphaVal -= 0.01f;
            tmp.a = alphaVal;
            sprite.color = tmp;

            yield return new WaitForSeconds(speed);
        }
        sprite.enabled = false;
        tmp = sprite.color;
        tmp.a = indicatorBaseAlpha;
        sprite.color = tmp;
    }

    private IEnumerator FadeOutAndHold(Side side, float speed, float holdDuration)
    {
        SpriteRenderer indicator;
        SpriteRenderer fire;
        Burnable[] toBurn;
        if (side == Side.A)
        {
            indicator = topHit;
            fire = topFlame;
            toBurn = sideABurnables;
        } else
        {
            indicator = bottomHit;
            fire = bottomFlame;
            toBurn = sideBBurnables;
        }
        indicator.enabled = true;
        float alphaVal = indicator.color.a;
        Color tmp = indicator.color;

        while (indicator.color.a > 0)
        {
            alphaVal -= 0.01f;
            tmp.a = alphaVal;
            indicator.color = tmp;

            yield return new WaitForSeconds(speed);
        }
        tmp = indicator.color;
        tmp.a = indicatorBaseAlpha;
        indicator.color = tmp;
        indicator.enabled = false;
        fire.enabled = true;
        foreach (Burnable b in toBurn)
        {
            b.Burn();
        }
        yield return new WaitForSeconds(holdDuration);
        fire.enabled = false;
    }

    public void Swipe(Side side, float delay)
    {
        destination = side == Side.A ? topPosition : bottomPosition;
        StartCoroutine(InstantDamage(delay, side));
    }

    public void FireBreath(Side side, float delay, float duration)
    {
        destination = side == Side.A ? topPosition : bottomPosition;
        StartCoroutine(ContinuousDamage(delay, duration, side));
    }

    private IEnumerator InstantDamage(float delaySeconds, Side side)
    {
        float fadeSpeed = delaySeconds / (indicatorBaseAlpha / .01f);
        StartCoroutine(FadeOut(side == Side.A ? topHit : bottomHit, fadeSpeed));
        yield return new WaitForSeconds(delaySeconds);
        if (player.GetSide() == side)
        {
            Debug.Log("Player Hit!");
        }
    }

    private IEnumerator ContinuousDamage(float delaySeconds, float duration, Side side)
    {
        float fadeSpeed = delaySeconds / (indicatorBaseAlpha / .01f);
        StartCoroutine(FadeOutAndHold(side, fadeSpeed, duration));
        yield return new WaitForSeconds(delaySeconds);
        StartCoroutine(ContinuousDamageCheck(duration, side));
    }

    private IEnumerator ContinuousDamageCheck(float duration, Side side)
    {
        FlagDamageOn(side);
        yield return new WaitForSeconds(duration);
        FlagDamageOff(side);
    }

    private void FlagDamageOn(Side side)
    {
        damageFlags[side] = true;
    }

    private void FlagDamageOff(Side side)
    {
        damageFlags[side] = false;
    }
}