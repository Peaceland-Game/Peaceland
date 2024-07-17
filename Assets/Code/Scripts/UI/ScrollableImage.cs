using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollableImage : MonoBehaviour
{
    [SerializeField] Image image;
    private Material mat;

    private Vector2 offset; 

    // Start is called before the first frame update
    void Start()
    {
        mat = image.materialForRendering;
        print(mat);

        if (mat == null)
            Destroy(this.gameObject);

        offset = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        offset += Vector2.one * Time.deltaTime;
        mat.SetVector("Offset", offset);
    }
}
