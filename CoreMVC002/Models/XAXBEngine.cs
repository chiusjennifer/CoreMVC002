namespace CoreMVC002.Models
{
    public class XAXBEngine
    {
        public string Secret { get; set; }
        public string Guess { get; set; }
        public string Result { get; set; }
        public List<string> GuessHistory { get; set; }  // 猜測歷史
        public int AttemptCount { get; set; }           // 累積猜測次數
        public List<string> GuessResult { get; set; }         // 每次猜測的結果

        public XAXBEngine()
        {
            // 將隨機數字轉成字串並設置為 Secret
            Secret = GenerateRandomSecret();
            Guess = null;
            Result = null;
            GuessHistory = new List<string>();  // 初始化猜測歷史
            AttemptCount = 0;                   // 初始化猜測次數
            GuessResult = new List<string>();   // 初始化猜測結果
        }

        public XAXBEngine(string secretNumber)
        {
            Secret = secretNumber;
            Guess = null;
            Result = null;
            GuessHistory = new List<string>();  // 初始化猜測歷史
            AttemptCount = 0;                   // 初始化猜測次數
            GuessResult = new List<string>();   // 初始化猜測結果
        }
        private string GenerateRandomSecret()
        {
            Random rand = new Random();
            HashSet<int> digits = new HashSet<int>();
            while (digits.Count < 4)
            {
                int digit = rand.Next(0, 10);
                digits.Add(digit);
            }
            return string.Join("", digits);
        }
        // 計算猜測中位置正確且數字正確的個數 (A)
        public int numOfA(string guessNumber)
        {
            int correctPosition = 0;
            for (int i = 0; i < 4; i++)
            {
                if (Secret[i] == guessNumber[i])
                {
                    correctPosition++;
                }
            }
            return correctPosition;
        }
        // 計算猜測中數字正確但位置不正確的個數 (B)
        public int numOfB(string guessNumber)
        {
            int correctNumber = 0;
            HashSet<char> secretSet = new HashSet<char>(Secret);

            foreach (char c in guessNumber)
            {
                if (secretSet.Contains(c) && Secret.IndexOf(c) != guessNumber.IndexOf(c))
                {
                    correctNumber++;
                }
            }
            return correctNumber;
        }
        // 判斷遊戲是否結束（當猜對時）
        public bool IsGameOver(string guessNumber)
        {
            // TODO 3
            return numOfA(guessNumber) == 4;
        }
    }
}
