using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBuffet.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionType { get; set; }
        public string ImageUrl { get; set; }
        public string  QuestionText { get; set; }
        public string AnswerText { get; set; }
    }
}
