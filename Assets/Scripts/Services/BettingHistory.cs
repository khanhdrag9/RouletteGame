using System.Collections.Generic;

namespace Game
{
    public class BettingResult
    {
        public int Number;
        public string Color;
    }

    public class BettingHistory
    {
        private Stack<BettingResult> history;

        public BettingHistory()
        {
            history = new Stack<BettingResult>();
        }

        public void Add(BettingResult result)
        {
            history.Push(result);
        }

        public BettingResult GetLast()
        {
            if(history.Count > 0)
                return history.Peek();
            
            return null;
        }
    }
}