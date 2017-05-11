using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MainMenuBehaviour : MonoBehaviour {


    public GameObject MainMenu;
    public GameObject Credits;
    public GameObject JoiningPanel;
    public GameObject IP;
 
    public GameObject Histoire1;
    public GameObject Histoire2;
    public InputField IpHolder;
    public NetworkManager Net;
    public AudioClip Validation;
    public AudioClip CrashSound;
    public AudioClip HighLight;
    public AudioClip launch;
    public GameObject Camera;
    private AudioSource Audio;
    public bool fadeIn;
    public bool fadeOut;

    public GameObject Voiture;
    public GameObject VoitureDestination;
    private bool GO;
    private bool Arrived;




    // Use this for initialization
    void Start () {
        GO = false;
        IpHolder.text = "192.168.1.";
        Net  = FindObjectOfType<NetworkManager>();

        Audio = Camera.GetComponent<AudioSource>();
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
       
            



    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GO && !Arrived)
        {

           Voiture.transform.position = Vector3.MoveTowards( Voiture.transform.position, VoitureDestination.transform.position, Time.fixedDeltaTime * 40.0F);
           
            if (Vector3.Distance(VoitureDestination.transform.position, Voiture.transform.position) < 1)
            {
                Arrived = true;


            }
           
        }

    }
    
        void Update () {

        
      


        if (fadeIn)
        {

            this.GetComponent<CanvasGroup>().alpha = this.GetComponent<CanvasGroup>().alpha + Time.deltaTime;
            if (this.GetComponent<CanvasGroup>().alpha >= 0.5F)
            {
                this.GetComponent<CanvasGroup>().alpha = 0.5F;
                fadeIn = false;
            }
        }
        if (fadeOut)
        {

            this.GetComponent<CanvasGroup>().alpha = this.GetComponent<CanvasGroup>().alpha - Time.deltaTime;
            if (this.GetComponent<CanvasGroup>().alpha <= 0)
            {
                this.GetComponent<CanvasGroup>().alpha = 0;
                fadeOut = false;
            }

        }

    }


    public void GoToCredits()
    {
   

        Audio.PlayOneShot(Validation);

        MainMenu.SetActive(false);
  
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
        Credits.SetActive(true);
      

    }

    public void ComeBackFromCredits()
    {
        Credits.SetActive(false);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
      
        MainMenu.SetActive(true);
        
    }

    public void QuitGame()
    {

        Application.Quit();
    }


    public void  GoToIp()
    {
        Audio.PlayOneShot(Validation);
        JoiningPanel.SetActive(false);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
        IP.SetActive(true);
        IpHolder.text = "192.168.1.";


    }
    public void BackFromIp()
    {
        IP.SetActive(false);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
        JoiningPanel.SetActive(true);
        IpHolder.text = "192.168.1.";


    }

    public void BackfromJoin()
    {
        JoiningPanel.SetActive(false);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
        MainMenu.SetActive(true);
        
    }

    public void GotoJoin()
    {
        Audio.PlayOneShot(Validation);
         MainMenu.SetActive(false);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
        JoiningPanel.SetActive(true);
 
    }

    public void Host()
    {

        StartCoroutine(LaunchGameHost());
        GO = true;

    }

    public void Join()
    {
    
        this.GetComponent<CanvasGroup>().alpha = 0f;
   
   

        StartCoroutine(LaunchGameClient());
        GO = true;



    }

    IEnumerator LaunchGameHost()
    {
        fadeOut = true;
        yield return new WaitForSeconds(1);
    
        JoiningPanel.SetActive(false);

        Audio.PlayOneShot(launch);
    
        yield return new WaitForSeconds(2);
        JoiningPanel.SetActive(false);
        Histoire1.SetActive(true);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
        yield return new WaitForSeconds(6);
        fadeOut = true;
        yield return new WaitForSeconds(2);
        Histoire1.SetActive(false);
        Histoire2.SetActive(true);
        yield return new WaitForSeconds(2);
        fadeIn = true;
        yield return new WaitForSeconds(6);
        fadeOut = true;
        Audio.PlayOneShot(CrashSound);
        yield return new WaitForSeconds(4);
        Histoire2.SetActive(false);

        Net.StartHost();
     


    }
    IEnumerator LaunchGameClient()
    {
        fadeOut = true;
        yield return new WaitForSeconds(1);
     
        JoiningPanel.SetActive(false);
        IP.SetActive(false);
      
        Audio.PlayOneShot(launch);
        
        yield return new WaitForSeconds(2);
        JoiningPanel.SetActive(false);
        Histoire1.SetActive(true);
        this.GetComponent<CanvasGroup>().alpha = 0f;
        fadeIn = true;
        yield return new WaitForSeconds(3);
        fadeOut = true;
        yield return new WaitForSeconds(1);
        Histoire1.SetActive(false);
        Histoire2.SetActive(true);
        yield return new WaitForSeconds(1);
        fadeIn = true;
        yield return new WaitForSeconds(3);
        fadeOut = true;
        yield return new WaitForSeconds(1);
        Histoire2.SetActive(false);



        Net.networkAddress = IpHolder.text;
        Net.networkPort = 7777;
        Net.StartClient();
      

    }
    
}
