using Snakefun.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminRowItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI id;
    [SerializeField] private TextMeshProUGUI nickname;
    [SerializeField] private TextMeshProUGUI password;
    [SerializeField] private TextMeshProUGUI avatarId;
    [SerializeField] private TextMeshProUGUI role;
    [SerializeField] private Button deleteButton;

    public void Initialize(int id, string nickname, string password, int avatarId, Role role, bool isDelButtonActive)
    {
        this.id.text = $"{id}";
        this.nickname.text = nickname;
        this.password.text = $"{password}";
        this.avatarId.text = $"{avatarId}";
        this.role.text = $"{role}";

        if (isDelButtonActive)
        {
            deleteButton.interactable = true;
            deleteButton.onClick.AddListener(() =>
            {
                DatabaseController.Instance.Database.DeleteUserByNickname(nickname);
                Destroy(gameObject);
            });
        }
        else
        {
            deleteButton.interactable = false;
        }
    }
}