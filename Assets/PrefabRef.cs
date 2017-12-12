using UnityEngine;
using System.Collections;

public class PrefabRef : MonoBehaviour
{

    public static System.Func<string, GameObject> objectInstantiateFunc;

    public string resourcePath;
    public bool isPrefabDisabled;

    void Awake()
    {
        if (objectInstantiateFunc != null && !string.IsNullOrEmpty(resourcePath))
        {
            var holderTr = transform;

            var o = objectInstantiateFunc(resourcePath);
            var tr = o.transform;

            tr.SetParent(holderTr.parent);
            tr.SetSiblingIndex(holderTr.GetSiblingIndex());
            tr.localPosition = holderTr.localPosition;
            tr.localScale = holderTr.localScale;
            tr.localRotation = holderTr.localRotation;
            
            o.name = this.gameObject.name;

            if (isPrefabDisabled)
            {
                o.SetActive(false);
            }

            Destroy(this.gameObject);

        }
    }



}
