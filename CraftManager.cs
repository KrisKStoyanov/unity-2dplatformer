using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour {

    public static CraftManager Instance { get; set; }

    public float craftPoints;

    public float throwables;

    public Canvas craftingInterface;

    static readonly int anim_Activate = Animator.StringToHash("Activate");
    static readonly int anim_Deactivate = Animator.StringToHash("Deactivate");

    static readonly int anim_ToggleOne = Animator.StringToHash("ToggleOne");
    static readonly int anim_ToggleTwo = Animator.StringToHash("ToggleTwo");
    static readonly int anim_ToggleThree = Animator.StringToHash("ToggleThree");

    private bool hideCraftUI = false;
    private float counter = 100f;

    public GameObject extensionOne;
    public GameObject extensionTwo;
    public GameObject extensionThree;

    private bool toggleExtensionOne = false;
    private bool toggleExtensionTwo = false;
    private bool toggleExtensionThree = false;

    public GameObject notEnoughCraftPoints;
    public GameObject maxInvReachedInfo;

    private bool hideMaxInvReach = false;
    private float maxInvCounter = 100f;

    private bool hideInsufficientPoints = false;
    private float insufPointCounter = 100f;

    public Text inventoryListing;

    public float capacitors;
    private float maxCapacitors;

    public float fuelLoaded;
    public float maxFuel = 100f;

    [Header("Unity UI")]
    public Text materialsAmount;
    public GameObject materialsUI;

    //public GameObject extensionHUD;
    public GameObject throwablesHUD;
    public Text throwablesText;
    public GameObject ultracapacitorsHUD;
    public Text capacitorsText;
    public GameObject activCapBtn;

    public Animator craftUIanim;
    public Animator craftExtensionAnim;

    public Button craftTouchBtn;

    public Image fuelTank;
    public GameObject fuelUI;
    public GameObject throwTCbuttton;

    private int maxThrowables = 10;

    [Header("Jetpack2D")]
    public GameObject jetpackSprite;

    private void Awake()
    {
        Instance = this;
    }
    public void GivePoints(float points)
    {
        craftPoints = craftPoints + points;
        StartCoroutine(ShowScrapCount());
        materialsAmount.text = craftPoints + "";
    }

    public void TakePoints(float points)
    {
        craftPoints = craftPoints - points;
        if(craftPoints < 0)
        {
            craftPoints = 0f;
        }
        inventoryListing.text = "" + craftPoints;
    }

    public void Craft()
    {
        craftingInterface.gameObject.SetActive(true);
        
        craftUIanim.SetTrigger(anim_Activate);
        //craftTouchBtn.gameObject.SetActive(false);
        inventoryListing.text = "" + craftPoints;
    }

    public void StopCraft()
    {
        craftUIanim.SetTrigger(anim_Deactivate);
        toggleExtensionOne = false;
        toggleExtensionTwo = false;
        toggleExtensionThree = false;
        craftExtensionAnim.SetBool(anim_ToggleOne, false);
        craftExtensionAnim.SetBool(anim_ToggleTwo, false);
        craftExtensionAnim.SetBool(anim_ToggleThree, false);
    }

    public void CraftThrowable()
    {
        if(craftPoints >= 10f)
        {
            if (throwables < maxThrowables)
            {
                PlayerAudioController.Instance.PlayCraftSound();
                throwables += 1f;
                TakePoints(10f);
                throwablesText.text = throwables + "";
                //extensionHUD.SetActive(true);
                throwablesHUD.gameObject.SetActive(true);
                throwTCbuttton.gameObject.SetActive(true);
            }
        }
        else if (craftPoints < 10f)
        {
            NotEnoughPoints();
        }
        else if(throwables == maxThrowables)
        {
            CraftNothing();
        }
    }
    public void CraftCapacitor()
    {
        if(craftPoints >= 50f)
        {
            PlayerAudioController.Instance.PlayCraftSound();
            capacitors += 1f;
            TakePoints(50f);
            if(HealthManager.Instance.currentHealth > 0 && HealthManager.Instance.currentHealth < 100)
            {
                activCapBtn.gameObject.SetActive(true);
            }
            else if(HealthManager.Instance.currentHealth == 0)
            {
                activCapBtn.gameObject.SetActive(false);
            }
            //extensionHUD.SetActive(true);
            ultracapacitorsHUD.gameObject.SetActive(true);
            capacitorsText.text = capacitors + "";
        }
        else if (craftPoints < 50f)
        {
            NotEnoughPoints();
        }
    }
    public void CraftFuelTank()
    {
        if(craftPoints >= 200f)
        {
            PlayerAudioController.Instance.PlayCraftSound();
            fuelLoaded = 100f;
            TakePoints(200f);
            fuelUI.gameObject.SetActive(true);
            //jetpackSprite.gameObject.SetActive(true);
        }
        else if (craftPoints < 200f)
        {
            NotEnoughPoints();
        }
    }


    void CraftNothing()
    {
        maxInvReachedInfo.gameObject.SetActive(true);
        hideMaxInvReach = true;
    }
    void HideMaxProjectileReached()
    {
        maxInvReachedInfo.gameObject.SetActive(false);
        hideMaxInvReach = false;
        maxInvCounter = 100f;
    }
    void NotEnoughPoints()
    {
        notEnoughCraftPoints.gameObject.SetActive(true);
        hideInsufficientPoints = true;
    }
    void HideInsufficientPoints()
    {
        notEnoughCraftPoints.gameObject.SetActive(false);
        hideInsufficientPoints = false;
        insufPointCounter = 100f;
    }
    private IEnumerator ShowScrapCount()
    {
        materialsUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        materialsUI.gameObject.SetActive(false);
    }

    public void ToggleExtensionOne()
    {
        toggleExtensionOne = !toggleExtensionOne;
        if (toggleExtensionOne == true)
        {
            toggleExtensionTwo = false;
            toggleExtensionThree = false;
            craftExtensionAnim.SetBool(anim_ToggleOne, true);
            craftExtensionAnim.SetBool(anim_ToggleTwo, false);
            craftExtensionAnim.SetBool(anim_ToggleThree, false);

            /*extensionOne.SetActive(true);
            extensionTwo.SetActive(false);
            extensionThree.SetActive(false);*/
        }
        else if (toggleExtensionOne == false)
        {

            craftExtensionAnim.SetBool(anim_ToggleOne, false);
            craftExtensionAnim.SetBool(anim_ToggleTwo, false);
            craftExtensionAnim.SetBool(anim_ToggleThree, false);

            /*extensionOne.SetActive(false);
            extensionTwo.SetActive(false);
            extensionThree.SetActive(false);*/
        }
    }

    public void ToggleExtensionTwo()
    {
        toggleExtensionTwo = !toggleExtensionTwo;
        if (toggleExtensionTwo == true)
        {
            toggleExtensionOne = false;
            toggleExtensionThree = false;
            craftExtensionAnim.SetBool(anim_ToggleOne, false);
            craftExtensionAnim.SetBool(anim_ToggleTwo, true);
            craftExtensionAnim.SetBool(anim_ToggleThree, false);

            /*extensionOne.SetActive(false);
            extensionTwo.SetActive(true);
            extensionThree.SetActive(false);*/
        }
        else if (toggleExtensionTwo == false)
        {

            craftExtensionAnim.SetBool(anim_ToggleOne, false);
            craftExtensionAnim.SetBool(anim_ToggleTwo, false);
            craftExtensionAnim.SetBool(anim_ToggleThree, false);

            /*extensionOne.SetActive(false);
            extensionTwo.SetActive(false);
            extensionThree.SetActive(false);*/
        }
    }

    public void ToggleExtensionThree()
    {
        toggleExtensionThree = !toggleExtensionThree;
        if (toggleExtensionThree == true)
        {
            toggleExtensionOne = false;
            toggleExtensionTwo = false;
            craftExtensionAnim.SetBool(anim_ToggleOne, false);
            craftExtensionAnim.SetBool(anim_ToggleTwo, false);
            craftExtensionAnim.SetBool(anim_ToggleThree, true);

            /*extensionOne.SetActive(false);
            extensionTwo.SetActive(false);
            extensionThree.SetActive(true);*/
        }
        else if (toggleExtensionThree == false)
        {

            craftExtensionAnim.SetBool(anim_ToggleOne, false);
            craftExtensionAnim.SetBool(anim_ToggleTwo, false);
            craftExtensionAnim.SetBool(anim_ToggleThree, false);

            /*extensionOne.SetActive(false);
            extensionTwo.SetActive(false);
            extensionThree.SetActive(false);*/
        }
    }


}
