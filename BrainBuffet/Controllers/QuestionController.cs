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
                QuestionText = "Which temple is this (hint: it is the oldest temple still in operation)?",
                AnswerText = "St. George, Utah"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "image",
                ImageUrl = "https://mormonhub.com/wp-content/uploads/2014/08/Elder-Bednar.jpg",
                QuestionText = "What did Elder Bednar talk about in the April 2018 General Conference?",
                AnswerText = "Being meek and lowly of heart"
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
                QuestionText = "In which US state is the lowest point in the Western Hemisphere (282 feet below sea level)?",
                AnswerText = "California"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "text",
                QuestionText = "What number is used most frequently in the Bible?",
                AnswerText = "One"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "text",
                QuestionText = "How did the rich, young man go away after talking with the Savior?",
                AnswerText = "Sorrowful"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "text",
                ImageUrl = "https://www.mormonnewsroom.org/media/480x640/book-of-mormon.jpg",
                QuestionText = "How many pages are there in the Book of Mormon?",
                AnswerText = "531"
            });
            q.Add(new Question()
            {
                Id = ++index,
                QuestionType = "text",
                ImageUrl = "https://askgramps.org/files/2015/02/doctrine-and-covenants-open.jpg",
                QuestionText = "How many of the 138 sections in the D&C are not from Joseph Smith (revelations, visions, prayers, instructions)?",
                AnswerText = "5\r\n" +
                "D&C 102 - Church council minutes\r\n" +
                "D&C 134 - Declaration on Government\r\n" +
                "D&C 135 - Annoucement of the Martyrdom\r\n" +
                "D&C 136 - Revelation through Brigham Young\r\n" +
                "D&C 138 - Vision of Joseph F. Smith"
            });
            _questions = q;
        }
    }
}
