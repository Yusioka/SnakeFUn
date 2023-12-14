using System.Collections;
using TMPro;
using UnityEngine;

namespace Snakefun.Game.UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMessage;
        [SerializeField] private GameObject body;

        private void OnEnable()
        {
            CloseTab();
        }

        public void ShowMessage(string text)
        {
            textMessage.text = text;
            body.SetActive(true);

            StartCoroutine(WaitForCloseTab());
        }

        private IEnumerator WaitForCloseTab()
        {
            yield return new WaitForSecondsRealtime(2);
            CloseTab();
        }

        public void CloseTab()
        {
            body.SetActive(false);
        }
    }
}