using Asasy.Domain.Entities.SettingTables;
using Asasy.Domain.ViewModel.Question;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.QuestionsInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Implementation.QuestionImplementation
{
    public class QuestionServices : IQuestionServices
    {
        private readonly ApplicationDbContext _context;

        public QuestionServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<QuestionViewModel>> GetQuestions()
        {
            var Questions = await _context.Questions
                                        .Select(q => new QuestionViewModel
                                        {
                                            Id = q.Id,
                                            QuestionAr = q.QuestionAr,
                                            AnswerAr = q.AnswerAr,
                                            QuestionEn = q.QuestionEn,
                                            AnswerEn = q.AnswerEn,
                                            IsActive = q.IsActive
                                        }).ToListAsync();
            return Questions;
        }
        public async Task<bool> CreateQuestion(CreateQuestionViewModel questionViewModel)
        {
            Question question = new Question
            {
                QuestionAr = questionViewModel.QuestionAr,
                AnswerAr = questionViewModel.AnswerAr,
                QuestionEn = questionViewModel.QuestionEn,
                AnswerEn = questionViewModel.AnswerEn,
                IsActive = true
            };
            _context.Questions.Add(question);
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public async Task<QuestionViewModel> GetQuestionDetails(int? id)
        {
            var question = await _context.Questions.Where(q => q.Id == id)
                                         .Select(q => new QuestionViewModel
                                         {
                                             Id = q.Id,
                                             QuestionAr = q.QuestionAr,
                                             AnswerAr = q.AnswerAr,
                                             QuestionEn = q.QuestionEn,
                                             AnswerEn = q.AnswerEn,
                                         }).FirstOrDefaultAsync();
            return question;
        }
        public async Task<bool> EditQuestion(QuestionViewModel questionViewModel)
        {
            try
            {
                var Question = await _context.Questions.FindAsync(questionViewModel.Id);
                if (Question == null)
                    return false;

                Question.QuestionAr = questionViewModel.QuestionAr;
                Question.AnswerAr = questionViewModel.AnswerAr; 
                Question.QuestionEn = questionViewModel.QuestionEn;
                Question.AnswerEn = questionViewModel.AnswerEn;

                _context.Questions.Update(Question);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionsExists(questionViewModel.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            throw new NotImplementedException();
        }
        public async Task<bool> ChangeState(int? id)
        {
            var Question = await _context.Questions.FindAsync(id);
            Question.IsActive = !Question.IsActive;
            await _context.SaveChangesAsync();
            return Question.IsActive;
        }
        public bool QuestionsExists(int? id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
