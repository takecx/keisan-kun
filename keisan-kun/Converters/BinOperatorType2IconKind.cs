using keisan_kun.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace keisan_kun.Converters
{
    public class BinOperatorType2IconKind : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() != typeof(BinaryOperationType))
            {
                throw new ArgumentException();
            }

            var opeType = (BinaryOperationType)value;
            switch (opeType)
            {
                case BinaryOperationType.plus:
                    return PackIconKind.Plus;
                case BinaryOperationType.minus:
                    return PackIconKind.Minus;
                case BinaryOperationType.multiply:
                    return PackIconKind.Close;
                case BinaryOperationType.division:
                    return PackIconKind.SlashForward;
                default:
                    throw new ArgumentException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
