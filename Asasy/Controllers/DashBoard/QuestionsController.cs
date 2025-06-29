using Asasy.Domain.Enums;
using Asasy.Domain.ViewModel.Question;
using Asasy.Infrastructure.Extension;
using Asasy.Persistence;
using Asasy.Service.DashBoard.Contract.QuestionsInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asasy.Controllers.DashBoard
{
    [AuthorizeRoles(Roles.Admin)]
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IQuestionServices _questionServices;

        public QuestionsController(ApplicationDbContext context, IQuestionServices questionServices)
        {
            _context = context;
            _questionServices = questionServices;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _questionServices.GetQuestions());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuestionViewModel questions)
        {
            if (ModelState.IsValid)
            {
                if (await _questionServices.CreateQuestion(questions))
                    return RedirectToAction(nameof(Index));
            }
            return View(questions);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!_questionServices.QuestionsExists(id))
            {
                return NotFound();
            }
            return View(await _questionServices.GetQuestionDetails(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuestionViewModel questionViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await _questionServices.EditQuestion(questionViewModel))
                    return RedirectToAction(nameof(Index));
            }
            return View(questionViewModel);
        }

        public async Task<IActionResult> ChangeState(int? id)
        {
            bool IsActive = await _questionServices.ChangeState(id);
            return Json(new { key = 1, data = IsActive });
        }


    }
}
