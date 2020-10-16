using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public class Word {
            public string w { get; set; }
            public double cnt { get; set; }
            public Word(char[] a)
            {
                w = new string(a);
                w = vid(w);
                cnt = 1;
            }
            public Word(string a)
            {
                w = a;
                cnt = 1;
            }

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void fileSystemWatcher2_Changed(object sender, System.IO.FileSystemEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
                Create_poem("Невідомий вірш Лесі Українки", @"c:\MyDir\c");
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void Create_base(string text,string name_base)
        {
            
            text += '\0';
            DirectoryInfo di = Directory.CreateDirectory(name_base);
            Word[] mas = new Word[10000];
            int cap = 0;
            mas[cap++] = new Word(word(text, 0));
            int find = -1;
            uint cnt_word = 1;
            for (int i = 0; text[i] != '\0'; i++)
            {
                if ((text[i] == '\n'||text[i]=='\r') && (text[i+1] == '\n' || text[i+1] == '\r') && (text[i+2] == '\n' || text[i+2] == '\r')&& (text[i+3] == '\n' || text[i+3] == '\r') && (text[i + 4] == '\n' || text[i + 4] == '\r') && (text[i + 5] == '\n' || text[i + 5] == '\r'))
                {
                    find = -1;
                    for (; text[i] == '\n'|| text[i] == '\r'; i++) ;
                    if (text[i] == '\0')
                        break;
                    if ((find = find_in_word(word(text, i), mas, cap)) == -1) mas[cap++] = new Word(word(text, i)); 
                    else mas[find].cnt++;
                    cnt_word ++;
                }   
            }
            StreamWriter sw = new StreamWriter(name_base + "\\" + 0 +".txt", true, System.Text.Encoding.Default);
            for (int i = 0; i < cap; i++)
            {
                mas[i].cnt /= (double)cnt_word;
                mas[i].cnt *= 100;
                string viv = vid(mas[i].w) +" "+  mas[i].cnt;
                sw.WriteLine(viv);
            }
            sw.Close();
            string file_word;
            cap = 0;
            for (int i = 0;i<text.Length&& text[i] != '\0'; i++)
            {
                for (; !not_znak(text[i]) && text[i] != '\0'; i++) ;
                file_word = new string((word(text, i)));
                file_word = vid(file_word);
                i += file_word.Length;
                if (i > text.Length)
                    break;
                cnt_word = 0;
                cap = 0;
                string file_word_now = file_word;
                if (!File.Exists(name_base + "\\" + file_word + ".txt"))
                {
                    for (int j = i;j<text.Length&& text[j] != '\0'; j++)
                    {
                        find = -1;
                        if (not_znak(text[j]))
                        {
                            if (file_word_now == file_word)
                            {
                                if (text[j] == '\0')
                                    break;
                                if ((find = find_in_word(word(text, j), mas, cap)) == -1) mas[cap++] = new Word(word(text, j));
                                else mas[find].cnt++;
                                cnt_word++;
                            }
                            file_word_now = new string(word(text, j)); file_word_now = vid(file_word_now);
                            j += file_word_now.Length;
                        }
                    }
                    sw = new StreamWriter(name_base + "\\" + (file_word) + ".txt", true, System.Text.Encoding.Default);
                    for (int j = 0; j < cap; j++)
                    {
                        string viv = vid(mas[j].w) + " " + ((mas[j].cnt * 100) / cnt_word);
                        sw.WriteLine(viv);
                    }
                    sw.Close();

                }
            }
        }
        public bool not_znak(char a)
        {
            if (a != ' ' && a != ',' && a != '.' && a != '!' && a != '?' && a != '\"' && a != '\'' && a != '\0' && a != '-' && a != '`'&&a!='\n'&&a!='\r'&&a!=';'&&a!=':'&&a!= '—'&&a!='«'&&a!= '…'&&a!= '»'&&a!= '–'&&a!= '„' && a != '*' && a != '#' && a != '(' && a != ')' && a != '[' && a != ']' && a != '/' && a != '\\')
                return true;
            return false;
        }
        public char[] word(string mas, int a) {
            char[] word1 = new char[255];
            int i;
            for (i = 0; not_znak(mas[a]); i++)
            {
                if ((mas[a] >= 1040 && mas[a] <= 1071) || mas[a] == 'Ё' || mas[a] == 'Є' || mas[a] == 'Ї' || mas[a] == 'І') {
                    if (mas[a] == 'Ё')
                    {
                        word1[i] = 'ё';
                        a++;
                        continue;
                    }
                    if (mas[a] == 'Ї')
                    {
                        word1[i] = 'ї';
                        a++;
                        continue;
                    }
                    if (mas[a] == 'I')
                    {
                        word1[i] = 'і';
                        a++;
                        continue;
                    }
                    if (mas[a] == 'Є')
                    {
                        word1[i] = 'є';
                        a++;
                        continue;
                    }
                    word1[i] = Convert.ToChar(mas[a] + 32);
                    a++;
                }
                else {
                    word1[i] = mas[a++];
                }
            }
            for (;!not_znak(mas[a])&&mas[a]!='\0' ;a++)
            {
                if (mas[a] == '\n')
                {
                    word1[i] = '~';
                    break;
                }
            }            
            return word1;
        }
        public int find_in_word(char[] a,Word [] w,int cap)
        {
            string b = new string(a);
            b = vid(b);
            int c = -1;
            for(int i = 0; i < cap; i++)
            {
                if (w[i].w == b)
                    return i;
            }
            return c;
        }
        static public string vid(string a )
        {
            string b="";
            for(int i = 0; i < a.Length&&a[i]!='\0';i++)
            {
                b += a[i];
            }
            return b;
        }
        public void Create_poem(string name_poem,string name_base)
        {
            bool up = true;
            label1.Text = name_poem;
            StreamReader sr;
            if (File.Exists(name_base + "\\" + 0 + ".txt"))
                sr = new StreamReader(name_base + "\\" + 0 + ".txt", System.Text.Encoding.Default);               
            else {
                richTextBox1.Text = "Технічні неполадки з базой данних";
                return;
            }
            string text = "", temp_text ="" ;
            Random rand = new Random();
            double temp, ver = 0; ;
            bool flag = true;
            for (int i = 0;i<96 ;)
            {
                ver = 0;
                temp = Convert.ToDouble(rand.Next(100000)) / 1000;
                while (true)
                {
                    string t = sr.ReadLine();

                    if (t == null) { flag = false;break; }
    
                    string[] line = t.Split(' ');
                    temp_text = line[0];
                    ver += Convert.ToDouble(line[1]);
                    if (ver > temp)
                    {
                        if (up == true)
                        {
                            line[0] = toup(line[0]);
                            up = false;
                        }
                        if (have_s(line[0]))
                        {
                            text += delete_s(line[0]);
                            text += '\n';
                            i++;
                            if(i%4==0)
                                text += '\n';
                            up = true;
                            break;
                        }
                        else
                        {
                            text += line[0] + ' ';
                            up = false;
                            break;
                        }
                        
                    }
                }
                if (flag == true)
                {
                    if (File.Exists(name_base + "\\" + temp_text + ".txt"))
                        sr = new StreamReader(name_base + "\\" + temp_text + ".txt", System.Text.Encoding.Default);
                    else
                    {
                        richTextBox1.Text = "Технічні неполадки з базой данних";
                        return;
                    }
                }
                else {
                    if (File.Exists(name_base + "\\" + temp_text + ".txt"))
                        sr = new StreamReader(name_base + "\\" + temp_text + ".txt", System.Text.Encoding.Default);
                    else
                    {
                        richTextBox1.Text = "Технічні неполадки з базой данних";
                        return;
                    }
                }

            }
            richTextBox1.Text = text;
        }
        public bool have_s(string a)
        {
            for (int i = 0; i < a.Length; i++)
                if (a[i] == '~')
                    return true;
            return false;
        }
        public string delete_s(string a)
        {
            string b = "";
            for(int i = 0; i < a.Length;i++)
            {
                if (a[i] != '~')
                    b+= a[i];
            }
            return b;
               
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Create_poem("Невідомий вірш Тараса Шевченко", @"c:\MyDir\a");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Create_poem("Невідомий вірш Івана Франка", @"c:\MyDir\b");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            string a = textBox1.Text, b = textBox2.Text; ;
            a = (a == null||Directory.Exists(a)||a=="") ? ("new" + (rand.Next(1000000000))):a;
            b = (b == null || b == "") ? ("Мій вірш"):b;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(openFileDialog1.FileName, System.Text.Encoding.Default);
                Create_base(sr.ReadToEnd(), @"c:\MyDir\"+a);
                sr.Close();
                Create_poem(b, @"c:\MyDir\" + a);
            }
           
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        public string toup(string mas)
        {
            string word1 = "";
                if ((mas[0] >= 1070 && mas[0] <= 1103) || mas[0] == 'ё' || mas[0] == 'є' || mas[0] == 'ї' || mas[0] == 'і')
                {
                    if (mas[0] == 'ё')
                    {
                        word1 += 'Ё';
                    }
                    if (mas[0] == 'ї')
                    {
                        word1+= 'Ї';

                    }
                    if (mas[0] == 'і')
                    {
                        word1+= 'І';
                    }
                    if (mas[0] == 'є')
                    {
                        word1 += 'Є';
                    }
                    word1 += Convert.ToChar(mas[0] - 32);
                }
            for (int i = 1; i < mas.Length; i++)
                word1 +=  mas[i];
            return word1;
        }
   }
    
}
