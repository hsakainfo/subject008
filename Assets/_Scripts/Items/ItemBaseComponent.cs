using UnityEngine;

public class ItemBaseComponent : MonoBehaviour
{
    public float time = 5;
    private float timeCounter = 0;
    protected UIController uiController;
    public virtual void Start() {
        uiController = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIController>();

        //Starts the Cooldown of the ability
		uiController.CoolDownTime = time;
		uiController.CoolDownTimer.fillAmount = 1;
		uiController.CoolDownActivated = true;
    }

    public virtual void Update() {
        timeCounter += Time.deltaTime;
		if (timeCounter >= time) {
			Destroy (this); 
		}
    }

    public virtual void OnDestroy() {
        //Check that we have a player
        var player = GameObject.FindGameObjectWithTag("Player");
        if(!player)
        {
            //If we don't hava player, clear the screen
            ClearUI();
            return;
        }
        //Get player character control script
        var playerCCS = player.GetComponent<CharacterControlScript>();
        playerCCS.hasAnActiveItem = false;
        //Is this needed???
		if (playerCCS.ItemInInventory == null)
		{
            ClearUI();
		}

    }

    private void ClearUI()
    {
        uiController.Item.sprite = null;
        uiController.Item.enabled = false;
    }
} 