using UnityEngine;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    // UI Ԫ�ص�����
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public Button registerButton;
    public Text feedbackText;

    private SQLiteManager dbManager;

    void Start()
    {
        // ��ʼ�����ݿ������
        dbManager = new SQLiteManager("TestDatabase.db");

        // ����ť���¼�
        loginButton.onClick.AddListener(OnLoginClicked);
        registerButton.onClick.AddListener(OnRegisterClicked);
    }

    // ��¼��ť����¼�
    private void OnLoginClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // ���� SQLiteManager ����û���¼
        bool loginSuccess = dbManager.LoginUser(username, password);

        if (loginSuccess)
        {
            feedbackText.text = "��¼�ɹ���";
            feedbackText.color = Color.green;
            // ��¼�ɹ�������û��������������
            usernameInput.text = "";
            passwordInput.text = "";
        }
        else
        {
            feedbackText.text = "�û������������";
            feedbackText.color = Color.red;
            // ��¼ʧ�ܺ���������
            passwordInput.text = "";
        }
    }

    // ע�ᰴť����¼�
    private void OnRegisterClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // ���� SQLiteManager ע���û�
        bool registerSuccess = dbManager.RegisterUser(username, password);

        if (registerSuccess)
        {
            feedbackText.text = "ע��ɹ���";
            feedbackText.color = Color.green;
            // ע��ɹ�������û��������������
            usernameInput.text = "";
            passwordInput.text = "";
        }
        else
        {
            feedbackText.text = "ע��ʧ�ܣ��û��������Ѵ��ڣ�";
            feedbackText.color = Color.red;
        }
    }
}
