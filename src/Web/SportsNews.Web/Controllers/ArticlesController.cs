﻿namespace SportsNews.Services.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using DataServices;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using SportsNews.Services.Models.Articles;
    using Web.Models.Articles;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;

        public ArticlesController(IArticlesService articlesService, ICategoriesService categoriesService)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
        }
        [Authorize]
        public IActionResult Create()
        {
            this.ViewData["Categories"] = this.categoriesService.GetAll()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateArticleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var id = await this.articlesService.Create(input.CategoryId,input.Content);
            return this.RedirectToAction("Details", new {id = id});
        }

        [Authorize]
        public IActionResult Update(int id)
        {
            //this.ViewData["Article"] = this.articlesService.GetArticles()
            //    .Where(x => x.Id == id)
            //    .Select(x => new SelectListItem
            //    {
            //        Value = x.Id.ToString(),
            //        Text = x.Content
            //    });
            var model = this.articlesService.GetArticles()
                .Where(x => x.Id == id)
                .Select(x => new UpdateArticleInputModel
                {
                    Content = x.Content,
                    CategoryId = x.CategoryId
                }).FirstOrDefault();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateArticleInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.articlesService.Update(input.Id, input.Content, input.CategoryId);
            return this.RedirectToAction("Details", new { id = input.Id });
        }

        public IActionResult Details(int id)
        {
            var article = this.articlesService.GetArticleById<ArticleDetailsViewModel>(id);
            return this.View(article);
        }
    }
}
