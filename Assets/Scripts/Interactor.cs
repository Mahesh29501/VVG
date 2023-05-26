using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Interactor : MonoBehaviour
{
    public LayerMask interactableLayerMask = 6;
    public LayerMask door = 7;
    public GameObject reticle;
    public GameObject textforclick;
    Text textforclicktxt;
    public Sprite dot;
    public Sprite eye;
    public Sprite hand;
    public float timeSwitchedOnTotal = 0f;
    bool onetime = true;
    public bool timerOn = false;
    //public double[] timeSwitchedOn = new double[600];
    public ArrayList timeSwitchedOn = new ArrayList();
    public ArrayList timePercentage = new ArrayList();
    public ArrayList nameNFT = new ArrayList();
    public double totalTimeSpent;
    public GameObject pausePanel;
    public bool paused = false;

    public GameObject percentagePanel;
    public Interactable interactable;
    public GameObject NFT_Holder;
    public ScoreManager scoreManager;
    public PlayerController rgb;

    Image imgg;

    public Text timer;
    public float timeleft;
    public bool interactorSet = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //rgb = gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        timeleft = 900f;
        imgg = reticle.GetComponent<Image>();
        timer.text = timeleft.ToString();
        textforclicktxt = textforclick.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit, 15, interactableLayerMask))
        {
            if (onetime)
            {
                if(hit.collider.tag == "door")
                {
                    imgg.sprite = hand;
                }
                else
                {
                    imgg.sprite = eye;
                }
                
                textforclick.SetActive(true);
                textforclicktxt.text = "LEFT CLICK TO VIEW";
            }
            onetime = false;
          
            if (hit.collider.GetComponent<Interactable>() != false)
            {
                // Debug.Log("in collided");


                if (interactable == null || interactable.ID != hit.collider.GetComponent<Interactable>().ID)
                {
                    interactable = hit.collider.GetComponent<Interactable>();

                    if (hit.collider.tag != "Door")
                    {
                        //Debug.Log("in collided");
                        interactable.timerOn = true;
                        interactorSet = true;
                        //Debug.Log("Interactable" + gameObject.name + " timeron " + interactable.timerOn);
                    }

                }
                else
                {
                    interactorSet = false;
                    //textforclicktxt.text = "LEFT CLICK TO OPEN";
                }
                
                

                if (Input.GetMouseButtonDown(0))
                {
                    interactable.onInteract.Invoke();
                    //SetToZero();
                    //gameObject.GetComponent<>
                    hit.collider.GetComponent<Interactable>().timerOn = true;
                    textforclick.SetActive(false);
                }
            }

            if (hit.collider.tag == "Door")
            {
                imgg.sprite = hand;
                textforclick.SetActive(true);
                textforclicktxt.text = "Left Click to Open";
            }
            
        }
        else
        {
            if (!onetime)
            {
                imgg.sprite = dot;
                textforclick.SetActive(false);

                if(interactable != null)
                {
                    //Debug.Log("inside else interactable: " + interactable.name);
                    interactable.timerOn = false;
                    interactable = null;
                }
            }
            onetime = true;
        }

        if (Input.GetKeyDown(KeyCode.Q) && !paused)
        {
            paused = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.R) && paused)
        {
            paused = false;
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E) && paused)
        {
            ShowTable();
            ExitMuseum();
        }

        timeleft -= Time.deltaTime;
        timer.text = timeleft.ToString();
        if(timeleft < 0f)
        {
            ShowTable();
            ExitMuseum();
        }
    }

    public void SetToZero()
    {
        rgb.mouseSensitivity= 0;
        rgb.walkSpeed= 0;
        rgb.sprintSpeed= 0;
        rgb.jumpForce= 0;
        //rgb.mouseLook.XSensitivity = 0;
        //rgb.mouseLook.YSensitivity = 0;
        //rgb.movementSettings.ForwardSpeed = 0;
        //rgb.movementSettings.BackwardSpeed = 0;
        //rgb.movementSettings.StrafeSpeed = 0;
    }
    public void SetToNormal()
    {
        rgb.mouseSensitivity = 3;
        rgb.walkSpeed = 3;
        rgb.sprintSpeed = 6;
        rgb.jumpForce = 300;
        //rgb.mouseLook.XSensitivity = 2;
        //rgb.mouseLook.YSensitivity = 2;
        //rgb.movementSettings.ForwardSpeed = 8;
        //rgb.movementSettings.BackwardSpeed = 4;
        //rgb.movementSettings.StrafeSpeed = 4;
    }

    public void ExitMuseum()
    {
        //Debug.Log(" exit museum ");
        CalculatePercentage();
        if (paused)
        {
            paused = false;
            Time.timeScale = 1;
        }
        ShowTable();
    }

    public void CalculatePercentage()
    {

        Interactable[] allChildren = NFT_Holder.GetComponentsInChildren<Interactable>();
        int i = 0;
        totalTimeSpent = 0f;
        foreach (Interactable child in allChildren)
        {
            //Debug.Log(" "+ i+" "+child.gameObject.GetComponent<Interactable>().timeSwitchedOnTotal);
            timeSwitchedOn.Add(child.gameObject.GetComponent<Interactable>().timeSwitchedOnTotal);
            nameNFT.Add(child.gameObject.GetComponent<Interactable>().titlenft);
            totalTimeSpent += child.gameObject.GetComponent<Interactable>().timeSwitchedOnTotal;

            i++;
        }
        i = 0;
        foreach (Interactable child in allChildren)
        {
            timePercentage.Add((child.gameObject.GetComponent<Interactable>().timeSwitchedOnTotal * 100) / totalTimeSpent);
            //Debug.Log(" Painting " + i + " score : " + (child.gameObject.GetComponent<Interactable>().timeSwitchedOnTotal * 100) / totalTimeSpent);
            //Debug.Log(child.gameObject.GetComponent<Interactable>().titlenft.GetType());
            scoreManager.AddScore(new Score(child.gameObject.GetComponent<Interactable>().titlenft, (child.gameObject.GetComponent<Interactable>().timeSwitchedOnTotal * 100) / totalTimeSpent));
            //Debug.Log(" Painting " + i + " score : " + timePercentage[i]);
            i++;
        }
    }
    public void ShowTable()
    {
        //Debug.Log(" Show table ");
        percentagePanel.SetActive(true);
    }
}
