using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppCalc
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private double _currentValue = 0;
        private double _secondValue = 0;
        private string _currentOperator = "";
        private bool _isScientificMode = false;
        private bool _isNewEntry = true;

        public MainPage()
        {
            this.InitializeComponent();
        }
        private void BasicButton_Checked(object sender, RoutedEventArgs e)
        {
            if (ScientificButton != null)
                ScientificButton.IsChecked = false;

            if (ScientificFunctionsPanel != null)
                ScientificFunctionsPanel.Visibility = Visibility.Collapsed;

            _isScientificMode = false;
        }

        private void ScientificButton_Checked(object sender, RoutedEventArgs e)
        {
            if (BasicButton != null)
                BasicButton.IsChecked = false;

            if (ScientificFunctionsPanel != null)
                ScientificFunctionsPanel.Visibility = Visibility.Visible;

            _isScientificMode = true;
        }

        // Handle Number Button Click
        private void OnNumberClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && Display != null)
            {
                string number = button.Content.ToString();

                if (_isNewEntry)
                {
                    Display.Text = number;
                    _isNewEntry = false;
                }
                else
                {
                    Display.Text += number;
                }
            }
        }

        // Handle Operator Button Click (+, -, ×, ÷)
        private void OnOperatorClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && Display != null && double.TryParse(Display.Text, out _currentValue))
            {
                _currentOperator = button.Content.ToString();
                _isNewEntry = true;
            }
        }

        // Handle Percentage Button Click
        private void OnPercentageClicked(object sender, RoutedEventArgs e)
        {
            if (Display != null && double.TryParse(Display.Text, out _currentValue))
            {
                _currentValue /= 100;
                Display.Text = _currentValue.ToString(CultureInfo.InvariantCulture);
                _isNewEntry = true;
            }
        }

        // Handle Clear Button Click
        private void OnClearClicked(object sender, RoutedEventArgs e)
        {
            if (Display != null)
            {
                Display.Text = "0";
                _currentValue = 0;
                _secondValue = 0;
                _currentOperator = "";
                _isNewEntry = true;
            }
        }

        // Handle Backspace Button Click
        private void OnBackspaceClicked(object sender, RoutedEventArgs e)
        {
            if (Display != null)
            {
                if (Display.Text.Length > 1)
                {
                    Display.Text = Display.Text.Substring(0, Display.Text.Length - 1);
                }
                else
                {
                    Display.Text = "0";
                }
            }
        }
        // Handle Equal Button Click
        private void OnEqualClicked(object sender, RoutedEventArgs e)
        {
            if (Display != null && double.TryParse(Display.Text, out _secondValue))
            {
                try
                {
                    switch (_currentOperator)
                    {
                        case "+":
                            _currentValue += _secondValue;
                            break;
                        case "−":
                            _currentValue -= _secondValue;
                            break;
                        case "×":
                            _currentValue *= _secondValue;
                            break;
                        case "÷":
                            if (_secondValue == 0)
                            {
                                Display.Text = "Cannot divide by zero";
                                return;
                            }
                            _currentValue /= _secondValue;
                            break;
                        default:
                            Display.Text = "Unknown operation";
                            return;
                    }

                    Display.Text = _currentValue.ToString(CultureInfo.InvariantCulture);
                }
                catch (Exception)
                {
                    Display.Text = "Error";
                }
                finally
                {
                    _isNewEntry = true;
                }
            }
        }

        // Handle Trigonometric and Square Root Functions (sin, cos, tan, √)
        private void OnFunctionClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && Display != null && double.TryParse(Display.Text, out _currentValue))
            {
                string function = button.Content.ToString();
                switch (function)
                {
                    case "sin":
                        _currentValue = Math.Sin(_currentValue * Math.PI / 180); // Convert to radians
                        break;
                    case "cos":
                        _currentValue = Math.Cos(_currentValue * Math.PI / 180); // Convert to radians
                        break;
                    case "tan":
                        _currentValue = Math.Tan(_currentValue * Math.PI / 180); // Convert to radians
                        break;
                    case "√":
                        if (_currentValue >= 0)
                            _currentValue = Math.Sqrt(_currentValue);
                        else
                            Display.Text = "Error";
                        return;
                }

                Display.Text = _currentValue.ToString(CultureInfo.InvariantCulture);
                _isNewEntry = true;
            }
        }

        // Handle Plus/Minus Button Click
        private void OnPlusMinusClicked(object sender, RoutedEventArgs e)
        {
            if (Display != null && double.TryParse(Display.Text, out _currentValue))
            {
                _currentValue = -_currentValue;
                Display.Text = _currentValue.ToString(CultureInfo.InvariantCulture);
            }
        }

        // Handle Decimal Button Click
        private void OnDecimalClicked(object sender, RoutedEventArgs e)
        {
            if (Display != null && !Display.Text.Contains("."))
            {
                Display.Text += ".";
                _isNewEntry = false;
            }
        }
    }
}