﻿namespace SportsNews.Services.DataServices
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Common;
    using Data.Models;
    using Mapping;
    using SportsNews.Services.Models.Articles;
    using SportsNews.Services.Models.Home;

    public class ArticlesService : IArticlesService
    {
        private readonly IRepository<Article> articlesRepository;
        private readonly IRepository<Category> categoryRepository;

        public ArticlesService(IRepository<Article> articlesRepository, IRepository<Category> categoryRepository)
        {
            this.articlesRepository = articlesRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<int> Create(int categoryId, string content, string title)
        {
            var article = new Article()
            {
               CategoryId = categoryId,
               Content = content,
               Title = title
            };

            await this.articlesRepository.AddAsync(article);
            await this.articlesRepository.SaveChangesAsync();

            return article.Id;
        }

        public IEnumerable<ArticleViewModel> GetArticles()
        {
            var articles = this.articlesRepository.All()
                .To<ArticleViewModel>().ToList();

            return articles;
        }

        public int GetCount()
        {
            return this.articlesRepository.All().Count();
        }

        public TViewModel GetArticleById<TViewModel>(int id)
        {
            var article = this.articlesRepository.All().Where(x => x.Id == id).To<TViewModel>().FirstOrDefault();

            return article;
        }

        public IEnumerable<ArticleViewModel> GetAllByCategory(int categoryId)
        {
            var articles = this.articlesRepository.All()
                .Where(x => x.CategoryId == categoryId)
                .To<ArticleViewModel>().ToList();

            return articles;
        }

        public async Task Update(int articleId, string content, int categoryId, string title)
        {
            var article = this.articlesRepository.All().FirstOrDefault(x => x.Id == articleId);
            article.Content = content;
            article.CategoryId = categoryId;
            article.Title = title;
            this.articlesRepository.Update(article);
            await this.articlesRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var article = this.articlesRepository.All().FirstOrDefault(x => x.Id == id);
            this.articlesRepository.Delete(article);
            await this.articlesRepository.SaveChangesAsync();
        }
    }
    
}
