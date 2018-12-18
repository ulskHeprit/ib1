using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ib
{
    public partial class Form1 : Form
    {
        Random rand;
        string str;
        string filename;
        int test1;

        public Form1()
        {
            InitializeComponent();
            rand = new Random();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= Convert.ToInt32(maskedTextBox1.Text); ++i)
            {
                if (backgroundWorker1.CancellationPending)//обработка отмены функции
                {
                    e.Cancel = true;
                    return;
                }
                str += rand.Next(0, 2);//рандом 0 или 1
                if (i % 10000 == 0)
                { 
                    backgroundWorker1.ReportProgress(i);//отправка прогресса в progresschanched
                }
            }

            e.Result = str; // будет передано в RunWorkerComрleted
        }

        public void asd()
        {
            
        }

        private void bw_RunWorkerCompleted(object sender,
    RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                MessageBox.Show("Работа генератора была отменена пользователем!");
            else if (e.Error != null)
                MessageBox.Show("Exception: " + e.Error);
            else
            {
                MessageBox.Show("Генерация выполнена успешно.");
                str = "";
                textBox1.Text = e.Result.ToString();//выводим результат в текстбокс и разблокируем элементы формы
                label2.Visible = false;
                progressBar1.Visible = false;
                generation_button.Enabled = true;
                maskedTextBox1.Enabled = true;
                save_button.Enabled = true;
                load_button.Enabled = true;
                clear_button.Enabled = true;
                test1_button.Enabled = true;
            }
        }

        private void bw_ProgressChanged(object sender,
          ProgressChangedEventArgs e)
        {            
            progressBar1.Value = e.ProgressPercentage*100/Convert.ToInt32(maskedTextBox1.Text);//вывод прогресса в процентах
            label2.Text = progressBar1.Value+"%";
        }


        private void generation_button_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text!="")
            {
                str = "";
                backgroundWorker1.RunWorkerAsync();//запускаем функцию и блокируем элементы формы
                label2.Visible = true;
                progressBar1.Visible = true;
                generation_button.Enabled = false;
                maskedTextBox1.Enabled = false;
                save_button.Enabled = false;
                load_button.Enabled = false;
                clear_button.Enabled = false;
                test1_button.Enabled = false;
            }
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)//сохранение в файл
                return;
            filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            System.IO.File.WriteAllText(filename, textBox1.Text);
            MessageBox.Show("Файл сохранен");
        }

        private void load_button_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)//загрузка из файла
                return;
            // получаем выбранный файл
            filename = openFileDialog1.FileName;
            // читаем файл в строку
            string fileText = System.IO.File.ReadAllText(filename);
            textBox1.Text = fileText;
            MessageBox.Show("Загружено.");
        }

        private void clear_button_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";//очистка текстбокса
        }

        private void button1_Click(object sender, EventArgs e)//кнопка отмена
        {
            backgroundWorker1.CancelAsync();//отмена функции
            label2.Visible = false;
            progressBar1.Visible = false;
            generation_button.Enabled = true;
            maskedTextBox1.Enabled = true;
            save_button.Enabled = true;
            load_button.Enabled = true;
            clear_button.Enabled = true;
            test1_button.Enabled = true;
        }

        private void test1_button_Click(object sender, EventArgs e)
        {
            if(textBox1.Text!="")//частотный тест
            {
                test1 = 0;
                str = textBox1.Text;
                foreach (char ch in str)
                {
                    if(ch=='0')
                    {
                        test1 -= 1;
                    }
                    else
                    {
                            test1 += 1;
                    }
                }
                MessageBox.Show("Кол-во: "+str.Length+" Сумма: "+test1.ToString()+" Статистика (S): "+(Math.Abs(test1)/Math.Sqrt(str.Length)).ToString() + (Math.Abs(test1) / Math.Sqrt(str.Length)<= 1.82138636?" Последовательность случайна.":" Последовательность не случайна."));
            }
            else
            {
                MessageBox.Show("Нечего проверять.");
            }
        }
    }
}
