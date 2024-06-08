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

        // ���� Swap ����
        numbers.Swap(0, 3);
        Debug.Log("After Swap(0, 3): " + string.Join(", ", numbers)); // ���: 1, 3, 8, 5, 9

        // ���� IsValidIndex ����
        Debug.Log("IsValidIndex(2): " + numbers.IsValidIndex(2)); // ���: True
        Debug.Log("IsValidIndex(5): " + numbers.IsValidIndex(5)); // ���: False

        // ���� FindBy ����
        int found = numbers.FindBy(n => n > 5);
        Debug.Log("FindBy(n > 5): " + found); // ���: 8

        // ���� FindAllBy ����
        List<int> foundAll = numbers.FindAllBy(n => n > 5);
        Debug.Log("FindAllBy(n > 5): " + string.Join(", ", foundAll)); // ���: 8, 9

        // ���� OrderBy ����
        numbers.OrderBy(n => n);
        Debug.Log("After OrderBy: " + string.Join(", ", numbers)); // ���: 1, 3, 5, 8, 9

        // ���� OrderByDescending ����
        numbers.OrderByDescending(n => n);
        Debug.Log("After OrderByDescending: " + string.Join(", ", numbers)); // ���: 9, 8, 5, 3, 1

        // ���� MaxBy ����
        int max = numbers.MaxBy(n => n);
        Debug.Log("MaxBy: " + max); // ���: 9

        // ���� MinBy ����
        int min = numbers.MinBy(n => n);
        Debug.Log("MinBy: " + min); // ���: 1

        // ����һ���������б�
        List<Person> people = new List<Person>
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 25 },
            new Person { Name = "Charlie", Age = 35 }
        };

        // ���� MaxBy �� MinBy ����
        Person oldest = people.MaxBy(p => p.Age);
        Person youngest = people.MinBy(p => p.Age);
        Debug.Log($"Oldest: {oldest.Name}, Age: {oldest.Age}"); // ���: Charlie, Age: 35
        Debug.Log($"Youngest: {youngest.Name}, Age: {youngest.Age}"); // ���: Bob, Age: 25

        // ���� OrderBy �� OrderByDescending ����
        people.OrderBy(p => p.Age);
        Debug.Log("After OrderBy Age: " + string.Join(", ", people.ConvertAll(p => p.Name))); // ���: Bob, Alice, Charlie

        people.OrderByDescending(p => p.Age);
        Debug.Log("After OrderByDescending Age: " + string.Join(", ", people.ConvertAll(p => p.Name))); // ���: Charlie, Alice, Bob
    }


    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
