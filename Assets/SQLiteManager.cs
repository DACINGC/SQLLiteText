using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using Newtonsoft.Json;

// 定义用户类
public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class SQLiteManager
{
    private string connectionString;

    public SQLiteManager(string databaseName)
    {
        // 确定数据库路径（在与 Asset 同级目录下的 DataBase 文件夹中）
        string databaseFolderPath = Path.Combine(Application.dataPath, "../DataBase");
        string databaseFilePath = Path.Combine(databaseFolderPath, databaseName);

        // 如果文件夹不存在，则创建
        if (!Directory.Exists(databaseFolderPath))
        {
            Directory.CreateDirectory(databaseFolderPath);
        }

        // 设置连接字符串
        connectionString = $"URI=file:{databaseFilePath}";

        // 初始化数据库（如果不存在则创建）
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        // 创建表（如果不存在）
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"CREATE TABLE IF NOT EXISTS Users (
                                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        Username TEXT NOT NULL UNIQUE,
                                        Password TEXT NOT NULL
                                    );";
                command.ExecuteNonQuery();
            }
        }
    }

    public bool RegisterUser(string username, string password)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Users (Username, Password) VALUES (@username, @password);";
                    command.Parameters.Add(new SqliteParameter("@username", username));
                    command.Parameters.Add(new SqliteParameter("@password", password));
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("注册用户时出错: " + ex.Message);
            return false;
        }
    }

    public bool LoginUser(string username, string password)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password;";
                    command.Parameters.Add(new SqliteParameter("@username", username));
                    command.Parameters.Add(new SqliteParameter("@password", password));

                    var result = command.ExecuteScalar();
                    return Convert.ToInt32(result) > 0;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("登录用户时出错: " + ex.Message);
            return false;
        }
    }

    public void ResetDatabase()
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DROP TABLE IF EXISTS Users;";
                    command.ExecuteNonQuery();
                }
            }

            // 重新初始化数据库
            InitializeDatabase();
        }
        catch (Exception ex)
        {
            Debug.LogError("重置数据库时出错: " + ex.Message);
        }
    }

    public void InsertUserFromJson(string filePath)
    {
        // 读取 JSON 文件
        string jsonData = File.ReadAllText(filePath);

        // 反序列化为 User 对象列表
        List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonData);

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            foreach (var user in users)
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Users (Username, Password) VALUES (@username, @password);";
                    command.Parameters.Add(new SqliteParameter("@username", user.Username));
                    command.Parameters.Add(new SqliteParameter("@password", user.Password));
                    command.ExecuteNonQuery();
                }
            }
        }
    }

    // 查询所有用户的方法
    public void GetAllUsers()
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Username, Password FROM Users;";
                    using (var reader = command.ExecuteReader())
                    {
                        // 遍历查询结果并打印
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string username = reader.GetString(1);
                            string password = reader.GetString(2);
                            Debug.Log($"User ID: {id}, Username: {username}, Password: {password}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("查询所有用户时出错: " + ex.Message);
        }
    }
}
