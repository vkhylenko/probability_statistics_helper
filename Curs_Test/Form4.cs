using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;


namespace Curs_Test
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        string inpfileText;
        string[] str;
        string init_path = "D:";  //to save the location of the selected object


        double Expv(double[] a)
        {
            if (a.Length == 0)
                throw new ArgumentException("Маccив не может быть пустым");
            double sum = 0;
            double ev;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
            }
            ev = sum / a.Length;
            return ev;
        }

        double Disp(double[] a)
        {
            if (a.Length == 0)
                throw new ArgumentException("Маccив не может быть пустым");
            double sum = 0;
            double dx;
            for (int i = 0; i < a.Length; i++)
            {
                sum += Math.Pow(a[i] - Expv(a), 2);
            }
            dx = sum / a.Length;
            return dx;           
        }

        double CalcMediana(double[] a)
        {
            if (a.Length == 0)
                throw new ArgumentException("Маccив не может быть пустым");
            double m;
            if (a.Length % 2 != 0)
            {
                m = a[(a.Length + 1) / 2];
                return m;
            }
            else
            {
                double m1 = a[a.Length / 2];
                double m2 = a[a.Length / 2 + 1];
                m = (m1 + m2) / 2;
                return m;
            }
        }

        double CalcMode(double[] a)
        {
            if (a.Length == 0)
                throw new ArgumentException("Маccив не может быть пустым");

            Dictionary<double, int> dict = new Dictionary<double, int>();
            foreach (double elem in a)
            {
                if (dict.ContainsKey(elem))
                    dict[elem]++;
                else
                    dict[elem] = 1;
            }

            int maxCount = 0;
            double mode = Double.NaN;
            foreach (double elem in dict.Keys)
            {
                if (dict[elem] > maxCount)
                {
                    maxCount = dict[elem];
                    mode = elem;
                }
            }

            return mode;
        }

        double CoefAs(double[] a)
        {
            double sum = 0;
            double[] mm = new double [a.Length];
            double asym;
            for (int i = 0; i < a.Length; i++)
            {
               mm[i] = Math.Pow(a[i] - Expv(a), 3);
            }
            sum = Expv(mm);
            asym = sum / Math.Pow(Math.Sqrt(Disp(a)), 3);
            return asym;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "TXT files|*.txt";
            theDialog.InitialDirectory = @"D:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = theDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            /* Запись в файловый поток */
                            inpfileText = System.IO.File.ReadAllText(theDialog.FileName);
                            str = inpfileText.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            MessageBox.Show("Файл успешно открыт");
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка! Не удалось открыть файл " + ex.Message);
                }

                double[] array = new double[str.Length];
                for (int i = 0; i < str.Length; i++)
                {
                    array[i] = Double.Parse(str[i], CultureInfo.InvariantCulture);
                }

                if (array.Length == 0)
                    MessageBox.Show("Маcив не може бути порожнім!"); 

                double temp;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    for (int j = i + 1; j < array.Length; j++)
                    {
                        if (array[i] > array[j])
                        {
                            temp = array[i];
                            array[i] = array[j];
                            array[j] = temp;
                        }
                    }
                }

                var lines = new string[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    lines[i] = array[i].ToString();
                }


                System.IO.File.WriteAllLines(@"D:\sort.txt", lines);
                string infofile = "D:\\sort.txt";
                webBrowser1.Navigate(@infofile);

                length.Text = "" + array.Length;
                

                double expected_value;
                expected_value= Expv(array);
                label9.Text = "" + expected_value;

                double dispersion;
                dispersion = Disp(array);
                label10.Text = "" + dispersion;

                double median;
                median = CalcMediana(array);
                label12.Text = "" + median;

                double mode;
                mode = CalcMode(array);
                label13.Text = "" + mode;

                double asymmetry;
                asymmetry = CoefAs(array);
                label14.Text = "" + asymmetry;
                
            }
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
