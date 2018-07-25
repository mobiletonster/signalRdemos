using BrainBuffet.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBuffet.Controllers
{
    public class QuestionController: Controller
    {
        private static readonly Random getrandom = new Random();

        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }
        private List<Question> _questions;

        public QuestionController()
        {
            InitializeQuestions();
        }

        [HttpGet("api/questions/random")]
        public IActionResult GetRandomQuestion()
        {
            var count = _questions.Count();
            int next = GetRandomNumber(1, count + 1);
            var question = _questions.FirstOrDefault(m => m.Id == next);
            return Ok(question);
        }


        private void InitializeQuestions()
        {
            var q = new List<Question>();
            int index = 0;
            //q.Add(new Question()
            //{
            //    Id = 1,
            //    QuestionType = "text",
            //    QuestionText = "What does the fox say?",
            //    AnswerText = "Jacha-chacha-chacha-chow!"
            //});
            //q.Add(new Question()
            //{
            //    Id = 2,
            //    QuestionType = "image",
            //    ImageUrl = "http://4.bp.blogspot.com/_Nyiipr-yxiQ/TPTRR8kJWiI/AAAAAAAAOFI/B15zJnI8C_g/s1600/Elvis+Presley+05.jpg",
            //    QuestionText = "Who is this?",
            //    AnswerText = "Elvis Presley"
            //});
            //q.Add(new Question()
            //{
            //    Id = 3,
            //    QuestionType = "text",
            //    QuestionText = "What is the answer to the question 'what is the meaning of life'?",
            //    AnswerText = "42"
            //});
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "image",
                ImageUrl = "https://bpicampus.com/wp-content/uploads/2017/07/LordOfTheRings-627x376.jpg",
                QuestionText = "What is this?",
                AnswerText = "The one ring of power (Lord of the Rings); or \"my Precious\"."
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "image",
                ImageUrl = "https://archive.sltrib.com/images/2011/0422/ldssingles_041511~1.jpg",
                QuestionText = "Who is this?",
                AnswerText = "Gerrit W. Gong (Quorum of the Twelve Apostles)"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "image",
                ImageUrl = "https://www.coralhills.com/wp-content/uploads/2011/12/St-George-temple.jpg",
                QuestionText = "Which is the oldest temple still in operation?",
                AnswerText = "St. George, Utah"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "text",
                QuestionText = "What are the 4 types of bears that live in Alaska?",
                AnswerText = "Black, Grizzly, Kodiak, Polar"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "text",
                QuestionText = "In what state in the US is the lowest point in the Western Hemisphere (282 feet below sea level)?",
                AnswerText = "California"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "text",
                QuestionText = "What number is used most frequently in the Bible?",
                AnswerText = "7"
            });
            //q.Add(new Question()
            //{
            //    Id = ++index,
            //    QuestionType = "text",
            //    QuestionText = "What is  ?",
            //    AnswerText = ""
            //});
            //q.Add(new Question()
            //{
            //    Id = ++index,
            //    QuestionType = "text",
            //    QuestionText = "What is  ?",
            //    AnswerText = ""
            //});
            //q.Add(new Question()
            //{
            //    Id = ++index,
            //    QuestionType = "text",
            //    QuestionText = "What is  ?",
            //    AnswerText = ""
            //});
            _questions = q;
        }
    }
}
