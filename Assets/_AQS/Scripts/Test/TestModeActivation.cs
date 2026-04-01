using UnityEngine;

namespace AQS.Test
{
    public class TestModeActivation : MonoBehaviour
    {
        private MalbersAnimations.Controller.MAnimal animal;

        private void Start()
        {
            animal = GetComponent<MalbersAnimations.Controller.MAnimal>();
            if (animal == null)
                Debug.LogError("TestMode: No MAnimal found!");
            else
                Debug.Log("TestMode: Ready. Press T=Attack2, Y=Attack1, U=Attack2 ForceActivate");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("TestMode: TryActivate Attack2 (Mode 2, Ability 1)...");
                bool result = animal.Mode_TryActivate(2, 1);
                string activeMode = animal.ActiveMode != null ? animal.ActiveMode.Name : "none";
                Debug.Log("TestMode: TryActivate returned " + result + ". ActiveMode: " + activeMode);
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                Debug.Log("TestMode: TryActivate Attack1 (Mode 1, Ability 1)...");
                bool result = animal.Mode_TryActivate(1, 1);
                string activeMode = animal.ActiveMode != null ? animal.ActiveMode.Name : "none";
                Debug.Log("TestMode: TryActivate returned " + result + ". ActiveMode: " + activeMode);
            }

            if (Input.GetKeyDown(KeyCode.U))
            {
                Debug.Log("TestMode: ForceActivate Attack2 (Mode 2)...");
                animal.Mode_ForceActivate(2, 1);
                string activeMode = animal.ActiveMode != null ? animal.ActiveMode.Name : "none";
                Debug.Log("TestMode: ForceActivate done. ActiveMode: " + activeMode);
            }
        }
    }
}
