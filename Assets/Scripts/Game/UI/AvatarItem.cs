using UnityEngine;
using UnityEngine.UI;

namespace Snakefun.Game.UI
{
    public class AvatarItem : MonoBehaviour
    {
        [SerializeField] private Image avatar;
        [SerializeField] private Button button;

        public delegate void AvatarUpdateDelegate(int avatarId);

        public void Initialize(Image avatar, int avatarId, AvatarUpdateDelegate updateFunction)
        {
            this.avatar.sprite = avatar.sprite;

            button.onClick.AddListener(() => updateFunction(avatarId));
        }
    }
}