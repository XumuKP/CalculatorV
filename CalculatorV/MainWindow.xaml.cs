using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CalculatorV
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                answer.Text = RemoveBrackets(TextBox.Text);
            }
        }

        public string RemoveBrackets(string text)
        {
            while (text.Contains('(') && text.Contains(')'))
            {
                int openIndex = 0;
                int closeIndex = 0;

                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '(')
                    {
                        openIndex = i;
                    }
                    if (text[i] == ')')
                    {
                        closeIndex = i;

                        text = text.Remove(openIndex, closeIndex - openIndex + 1).Insert(openIndex, ResolvBreackets(openIndex, closeIndex, text));

                        break;
                    }
                }
            }

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i] == '-' && (text[i-1] == '*' || text[i-1] == '/'))
                {
                    for (int j = i-1; j >= 0; j--)
                    {
                        if (text[j] == '+')
                        {
                            StringBuilder text1 = new StringBuilder(text);

                            text1[j] = '-';

                            text = text1.ToString();

                            text = text.Remove(i, 1);

                            break;
                        }
                        else if (text[j] == '-')
                        {
                            StringBuilder text1 = new StringBuilder(text);

                            text1[j] = '+';

                            text = text1.ToString();

                            text = text.Remove(i, 1);

                            break;
                        }
                    }

                }
            }

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i] == '-' && (text[i - 1] == '-' || text[i - 1] == '+'))
                {
                    if (text[i-1] == '-')
                    {
                        StringBuilder text1 = new StringBuilder(text);

                        text1[i] = '+';

                        text = text1.ToString();

                        text = text.Remove(i - 1, 1);
                    }
                    else
                    {
                        StringBuilder text1 = new StringBuilder(text);

                        text1[i] = '-';

                        text = text1.ToString();

                        text = text.Remove(i - 1, 1);
                    }
                }
                else if (text[i] == '+' && (text[i-1] == '-' || text[i-1] == '+'))
                {
                    if (text[i-1] == '-')
                    {
                        StringBuilder text1 = new StringBuilder(text);

                        text1[i] = '-';
                         
                        text = text1.ToString();

                        text = text.Remove(i-1, 1);
                    }
                    else
                    {
                        StringBuilder text1 = new StringBuilder(text);

                        text1[i] = '+';

                        text = text1.ToString();

                        text = text.Remove(i-1, 1);
                    }
                }
            }    

            if (text[0] == '-')
            {
                text = '0' + text;
            }

            return Calculate(text);

        }

        public string ResolvBreackets(int openIndex, int closeIndex, string text)
        {
            string bracketsAnswer = Calculate(text.Substring(openIndex + 1, closeIndex - openIndex - 1));

            return bracketsAnswer;
        }

        public string Calculate(string text)
        {
            double finaltextBox = AddAndSubstract(text);

            answer.Text = finaltextBox.ToString();

            return finaltextBox.ToString();
        }

        public double AddAndSubstract(string text1)
        {
            string[] text = text1.Split('-');
            List<string> textlist = new List<string>();

            for (int i = 0; i < text.Length; i++)
            {
                textlist.Add(text[i]);
                if (i != text.Length-1)
                {
                    textlist.Add("-");
                }
            }
            for (int i = 0; i < textlist.Count; i++)
            {
                if (textlist[i].Contains('+') && textlist[i].Length >1)
                {
                    string[] textPart = textlist[i].Split('+');

                    textlist.RemoveAt(i);

                    for (int j = textPart.Length - 1; j >= 0; j--)
                    {
                        textlist.Insert(i, textPart[j]);
                        if (j != 0)
                        {
                            textlist.Insert(i, "+");
                        }
                    }
                }

            }

            double total;
            if (textlist[0].Contains('*') || textlist[0].Contains('/'))
            {
                total = DevideAndMultiply(textlist[0]);
            }
            else
            {
                total = Convert.ToDouble(textlist[0]);
            }
            for (int i = 1; i < textlist.Count; i += 1)
            {
                if(textlist[i-1] == "-")
                {
                    total = total - DevideAndMultiply(textlist[i]);
                }
                else if (textlist[i-1] == "+")
                { 
                    total = total + DevideAndMultiply(textlist[i]);
                }
            }
            return total;
        }
        private double DevideAndMultiply(string text1)
        {
            string[] text = text1.Split('*');
            List<string> textlist = new List<string>();

            for (int i = 0; i < text.Length; i++)
            {
                textlist.Add(text[i]);
                if (i != text.Length - 1)
                {
                    textlist.Add("*");
                }
            }
            for (int i = 0; i < textlist.Count; i++)
            {
                if (textlist[i].Contains('/') && textlist[i].Length > 1)
                {
                    string[] textPart = textlist[i].Split('/');

                    textlist.RemoveAt(i);

                    for (int j = textPart.Length - 1; j >= 0; j--)
                    {
                        textlist.Insert(i, textPart[j]);
                        if (j != 0)
                        {
                            textlist.Insert(i, "/");
                        }
                    }
                }

            }

            double total = Convert.ToDouble(textlist[0]);
            for (int i = 2; i < textlist.Count; i += 2)
            {
                if (textlist[i - 1] == "/")
                {
                    total = total / Convert.ToDouble(textlist[i]);
                }
                else if (textlist[i - 1] == "*")
                {
                    total = total * Convert.ToDouble(textlist[i]);
                }
            }

            return total;
        }
        private void Button_ClickP(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "+";
        }
        private void Button_ClickM(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "-";
        }

        private void Button_ClickY(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "*";
        }

        private void Button_ClickR(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "/";
        }

        private void Button_ClickE(object sender, RoutedEventArgs e)
        {
            answer.Text = RemoveBrackets(TextBox.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "0";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "1";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "2";
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "3";
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "4";
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "5";
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "6";
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "7";
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "8";
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            TextBox.Text += "9";
        }

        private void Button_Click_С(object sender, RoutedEventArgs e)
        {
            TextBox.Clear();
            answer.Clear();
        }

        private void Button_Click_D(object sender, RoutedEventArgs e)
        {

            if (TextBox.Text.Length > 0)
            {
                TextBox.Text = TextBox.Text.Substring(0, TextBox.Text.Length - 1);
            }
        }

        private void Button_Click_PM(object sender, RoutedEventArgs e)
        {
            double i = double.Parse(TextBox.Text);
                TextBox.Text = (-i).ToString();
        }

        private void Button_Click_Z(object sender, RoutedEventArgs e)
        {
            TextBox.Text += ",";
        }
    }
}
