using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Alter {

    public class Panel_GameInfo : MonoBehaviour {

        [SerializeField] TextMeshProUGUI levelTxt;

        public void Ctor() {
            levelTxt.text = "Lv.0";
        }

        public void Level_Set(int level) {
            levelTxt.text = "Lv." + level.ToString();
        }

    }

}