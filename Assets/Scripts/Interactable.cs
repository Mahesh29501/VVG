using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading.Tasks;
public class Interactable : MonoBehaviour
{
    public GameObject nftPanel;
    public GameObject nftPanelImg;
    public Image YourRawImage;
    public float timeSwitchedOnTotal = 0f;
    public UnityEvent onInteract;
    public Text title;
    public Text price;
    public Text desc;
    public bool timerOn = false;
    public string titlenft;
    public string pricenft;
    public string descnft;
    RawImage img;
    public Sprite nftImg;
    public int ID;
    public Interactor plaInt;
    public Text qtxt;
    public GameObject qtxtgo;
    public string imgUrl;
    public Texture imgTex;
    public Material imgMat;
    public float texRatio =0.0f;
    public float imgWidth, imgHeight;
    public UnityEngine.Networking.UnityWebRequest.Result result;
    // Start is called before the first frame update
    void Start()
    {
        plaInt = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Interactor>();
        ID = Random.Range(0, 999999);
        StartCoroutine(DownloadImage(imgUrl));
        //CreateMaterials(imgTex);


    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            timeSwitchedOnTotal += Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1) && nftPanel.activeSelf)
        {

            //timerOn = false;
            nftPanel.SetActive(false);
            plaInt.SetToNormal();
           
        }
        if (Input.GetMouseButtonDown(1) && nftPanel.activeSelf)
        {
            gameObject.GetComponent<AnimationHandler>().ToggleBool("Open");
        }
        if (Input.GetKeyDown(KeyCode.Space) && nftPanel.activeSelf)
        {
            nftPanel.SetActive(false);
        }
        
        if (!nftPanel.activeSelf)
        {
           // timerOn = false;
        }
    }
    public void showText()
    {
        qtxt = qtxtgo.GetComponent<Text>();
        qtxt.text = "Press Q to Exit";
        qtxt.fontSize = 35;
        //qtxtgo
    }
    public void viewPanel()
    {
        
        //timerOn = true;
        nftPanel.SetActive(true);
        title.text = titlenft;
        price.text = pricenft;
        desc.text = descnft;

        //img.GetComponent<RawImage>().texture = imgTex;

        img = nftPanelImg.GetComponent<RawImage>();
        imgWidth = img.GetComponent<RectTransform>().rect.width;
        imgHeight = img.GetComponent<RectTransform>().rect.height;
        Debug.Log("Img Width " + imgWidth);
        Debug.Log("Img Height " + imgHeight);
        Debug.Log("TexRatio " + texRatio);

        if (texRatio >= 1) //portrait
        {
            img.rectTransform.sizeDelta = new Vector2(250, texRatio * 250);
        }
        else //landscape
        {
            img.rectTransform.sizeDelta = new Vector2(350, texRatio * 350);
        }

        //img.GetComponent<Image>().material = imgTex;
        img.GetComponent<RawImage>().texture = imgTex;

        plaInt.SetToZero();
        //img.sprite = nftImg;
        //img.SetNativeSize();
    }
    public void disablePanel()
    {
        gameObject.SetActive(false);
    }
    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Debug.Log("Great Success");

            //YourRawImage.material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            //gameObject.GetComponent<Material>().mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            //gameObject.material.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Qw();
           // CreateMaterials(((DownloadHandlerTexture)request.downloadHandler).texture);
            //Texture tex = await GetRemoteTexture(imgUrl);
            
        }

    }

    private async Task Qw()
    {
        CreateMaterials(await GetRemoteTexture(imgUrl));
    }

    public void CreateMaterials(Texture t)
    {

            Debug.Log("Creating material from: " + t);
        Debug.Log("Width " + t.width);
        Debug.Log("Height " + t.height);
        imgTex = t;
       
        //gameObject.GetComponent<RectTransform>().rect.size = imgWidth;
        // gameObject.rectTransform.sizeDelta = new Vector2(250, texRatio * 250);

        texRatio = (float)(t.height) / (float)(t.width);
        Debug.Log("Tex ratio " + texRatio);

        gameObject.GetComponent<MeshRenderer>().material.mainTexture = imgTex;

        if (texRatio >= 1) //portrait
        {
            gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f*texRatio);
        }
        else //landscape
        {
            gameObject.transform.localScale = new Vector3(0.5f, 0.25f, 0.5f*texRatio);
        }
        

       // Texture selected = t as Texture;

            //Material material = new Material(Shader.Find("Standard"));
            //material.mainTexture = (Texture)t;
       // material.SetTexture("", imgTex);


       // string savePath = AssetDatabase.GetAssetPath(selected);
            //savePath = savePath.Substring(0, savePath.LastIndexOf('/') + 1);

       /// string newAssetName = savePath + selected.name + ".mat";

          //  AssetDatabase.CreateAsset(material, newAssetName);

          //  AssetDatabase.SaveAssets();

        //imgMat = material;
        //gameObject.GetComponent<MeshRenderer>().material.mainTexture = imgTex;

    }
    IEnumerator setImage(string url)
    {
        //Texture2D texture = profileImage.canvasRenderer.GetMaterial().mainTexture as Texture2D;

        WWW www = new WWW(url);
        yield return www;

        Debug.Log("Why on earh is this never called?");

        //www.LoadImageIntoTexture(texture);
        www.Dispose();
        www = null;
    }

    public async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz
            Debug.Log("in getRT");
            // read results:
            if (www.isNetworkError || www.isHttpError)
            // if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
            {
                // log error:
#if DEBUG
                Debug.Log($"{www.error}, URL:{www.url}");
#endif

                // nothing to return on error:
                return null;
                Debug.Log("returning null");
            }
            else
            {
                // return valid results:
                return DownloadHandlerTexture.GetContent(www);
                Debug.Log("returning downloaded");
            }
        }
    }
}
