using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVC002.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreMVC002.Controllers
{
    public class GameController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            // 初始化秘密數字
            string secretNumber = GenerateSecretNumber();
            // 傳遞到 View 內部後再回到 Controller
            TempData["secretNumber"] = secretNumber;
            TempData["attemptCount"] = 0; // 初始化猜測次數
            TempData["guessHistory"] = new List<string>(); // 初始化猜測歷史
            // 創建猜測模型: 猜測數字+比對結果+比對邏輯
            var model = new XAXBEngine();
           // {
           //     GuessHistory = new List<string>(),
           //     AttemptCount = 0,
            //    GuessResult = new List<string>() // 初始化 GuessResult
            //};
            // 使用強型別
            return View(model);
        }
        [HttpPost]
        public ActionResult Guess(XAXBEngine model)
        {
            // 檢查 TempData 中的 secretNumber 是否為 null
            if (TempData["secretNumber"] == null)
            {
                // 重新生成秘密數字並設置 TempData
                string secretNumber = GenerateSecretNumber();
                TempData["secretNumber"] = secretNumber;
            }

            // 從 TempData 取得秘密數字
            string secretNumberFromTempData = TempData["secretNumber"] as string;

            // 從 TempData 取得猜測次數並增加
            int attemptCount = (int)(TempData["attemptCount"] ?? 0);
            attemptCount++;
            TempData["attemptCount"] = attemptCount;

            // 獲取猜測結果
            model.Result = GetGuessResult(secretNumberFromTempData, model.Guess);
            model.AttemptCount = attemptCount; // 更新模型中的猜測次數

            // 獲取和更新猜測歷史
            var guessHistory = TempData["guessHistory"] as List<string> ?? new List<string>();
            guessHistory.Add($"{model.Guess} => {model.Result}");
            TempData["guessHistory"] = guessHistory; // 更新 TempData 中的猜測歷史

            // 將歷史記錄傳遞給模型
            model.GuessHistory = guessHistory;

            // 如果猜測正確，顯示重新開始的選項
            if (model.Result == "4A0B")
            {
                ViewBag.GameWon = true;
            }

            return View("Index", model);
        }
        // ------ 遊戲相關之邏輯 ------
        private string GenerateSecretNumber()
        {
            // 生成一個隨機的4位數字作為秘密數字
            // 你可以根據需要自定義生成規則
            Random rand = new Random();
            HashSet<int> digits = new HashSet<int>();
            while (digits.Count < 4)
            {
                int digit = rand.Next(0, 10);
                digits.Add(digit);
            }
            return string.Join("", digits);
        }

        private string GetGuessResult(string secretNumber, string guess)
        {
            // 檢查猜測結果，並返回結果字符串
            int correctPosition = 0; // 正確位置的數字數量
            int correctNumber = 0;   // 正確數字但位置不對的數量
            // 你可以根據遊戲規則自定義檢查邏輯
            // 計算正確位置的數字
            for (int i = 0; i < 4; i++)
            {
                if (secretNumber[i] == guess[i])
                {
                    correctPosition++;
                }
                else if (secretNumber.Contains(guess[i]))
                {
                    correctNumber++;
                }
            }
            return $"{correctPosition}A{correctNumber}B";
        }
    }
}

