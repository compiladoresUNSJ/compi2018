using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using text_Box_Mio;

namespace BottonAwait
{

    public static class ButtonAwaiterExtensions
    {
        public static ButtonAwaiter GetAwaiter(this Button button)
        {
            return new ButtonAwaiter()
            {
                Button = button
            };
        }
    }

    public class ButtonAwaiter : INotifyCompletion
    {
        public bool IsCompleted
        {
            get { return false; }
        }

        public void GetResult()
        {

        }

        public Button Button { get; set; }

        public void OnCompleted(Action continuation)
        {
            RoutedEventHandler h = null;
            h = (o, e) =>
            {
                Button.Click -= h;
                continuation();
            };
            Button.Click += h;
        }
    }
}