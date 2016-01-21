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
        WWW request = new WWW(urlInput.text);
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

    public void AddPair()
    {
        var keyValueObj = GameObject.Instantiate<GameObject>(baseKeyValueObj);
        keyValueObj.transform.SetParent(paramsScrollViewContent.transform, false);
    }

    private void AddOutput(string text)
    {
        for(var i = 0; i < text.Length / maxCharsPerText; i++)
        {
            var LblOutputObj = GameObject.Instantiate<GameObject>(baseLblOutput);
            LblOutputObj.GetComponent<Text>().text = text.Substring(i * maxCharsPerText, Mathf.Min((i+1) * maxCharsPerText, text.Length));
            LblOutputObj.transform.SetParent(resultScrollViewContent.transform, false);
        }
    }

}
