﻿using FacilitEase.Models.ApiModels;

namespace FacilitEase.Services
{
    public interface ICategoryService
    
        
        {
            IEnumerable<CategoryDto> GetCategory();
            void CreateCategory(CategoryDto categoryDto);
        }
    }



