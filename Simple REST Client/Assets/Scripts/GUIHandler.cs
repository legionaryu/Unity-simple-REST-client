using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GUIHandler : MonoBehaviour {
    public GameObject baseKeyValueObj;
    public GameObject paramsScrollViewContent;

    private static List<GameObject> allKeyValueObjs = new List<GameObject>();

    public void Start()
    {
        foreach(Transform child in paramsScrollViewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        var keyValueObj = GameObject.Instantiate<GameObject>(baseKeyValueObj);
        keyValueObj.transform.parent = paramsScrollViewContent.transform;
        allKeyValueObjs.Add(keyValueObj);
        keyValueObj.transform.localPosition = baseKeyValueObj.transform.position;
        var allInputFields = keyValueObj.GetComponentsInChildren<InputField>();
        foreach(var field in allInputFields)
        {
            if(field.name.ToLower().Contains("key"))
            {
                var fieldKey = field;
                field.onEndEdit.AddListener(delegate { OnEndEditKey(fieldKey); });
            }
            else if (field.name.ToLower().Contains("value"))
            {
                var fieldValue = field;
                field.onEndEdit.AddListener(delegate { OnEndEditValue(fieldValue); });
            }
        }
    }

    public void OnEndEditKey(InputField field)
    {
        Debug.Log("OnEndEditKey", field);
    }

    public void OnEndEditValue(InputField field)
    {
        Debug.Log("OnEndEditValue", field);
    }
}
