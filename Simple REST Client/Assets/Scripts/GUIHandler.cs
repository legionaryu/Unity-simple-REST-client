using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIHandler : MonoBehaviour {
    public GameObject baseKeyValueObj;
    public GameObject baseLblOutput;
    public InputField urlInput;
    public Dropdown methodInput;
    public RectTransform paramsScrollViewContent;
    public RectTransform resultScrollViewContent;

    private const float keyValueObjMargin = 5;
    private const float lblOutputMargin = 5;
    private const int maxCharsPerText = 1600;

    public void Send()
    {
        StartCoroutine(SendCoroutine());
    }
    
    private IEnumerator SendCoroutine()
    {
        System.UriBuilder uriBuilder = null;
        var form = new WWWForm();
        if (!urlInput.text.Contains("://"))
        {
            urlInput.text = "http://" + urlInput.text;
        }
        try
        {
            uriBuilder = new System.UriBuilder(urlInput.text);
        }
        catch(System.Exception e)
        {
            AddOutput(e.ToString());
        }
        if(uriBuilder != null)
        {
            if (methodInput.value == 0) //GET
            {
                foreach (Transform keyValueGroup in paramsScrollViewContent)
                {
                    if (keyValueGroup.name.ToLower().Contains("keyvaluegroup"))
                    {
                        string key = "", value = "";
                        foreach(Transform item in keyValueGroup)
                        {
                            if (item.name.ToLower().Contains("key"))
                            {
                                key = WWW.EscapeURL(item.GetComponent<InputField>().text);
                            }
                            else if (item.name.ToLower().Contains("value"))
                            {
                                value = WWW.EscapeURL(item.GetComponent<InputField>().text);
                            }
                        }
                        string queryToAppend = string.IsNullOrEmpty(key) ? "" : (string.IsNullOrEmpty(value) ? key : string.Format("{0}={1}", key, value));
                        if (!string.IsNullOrEmpty(queryToAppend))
                        {
                            if (uriBuilder.Query != null && uriBuilder.Query.Length > 1)
                                uriBuilder.Query = uriBuilder.Query.Substring(1) + "&" + queryToAppend;
                            else
                                uriBuilder.Query = queryToAppend;
                        }

                    }
                }
            }
            else
            {
                foreach (Transform keyValueGroup in paramsScrollViewContent)
                {
                    if (keyValueGroup.name.ToLower().Contains("keyvaluegroup"))
                    {
                        string key = "", value = "";
                        foreach (Transform item in keyValueGroup)
                        {
                            if (item.name.ToLower().Contains("key"))
                            {
                                key = WWW.EscapeURL(item.GetComponent<InputField>().text);
                            }
                            else if (item.name.ToLower().Contains("value"))
                            {
                                value = WWW.EscapeURL(item.GetComponent<InputField>().text);
                            }
                        }
                        if(!string.IsNullOrEmpty(key))
                        {
                            form.AddField(key, value);
                        }
                    }
                }

            }
            Debug.Log(uriBuilder.ToString());
            WWW request = methodInput.value == 0 ? new WWW(uriBuilder.ToString()) : new WWW(uriBuilder.ToString(), form);
            //while(!request.isDone)
            //{
            //    yield return new WaitForSeconds(1);
            //    Debug.Log("progress: " +request.progress);
            //}
            //Debug.Log("request.error: " + request.error + " | text: " + request.text + " | size: " + request.size);
            yield return request;
            if (string.IsNullOrEmpty(request.error))
            {
                AddOutput(request.text);
            }
            else
            {
                AddOutput(request.error);
            }
        }
    }

    public void AddPair()
    {
        var keyValueObj = GameObject.Instantiate<GameObject>(baseKeyValueObj);
        keyValueObj.transform.SetParent(paramsScrollViewContent.transform, false);
        foreach(Transform child in keyValueObj.transform)
        {
            if (child.name.ToLower().Contains("remove"))
            {
                child.GetComponent<Button>().onClick.AddListener(delegate { RemovePair(keyValueObj); });
            }
        }
    }

    private void AddOutput(string text)
    {
        Debug.Log("text: " + text);
        for (var i = 0; i < Mathf.Max(text.Length / maxCharsPerText, 1); i++)
        {
            var LblOutputObj = GameObject.Instantiate<GameObject>(baseLblOutput);
            LblOutputObj.GetComponent<Text>().text = text.Substring(i * maxCharsPerText, Mathf.Min((i+1) * maxCharsPerText, text.Length));
            LblOutputObj.transform.SetParent(resultScrollViewContent.transform, false);
        }
    }

    public void RemovePair(GameObject pair)
    {
        GameObject.Destroy(pair);
    }

    public void ClearOutput()
    {
        foreach(Transform child in resultScrollViewContent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

}
