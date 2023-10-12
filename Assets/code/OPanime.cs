using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class OPanime : MonoBehaviour
{

    public GameObject Logo;
    public GameObject ButtomUI;
    public TextMeshProUGUI ButtomTest;
   


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LogoAnime());
        StartCoroutine(ColorAlpha());

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void MoveLogo()
    {
        
    
    
    }

    private IEnumerator LogoAnime()
    {
        for (int i = 1; i < 24; i++)
        {

            Logo.transform.position -= new Vector3(0, 0.35f , 0);
            yield return new WaitForSeconds(0.3f);

        }
        yield return new WaitForSeconds(1);
        ButtomUI.SetActive(true);

        

    }


   

    private IEnumerator ColorAlpha()
    {
        for (int i = 0; i < Mathf.Infinity; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                ButtomTest.color -= new Color(0, 0, 0, 0.05f);
                yield return new WaitForSeconds(0.02f);
                //print(testStart.color.ToString());

            }

            for (int k = 0; k < 20; k++)
            {
                ButtomTest.color += new Color(0, 0, 0, 0.05f);
                yield return new WaitForSeconds(0.02f);
                //print(testStart.color.ToString());
            }
        }




    }




}
