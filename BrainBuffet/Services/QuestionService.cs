using BrainBuffet.Data;
using BrainBuffet.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrainBuffet.Services
{

    public class QuestionService
    {
        private BrainBuffetContext _context;

        public QuestionService(BrainBuffetContext context)
        {
            _context = context;
        }

        public  ActionResult<Question> GetQuestionById(int id)
        {
            return _context.Questions.FirstOrDefault(m => m.Id == id);
        }

        public int GetQuestionCount()
        {
            return _context.Questions.Max(m => m.Id);
        }
    }
}
