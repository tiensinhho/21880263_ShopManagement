using BespokeFusion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.CompilerServices;

namespace _21880263.GUI
{
    public class MyMessageBox
    {
        public static MessageBoxResult result;
        public static bool Show(string Message, string Titile) {
            CustomMaterialMessageBox _mgs = new CustomMaterialMessageBox
            {
                TxtMessage = { Foreground = Brushes.Black },
                TxtTitle = { Foreground = Brushes.DeepSkyBlue, },
                BtnOk = { Content = "OK", Background = Brushes.White, BorderBrush = Brushes.DeepSkyBlue, Foreground = Brushes.DeepSkyBlue },
                BtnCancel = { Content = "Cancel", Background = Brushes.White, BorderBrush = Brushes.DeepSkyBlue, Foreground = Brushes.DeepSkyBlue, },
                BtnCopyMessage = { Visibility = Visibility.Hidden },
                MainContentControl = { Background = Brushes.White },
                TitleBackgroundPanel = { Background = Brushes.White },
                SizeToContent = SizeToContent.WidthAndHeight,
                BorderBrush = Brushes.White
            };
            _mgs.TxtMessage.Text = Message;
            _mgs.TxtTitle.Text = Titile;
            _mgs.Show();
            result = _mgs.Result;
            return _mgs.Result==MessageBoxResult.OK;
        }

    }
}
