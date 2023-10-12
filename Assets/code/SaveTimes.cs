using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro ;
using UnityEngine.UI;

public class SaveTimes : MonoBehaviour
{

    public int combo ;
    public int forwardCount ;
    public int backCount ;
    public int nearAtkCount ;
    public int farAtkCount ;
    public int heathCount ;
    public int defendCount ;
    public GameObject UIOpenUse ;

    public GameObject BallOpenUse ;
    public GameObject ButtomGo ;


    public TextMeshProUGUI textCombo;
    public TextMeshProUGUI textForward;
    public TextMeshProUGUI textBack;
    public TextMeshProUGUI textFar;
    public TextMeshProUGUI textNear;
    public TextMeshProUGUI textGoORBack;
    public TextMeshProUGUI textTimes;
    

    public Image UIclose ;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TextCount.transform.Find("COMBO數").GetComponent<TestMeshProUGUI>().test = "5566";
        textCombo.text = "COMBO：" + combo.ToString() ;
        textForward.text = "前進：" + forwardCount.ToString() ;
        textBack.text = "後退：" + backCount.ToString() ;
        textFar.text = "遠射：" + farAtkCount.ToString() ;
        textNear.text = "近戰：" + nearAtkCount.ToString() ;

        if(forwardCount - backCount >=1 )
        {
        textGoORBack.text = "前進!!"  ;
        }

        if(forwardCount - backCount <0 )
        {
        textGoORBack.text = "後退!!"  ;
        }
        if(forwardCount - backCount ==0 )
        {
        textGoORBack.text = "停止移動!"  ;
        }

        if(combo==0) textTimes.text = "1.0x";
        if(combo>=3)
        {
        textTimes.text = "1.3x";
        if(combo>=5)
        {
        textTimes.text = "1.5x";
        if(combo>=7)
        {
        textTimes.text = "2.0x";
        }
        }

        }

        if(combo >=1)
        {
        UIOpenUse.SetActive(true);
        //UIclose.fillAmount += 0.01f;
        Invoke("CloseBall" , 7.5f);
        }
        if(combo ==0)
        {
        Invoke("CloseUI" , 4);
        //Invoke("ImageOP" , 0.1f);
        
        //UIOpenUse.SetActive(false);
        //BallOpenUse.SetActive(true);
        }
        
    }


    void CloseUI()
    {
    ButtomGo.SetActive(false);
    UIOpenUse.SetActive(false);
    BallOpenUse.SetActive(true);
    UIclose.fillAmount -= 0.01f;
    }
    void CloseBall()
    {
    ButtomGo.SetActive(true);
    UIclose.fillAmount += 0.01f;
    BallOpenUse.SetActive(false);
    }

    void ImageOP()
    {
    UIclose.fillAmount -= 0.05f;

    }


}
