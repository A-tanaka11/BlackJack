using System.Windows.Forms;

public class DoubleBufferingTLP : TableLayoutPanel
{
    public DoubleBufferingTLP()
    {
        //ダブルバッファリングを有効にする
        this.SetStyle(
           ControlStyles.DoubleBuffer |
           ControlStyles.UserPaint |
           ControlStyles.AllPaintingInWmPaint,
           true);
    }
}