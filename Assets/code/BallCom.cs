using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCom : MonoBehaviour
{

    public int combo ;
    public int forwardCount ;
    public int backCount ;
    public int nearAtkCount ;
    public int farAtkCount ;
    public int heathCount ;
    public int defendCount ;
    public GameObject filling;
    public bool isFillingActive ;
    public SaveTimes saveTimes ;


    // Start is called before the first frame update
    void Start()
    {
        saveTimes = GameObject.Find("SaveTimes").GetComponent<SaveTimes>();
    }

    // Update is called once per frame
    void Update()
    {

    if(combo > 0)
    {
    combo = 0 ;
 
    saveTimes.combo++;
    }
    
    if(forwardCount > 0)
    {
    forwardCount = 0 ;
 
    saveTimes.forwardCount++;
    }
    
    if(backCount > 0)
    {
    backCount = 0 ;
   
    saveTimes.backCount++;
    }



    if(farAtkCount > 0)
    {
    farAtkCount = 0 ;
 
    saveTimes.farAtkCount++;
    }

    if(nearAtkCount > 0)
    {
    nearAtkCount = 0 ;
  
    saveTimes.nearAtkCount++;
    }

   
    }


}
