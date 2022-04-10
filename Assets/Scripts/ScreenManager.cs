using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject newsFeedScreen;
    public GameObject newsDetailScreen;
    public NewsDetailView newsDetailView;

    // Start is called before the first frame update
    void Start()
    {
        GoToNewsFeedScreen();
    }

    public void GoToNewsFeedScreen()
    {
        newsDetailScreen.SetActive(false);
        newsFeedScreen.SetActive(true);
    }

    public void GoToNewsDetailScreen(string headline, string websiteName, string date,string content, Texture imgTexture)
    {
        newsDetailView.newsTitleText.text = headline;
        newsDetailView.newsWebsiteNameText.text = websiteName;
        newsDetailView.newsDateText.text = date;
        newsDetailView.newsContentText.text = content;
        newsDetailView.newsContentImage.texture = imgTexture;


        newsDetailScreen.SetActive(true);
        newsFeedScreen.SetActive(false);
    }

}
