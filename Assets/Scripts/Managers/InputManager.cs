using UnityEngine;

namespace Managers {

    public class InputManager {

        public InputManager() {

        }

        public Movement checkForInput() {

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {

                return Movement.Up;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {

                return Movement.Down;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {

                return Movement.Left;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {

                return Movement.Right;
            }

            return Movement.None;
        }
    }
}