using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileIOTester : MonoBehaviour
{
    void Start()
    {
        DataStoreLoad dsl = new DataStoreLoad();
        dsl.StoreSample("Package", "", 5, 0, 1);
    }
}
