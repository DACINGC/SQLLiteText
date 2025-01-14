using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Connect : MonoBehaviour
{
    SQLiteManager dbManager;

    void Start()
    {
        dbManager = new SQLiteManager("MyData.db");

        // �����ֶ������� JSON �ļ��е��û�����
        string jsonFilePath = Path.Combine(Application.dataPath, "Users.json");
        dbManager.InsertUserFromJson(jsonFilePath);
        //InsertRandomUsers(6);
        //dbManager.GetAllUsers();
        //Debug.Log("���ݿ��ѳɹ�������");
    }

    // ��������û�����
    public void InsertRandomUsers(int count)
    {
        System.Random random = new System.Random();
        string[] sampleUsernames = { "JohnDoe", "JaneSmith", "AliceW", "BobJohnson", "CharlieBrown" };

        for (int i = 0; i < count; i++)
        {
            string username = sampleUsernames[random.Next(sampleUsernames.Length)] + random.Next(1000, 9999);
            string password = "Password" + random.Next(1000, 9999);

            dbManager.RegisterUser(username, password);
            Debug.Log($"�����û�: {username}, ����: {password}");
        }
    }
}
