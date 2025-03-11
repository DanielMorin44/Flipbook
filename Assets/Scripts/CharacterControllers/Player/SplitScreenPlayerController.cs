using UnityEngine;
using UnityEngine.Tilemaps;

public class SplitScreenPlayerController : PlayerController
{
    private bool sideA;
    public TilemapCollider2D sideAMap;
    public TilemapCollider2D sideBMap;
    public GameObject playerShadow;

    void Start()
    {
        Initialize();
    }

    new void Initialize()
    {
        base.Initialize();
        inventory.AddFlipToken(new FlipTokenController());
        terrain = LayerMask.GetMask("SideA");
        sideA = true;
        sideBMap.enabled = false;
    }

    private bool CheckSplitScreenFlipAllowed()
    {
        return Physics2D.OverlapBoxAll(box.bounds.center,
            new Vector2(box.bounds.size.x, box.bounds.size.y * .9f), 
            0,
            sideA ? LayerMask.GetMask("SideA") : LayerMask.GetMask("SideB")).Length == 0;
    }

    public void HandleSplitScreenFlip()
    {
        if (CheckSplitScreenFlipAllowed())
        {
            sideA = !sideA;
            if (sideA)
            {
                terrain = LayerMask.GetMask("SideA");
                gameObject.layer = LayerMask.NameToLayer("SideAPlayer");
                playerShadow.layer = LayerMask.NameToLayer("SideBPlayer");
                sideAMap.enabled = true;
                sideBMap.enabled = false;
            }
            else
            {
                terrain = LayerMask.GetMask("SideB");
                gameObject.layer = LayerMask.NameToLayer("SideBPlayer");
                playerShadow.layer = LayerMask.NameToLayer("SideAPlayer");
                sideBMap.enabled = true;
                sideAMap.enabled = false;
            }
        }
    }

    new protected void Flip()
    {
        shouldFlip = false;
        HandleSplitScreenFlip();
    }

    new public void FlipSuccess()
    {
        AudioTrigger(PlayerAudioSignal.PAGE_FLIP);
    }
}
