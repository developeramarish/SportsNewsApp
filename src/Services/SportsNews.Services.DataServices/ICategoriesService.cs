﻿namespace SportsNews.Services.DataServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Categories;

    public interface ICategoriesService
    {
        IEnumerable<CategoryIdAndNameViewModel> GetAll();

        Task Create(string name, string image);

        bool IsCategoryIdValid(int categoryId);
    }
}
