using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    static class Program
    {
        static bool isSuspended = true;
 
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {

            var cts = new CancellationTokenSource();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Task.WhenAny(
                Task.Run(() => EventLoop(cts))
                );

            Application.Run(new Form1());

        }


        // キー受付のループ
        static void EventLoop(CancellationTokenSource cts)
        {
            while (!cts.IsCancellationRequested)
            {
                // 文字を読み込む
                // (「キーが押される」というイベントの発生を待つ)
                string line = Console.ReadLine();
                if(line != null) { 
                char eventCode = line.Length == 0 ? '\0' : line[0];

                // イベント処理
                switch (eventCode)
                {
                    case '\0':
                        isSuspended = true;
                        MessageBox.Show("w");
                        break;
                    case 's': // suspend
                        isSuspended = true;
                        break;
                }
                }
            }
        }

    }
}
