using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        //各種値を定数化
        const int WeekCol = 7;       //1週間の曜日数
        const int WeekRow = 6;       //1ヶ月の週数
        const int DayWidth = 60;     //日付の幅
        const int DayHeigth = 60;    //日付の高さ
        const int DayStartX = 50;    //日付の描画開始位置(x座標)
        const int DayStartY = 160;   //日付の描画開始位置(y座標)
        const int DayIntervalX = 60; //日付の描画間隔(x座標)
        const int DayIntervalY = 60; //日付の描画間隔(y座標)

        //カレンダー用の変数達
        private Label[] lblCal;      //カレンダー(日付)のラベル
        private Label[] lblHeadCal;  //カレンダー(曜日)のラベル
        private String[] strDay;     //カレンダー(日付)の文字列
        private DateTime dispDate = DateTime.Now; //現在表示している月
        private DateTime dispDateB;               //前月
        private DateTime dispDateA;               //翌月
        private int dispYYYY;                     //表示年
        private int dispMM;                       //表示月
        private int dispDD;                       //表示日

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Button button1 = new Button();
            button1.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            button1.Location = new System.Drawing.Point(80, 82);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(80, 32);
            button1.TabIndex = 4;
            button1.Text = "前月へ";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new System.EventHandler(button1_Click);
            this.Controls.Add(button1);

            dispDate = DateTime.Now;               //本日をdispDateへ
            dispDateB = dispDate.AddMonths(-1);    //dispDateから1ヶ月マイナス
            dispDateA = dispDate.AddMonths(1);     //dispDateから1ヶ月プラス
            textBox1.Text = dispDateB.ToString("yyyy/MM/dd"); //隠しテキスト
            textBox2.Text = dispDate.ToString("yyyy/MM/dd");  //隠しテキスト
            textBox3.Text = dispDateA.ToString("yyyy/MM/dd"); //隠しテキスト

            //画面上のカレンダー削除
            delCalendar();

            //画面読み込み時の処理
            dispCalendar();

        }

        private void DayClick(object sender, System.EventArgs e)
        {
            //日付がクリックされた時の処理

            //ラベルの文字(日付)を数値に変換
            int sd = int.Parse(((Label)sender).Text);
            //年月日をYYYY/MM/DDにフォーマット
            String strD = String.Format("{0:D4}/{1:D2}/{2:D2}", dispYYYY,dispMM,sd);

            //ステータス用ラベルに通知
            lbl_status.Text = strD+"が押された";

            //このクリックに関しては、必要あれば使う。

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //当月へボタンを押した時の処理
            dispDate = DateTime.Now;              //本日をdispDateへ
            dispDateB = dispDate.AddMonths(-1);   //dispDateから1ヶ月マイナス
            dispDateA = dispDate.AddMonths(1);    //dispDateから1ヶ月プラス
            textBox1.Text = dispDateB.ToString("yyyy/MM/dd"); //隠しテキスト
            textBox2.Text = dispDate.ToString("yyyy/MM/dd");  //隠しテキスト
            textBox3.Text = dispDateA.ToString("yyyy/MM/dd"); //隠しテキスト

            //画面上のカレンダー削除
            delCalendar();

            //画面読み込み時の処理
            dispCalendar();

            //ステータス用ラベルに通知
            lbl_status.Text = "当月ボタンが押された";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dispDate = dispDateB;                         //隠しの前月をdispDateへ
            dispDateB = dispDate.AddMonths(-1);           //dispDateから1ヶ月マイナス
            dispDateA = dispDate.AddMonths(1);            //dispDateから1ヶ月プラス
            textBox1.Text = dispDateB.ToString("yyyy/MM/dd"); //隠しテキスト
            textBox2.Text = dispDate.ToString("yyyy/MM/dd");  //隠しテキスト
            textBox3.Text = dispDateA.ToString("yyyy/MM/dd"); //隠しテキスト
            //画面上のカレンダー削除
            delCalendar();

            //画面読み込み時の処理
            dispCalendar();

            //ステータス用ラベルに通知
            lbl_status.Text = "前月ボタンが押された";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dispDate = dispDateA;                        //隠しの翌月をdispDateへ
            dispDateB = dispDate.AddMonths(-1);          //dispDateから1ヶ月マイナス
            dispDateA = dispDate.AddMonths(1);           //dispDateから1ヶ月プラス
            textBox1.Text = dispDateB.ToString("yyyy/MM/dd"); //隠しテキスト
            textBox2.Text = dispDate.ToString("yyyy/MM/dd");  //隠しテキスト
            textBox3.Text = dispDateA.ToString("yyyy/MM/dd"); //隠しテキスト
            //画面上のカレンダー削除
            delCalendar();
            //画面読み込み時の処理
            dispCalendar();
            //ステータス用ラベルに通知
            lbl_status.Text = "翌月ボタンが押された";
        }
        protected void dispCalendar()
        {
            lblCal = new Label[WeekCol * WeekRow];  //カレンダー用ラベルを生成
            strDay = new String[WeekCol * WeekRow]; //日付用配列を生成

            dispYYYY = dispDate.Year;  //dispDateから年を抽出
            dispMM = dispDate.Month;   //dispDateから月を抽出
            dispDD = dispDate.Day;     //dispDateから日を抽出

            //lbl_dispCal(表示している月の更新)
            lbl_dispCal.Text = String.Format("{0:D4}年{1:D2}月",dispYYYY,dispMM);

            //1日の曜日を取得
            DateTime FirstDay = new DateTime(dispYYYY, dispMM, 1);

            //月の日数を取得
            int lastDay = DateTime.DaysInMonth(dispYYYY, dispMM);
            //日曜日は0

            //日付用配列に日を設定
            int calDay = 1;
            for (int i = 0; i < WeekCol * WeekRow; i++)
            {
                //1日の曜日未満または日付が付きの日数より大きい場合
                if (i < (int)FirstDay.DayOfWeek || calDay > lastDay)
                {
                    strDay[i] = " ";
                }
                else
                {
                    strDay[i] = calDay.ToString();
                    calDay++;
                }
            }

            int intRow = 0;
            int intCol = 0;
            int DispX = DayStartX;
            int DispY = DayStartY;
            int workX;
            int workY;


            lblHeadCal = new Label[WeekCol];
            for(int i = 0; i < WeekCol; i++)
            {
                lblHeadCal[i] = new Label();
            }
            lblHeadCal[0].Text = "日";
            lblHeadCal[1].Text = "月";
            lblHeadCal[2].Text = "火";
            lblHeadCal[3].Text = "水";
            lblHeadCal[4].Text = "木";
            lblHeadCal[5].Text = "金";
            lblHeadCal[6].Text = "土";

            intCol = 1;
            intRow = 1;

            for (int i=0;i<WeekCol;i++)
            {
                lblHeadCal[i].AutoSize = false;
                workX = (intCol * DayIntervalX) - DayIntervalX;
                workY = (intRow * DayIntervalY) - DayIntervalY;
                lblHeadCal[i].Location = new Point(DispX + (workX), DispY + (workY));
                lblHeadCal[i].Name = "lblHead_" + i;
                lblHeadCal[i].Size = new System.Drawing.Size(DayWidth, DayHeigth);
                lblHeadCal[i].TabIndex = i + 1;
                lblHeadCal[i].Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
                switch (i % 7)
                {
                    case 0:
                        lblHeadCal[i].ForeColor = Color.Red;
                        break;
                    case 6:
                        lblHeadCal[i].ForeColor = Color.Blue;
                        break;
                    default:
                        lblHeadCal[i].ForeColor = Color.Black;
                        break;
                }
                this.Controls.Add(lblHeadCal[i]);
                intCol++;

            }

            intCol = 0;

            for (int i = 0; i < WeekCol * WeekRow; i++)
            {
                lblCal[i] = new Label();
                lblCal[i].AutoSize = false;
                lblCal[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                workX = intCol * DayIntervalX;
                workY = intRow * DayIntervalY;
                lblCal[i].Location = new Point(DispX + (workX), DispY + (workY));
                lblCal[i].Name = "lbl_" + i;
                lblCal[i].Size = new System.Drawing.Size(DayWidth, DayHeigth);
                lblCal[i].TabIndex = i + 1;
                lblCal[i].Text = strDay[i];
                lblCal[i].Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                if (strDay[i] != " ")
                {
                lblCal[i].Click += new System.EventHandler(DayClick);
                }

                switch ( i % 7 ) {
                    case 0:
                    lblCal[i].ForeColor = Color.Red;
                        break;
                    case 6:
                        lblCal[i].ForeColor = Color.Blue;
                        break;
                    default:
                        lblCal[i].ForeColor = Color.Black;
                        break;
                }
                if (!(intRow == 6 && strDay[i] == " ")) {
                    this.Controls.Add(lblCal[i]);
                }
                if (i % 7 == 6)
                {
                    intRow++;
                    intCol = 0;
                }
                else
                {
                    intCol++;
                }

            }

        }

        protected void delCalendar()
        {
            for (int i=0; i<WeekCol * WeekRow; i++) {
                this.Controls.RemoveByKey("lbl_" + i);
            }
        }

    }
}
