using System;

namespace Assets.Scripts.Utils
{
    public struct UFloat
    {
        private float value;

        public UFloat(float value) => this.value = Math.Abs(value);

        public UFloat(int value) => this.value = Math.Abs(value);

        public static implicit operator UFloat(float input) => new UFloat(input);

        public static implicit operator UFloat(int input) => new UFloat(input);
    }
}
