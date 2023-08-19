using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PG.UI
{
    public class ButtonControls :BaseControls
    {
        [Header ("Buttons")]
        public ButtonCustom AccelerationBtn;
        public ButtonCustom BrakeReverseBtn;
        public ButtonCustom LeftSteerBtn;
        public ButtonCustom RightSteerBtn;
        public ButtonCustom HandBrakeBtn;
        public ButtonCustom BoostBtn;

        public override void Init (CarControllerInput userInput)
        {
            base.Init (userInput);

            AccelerationBtn.OnPointerDownAction += StartAccelerate;
            AccelerationBtn.OnPointerUpAction += StopAccelerate;

            BrakeReverseBtn.OnPointerDownAction += () => UserInput.SetBrakeReverse (1);
            BrakeReverseBtn.OnPointerUpAction += () => UserInput.SetBrakeReverse (0);

            LeftSteerBtn.OnPointerEnterAction += SteerLeft;
            LeftSteerBtn.OnPointerExitAction += ResetSteer;

            RightSteerBtn.OnPointerEnterAction += SteerRight;
            RightSteerBtn.OnPointerExitAction += ResetSteer;

            HandBrakeBtn.OnPointerEnterAction += () => UserInput.SetHandBrake (true);
            HandBrakeBtn.OnPointerExitAction += () => UserInput.SetHandBrake (false);

            BoostBtn.OnPointerEnterAction += () => UserInput.SetBoost (true);
            BoostBtn.OnPointerExitAction += () => UserInput.SetBoost (false);
        }

        private void StartAccelerate()
        {
            UserInput.SetAcceleration(1f);
        }

        private void StopAccelerate()
        {
            UserInput.SetAcceleration(0f);
        }

        private void SteerLeft()
        {
            UserInput.SetSteer(-1f);
        }

        private void SteerRight()
        {
            UserInput.SetSteer(1f);
        }

        private void ResetSteer()
        {
            UserInput.SetSteer(0f);
        }
    }
}
