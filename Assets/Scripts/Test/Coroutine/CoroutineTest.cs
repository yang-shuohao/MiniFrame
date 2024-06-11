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
            Debug.LogWarning("按下空格键");
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
        Debug.LogWarning("调用GetEnumerator后");

        while (iterator.MoveNext())
        {
            Debug.LogWarning("遍历的值 " + iterator.Current);
        }
    }
}


public class TestEnumerable : IEnumerable
{
    public int[] intArray { get; set; }

    public IEnumerator GetEnumerator()
    {
        Debug.LogWarning("GetEnumerator内部");

        for (int i = 0; i < intArray.Length; i++)
        {
            Debug.LogWarning("前-遍历的索引 i = " + i);

            yield return intArray[i];

            Debug.LogWarning("后-遍历的索引 i = " + i);
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