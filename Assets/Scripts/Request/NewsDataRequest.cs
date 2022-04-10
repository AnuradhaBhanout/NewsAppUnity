using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NewsDataRequest : MonoBehaviour
{
    public string url = "https://newsapi.org/v2/top-headlines?language=en&apiKey=f04433086b174a4ba7b3db3254c70edc";
    public string jsonDataPlayerPrefKey = "jsonData";


    [Header("NewsDetails")]
    public NewsItem newsItemTemplate;
    public NewsContentData newsContentData;
    public ScreenManager screenManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetCurrentNewsData(url));
    }


    IEnumerator GetCurrentNewsData(string urlObj)
    {
        
        using (UnityWebRequest request = UnityWebRequest.Get(urlObj))
        {
            if (!NetworkChecker.CheckInternetConnection() && PlayerPrefs.HasKey(jsonDataPlayerPrefKey))
            {
                PutDataIntoFields(PlayerPrefs.GetString(jsonDataPlayerPrefKey));
                yield break;
            }
            else
            {
                Debug.Log("Online Application.");
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("error");
                
            }

            else
            {
                PutDataIntoFields(request.downloadHandler.text);
            }

        }

    }


    void PutDataIntoFields(string dataToRetrieve)
    {
        PlayerPrefs.SetString(jsonDataPlayerPrefKey, dataToRetrieve);
        newsContentData = JsonUtility.FromJson<NewsContentData>(dataToRetrieve);

        for (int i = 0; i < newsContentData.articles.Count-1; i++)
        {
            NewsItem newObjItem = Instantiate(newsItemTemplate, this.transform) as NewsItem;
            string imageUrlNews = newsContentData.articles[i].urlToImage;
            newObjItem.newsTitleText.text = newsContentData.articles[i].title.ToString();
            string date = DateTime.UtcNow.ToString(newsContentData.articles[i].publishedAt.ToString());
            date = DateTime.Parse(date).ToShortDateString().ToString();
            newObjItem.newsDate = date;
            newObjItem.newsWebsiteText.text = newsContentData.articles[i].source.name.ToString() + "  "+date;
            newObjItem.newsSource = newsContentData.articles[i].source.name;
            newObjItem.newsContent = newsContentData.articles[i].content;


            newObjItem.GetComponent<Button>().onClick.AddListener(()=> 
            { screenManager.GoToNewsDetailScreen(newObjItem.newsTitleText.text,
                newObjItem.newsSource, newObjItem.newsDate, newObjItem.newsContent, newObjItem.newsContentImage.texture );
            });

            if (!NetworkChecker.CheckInternetConnection())
            {
                LoadImageFromDisk(newsContentData.articles[i].urlToImage, newObjItem.newsContentImage);
                
            }
            else
            StartCoroutine(LoadImage(imageUrlNews,
                (urlImage, textItem) =>
                {
                    if (urlImage.Equals(imageUrlNews))
                    {
                        newObjItem.newsContentImage.texture = textItem;
                    }
                    else
                    {
                        Debug.Log("error while loading the image " + urlImage + "\n  " + imageUrlNews);
                    }
                }
                ));
        }
        
       
    }



  IEnumerator LoadImage(string imageUrl, Action<string, Texture2D> onSuccess)
    {
        UnityWebRequest imgRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        
        yield return imgRequest.SendWebRequest();

        if (imgRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("error");
        }

        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(imgRequest);
            onSuccess?.Invoke(imageUrl, texture);
            StoreImage(imageUrl, texture.EncodeToJPG());
        }

    }


    private void LoadImageFromDisk(string imgName, RawImage rawImg)
    {
        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(Path.Combine(Application.persistentDataPath, GetImageName(imgName))))
        {
            fileData = File.ReadAllBytes(Path.Combine(Application.persistentDataPath, GetImageName(imgName)));
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            rawImg.texture = tex;
        }

    }

    private void StoreImage(string imgName,byte[] bytes)
    {

        if (!File.Exists(Path.Combine(Application.persistentDataPath, GetImageName(imgName))))
        {
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, GetImageName(imgName)), bytes);

        }

    }


    private string GetImageName(string url)
    {
        return url.GetHashCode().ToString()+".jpeg";
    }





}

