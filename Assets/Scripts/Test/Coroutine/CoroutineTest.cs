using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoroutineTest : MonoBehaviour
{
    private IEnumerator enumerator;

    private void Start()
    {
        enumerator = MyCoroutine();

        StartCoroutine(enumerator);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.LogWarning("���¿ո��");
            enumerator.MoveNext();
        }
    }

    IEnumerator MyCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.LogWarning(i + "---" + Time.frameCount);
            yield return null;
        }
    }

    private void TestForeach()
    {
        TestEnumerable testEnumerable = new TestEnumerable()
        {
            intArray = new int[3]
            {
                1,
                2,
                3
            }
        };

        IEnumerator iterator = testEnumerable.GetEnumerator();
        Debug.LogWarning("����GetEnumerator��");

        while (iterator.MoveNext())
        {
            Debug.LogWarning("������ֵ " + iterator.Current);
        }
    }
}


public class TestEnumerable : IEnumerable
{
    public int[] intArray { get; set; }

    public IEnumerator GetEnumerator()
    {
        Debug.LogWarning("GetEnumerator�ڲ�");

        for (int i = 0; i < intArray.Length; i++)
        {
            Debug.LogWarning("ǰ-���������� i = " + i);

            yield return intArray[i];

            Debug.LogWarning("��-���������� i = " + i);
        }
    }
}

public class TestEnumerator : IEnumerator
{
    public int[] target { get; set; }

    private int index = -1;

    public object Current
    {
        get
        {
            return target[index];
        }
    }

    public bool MoveNext()
    {
        index++;
        return index <= target.Length - 1;
    }

    public void Reset()
    {

    }
}