using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;
using Newtonsoft.Json;

// �����û���
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
        // ȷ�����ݿ�·�������� Asset ͬ��Ŀ¼�µ� DataBase �ļ����У�
        string databaseFolderPath = Path.Combine(Application.dataPath, "../DataBase");
        string databaseFilePath = Path.Combine(databaseFolderPath, databaseName);

        // ����ļ��в����ڣ��򴴽�
        if (!Directory.Exists(databaseFolderPath))
        {
            Directory.CreateDirectory(databaseFolderPath);
        }

        // ���������ַ���
        connectionString = $"URI=file:{databaseFilePath}";

        // ��ʼ�����ݿ⣨����������򴴽���
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        // ��������������ڣ�
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
            Debug.LogError("ע���û�ʱ����: " + ex.Message);
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
            Debug.LogError("��¼�û�ʱ����: " + ex.Message);
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

            // ���³�ʼ�����ݿ�
            InitializeDatabase();
        }
        catch (Exception ex)
        {
            Debug.LogError("�������ݿ�ʱ����: " + ex.Message);
        }
    }

    public void InsertUserFromJson(string filePath)
    {
        // ��ȡ JSON �ļ�
        string jsonData = File.ReadAllText(filePath);

        // �����л�Ϊ User �����б�
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

    // ��ѯ�����û��ķ���
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
                        // ������ѯ�������ӡ
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
            Debug.LogError("��ѯ�����û�ʱ����: " + ex.Message);
        }
    }
}
