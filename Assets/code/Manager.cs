using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    


    public void FarStart()
    {

     SceneManager.LoadScene("01");

    }

    public void NearStart()
    {

     SceneManager.LoadScene("03");

    }


}
