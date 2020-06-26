using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackJack_v3
{
    public partial class BlackJack : Form
    {
        public static DrawnCard drawnCardList;
        private PictureBox pictureBox;
        private List<PictureBox>[] playerPictureBoxes = new List<PictureBox>[2];
        private List<PictureBox> dealerPictureBoxes = new List<PictureBox>();
        private Player player;
        private Player dealer;
        private Player splitPlayer;
        private Player nowPlayer;       //現在どのプレイヤーのターンか
        private FlowLayoutPanel[] flp;      //0…プレイヤー,1…ディーラー
        private bool restart;
        private FlowLayoutPanel[] sflp = new FlowLayoutPanel[2];
        private bool splitFlag;             //スプリットしたかどうかのフラグ
        private bool titleFlag = true;
        private bool titleLaVisibleFlag = true;
        
        public BlackJack()
        {
            InitializeComponent();
            
        }

        public void Split()
        {
            if(player.GetCards()[0].Number == player.GetCards()[1].Number)
            {
                SplitButton.Enabled = true;
            }
        }

        private void BlackJack_Load(object sender, EventArgs e)
        {

            SplitLabel1.Enabled = false;
            SplitLabel2.Enabled = false;
            //プレイヤーの作成
            player = new Player();
            player.Number = 1;
            //ディーラーの作成
            dealer = new Player();
            dealer.Number = 0;
            //スプリットしたとき用のプレイヤー
            splitPlayer = new Player();
            splitPlayer.Number = 2;

            //フローレイアウトパネルの設定
            flp = new FlowLayoutPanel[2];

            for (int i = 0; i < flp.Length; i++)
            {
                flp[i] = new FlowLayoutPanel();
            }

            flp[0].Dock = DockStyle.Left;
            flp[1].Dock = DockStyle.Top;
            flp[1].FlowDirection = FlowDirection.RightToLeft;

            flp[0].Size = new Size(this.ClientSize.Width, 140);
            flp[1].Size = new Size(this.ClientSize.Width, 140);

            tableLayoutPanel1.Controls.Add(flp[0], 1, 6);
            tableLayoutPanel1.Controls.Add(flp[1], 1, 1);

            ////スプリット用の画面配置にする
            //for (int i = 0; i < 2; i++)
            //{
            //    sflp[i] = new FlowLayoutPanel();

            //    //flp[0].Controls.Add(sflp[i]);
            //    sflp[i].Parent = flp[0];
            //}

            //sflp[0].Size = new Size(this.ClientSize.Width / 2 - 120, 300);
            //sflp[1].Size = new Size(this.ClientSize.Width / 2 - 100, 300);

            for (int i = 0; i < 2; i++)
            {
                playerPictureBoxes[i] = new List<PictureBox>();
            }


            //引いたカードリストの作成
            drawnCardList = new DrawnCard();

            //プレイヤーの初期化
            for (int i = 0; i < 2; i++)
            {
                player.DrawCard(drawnCardList);
            }

            label1.Text = player.Total().ToString();
            //プレイヤーのカードの描画
            PlayerDraw();

            //ディーラーの初期化
            for (int i = 0; i < 2; i++)
            {
                dealer.DrawCard(drawnCardList);
            }
            //ディーラーのカードの描画
            DealerDraw(true);

            SplitButton.Enabled = false;

            Split();

        }

        //プレイヤーのカードの描画
        public void PlayerDraw()
        {
            //ピクチャボックスを削除
            foreach (PictureBox pictureBox in playerPictureBoxes[0])
            {
                flp[0].Controls.Remove(pictureBox);
            }

            //リストをクリア
            playerPictureBoxes[0].Clear();

            //持っているカード数ピクチャボックスを作成
            foreach (Card card in player.GetCards())
            {
                //ピクチャボックスの作成
                pictureBox = new PictureBox();
                //画像の読み込み
                Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("card_" + card.Type + card.Number, Properties.Resources.Culture);
                pictureBox.Image = bitmap;

                pictureBox.Size = new Size(70, 140);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Anchor = AnchorStyles.Right;
                pictureBox.Parent = flp[0];

                playerPictureBoxes[0].Add(pictureBox);
            }
        }


        //スプリットした時の描画
        public void PlayerSplitDraw(Player player)
        {
            int playerNum = player.Number - 1;
            //ピクチャボックスを削除
            foreach (PictureBox pictureBox in playerPictureBoxes[playerNum])
            {
                sflp[playerNum].Controls.Remove(pictureBox);
            }

            //リストをクリア
            playerPictureBoxes[playerNum].Clear();

            //持っているカード数ピクチャボックスを作成
            foreach (Card card in player.GetCards())
            {
                //ピクチャボックスの作成
                pictureBox = new PictureBox();
                //画像の読み込み
                Bitmap bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("card_" + card.Type + card.Number, Properties.Resources.Culture);
                pictureBox.Image = bitmap;

                pictureBox.Size = new Size(70, 140);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Anchor = AnchorStyles.Right;
                pictureBox.Parent = sflp[playerNum];

                playerPictureBoxes[playerNum].Add(pictureBox);
            }
        }

        //ディーラーのカードの描画
        public void DealerDraw(bool flag)
        {
            //ピクチャボックスを削除
            foreach (PictureBox pictureBox in dealerPictureBoxes)
            {
                flp[1].Controls.Remove(pictureBox);
            }

            //リストをクリア
            dealerPictureBoxes.Clear();

            int i = 0;

            //持っているカード数ピクチャボックスを作成
            foreach (Card card in dealer.GetCards())
            {
                //ピクチャボックスの作成
                pictureBox = new PictureBox();
                Bitmap bitmap = null;

                //画像の読み込み
                bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("card_" + card.Type + card.Number, Properties.Resources.Culture);
                if(flag)
                {
                    if (i == 1)
                    {
                        bitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("card_00", Properties.Resources.Culture);
                    }
                }
                pictureBox.Image = bitmap;

                pictureBox.Size = new Size(70, 140);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Anchor = AnchorStyles.Right;
                pictureBox.Parent = flp[1];

                dealerPictureBoxes.Add(pictureBox);
                i++;
            }
        }

        public DrawnCard GetDrawnCard()
        {
            return drawnCardList;
        }

        //「ヒット」のボタンを押したとき
        private void DrawButton_Click_1(object sender, EventArgs e)
        {
            SplitButton.Enabled = false;
            if (restart == false)
            {
                if(!splitFlag)
                {
                    //カードを引く
                    player.DrawCard(drawnCardList);
                    //描画
                    PlayerDraw();
                    label1.Text = player.Total().ToString();
                    //合計がバーストしてないか
                    if (player.GetCalculation().Burst(player.Total()) == true)
                    {
                        label1.Text = "負け";
                        restart = true;
                        DrawButton.Text = "もう一度";
                        StandButton.Text = "終了";
                    }
                }
                else
                {
                    SplitDraw(nowPlayer);
                }
            }
            else
            {
                Reset();
            }
        }

        private void StandButton_Click(object sender, EventArgs e)
        {
            SplitButton.Enabled = false;
            if (!splitFlag)
            {
                int total = dealer.Total();
                if (restart == false)
                {
                    while (total <= 16)
                    {
                        dealer.DrawCard(drawnCardList);
                        DealerDraw(false);
                        total = dealer.Total();
                    }
                    if (dealer.GetCalculation().Burst(dealer.Total()) == true)
                    {
                        label1.Text = "勝ち";
                        restart = true;
                        DrawButton.Text = "もう一度";
                    }
                    else
                    {
                        label1.Text = player.GetCalculation().Km(player.Total(), total);
                    }
                    //ディーラーのカードの描画
                    DealerDraw(false);

                    restart = true;
                    DrawButton.Text = "もう一度";
                    StandButton.Text = "終了";
                }
                else
                {
                    Application.Exit();
                }
            }
            else
            {
                //スプリットの時のスタンド
                SplitStand();
            }
        }

        public void Reset()
        {
            if(!splitFlag)
            {
                //ピクチャボックスを削除
                foreach (PictureBox pictureBox in playerPictureBoxes[0])
                {
                    flp[0].Controls.Remove(pictureBox);
                }

                //リストをクリア
                playerPictureBoxes[0].Clear();
                //手札をリセット
                player.Reset();
            }
            else
            {
                SplitReset();
            }

            //ディーラーのリセット
            //ピクチャボックスを削除
            foreach (PictureBox pictureBox in dealerPictureBoxes)
            {
                flp[1].Controls.Remove(pictureBox);
            }
            dealerPictureBoxes.Clear();
            dealer.Reset();

            //引いたカードのリセット
            drawnCardList.Reset();

            DrawButton.Text = "ヒット";
            StandButton.Text = "スタンド";
            restart = false;

            //プレイヤーの初期化
            for (int i = 0; i < 2; i++)
            {
                player.DrawCard(drawnCardList);
            }

            label1.Text = player.Total().ToString();
            //プレイヤーのカードの描画
            PlayerDraw();

            //ディーラーの初期化
            for (int i = 0; i < 2; i++)
            {
                dealer.DrawCard(drawnCardList);
            }
            //ディーラーのカードの描画
            DealerDraw(true);

            Split();
        }

        private void SplitButton_Click(object sender, EventArgs e)
        {
            splitFlag = true;
            SplitButton.Enabled = false;

            SplitLabel1.Enabled = true;
            SplitLabel1.Text = "";
            SplitLabel2.Enabled = true;
            SplitLabel2.Text = "";

            //手札を1枚スプリットに渡す
            splitPlayer.GetCards().Add(player.GetCards()[1]);

            //手札を1枚リストから取り除く
            player.SplitDeleteCards();

            //ピクチャボックスを削除
            foreach (PictureBox pictureBox in playerPictureBoxes[0])
            {
                flp[0].Controls.Remove(pictureBox);
            }

            //スプリット用の画面配置にする
            for (int i = 0; i < 2; i++)
            {
                sflp[i] = new FlowLayoutPanel();

                flp[0].Controls.Add(sflp[i]);
                //sflp[i].Parent = flp[0];
            }

            sflp[0].Size = new Size(this.ClientSize.Width / 2 - 120, 300);
            sflp[1].Size = new Size(this.ClientSize.Width / 2 - 100, 300);

            //描画
            PlayerSplitDraw(player);
            PlayerSplitDraw(splitPlayer);


            //プレイヤーのターン
            nowPlayer = player;
            SplitLabel1.Text = "プレイヤー１のターン";
        }


        //スプリットした時のカードを引く
        private void SplitDraw(Player drawPlayer)
        {
            //カードを引く
            drawPlayer.DrawCard(drawnCardList);
            //描画
            PlayerSplitDraw(drawPlayer);
            label1.Text = drawPlayer.Total().ToString();
            //合計がバーストしてないか
            if (drawPlayer.GetCalculation().Burst(drawPlayer.Total()) == true)
            {
                if(drawPlayer == player)
                {
                    SplitLabel1.Text = "負け";
                    SplitLabel2.Text = "プレイヤー2のターン";
                    nowPlayer = splitPlayer;
                }
                else if(drawPlayer == splitPlayer)
                {
                    SplitLabel2.Text = "負け";
                    SplitStand();
                }
                //restart = true;
                //DrawButton.Text = "もう一度";
                //StandButton.Text = "終了";
            }
        }


        //スプリットした時のスタンド
        private void SplitStand()
        {
            if(nowPlayer == player)
            {
                nowPlayer = splitPlayer;
                SplitLabel1.Text = "";
                SplitLabel2.Text = "プレイヤー2のターン";
            }
            else if(nowPlayer == splitPlayer)
            {
                int total = dealer.Total();
                if (restart == false)
                {
                    while (total <= 16)
                    {
                        dealer.DrawCard(drawnCardList);
                        DealerDraw(false);
                        total = dealer.Total();
                    }
                    //プレイヤー1がバーストしてないか
                    if(SplitLabel1.Text != "負け")
                    {
                        //ディーラーがバーストしてないか
                        if (dealer.GetCalculation().Burst(dealer.Total()) == true)
                        {
                            SplitLabel1.Text = "勝ち";
                        }
                        else
                        {
                            SplitLabel1.Text = player.GetCalculation().Km(player.Total(), total);
                        }
                    }

                    //プレイヤー2がバーストしてないか
                    if (SplitLabel2.Text != "負け")
                    {
                        //ディーラーがバーストしてないか
                        if (dealer.GetCalculation().Burst(dealer.Total()) == true)
                        {
                            SplitLabel2.Text = "勝ち";
                        }
                        else
                        {
                            SplitLabel2.Text = splitPlayer.GetCalculation().Km(splitPlayer.Total(), total);
                        }
                    }

                    //ディーラーのカードの描画
                    DealerDraw(false);

                    restart = true;
                    DrawButton.Text = "もう一度";
                    StandButton.Text = "終了";
                }
                else
                {
                    Application.Exit();
                }
            }
            
        }


        //スプリットした時のリセット
        public void SplitReset()
        {

            //ピクチャボックスを削除
            foreach (PictureBox pictureBox in playerPictureBoxes[0])
            {
                sflp[0].Controls.Remove(pictureBox);
            }

            //フローレイアウトパネルを削除
            for(int i = 1; i > -1; i--)
            {
                flp[0].Controls.Remove(sflp[i]);
            }
            
            //リストを削除
            for(int i = 0;i < 2; i++)
            {
                playerPictureBoxes[i].Clear();
            }

            //手札をリセット
            player.Reset();
            splitPlayer.Reset();

            nowPlayer = player;

            //スプリット終了
            splitFlag = false;
        }

        private void tableLayoutPanel2_Click(object sender, EventArgs e)
        {
            if (titleFlag)
            {
                tableLayoutPanel1.Visible = true;
                tableLayoutPanel2.Visible = false;

                titleFlag = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(titleFlag)
            {
                //tableLayoutPanel2.SuspendLayout();
                titleLaVisibleFlag = !titleLaVisibleFlag;
                ClickLabel.Visible = titleLaVisibleFlag;
                //tableLayoutPanel2.ResumeLayout();
            }

            
        }
    }
}
