// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System;
using System.Collections.Generic;


[System.Serializable]
public class Source
{
    public string id ;
    public string name ;
}

[System.Serializable]
public class ArticleData
{
    public Source source ;
    public string author ;
    public string title ;
    public string description ;
    public string url ;
    public string urlToImage ;
    public String publishedAt ;
    public string content ;
}

[System.Serializable]
public class NewsContentData
{
    public string status ;
    public int totalResults ;
    public List<ArticleData> articles ;
}

