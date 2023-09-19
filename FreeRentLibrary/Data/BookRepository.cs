﻿using System.Collections.Generic;
using System.Linq;
using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FreeRentLibrary.Data
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly DataContext _context;
        public BookRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public IQueryable GetAllWithUsers()
        {
            return _context.Books.Include(p => p.User);
        }

        public IEnumerable<SelectListItem> GetComboBooks()
        {
            var list = _context.Books.Select(p => new SelectListItem
            {
                Text = p.Title,
                Value = p.Id.ToString()

            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a product...)",
                Value = "0"
            });
            return list;
        }
    }
}