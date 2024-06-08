using System.Collections.Generic;
using UnityEngine;

public class CollectTest : MonoBehaviour
{
    void Start()
    {
        TestListExtensions();
    }

    void TestListExtensions()
    {
        List<int> numbers = new List<int> { 5, 3, 8, 1, 9 };

        // 测试 Swap 方法
        numbers.Swap(0, 3);
        Debug.Log("After Swap(0, 3): " + string.Join(", ", numbers)); // 输出: 1, 3, 8, 5, 9

        // 测试 IsValidIndex 方法
        Debug.Log("IsValidIndex(2): " + numbers.IsValidIndex(2)); // 输出: True
        Debug.Log("IsValidIndex(5): " + numbers.IsValidIndex(5)); // 输出: False

        // 测试 FindBy 方法
        int found = numbers.FindBy(n => n > 5);
        Debug.Log("FindBy(n > 5): " + found); // 输出: 8

        // 测试 FindAllBy 方法
        List<int> foundAll = numbers.FindAllBy(n => n > 5);
        Debug.Log("FindAllBy(n > 5): " + string.Join(", ", foundAll)); // 输出: 8, 9

        // 测试 OrderBy 方法
        numbers.OrderBy(n => n);
        Debug.Log("After OrderBy: " + string.Join(", ", numbers)); // 输出: 1, 3, 5, 8, 9

        // 测试 OrderByDescending 方法
        numbers.OrderByDescending(n => n);
        Debug.Log("After OrderByDescending: " + string.Join(", ", numbers)); // 输出: 9, 8, 5, 3, 1

        // 测试 MaxBy 方法
        int max = numbers.MaxBy(n => n);
        Debug.Log("MaxBy: " + max); // 输出: 9

        // 测试 MinBy 方法
        int min = numbers.MinBy(n => n);
        Debug.Log("MinBy: " + min); // 输出: 1

        // 创建一个测试类列表
        List<Person> people = new List<Person>
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 },
            new Person { Name = "Charlie", Age = 35 }
        };

        // 测试 MaxBy 和 MinBy 方法
        Person oldest = people.MaxBy(p => p.Age);
        Person youngest = people.MinBy(p => p.Age);
        Debug.Log($"Oldest: {oldest.Name}, Age: {oldest.Age}"); // 输出: Charlie, Age: 35
        Debug.Log($"Youngest: {youngest.Name}, Age: {youngest.Age}"); // 输出: Bob, Age: 25

        // 测试 OrderBy 和 OrderByDescending 方法
        people.OrderBy(p => p.Age);
        Debug.Log("After OrderBy Age: " + string.Join(", ", people.ConvertAll(p => p.Name))); // 输出: Bob, Alice, Charlie

        people.OrderByDescending(p => p.Age);
        Debug.Log("After OrderByDescending Age: " + string.Join(", ", people.ConvertAll(p => p.Name))); // 输出: Charlie, Alice, Bob
    }


    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
