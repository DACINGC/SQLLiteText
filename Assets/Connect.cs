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

        // 插入手动创建的 JSON 文件中的用户数据
        string jsonFilePath = Path.Combine(Application.dataPath, "Users.json");
        dbManager.InsertUserFromJson(jsonFilePath);
        //InsertRandomUsers(6);
        //dbManager.GetAllUsers();
        //Debug.Log("数据库已成功创建！");
    }

    // 随机插入用户数据
    public void InsertRandomUsers(int count)
    {
        System.Random random = new System.Random();
        string[] sampleUsernames = { "JohnDoe", "JaneSmith", "AliceW", "BobJohnson", "CharlieBrown" };

        for (int i = 0; i < count; i++)
        {
            string username = sampleUsernames[random.Next(sampleUsernames.Length)] + random.Next(1000, 9999);
            string password = "Password" + random.Next(1000, 9999);

            dbManager.RegisterUser(username, password);
            Debug.Log($"插入用户: {username}, 密码: {password}");
        }
    }
}
