using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    // UI 元素的引用
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public Text feedbackText;

    private SQLiteManager dbManager;

    void Start()
    {
        // 初始化数据库管理类
        dbManager = new SQLiteManager("TestDatabase.db");

        // 给按钮绑定事件
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    // 登录按钮点击事件
    private void OnLoginClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // 调用 SQLiteManager 检查用户登录
        bool loginSuccess = dbManager.LoginUser(username, password);

        if (loginSuccess)
        {
            feedbackText.text = "登录成功！";
            feedbackText.color = Color.green;
            // 登录成功后清空用户名和密码输入框
            usernameInput.text = "";
            passwordInput.text = "";
        }
        else
        {
            feedbackText.text = "用户名或密码错误！";
            feedbackText.color = Color.red;
            // 登录失败后清空密码框
            passwordInput.text = "";
        }
    }

    // 注册按钮点击事件
    private void OnRegisterClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // 调用 SQLiteManager 注册用户
        bool registerSuccess = dbManager.RegisterUser(username, password);

        if (registerSuccess)
        {
            feedbackText.text = "注册成功！";
            feedbackText.color = Color.green;
            // 注册成功后清空用户名和密码输入框
            usernameInput.text = "";
            passwordInput.text = "";
        }
        else
        {
            feedbackText.text = "注册失败，用户名可能已存在！";
            feedbackText.color = Color.red;
        }
    }
}
