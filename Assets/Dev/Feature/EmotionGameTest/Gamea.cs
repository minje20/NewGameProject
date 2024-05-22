using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gamea : MonoBehaviour
{
    public TMP_InputField Question;
    public TMP_InputField Answer;

    public TMP_Text Result;


    public static readonly List<(string, List<string>)> GreatTable = new List<(string, List<string>)>()
    {
        ("분노", new() { "안정" }),
        ("슬픔", new() { "위로" }),
        ("불안", new() { "안정" }),
        ("아픔", new() { "따뜻함" }),
        ("기쁨", new() { "기분좋음" }),
        ("사랑", new() { "설렘" }),
        ("소망(바람)", new() { "용기" }),
    };
    
    public static readonly List<(string, List<string>)> GoodTable = new List<(string, List<string>)>()
    {
        ("분노", new() { }),
        ("슬픔", new() { "안정", "따뜻함" }),
        ("불안", new() { "위로", "용기" }),
        ("아픔", new() { "위로", "안정" }),
        ("기쁨", new() { "설렘", "따뜻함" }),
        ("사랑", new() { "안정", "따뜻함", "기분좋음" }),
        ("소망(바람)", new() { "안정", "따뜻함", "설렘" }),
    };
    
    public static readonly List<(string, List<string>)> BadTable = new List<(string, List<string>)>()
    {
        ("분노", new() { "용기" }),
        ("슬픔", new() { "설렘" }),
        ("불안", new() { "기분좋음" }),
        ("아픔", new() { "설렘" }),
        ("기쁨", new() { "" }),
        ("사랑", new() { "" }),
        ("소망(바람)", new() { "안정" }),
    };

    public void Calculate()
    {
        var question = Question.text;
        var answer = Answer.text;

        if (Find(GreatTable, question, answer))
        {
            Result.text = "Great";
        }
        else if (Find(GoodTable, question, answer))
        {
            Result.text = "Good";
        }
        else if (Find(BadTable, question, answer))
        {
            Result.text = "Bad";
        }
        else
        {
            Result.text = "Nothing";
        }
    }

    private bool Find(List<(string, List<string>)> table, string question, string answer)
    {
        var tuple = table.Find(x => x.Item1 == question);

        if (tuple.Item2 == null)
        {
            return false;
        }

        var result  = tuple.Item2.Find(x => x == answer);

        if (result == null)
        {
            return false;
        }

        return true;
    }

    public void QuestionReset()
    {
        var str = GreatTable[Random.Range(0, GreatTable.Count - 1)].Item1;
        Question.text = str;
        Answer.text = "";
    }
}
