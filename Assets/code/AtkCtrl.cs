﻿using UnityEngine; using UnityEngine.UI; using TMPro;  public class AtkCtrl : MonoBehaviour {      public GameObject farChter ;     public GameObject nearChter ;     public GameObject bossChter ;     public BallCom ballCom ;     public SaveTimes saveTimes ;     public Animator ani01;     public Animator ani02;     public Animator aniBoss;     public Image bossHp ;      public Image playerHp ;      public GameObject nearPoint ;     public GameObject farPoint ;     public GameObject UIDamage ;     public GameObject UIDamagePlayer ;      int combo ;      int farAtkC ;      int nearAtkC ;      int forwardC ;      int backC ;      public int moveStep ;       public bool farCanAtk ;     public bool nearCanAtk ;          public AudioSource saberSFX;     public AudioSource gunSFX;      public TextMeshProUGUI textDamage;      public bool oneTimes = false;     public bool threeTimes = false;     public bool fiveTimes = false;     public bool sevenTimes = false;                     // Start is called before the first frame update     void Start()     {         ballCom = GameObject.Find("BallCom").GetComponent<BallCom>();         saveTimes = GameObject.Find("SaveTimes").GetComponent<SaveTimes>();              }                  // Update is called once per frame     void Update()     {          //if(saveTimes.combo == 1)Invoke("Action", 1.5f) ;     if(saveTimes.combo > 0)     {     combo = saveTimes.combo ;     saveTimes.combo = 0 ;     if(combo>=1)      {       oneTimes = true;       threeTimes = false;       fiveTimes = false;       sevenTimes = false;       if(combo>=3)        {       oneTimes = false;       threeTimes = true;       fiveTimes = false;       sevenTimes = false;         if(combo>=5)       {       oneTimes = false;       threeTimes = false;       fiveTimes = true;       sevenTimes = false;
      if(combo>=7)
      {
      oneTimes = false;       threeTimes = false;       fiveTimes = false;       sevenTimes = true;
      }
      }       }             }                 //Invoke("FarAtk" , 3);     //Invoke("FarAtk" , 4);     Invoke("Action", 1.5f) ;          }          if(saveTimes.forwardCount > 0)     {     forwardC = saveTimes.forwardCount;     saveTimes.forwardCount = 0 ;     Invoke("JumpFarChter" , 1);     Invoke("JumpFarChter" , 2);     Invoke("RunNearChter" , 1);     Invoke("RunNearChter" , 2);         }          if(saveTimes.backCount > 0)     {     backC = saveTimes.backCount;     saveTimes.backCount = 0 ;     Invoke("JumpFarChter" , 1);     Invoke("JumpFarChter" , 2);     Invoke("RunNearChter" , 1);     Invoke("RunNearChter" , 2);          }        if(saveTimes.farAtkCount > 0)     {     farAtkC = saveTimes.farAtkCount;     saveTimes.farAtkCount = 0 ;          Invoke("FarAtk" , 3);     //Invoke("FarAtk" , 4);     Invoke("UICloseDamage" , 4.5f);          }       if(saveTimes.nearAtkCount > 0)     {     nearAtkC = saveTimes.nearAtkCount;     saveTimes.nearAtkCount = 0 ;          Invoke("NearAtk" , 3);     //Invoke("NearAtk" , 4);     Invoke("UICloseDamage" , 4.5f);          }                 }         void UICloseDamage()     {     UIDamage.SetActive(false);     }       public void FarAtk()     {     //Invoke("BossAtk",1);     if(farCanAtk && farChter.activeInHierarchy)     {          gunSFX.Play();     ani01.SetTrigger("atk01");     aniBoss.SetTrigger("bosshurt");      if(oneTimes)      {     UIDamage.SetActive(true);     textDamage.text = "-10" ;     bossHp.fillAmount -= 0.1f ;      }     if(threeTimes)     {     UIDamage.SetActive(true);     textDamage.text = "-13" ;     bossHp.fillAmount -= 0.13f ;      }     if(fiveTimes)      {     UIDamage.SetActive(true);     textDamage.text = "-15" ;     bossHp.fillAmount -= 0.15f ;      }     if(sevenTimes)     {     UIDamage.SetActive(true);     textDamage.text = "-20" ;     bossHp.fillAmount -= 0.2f ;      }                    }     }               public void NearAtk()     {     //Invoke("BossAtk",1.5f);     if(nearCanAtk && nearChter.activeInHierarchy)     {     saberSFX.Play();     ani02.SetTrigger("atk02");     aniBoss.SetTrigger("bosshurt");      if(oneTimes)      {     UIDamage.SetActive(true);     textDamage.text = "-10" ;     bossHp.fillAmount -= 0.1f ;      }     if(threeTimes)     {     UIDamage.SetActive(true);     textDamage.text = "-13" ;     bossHp.fillAmount -= 0.13f ;      }     if(fiveTimes)      {     UIDamage.SetActive(true);     textDamage.text = "-15" ;     bossHp.fillAmount -= 0.15f ;      }     if(sevenTimes)     {     UIDamage.SetActive(true);     textDamage.text = "-20" ;     bossHp.fillAmount -= 0.2f ;      }       //if(combo>=1) bossHp.fillAmount -= 0.1f ;      //if(combo>=3) bossHp.fillAmount -= 0.13f ;      //if(combo>=5) bossHp.fillAmount -= 0.15f ;      //if(combo>=7) bossHp.fillAmount -= 0.2f ;                }     }      public void JumpFarChter()     {     ani01.SetTrigger("jup01");          }      public void RunNearChter()     {     ani02.SetTrigger("run02");          }               public void Action()     {     moveStep = forwardC - backC;  //正值=往前     if(moveStep > 0)     {     farCanAtk = false ;     nearCanAtk = true ;     farChter.transform.position = nearPoint.transform.position ;     nearChter.transform.position = nearPoint.transform.position ;     moveStep = 0 ;     }     if(moveStep < 0)     {     farCanAtk = true ;     nearCanAtk = false ;     farChter.transform.position = farPoint.transform.position ;     nearChter.transform.position = farPoint.transform.position ;     moveStep = 0 ;     }      if(moveStep == 0)     {     //沒動作     }      combo =0 ;      farAtkC=0 ;      nearAtkC=0 ;      forwardC=0 ;      backC=0 ;       UIDamage.SetActive(false);               Invoke("BossAtk",2.3f);     }         void BossAtk()     {     UIDamagePlayer.SetActive(true);     gunSFX.Play();     aniBoss.SetTrigger("bossatk");     ani01.SetTrigger("hurt01");     ani02.SetTrigger("hurt02");     playerHp.fillAmount -= 0.08f ;      Invoke("UIDamagePlayerCL",0.8f);      }      void UIDamagePlayerCL()     {     UIDamagePlayer.SetActive(false);

    }   } 