using BlackJack_v3;
using System;
using System.Collections.Generic;

public class Player
{
    private List<Card> cards = new List<Card>();
    Random random = new Random((int)DateTime.Now.Ticks);

    private int number;

    private Calculation calculation;    //計算
    
    public Player()
    {
        calculation = new Calculation();
        number = 0;
    }

    //カードを引く
    public void DrawCard(DrawnCard drawnCard)
    {
        int type = random.Next(0,4);
        int number = random.Next(1, 3);

        ////すでに引いたカードかチェック
        bool drawn = drawnCard.DrawnCardCheck(type, number);

        //まだ引いてないカードが出るまで引く
        while (drawn)
        {
            type = random.Next(0,4);
            number = random.Next(1, 14);
            drawn = drawnCard.DrawnCardCheck(type, number);
        }
        //カードの作成
        Card card = new Card(type, number);
        //引いたカードを引いたカードリストに登録
        drawnCard.SetDrawnCard(type, number);
        //カードをリストに追加
        cards.Add(card);
       
    }


    public int Total()
    {
        int total = 0;
        bool ace=false;
        foreach (Card card in cards)
        {
            if (ace == false && card.Number == 1)
            {
                ace = true;
            }
            else
            {
                total += calculation.ConvertTo10(card.Number);
            }
        }
        if (total <= 10&&ace==true)
        {
            total = total + 11;
        }
        else if(total >=10&&ace==true)
        {
            total = total + 1;
        }
        return total;
    }

    public void Reset()
    {
        for (int i = cards.Count - 1; i >= 0; i--)
        {

            cards.RemoveAt(i);

        }
    }

    public void SplitDeleteCards()
    {
        cards.RemoveAt(1);
    }

    public List<Card> GetCards()
    {
        return cards;
    }

    public Calculation GetCalculation()
    {
        return calculation;
    }

    public int Number
    {
        set { number = value; }
        get { return number; }
    }

}