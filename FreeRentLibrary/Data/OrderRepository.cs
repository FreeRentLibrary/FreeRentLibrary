﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FreeRentLibrary.Data.Entities;
using FreeRentLibrary.Helpers;
using FreeRentLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace FreeRentLibrary.Data
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }
            var book = await _context.Books.FindAsync(model.BookId);
            if (book == null)
            {
                return;
            }
            var orderDetailTemp = await _context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Book == book)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Book = book,
                    Quantity = model.Quantity,
                    User = user,
                };

                _context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                _context.OrderDetailTemps.Update(orderDetailTemp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }
            var orderTmps = await _context.OrderDetailTemps
                .Include(o => o.Book)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Book = o.Book,
                Quantity = o.Quantity,
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            await CreateAsync(order);
            _context.OrderDetailTemps.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var bookDetailTemp = await _context.OrderDetailTemps.FindAsync(id);

            if (bookDetailTemp == null)
            {
                return;
            }
            _context.OrderDetailTemps.Remove(bookDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task DeliveryOrder(DeliveryViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.id);
            if (order == null)
            {
                return;
            }
            order.DeliveryDate = model.DeliveryDate;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }
            return _context.OrderDetailTemps
                .Include(p => p.Book)
                .Where(o => o.User == user)
                .OrderBy(o => o.Book.Name);
        }

        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }
            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(p => p.Book)
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Book)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await _context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }
            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                _context.OrderDetailTemps.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }

        }
    }
}
