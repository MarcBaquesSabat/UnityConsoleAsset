using UnityEngine;
using UnityEngine.UI;

namespace BlackRefactory.Console
{
    public class ConsoleAutoCompleteCommand : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI commandPreviewText = null;
        [SerializeField]
        private TMPro.TextMeshProUGUI commandDescriptionText = null;
        [SerializeField]
        private Image selectedImage = null;

        private string commandPreview = "";
        private string commandDescription = "";

        public void Build(string commandPreview, string commandDescription)
        {
            this.commandPreview = commandPreview;
            commandPreviewText.text = this.commandPreview;

            this.commandDescription = commandDescription;
            commandDescriptionText.text = this.commandDescription;
            
            Unselect();
        }

        public void Select()
        {
            selectedImage.enabled = true;
        }

        public void Unselect()
        {
            selectedImage.enabled = false;
        }
    }
}