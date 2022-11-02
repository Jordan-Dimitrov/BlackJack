using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace BlackJack
{
    static class ExtensionsClass
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int dealerSum = 0;
            int yourSum = 0;
            int dealerAceCount = 0;
            int yourAceCount = 0;
            string hidden = string.Empty;
            bool canHit = true;
            int bet = 0;
            List<string> deck = new List<string>();
            BuildDeck(deck);
            Shuffle(deck);
            StartGame(deck,dealerSum,hidden, dealerAceCount, yourSum, yourAceCount, canHit,bet);
        }
        static void BuildDeck(List<string> deck)
        {
            string[] types = { "♣", "♦", "♥", "♠" };
            string[] cards = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            for (int i = 0; i < types.Length; i++)
            {
                for (int j = 0; j < cards.Length; j++)
                {
                    deck.Add(cards[j] + "-" + types[i]);
                }
            }
        }
        static void Shuffle(List<string> deck)
        {
            deck.Shuffle();
        }
        static void StartGame(List<string> deck, int dealerSum, string hidden, int dealerAceCount, int yourSum, int yourAceCount, bool canHit, int bet)
        {
            Console.WriteLine("Enter a bet");
            while (true)
            {
                try
                {
                    bet = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Enter a num!");
                }
                if (bet <= 0)
                {
                    Console.WriteLine("Enter more than 0!");
                }
                else
                {
                    break;
                }
            }
            hidden = deck.Last();
            deck.RemoveAt(deck.IndexOf(hidden));
            dealerSum += GetValue(hidden);
            dealerAceCount += CheckAce(hidden);
            while(dealerSum < 17)
            {
                string card = deck.Last();
                deck.RemoveAt(deck.IndexOf(card));
                dealerSum += GetValue(card);
                Console.WriteLine($"Dealer's cards - {card}");
            }
            for (int i = 0; i < 2; i++)
            {
                string card = deck.Last();
                deck.RemoveAt(deck.IndexOf(card));
                yourSum += GetValue(card);
                yourAceCount += CheckAce(card);
                Console.WriteLine("Your cards - " + card);
            }
            Console.WriteLine("H - for hit and S - for stay");
            while (true)
            {
                string command = Console.ReadLine();
                if (command=="H")
                {
                    Hit(canHit, deck, hidden, yourSum, yourAceCount);
                }
                if (command=="S")
                {
                    Stay(canHit, deck, hidden, yourSum, yourAceCount, dealerSum, dealerAceCount,bet);
                }
            }
        }
        static int GetValue(string hidden)
        {
            string[] data = hidden.Split("-");
            string value = data[0];

            if (value!="2"&&value!="3"&&value!="4"&& value != "5" && value != "6" && value != "7"&& value != "8" && value != "9" && value != "10")
            {
                if (value=="A")
                {
                    return 11;
                }
                return 10;
            }
            else
            {
                return int.Parse(value);
            }
        }
        static int CheckAce(string hidden)
        {
            if (hidden[0].ToString()=="A")
            {
                return 1;
            }
            return 0;
        }
        static void Hit(bool canHit, List<string> deck, string hidden, int yourSum, int yourAceCount)
        {
            if (!canHit)
            {
                return;
            }
            string card = deck.Last();
            deck.RemoveAt(deck.IndexOf(card));
            yourSum += GetValue(card);
            yourAceCount += CheckAce(card);
            Console.WriteLine("Your cards - " + card);
            if (reduceAce(yourSum, yourAceCount)> 21)
            {
                canHit = false;
            }
        }
        static void Stay(bool canHit, List<string> deck, string hidden, int yourSum, int yourAceCount, int dealerSum, int dealerAceCount, int bet)
        {
            dealerSum = reduceAce(dealerSum, dealerAceCount);
            yourSum = reduceAce(yourSum, yourAceCount);
            canHit = false;
            Console.WriteLine("Dealer's card - " + hidden);
            string message = "";
            if (yourSum > 21)
            {
                message = $"You Lose! -{bet} credits!";
            }
            else if (dealerSum > 21)
            {
                message = $"You win! +{bet*2}";
            }
            else if (yourSum == dealerSum)
            {
                message = "Tie! Nothing lost";
            }
            else if (yourSum > dealerSum)
            {
                double bett = bet * 2.5;
                message = $"You win! +{bett} credits!";
            }
            else if (yourSum < dealerSum)
            {
                message = $"You Lose! -{bet} credits!";
            }
            Console.WriteLine("dealer sum " + dealerSum);
            Console.WriteLine("your sum " + yourSum);
            Console.WriteLine("results " + message);
        }
        static int reduceAce(int yourSum, int yourAceCount)
        {
            while (yourSum > 21 && yourAceCount > 0)
            {
                yourSum -= 10;
                yourAceCount -= 1;
            }
            return yourSum;
        }
    }
}