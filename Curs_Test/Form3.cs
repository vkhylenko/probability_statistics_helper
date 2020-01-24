using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;

namespace Curs_Test
{
    
    public partial class Form3 : Form
    {

        double x1, x2, y1, y2;
        int i1, i2, j1, j2;

        public Form3()
        {
            InitializeComponent();
        }

        int XtoI(double x) //переводит декартову координату абсцисс в понятную C#
        {
            int i = (int)(i1 + ((x - x1) * (i2 - i1) / (x2 - x1)));
            return i;
        }

        int YtoJ(double y)//переводит декартову координату ординат в понятную C#
        {
            int j = (int)(j1 + ((y - y1) * (j2 - j1) / (y2 - y1)));
            return j;
        }

        void DrawXoY()//проводит координатные прямые с делениями и подписями
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
            Font myFont = new Font("Arial", 9, FontStyle.Regular);

            g.DrawLine(Pens.Black, XtoI(0), YtoJ(y1), XtoI(0), YtoJ(y2));
            g.DrawLine(Pens.Black, XtoI(x1), YtoJ(0), XtoI(x2), YtoJ(0));
            for (int i = (int)x1; i <= (int)x2; i++)
            {
                if (i != 0)
                {
                    g.DrawLine(Pens.Black, XtoI(i), YtoJ(0.08), XtoI(i), YtoJ(-0.08));
                    g.DrawString(i.ToString(), myFont, Brushes.Black, XtoI(i) - 6, YtoJ(0) + 10);
                    if (i == x2)
                    {
                        g.DrawString("x", myFont, Brushes.Black, XtoI(i) - 16, YtoJ(0) - 18);
                    }
                }
            }

            for (int j = (int)y2; j <= (int)y1; j++)
            {
                if (j != 0)
                {
                    g.DrawLine(Pens.Black, XtoI(0.08), YtoJ(j), XtoI(-0.08), YtoJ(j));
                    g.DrawString(j.ToString(), myFont, Brushes.Black, XtoI(0) - 20, YtoJ(j) - 6);
                    if (j == y1)
                    {
                        g.DrawString("y", myFont, Brushes.Black, XtoI(0) + 10, YtoJ(j) - 2);
                    }
                }

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawXoY();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            i1 = 0;
            i2 = pictureBox1.Width;
            j1 = 0;
            j2 = pictureBox1.Height;
            x1 = -5;
            x2 = 5;
            y1 = 5;
            y2 = -1;
        }

        class Distribution
        {
            public double[] x;
            public double[] p;
            int n = 100;

                     
            public virtual double Ma()
            {
                double sum = 0;
                for(int i = 0; i < n; i++)
                {                    
                    sum += x[i] * p[i];          
                }
                return sum;
            }

            public virtual double Dis()
            {
                double sum = 0;
                for (int i = 0; i < n; i++)
                {                    
                    sum += x[i]*x[i] * p[i];                    
                }
                return sum;
            }


            ~Distribution() { }
        }


        class Pois: Distribution
        {
            public double lambda;

            public Pois(double lambda)
            {
                this.lambda = lambda;
            }

            public double Ma()
            {
                var ma = (double)lambda;
                return ma;
            }

            public double Dis()
            {
                var dis = (double) lambda;
                return dis;
            }
            ~Pois(){ }
        }

        class Geom : Distribution
        {
            public double p;
            public Geom(double p)
            {
                this.p = p;
            }

            public double Ma()
            {
                var ma = (double) (1-p) / p;
                return ma;
            }

            public double Dis()
            {
                var dis = (double) (1 - p) / (p * p);
                return dis;
            }
            ~Geom() { }

        }

        class Bin : Distribution
        {
            public double p;
            public double n;
            public Bin(double n, double p)
            {
                this.p = p;
                this.n = n;
            }

            public double Ma()
            {
                var ma = (double)n * p;
                return ma;
            }

            public double Dis()
            {
                var dis = (double)(1 - p) * n * p;
                return dis;
            }

            ~Bin(){ }
        }
        
        class U : Distribution
        {
            public double a;
            public double b;
            public U(double a, double b)
            {
                this.a = a;
                this.b = b;
            }

            public double Ma()
            {
                var ma = (double)(a + b)/2;
                return ma;
            }

            public double Dis()
            {
                var dis = (double)Math.Pow(b - a, 2)/12;
                return dis;
            }

            ~U() { }
        }

        class Exp : Distribution
        {
            public double lambda;

            public Exp(double lambda)
            {
                this.lambda = lambda;
            }

            public double Ma()
            {
                var ma = (double)1 / lambda;
                return ma;
            }

            public double Dis()
            {
                var dis = (double)1/(lambda * lambda);
                return dis;
            }
            ~Exp() { }
        }

        class N : Distribution
        {
            public double a;
            public double Dx;
            public N(double a, double Dx)
            {
                this.a = a;
                this.Dx = Dx;
            }

            public double Ma()
            {
                var ma = (double)a;
                return ma;
            }

            public double Dis()
            {
                var dis = (double)Dx;
                return dis;
            }

            ~N(){ }
        }


        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            double a = double.Parse(textBox1.Text, CultureInfo.InvariantCulture);

            if (comboBox1.SelectedIndex == 0)
            {
                MessageBox.Show("Ви обрали пуасонівський розподіл!");
                //textBox2.Visible = false;
                //label11.Visible = false;
                //label12.Visible = false;
                if (a < 0)
                {
                    MessageBox.Show("Параметр має бути більше 0!");
                }
                else
                {
                    Pois p1 = new Pois(a);
                    double expv;
                    expv = p1.Ma();
                    lbl_MO.Text = "Ex = lambda = " + expv;
                    double disp;
                    disp = p1.Dis();
                    label7.Text = "Dx = lambda = " + disp;
                    label8.Text = "Розподіл дискретний!";
                    label9.Text = "Розподіл дискретний!";
                    label10.Text = "Розподіл дискретний!";
                }

            }

            if (comboBox1.SelectedIndex == 1)
            {
                double b = double.Parse(textBox2.Text, CultureInfo.InvariantCulture);
                MessageBox.Show("Ви обрали рівномірний розподіл!");                
                if (a>b)
                {
                    MessageBox.Show("b має бути більше, ніж a!");
                }
                else
                {
                    U u1 = new U(a, b);
                    double expv;
                    expv = u1.Ma();
                    lbl_MO.Text = "Ex = (a+b)/2 = " + expv;
                    double disp;
                    disp = u1.Dis();
                    label7.Text = "Dx = (b-a)^2/12 = " + disp;
                    double c = b - a;
                    label8.Text = "F(x)=(x-a)/(b-a) = (x-" + a + ")/" + c + "";
                    label9.Text = "f=1/(b-a) = 1/"+ c;

                    Graphics g = pictureBox1.CreateGraphics();

                    x1+=(a+b)/2;
                    x2+=(a+b)/2;
                    y1 = 1 / (b - a) + 1;
                    g.Clear(Color.White);
                    DrawXoY();

                    Pen my = new Pen(Brushes.Blue, 3);
                    g.DrawLine(my, XtoI(a), YtoJ(1/(b-a)), XtoI(b), YtoJ(1/(b-a)));
                    g.DrawLine(my, XtoI(x1), YtoJ(0), XtoI(a), YtoJ(0));
                    g.DrawLine(my, XtoI(b), YtoJ(0), XtoI(x2), YtoJ(0));
                }
            }

            if (comboBox1.SelectedIndex == 2)
            {
                MessageBox.Show("Ви обрали експоненціальний розподіл!");
                //textBox2.Visible = false;
                //label11.Visible = false;
                //label12.Visible = false;
                if (a < 0)
                {
                    MessageBox.Show("Параметр має бути більше 0!");
                }
                else
                {
                    Exp e1 = new Exp(a);
                    double expv;
                    expv = e1.Ma();
                    lbl_MO.Text = "Ex = 1/lambda = " + expv;
                    double disp;
                    disp = e1.Dis();
                    label7.Text = "Dx = 1/lambda^2 = " + disp;
                    label8.Text = "F(x) = 1 - e^(-lambda*x) = 1 - e^(-"+a+"*x)";
                    label9.Text = "f = lambda*e^(--lambda*x) = "+a+ "*e^(-" + a + "*x)";

                    x1-=a-5;
                    x2+=a+5;
                    y1+=a-4;
                    //y2-=a+1;
                    Graphics g = pictureBox1.CreateGraphics();
                    Pen my = new Pen(Brushes.Red, 2);
                    DrawXoY();

                    for (double i = 0; i <= x2; i += 0.003)
                    {                        
                        double j = a * Math.Pow(Math.E, -a * i);
                        g.DrawEllipse(my, XtoI(i), YtoJ(j), 1, 1);
                    }
                }
            }

            if (comboBox1.SelectedIndex == 3)
            {
                double b = double.Parse(textBox2.Text, CultureInfo.InvariantCulture);
                MessageBox.Show("Ви обрали гаусівський розподіл!");
                label14.Visible = true;
                if (b < 0)
                {
                    MessageBox.Show("Dx має бути більше 0!");
                }
                else
                {
                    N n1 = new N(a, b);
                    double expv;
                    expv = n1.Ma();
                    lbl_MO.Text = "Ex = a = " + expv;
                    double disp;
                    disp = n1.Dis();
                    label7.Text = "Dx = " + disp;
                    label8.Text = "F(x) = 1/2 + Ф((x-a)/(sqrt(Dx)) = 1/2 + Ф((x-" + a + ")/" + Math.Sqrt(b) + ")";
                    label9.Text = "f = 1/(sqrt(Dx*2*pi)) * e^(-(x-a)^2 / (2 * Dx) = \n = "
                    + 1/(Math.Sqrt(b*2*Math.PI))+ "*e^(-(x - " + a + ")^2 / " + 2*b;

                    y1 = 1.5;
                    x1 += a;
                    x2 += a;
                    Graphics g = pictureBox1.CreateGraphics();
                    Pen my = new Pen(Brushes.Red, 2);
                    DrawXoY();

                    for (double i = x1; i <= x2; i += 0.003)
                    {
                        double j = 1 / (Math.Sqrt(b * 2 * Math.PI)) * Math.Pow(Math.E, -(Math.Pow((i-a), 2)/(2*b)));
                        g.DrawEllipse(my, XtoI(i), YtoJ(j), 1, 1);
                    }

                }

            }

            if (comboBox1.SelectedIndex == 4)
            {
                MessageBox.Show("Ви обрали геометричний розподіл!");
                textBox2.Visible = false;
                label11.Visible = false;
                label12.Visible = false;
                if (a > 1 || a < 0)
                {
                    MessageBox.Show("p має бути від 0 до 1!");
                }
                else
                {
                    Geom g1 = new Geom(a);
                    double expv;
                    expv = g1.Ma();
                    lbl_MO.Text = "Ex = q/p = " + expv;
                    double disp;
                    disp = g1.Dis();
                    label7.Text = "Dx = q/p^2 = " + disp;
                    label8.Text = "Розподіл дискретний!";
                    label9.Text = "Розподіл дискретний!";
                    label10.Text = "Розподіл дискретний!";
                }
            }

            if (comboBox1.SelectedIndex == 5)
            {
                double b = double.Parse(textBox2.Text, CultureInfo.InvariantCulture);
                MessageBox.Show("Ви обрали біноміальний розподіл!");
                if (a < 0)
                {
                    MessageBox.Show("n має бути більше 0!");
                }
                else { 
                    if (b > 1 || b < 0)
                        {
                            MessageBox.Show("p має бути від 0 до 1!");
                        }
                    else
                    {
                        Bin b1 = new Bin(a, b);
                        double expv;
                        expv = b1.Ma();
                        lbl_MO.Text = "Ex = n*p = " + expv;
                        double disp;
                        disp = b1.Dis();
                        label7.Text = "Dx = n*p*q = " + disp;
                        label8.Text = "Розподіл дискретний!";
                        label9.Text = "Розподіл дискретний!";
                        label10.Text = "Розподіл дискретний!";

                    }
                }
            }
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
