using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Web3Unity;
using ZXing;
using ZXing.QrCode;

public class Web3Modal : MonoBehaviour
{
    //public RawImage image;



    void Start()
    {

        Web3Connect.Instance.OnConnected += Instance_Connected;

    }

  

  
    public void BtnMetamask_clicked()
    {
        Web3Connect.Instance.ConnectMetamask(true);
    }

    private void Instance_Connected(object sender, string e)
    {

        Debug.Log("connected account " + e);
        Web3Connect.Instance.OnConnected -= Instance_Connected;
        SceneManager.UnloadSceneAsync("Web3Modal");
    }

    void Update()
    {

      
    }

   
}
