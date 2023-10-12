using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public bool turnRoundBool = true ;
    public bool autoRoundBool = false ;
    public BallCom ballCom ;
    public SaveTimes saveTimes ;
    public GameObject atkCtrl ;
    public GameObject button ;
    public Image countDown ;
    public Image bossHp ;
    public Image playerHp ;

    public Animator bossani;
    public Animator playerani;


    void Start()
    {
        ballCom = GameObject.Find("BallCom").GetComponent<BallCom>();
        saveTimes = GameObject.Find("SaveTimes").GetComponent<SaveTimes>();

    }

    

    

    // Update is called once per frame
    void Update()
    {
       if(ballCom.combo > 0)
       {

       //Invoke("ACT" , 1);
       

       }

       if(bossHp.fillAmount == 0)
       {
       bossani.SetTrigger("bossdie");
       Invoke("QuitGame",5);
       //關卡結束

       }
       if(playerHp.fillAmount == 0)
       {
       playerani.SetTrigger("die01");
       //playerani.SetTrigger("die02");
       Invoke("ReStart",4.8f);
       //關卡重新

       }


    }

   

    public void AttackRound()
    {
    atkCtrl.gameObject.SetActive(true);
    Invoke("PlayRound",5);
    Invoke("CountDown",0.1f);
    

    }
    public void PlayRound()
    {
    atkCtrl.gameObject.SetActive(false);
    countDown.fillAmount = 1 ;
    //botton.Clickable;

    }

    public void CountDown()
    {
    countDown.fillAmount = 0 ;
    }

    public void QuitGame()
    {
    Application.Quit();
    }
   
    public void ReStart()
    {

     SceneManager.LoadScene("OP");

    }


}
