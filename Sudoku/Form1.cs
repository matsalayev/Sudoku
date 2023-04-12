using System.Runtime.InteropServices;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        Button[,] btn = new Button[9, 9];
        int[,] map = new int[9, 9];
        int[,] value = new int[9, 9];
        Button[] butt = new Button[9];
        Label[] lbl = new Label[9];
        List<int> remove = new List<int>();
        Random ran = new Random();
        public Form1()
        {
            InitializeComponent();
            this.KeyUp += new KeyEventHandler(keyFunc);
            timer1.Start();
            cmap();
        }
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        void cmap()
        {
            map = new int[9, 9]
            {
                {0,0,0,1,1,1,0,0,0},
                {0,0,0,1,1,1,0,0,0},
                {0,0,0,1,1,1,0,0,0},
                {1,1,1,0,0,0,1,1,1},
                {1,1,1,0,0,0,1,1,1},
                {1,1,1,0,0,0,1,1,1},
                {0,0,0,1,1,1,0,0,0},
                {0,0,0,1,1,1,0,0,0},
                {0,0,0,1,1,1,0,0,0}
            };
            Create();
            label2.Text = "00:00";
            time = 0;
            timer1.Start();
            removeBtn();
        }
        void Create()
        {
            int n = ran.Next(0, 9);
            int N = 1;
            int x = 1;
            panel1.Controls.Clear();
            try
            {
                for (int i = 0; i < 9; i++)
                {
                    this.Controls.Remove(butt[i]);
                }
            }
            catch { }
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    btn[a, b] = new Button();
                    btn[a, b].Text = ((a * 3 + a / 3 + b + n) % 9 + 1).ToString();
                    btn[a, b].Name = x.ToString(); x++;
                    value[a, b] = (a * 3 + a / 3 + b + n) % 9 + 1;
                    btn[a, b].FlatStyle = FlatStyle.Flat;
                    if (map[a, b] == 0) btn[a, b].BackColor = Color.FromArgb(50, 50, 60);
                    else btn[a, b].BackColor = Color.FromArgb(40, 40, 50);
                    btn[a, b].FlatAppearance.BorderSize = 0;
                    btn[a, b].Size = new Size(50, 50);
                    btn[a, b].Location = new Point(50 * a, 50 * b);
                    btn[a, b].Click += new EventHandler(btn_Click);
                    panel1.Controls.Add(btn[a, b]);
                }
            }
            remove.Clear();
            while (N < 40)
            {
                for (int a = 0; a < 9; a++)
                {
                    for (int b = 0; b < 9; b++)
                    {
                        if (value[a, b] == ran.Next(1, 10) && btn[a, b].Text != "")
                        {
                            remove.Add(int.Parse(btn[a, b].Text));
                            btn[a, b].Text = "";
                            N++;
                        }
                    }
                }
            }
            for (int i = 0; i < 9; i++)
            {

                for (int j = 0; j < 9; j++)
                {
                    if (remove[i] > remove[j])
                    {
                        int d = remove[i];
                        remove[i] = remove[j];
                        remove[j] = d;
                    }
                }
            }
            int l = 65;
            for (int i = 0; i < 9; i++)
            {
                butt[i] = new Button();
                butt[i].FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                butt[i].Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                butt[i].Location = new System.Drawing.Point(50 * i + 27, 510);
                butt[i].Size = new System.Drawing.Size(45, 45);
                butt[i].Name = ((char)(l)).ToString(); l++;
                butt[i].Click += new EventHandler(btn_Click);
                butt[i].Text = (i + 1).ToString();
                this.Controls.Add(lbl[i]);
                this.Controls.Add(butt[i]);
            }
            bgcolor();
        }
        void bgcolor()
        {
            for (int i = 0; i < 9; i++)
            { butt[i].BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(60))))); }
        }
        void removeBtn()
        {
            label1.Text = "";
            for (int a = 1; a < 10; a++)
            {
                int son = 0;
                for (int i = 0; i < remove.Count; i++)
                {
                    if (remove[i] == a) son++;
                }
                label1.Text += son + "        ";
            }
        }
        int x = 0;
        int h = 0;
        private void btn_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            for (int i = 0; i < 9; i++)
            {
                if (button.Name == butt[i].Name)
                {
                    bgcolor();
                    x = int.Parse(button.Text);
                    button.BackColor = Color.Green;
                }

            }
            if (button.Text == "")
            {
                for (int a = 0; a < 9; a++)
                {
                    for (int b = 0; b < 9; b++)
                    {
                        if (button.Name == btn[a, b].Name)
                        {
                            if (value[a, b] == x)
                            {
                                button.Text = x.ToString();
                                remove.Remove(value[a, b]);
                                butt[x - 1].BackColor = Color.FromArgb(40, 40, 60);
                            }
                            else
                            {
                                try
                                {
                                    butt[x - 1].BackColor = Color.Red;
                                    if (h < 3) {
                                        h++;
                                            label3.Text = h + "/3"; }
                                    
                                    if (h == 3)
                                    {
                                        label3.ForeColor = Color.Red;
                                        label3.Text = "Mag'lubiyat!";
                                        for (int i = 0; i < 9; i++)
                                        {
                                            butt[i].Enabled= false;
                                        }
                                        timer1.Stop();
                                    }
                                }
                                catch { }
                            }
                        }
                        
                    }
                }

            }
            bool f = true;
            for (int a = 0; a < 9; a++)
            {
                for (int b = 0; b < 9; b++)
                {
                    
                    if (btn[a, b].Text == "") f=false;
                }
            }
            if (f)
            {
                label3.ForeColor = Color.Green;
                label3.Text = "G'alaba!";
                timer1.Stop();
            }
            removeBtn();
        }
        private void keyFunc(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.NumPad1:
                    butt[0].BackColor = Color.Green;
                    break;
                case Keys.Delete:
                    Application.Exit();
                    break;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            cmap();
            label3.ForeColor = Color.White;
            label3.Text = "0/3";
            h = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        int time = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            if (time < 10) label2.Text = "00:0" + time;
            else if (time < 60) label2.Text = "00:" + time;
            else
            {
                if (time / 60 < 10 && time % 60 < 10) label2.Text = "0" + time / 60 + ":0" + time % 60;
                else if (time / 60 < 10) label2.Text = "0" + time / 60 + ":" + time % 60;
                else
                {
                    if (time / 60 > 10 && time % 60 < 10) label2.Text =  time / 60 + ":0" + time % 60;
                    else if (time / 60 > 10) label2.Text =  time / 60 + ":" + time % 60;
                }
            }

        }
    }
}