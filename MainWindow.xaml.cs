using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;




namespace projecte_ordenacio_barres
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int MAXIM_QUADRAS = 300;

        List<int> numeros;
        List<Rectangle> rectangles;
        Random r;
        double esquerraActual;
        SolidColorBrush correcte;
        double midaQuadrat;
        SolidColorBrush incorrecte;
        SolidColorBrush canvas;
        System.Windows.Forms.ColorDialog dialegColor;
        bool esRectangle;
        int nRectangles;
        bool animacio;
        
        DateTime inici;
        


        public MainWindow()
        {
            InitializeComponent();
            animacio = true;
            r = new Random();
            numeros = new List<int>();
            rectangles = new List<Rectangle>();
            nRectangles = (int)iudNRectangles.Value;
            midaQuadrat = cnvPaper.Width / nRectangles;
            esquerraActual = 0;
            incorrecte = new SolidColorBrush(Colors.Tomato);
            correcte = new SolidColorBrush(Colors.LightGreen);
            canvas = new SolidColorBrush(Color.FromArgb(255, 81, 55, 55));
            cnvPaper.Background = canvas;
            cpCorrecte.SelectedColor = correcte.Color;
            cpIncorrecte.SelectedColor = incorrecte.Color;
            cpFons.SelectedColor = canvas.Color;
            esRectangle = true;
            inici = DateTime.Now;
           
            



            cmbOrdenacio.Items.Add("Insercio");
            cmbOrdenacio.Items.Add("QuickSort");
            cmbOrdenacio.Items.Add("Sacsejada");
            cmbOrdenacio.Items.Add("Seleccio");
            cmbOrdenacio.Items.Add("ShellSort");


        }

        private void btnDesordena_Click(object sender, RoutedEventArgs e)
        {
            Desordenar();
        }

        private void Desordenar()
        {
            int i;
            nRectangles = (int)iudNRectangles.Value;
            midaQuadrat = cnvPaper.Width / nRectangles;

            List<int> aux = new List<int>();
            numeros = new List<int>();
            rectangles = new List<Rectangle>();
            cnvPaper.Children.Clear();
            Rectangle rect;
            for (i = 0; i < nRectangles; i++)
            {
                numeros.Add((i + 1));
            }

            while (numeros.Count > 0)
            {
                i = r.Next(0, numeros.Count);
                aux.Add(numeros[i]);
                numeros.Remove(numeros[i]);
            }

            numeros = aux;
            esquerraActual = 1;
            for (i = 0; i < numeros.Count; i++)
            {
                rect = new Rectangle();
                rect.Width = midaQuadrat;

                rect.Height = numeros[i] * cnvPaper.Height / nRectangles;

                ColorRectangle(rect, i);

                cnvPaper.Children.Add(rect);
                rect.SetValue(Canvas.ZIndexProperty, 1);
                rect.SetValue(Canvas.LeftProperty, esquerraActual);
                Canvas.SetBottom(rect, 0);
                esquerraActual += midaQuadrat;
                rectangles.Add(rect);


            }

            if (!esRectangle)
                CanviarAElipses();

            btnOrdena.IsEnabled = true;
        }

        private void CanviarAElipses()
        {
            int i;
            Rectangle r;
            for (i = 0; i < nRectangles; i++)
            {
                r = rectangles[i];
                r.RadiusX = midaQuadrat;
                r.RadiusY = midaQuadrat;
                r.SetValue(Canvas.TopProperty, cnvPaper.Height - r.Height);
                r.Height = midaQuadrat;
                ColorRectangle(r, i);
            }
        }

        private void CanviarARectangles()
        {
            double topRect;
            int i;
            Rectangle r;
            for (i = 0; i < nRectangles; i++)
            {
                r = rectangles[i];
                topRect = (double)r.GetValue(Canvas.TopProperty);
                r.RadiusY = 0;
                r.RadiusX = 0;
                Canvas.SetBottom(r, 0);
                r.Height = cnvPaper.Height - topRect;
                ColorRectangle(r, i);
            }
        }

        private void ColorRectangle(Rectangle r, int pos)
        {
            if (esRectangle)
            {
                if ((int)((pos + 1) * cnvPaper.Height / nRectangles) == (int)r.Height)
                    r.Fill = correcte;
                else
                    r.Fill = incorrecte;
            }
            else
            {
                if ((int)((pos + 1) * cnvPaper.Height / nRectangles) == (int)(cnvPaper.Height - (double)r.GetValue(Canvas.TopProperty)))
                    r.Fill = correcte;
                else
                    r.Fill = incorrecte;
            }
        }


        private void Intercanvi(int pos, int pos2)
        {
            int aux;
            double esqAct;
            Rectangle rAux;
            SolidColorBrush color= new SolidColorBrush(Colors.LemonChiffon);
            Storyboard stb = new Storyboard();
            DoubleAnimation anim1 = new DoubleAnimation();
            DoubleAnimation anim2 = new DoubleAnimation();



            if (!animacio)
            {
                aux = numeros[pos];
                numeros[pos] = numeros[pos2];
                numeros[pos2] = aux;

            }
            else
            {
                anim1.To = (double)rectangles[pos2].GetValue(Canvas.LeftProperty);
                anim1.Duration = TimeSpan.FromSeconds((double)(iudPausa.Value) / 2);



                anim2.To = (double)rectangles[pos].GetValue(Canvas.LeftProperty);
                anim2.Duration = TimeSpan.FromSeconds((double)(iudPausa.Value) / 2);



                rectangles[pos].Fill = color;
                rectangles[pos].SetValue(Canvas.ZIndexProperty, 100);
                rectangles[pos2].Fill = color;
                rectangles[pos2].SetValue(Canvas.ZIndexProperty, 100);


                stb.Children.Add(anim1);
                stb.Children.Add(anim2);
                Storyboard.SetTarget(anim1, rectangles[pos]);
                Storyboard.SetTargetProperty(anim1, new PropertyPath(Canvas.LeftProperty));
                Storyboard.SetTarget(anim2, rectangles[pos2]);
                Storyboard.SetTargetProperty(anim2, new PropertyPath(Canvas.LeftProperty));
                stb.Begin();


                Espera((double)iudPausa.Value);

                rectangles[pos].SetValue(Canvas.ZIndexProperty, 1);
                rectangles[pos2].SetValue(Canvas.ZIndexProperty, 1);

                rectangles[pos].BeginAnimation(LeftProperty, null);
                rectangles[pos2].BeginAnimation(LeftProperty, null);


                aux = numeros[pos];
                rAux = rectangles[pos];
                esqAct = (double)rectangles[pos].GetValue(Canvas.LeftProperty);

                numeros[pos] = numeros[pos2];
                rectangles[pos].SetValue(Canvas.LeftProperty, rectangles[pos2].GetValue(Canvas.LeftProperty));
                rectangles[pos] = rectangles[pos2];

                numeros[pos2] = aux;
                rectangles[pos2].SetValue(Canvas.LeftProperty, esqAct);
                rectangles[pos2] = rAux;





                ColorRectangle(rectangles[pos], pos);
                ColorRectangle(rectangles[pos2], pos2);
            }

            
          //  DoEvents();
          //  Thread.Sleep((int)iudPausa.Value);
            
        }

       


        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background,
                             new Action(delegate { }));
        }

        public void btnOrdena_Click(object sender, RoutedEventArgs e)
        {
            DateTime final;
            inici = DateTime.Now;
            txbTimer.Text = "00:00:00";
            txbTotal.Text = "00:00:00";
            System.Windows.Forms.Timer tim= new System.Windows.Forms.Timer();
            //tim.Interval = 1000;
            tim.Enabled = true;
            tim.Tick += new EventHandler(TimeEvent);


            btnInvertir.IsEnabled = false;
            btnDesordena.IsEnabled = false;

            

            switch (cmbOrdenacio.Text)
            {
                case "Insercio":
                    OrdenaInsercio();
                    break;
                case "QuickSort":
                    QuickSort(0, numeros.Count - 1);
                    break;
                case "Sacsejada":
                    Sacsejada();
                    break;
                case "Seleccio":
                    OrdenaSeleccio();
                    break;
                case "ShellSort":
                    ShellSort();
                    break;
            }

            btnDesordena.IsEnabled = true;
            btnInvertir.IsEnabled = true;
            btnOrdena.IsEnabled = false;
            tim.Enabled = false;

            final = DateTime.Now;
            txbTotal.Text = (final - inici).ToString(@"hh\:mm\:ss\.ffff");
            tim.Enabled = false;
            

        }

        private void TimeEvent(object sender, EventArgs e)
        {
            DateTime actual = DateTime.Now;
            TimeSpan tSpan=actual-inici;
            txbTimer.Text = tSpan.ToString(@"hh\:mm\:ss\.fff");
                
        }


        private void OrdenaInsercio()
        {
            int i = 1;
            int j;
            int actual;

            for (i = 1; i < numeros.Count; i++)
            {
                j = i - 1;
                actual = numeros[i];
                while (j >= 0 && numeros[j] > actual)
                {
                    Intercanvi(j, j + 1);
                    j--;
                }


            }
        }

        private void QuickSort(int esq, int dreta)
        {
            int i = esq, j = dreta;
            int pivot = numeros[(esq + dreta) / 2];
            while (i <= j)
            {
                while (numeros[i].CompareTo(pivot) < 0)
                {
                    i++;
                }
                while (numeros[j].CompareTo(pivot) > 0)
                {
                    j--;
                }
                if (i <= j)
                {
                    Intercanvi(i, j);

                    i++;
                    j--;
                }
            }

            if (esq < j)
            {
                QuickSort(esq, j);
            }

            if (i < dreta)
            {
                QuickSort(i, dreta);
            }
        }

        private void Sacsejada()
        {
            int i = 0, j;
            bool intercanvi = true;

            while(intercanvi && i<numeros.Count)
            {
                intercanvi = false;
                for(j=1;j<numeros.Count-i;j++)
                {
                    if(numeros[j]<numeros[j-1])
                    {
                        Intercanvi(j, j - 1);
                        intercanvi = true;
                    }
                }

                for (j = numeros.Count-1-i; j >0; j--)
                {
                    if (numeros[j] < numeros[j - 1])
                    {
                        Intercanvi(j, j - 1);
                        intercanvi = true;
                    }
                }

            }

        }

        private void ShellSort()
        {
            int i, j, increment;
            int tmp;

            increment = numeros.Count / 2;

            while(increment>0)
            {
                for(i=0;i<numeros.Count;i++)
                {
                    j = i;
                    tmp = numeros[i];
                    while((j>=increment)&&(numeros[j-increment]).CompareTo(tmp)>0)
                    {
                        Intercanvi(j, j - increment);
                        j = j - increment;
                    }
                    
                }
                if (increment == 2)
                    increment = 1;
                else
                    increment = increment * 5 / 11;
            }
        }

        private void OrdenaSeleccio()
        {
            int i = 0, j = 0;
            int min = 0, posMin = 0;

            for (i = 0; i < numeros.Count - 1; i++)
            {
                min = numeros[i];
                posMin = i;
                for (j = i; j < numeros.Count; j++)
                {
                    if (numeros[j] < min)
                    {
                        posMin = j;
                        min = numeros[j];
                    }
                }
                if (numeros[i] != min)
                {
                    Intercanvi(i, posMin);
                }
            }
        }

        private void cpCorrecte_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            correcte.Color = cpCorrecte.SelectedColor;
        }

        private void cpIncorrecte_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            incorrecte.Color = cpIncorrecte.SelectedColor;
        }

        private void cpFons_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            canvas.Color = cpFons.SelectedColor;
        }

        private void rdbRectangles_Checked(object sender, RoutedEventArgs e)
        {
            if (rectangles != null)
            {
                esRectangle = true;
                CanviarARectangles();
            }
        }

        private void rdbPunts_Checked(object sender, RoutedEventArgs e)
        {
            if (rectangles != null)
            {
                esRectangle = false;
                CanviarAElipses();

            }
        }


        private void btnIvertir_Click(object sender, RoutedEventArgs e)
        {
            Invertir();
        }

        private void Invertir()
        {
            int i;

            nRectangles = (int)iudNRectangles.Value;
            midaQuadrat = cnvPaper.Width / nRectangles;
            List<int> aux = new List<int>();
            numeros = new List<int>();
            rectangles = new List<Rectangle>();
            cnvPaper.Children.Clear();
            Rectangle rect;
            for (i = nRectangles - 1; i >= 0; i--)
            {
                numeros.Add((i + 1));
            }


            esquerraActual = 1;
            for (i = 0; i < numeros.Count; i++)
            {
                rect = new Rectangle();
                rect.Width = midaQuadrat;

                rect.Height = numeros[i] * cnvPaper.Height / nRectangles;

                ColorRectangle(rect, i);

                cnvPaper.Children.Add(rect);

                rect.SetValue(Canvas.ZIndexProperty, 1);
                rect.SetValue(Canvas.LeftProperty, esquerraActual);
                Canvas.SetBottom(rect, 0);
                esquerraActual += midaQuadrat;
                rectangles.Add(rect);


            }

            if (!esRectangle)
                CanviarAElipses();

            btnOrdena.IsEnabled = true;


        }

        private void Espera(double segons)
        {
            var frame = new DispatcherFrame();
            new Thread((ThreadStart)(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(segons));
                frame.Continue = false;
            })).Start();
            Dispatcher.PushFrame(frame);
        }

        private void chkAnimacio_Checked(object sender, RoutedEventArgs e)
        {
            animacio = true;
        }

        private void chkAnimacio_Unchecked(object sender, RoutedEventArgs e)
        {
            animacio = false;
        }




    }



}
