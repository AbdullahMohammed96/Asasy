using Asasy.Domain.ViewModel.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asasy.Service.DashBoard.Contract.QuestionsInterfaces
{
    public interface IQuestionServices
    {
        Task<List<QuestionViewModel>> GetQuestions();
        Task<bool> CreateQuestion(CreateQuestionViewModel questionViewModel);
        Task<QuestionViewModel> GetQuestionDetails(int? id);
        Task<bool> EditQuestion(QuestionViewModel questionViewModel);
        Task<bool> ChangeState(int? id);
        bool QuestionsExists(int? id);

    }
}
