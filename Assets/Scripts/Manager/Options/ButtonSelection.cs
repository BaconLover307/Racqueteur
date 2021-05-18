using UnityEngine;
using UnityEngine.UI;

namespace Manager.Options
{
    public class ButtonSelection : MonoBehaviour
    {
        public Button[] buttonList;

        private void Start()
        {
            for (var i = 0; i < buttonList.Length; i++)
            {
                var idx = i;
                buttonList[i].onClick.AddListener(delegate { OnClick(idx); });
            }
        }

        private void OnClick(int idx)
        {
            foreach (var button in buttonList) button.interactable = true;
            buttonList[idx].interactable = false;
        }
    }
}