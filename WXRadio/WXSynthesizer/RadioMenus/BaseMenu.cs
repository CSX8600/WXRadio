using System;

namespace WXRadio.WXSynthesizer.RadioMenus
{
    public abstract class BaseMenu
    {
        private bool _configHasBeenSet = false;
        private Action<string> labelChangeCallback;
        public Action<string> LabelChangeCallback { set { labelChangeCallback = value; } }

        protected Config config;
        public Config Config
        {
            set
            {
                config = value;

                if (!_configHasBeenSet)
                {
                    OnConfigSet();
                    _configHasBeenSet = true;
                }
            }
        }

        protected Action<Config> saveConfigCallback;
        public Action<Config> SaveConfigCallback { set { saveConfigCallback = value; } }

        protected Func<IntPtr> getDashboardControlHandle;
        public Func<IntPtr> GetDashboardControlHandleCallback { set { getDashboardControlHandle = value; } }

        protected Action<ButtonResult> forceMenuCallback;
        public Action<ButtonResult> ForceMenuCallback { set { forceMenuCallback = value; } }

        protected void ChangeLabel(string text)
        {
            labelChangeCallback?.Invoke(text);
        }

        public enum ButtonTypes
        {
            Menu,
            Select,
            Up,
            Down,
            Left,
            Right,
            WeatherSnooze
        }

        public virtual void OnTimer() { }
        protected virtual void OnConfigSet() { }

        public abstract ButtonResult ButtonPressed(ButtonTypes buttonType);
        public abstract string GetTextForInitialDisplay();

        public class ButtonResult
        {
            public BaseMenu NextMenu { get; set; }
            public bool PressSuccess { get; set; }
            public bool PlayButtonSound { get; set; }

            public ButtonResult(bool pressSuccess, bool playButtonSound = true)
            {
                if (pressSuccess)
                {
                    throw new InvalidOperationException("If button press was successful, next menu must be specified");
                }

                PressSuccess = pressSuccess;
                PlayButtonSound = playButtonSound;
            }

            public ButtonResult(bool pressSuccess, BaseMenu nextMenu, bool playButtonSound = true)
            {
                NextMenu = nextMenu;
                PressSuccess = pressSuccess;
                PlayButtonSound = playButtonSound;
            }
        }
    }
}
