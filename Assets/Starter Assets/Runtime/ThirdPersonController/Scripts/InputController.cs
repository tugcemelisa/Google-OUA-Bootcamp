using System.Collections;
using UnityEngine;

namespace StarterAssets
{
    public class InputController : MonoBehaviour
    {
        public static InputController Instance;

        private void Awake()
        {
            Instance = this;
        }

        private bool isInputEnabled = true;

        public bool IsInputEnabled { get => isInputEnabled; }


        public void DisableInputForSeconds(float time)
        {
            isInputEnabled = false;
            Invoke("ActivateInput", time);
        }

        public void DisableInputs()
        {
            print("input disabled");
            isInputEnabled = false;
        }

        public void EnableInputs()
        {

            print("input enabled");
            isInputEnabled = true;
        }

        void ActivateInput()
        {
            isInputEnabled = true;
        }
    }

}
