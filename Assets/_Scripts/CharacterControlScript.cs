using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using JetBrains.Annotations;
//using UnityEditor;
using UnityEngine;

public class CharacterControlScript : MonoBehaviour
{
    private String theme;
    private AudioSource source;
    private AudioClip moveSand;
    private AudioClip moveGrass;
    private AudioClip moveSnow;

    private float deltatime = 0.0f;
    private float timeLastMoved = 0.0f;

    public Vector2 direction; //The Direction the Charcter is going to go to
    public float speed = 10.0f; //The Standard Movment Speed of the Character

    public uint DashCounter; //Amount of overall dashes, used for some items
    public float dashLengthInSeconds = 0.2f; //How many seconsd the charter is trapped inside of the Dash
    public float dashLength = 50f; //How many Units the Character dashes
    public float dashSlow = 0.25f; //Bye how much the Character slowed
    public float chargeRatePerSecond = 1.5f;

    public float currentCharge;
    public float dashMultiplierLevelOne = 1;
    public float dashMultiplierLevelTwo = 2;
    public float dashMultiplierLevelThree = 3;
    public bool charging; //Wheteher or not the User is charging his dash
    public bool dashing; //Wherether or not the Character is curretnly "trapped" in the dash


    public bool hasAnActiveItem;
    public Item ItemInInventory;

    private float ItemActiveForFixedUpdates;
    public GameObject[] Cameras;
    public GameObject[] Microphones;
    private float dashedForFixedUpdates; //private count how often the loop refreshed until the player regains control
    private Rigidbody2D rb;
    private UIController _uiController;
    private DashTrail _dashTrail;

    private Animator animator;
    private bool lastFrameDashing;

    private TimeController _timeController;

    void Start()
    {
        _timeController = EventController.Instance.GetComponent<TimeController>();
        _timeController.StartTime();
        _uiController = GameObject.Find("Canvas").GetComponent<UIController>();
        rb = GetComponent<Rigidbody2D>();
        Cameras = GameObject.FindGameObjectsWithTag("Camera");
        Microphones = GameObject.FindGameObjectsWithTag("Microphone");
        animator = GetComponent<Animator>();
        _dashTrail = GetComponentInChildren<DashTrail>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!source)
        {
            source = EventController.Instance.GetComponent<AudioSource>();
            moveGrass = Resources.Load("Sounds/gehen Gras") as AudioClip;
            moveSand = Resources.Load("Sounds/gehen Sand") as AudioClip;
            moveSnow = Resources.Load("Sounds/schritte Schnee") as AudioClip;
            theme = GameObject.FindGameObjectWithTag("Room").GetComponent<RoomBehaviour>().theme;
        }
        if (Input.GetButtonDown("Fire1"))
        {
            if (!hasAnActiveItem && ItemInInventory != null)
            {
                hasAnActiveItem = true;
                ItemInInventory.UseItem(transform.gameObject);
                ItemInInventory = null;
                SendMessageToHat();
            }
        }
        if (Input.GetKey(KeyCode.M) && Input.GetKeyDown(KeyCode.N)) {
            Debug.Log("Cheater");
            var number = 0;
            if(Input.GetKey(KeyCode.Alpha1)) {
                number = 0;
            }
            else if(Input.GetKey(KeyCode.Alpha2)) {
                number = 1;
            }
            else if(Input.GetKey(KeyCode.Alpha3)) {
                number = 2;
            }
            else if(Input.GetKey(KeyCode.Alpha4)) {
                number = 3;
            }
            
            var spawner = EventController.Instance.GetComponent<CheatItemSpawnpoint>();
            var obj = spawner.GenerateItem(number);
            var ctrl = obj.GetComponent<ItemController>();
            ctrl.AddItemToInventory(this);
            Destroy(obj);
        }
        if (Input.GetKey(KeyCode.M) && Input.GetKeyDown(KeyCode.L)) {
            Debug.Log("Life long and prosper");
            EventController.Instance.GetComponent<AwarenessController>().lifecount += 10;
        }

        if (!dashing)
        {
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            direction = direction.normalized;

            animator.SetFloat("speed", direction.magnitude);

            if (direction.magnitude > 0.1)
            {
                animator.SetBool("right", false);
                animator.SetBool("left", false);
                animator.SetBool("backward", false);
                animator.SetBool("forward", false);

                if (direction.x > 0)
                {
                    animator.SetBool("right", true);
                }
                else if (direction.x < 0)
                {
                    animator.SetBool("left", true);
                }
                else if (direction.y > 0)
                {
                    animator.SetBool("backward", true);
                }
                else
                {
                    animator.SetBool("forward", true);
                }
            }
        }
        if (Input.GetButton("Jump") && !dashing
        ) //Space triggers the dash aslong as the character is not already dashing
        {
            //and the dashing state set true
            currentCharge += chargeRatePerSecond * Time.deltaTime;
            animator.SetBool("charging", true);
            charging = true;
        }
        else if (Input.GetButtonUp("Jump") && charging && !dashing)
        {
            charging = false;
            dashing = true;
        }
    }

    public void SendMessageToHat()
    {
        if (ItemInInventory != null)
        {
            SendMessage("ChangeItem", ItemInInventory.GetName());
        }
        else
        {
            SendMessage("ChangeItem", "none");
        }
    }

    private void FixedUpdate()
    {
        if (!dashing && !charging)
        {
            rb.velocity = direction * speed; //Velocity is the product of the set speed with the raw Axis Input
            currentCharge = 0f;

            if ((deltatime > 0.3f) && (direction != Vector2.zero))
            {
                switch (theme)
                {
                    case "ice":
                        source.PlayOneShot(moveSand, 1.0f);
                        break;
                    case "grass":
                        source.PlayOneShot(moveGrass, 1.0f);
                        break;
                    case "beach":
                        source.PlayOneShot(moveSand, 1.0f);
                        break;
                    default:
                        source.PlayOneShot(moveGrass, 1.0f);
                        break;
                }
                deltatime = 0.0f;
            }
            deltatime += Time.deltaTime;
        }
        else if (charging)
        {
            rb.velocity = direction * speed * dashSlow; //The Character is slowed,per default by 25%
            currentCharge = currentCharge + (chargeRatePerSecond / 50); //Charge goes up by 0.25 in one Second
            _uiController.DashLevelOne.fillAmount = currentCharge;
            if (currentCharge >= 1 && currentCharge < 2)
            {
                _uiController.DashLevelOne.fillAmount = 1f;
                _uiController.DashLevelTwo.fillAmount = currentCharge - 1f;
            }
            else if (currentCharge >= 2 && currentCharge < 3)
            {
                _uiController.DashLevelTwo.fillAmount = 1f;
                _uiController.DashLevelTwo.fillAmount = 1f;
                _uiController.DashLevelThree.fillAmount = currentCharge - 2f;
            }
            else if (currentCharge >= 3) //The Current Charge can not go over 3
            {
                currentCharge = 3;
                _uiController.DashLevelThree.fillAmount = 1f;
                charging = false;
                dashing = true;
            }
        }
        else if (dashing)
        {
            animator.SetBool("dashing", true);
            if (currentCharge >= 1 && currentCharge < 2)
            {
                Dash(dashMultiplierLevelOne, 1);
            }
            else if (currentCharge >= 2 && currentCharge < 3)
            {
                Dash(dashMultiplierLevelTwo, 2);
            }
            else if (currentCharge == 3)
            {
                Dash(dashMultiplierLevelThree, 3);
            }
            else
            {
                // Break, if charge not big enough
                animator.SetBool("dashing", false);
                animator.SetBool("charging", false);
                dashing = false;
                currentCharge = 0f;
                _uiController.DashLevelOne.fillAmount = 0f;
                _uiController.DashLevelTwo.fillAmount = 0f;
                _uiController.DashLevelThree.fillAmount = 0f;
            }
        }

        if (lastFrameDashing && !dashing)
            _dashTrail.StopTrail();
        if (!lastFrameDashing && dashing)
            _dashTrail.StartTrail();

        lastFrameDashing = dashing;
        lastFrameDashing = dashing;
    }

    private void Dash(float dashMultiplier, float ChargeLeft)
    {
        if (dashedForFixedUpdates <= dashLengthInSeconds / 0.02)
        {
            //The Dash Velocity should be the product of dash Speed and the direction cut into as many steps as wanted

            rb.velocity = direction * (dashLength / dashLengthInSeconds) * dashMultiplier; //s/t=v
            dashedForFixedUpdates++;
        }
        else //IF the Character already dashed for the proper Amount of Updates the Chacrter is taken out of the Loop
        {
            dashedForFixedUpdates = 0;
            dashing = false;
            animator.SetBool("dashing", false);
            animator.SetBool("charging", false);
            currentCharge = currentCharge - ChargeLeft;
            DashCounter++;
            _uiController.DashLevelOne.fillAmount = 0f;
            _uiController.DashLevelTwo.fillAmount = 0f;
            _uiController.DashLevelThree.fillAmount = 0f;
        }
    }
}