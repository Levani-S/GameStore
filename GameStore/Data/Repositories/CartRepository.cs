﻿using GameStore.Data.Repositories.RepositoryInterfaces;
using GameStore.Models;
using GameStore.ValidateData;
using GameStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameStore.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly GameStoreContext _context;
        public CartRepository(GameStoreContext context)
        {
            _context = context;
        }

        public async Task<CartModel> GetAllItemsFromCart(Guid cartId)
        {
            var cart = await _context.Carts.Where(x => x.CartId == cartId && !x.IsFinished).Include(x => x.Orders).FirstOrDefaultAsync();

            ValidateOnNull<CartModel>.ValidateDataOnNull(cart);

            return cart;
        }
        private async Task AddGameToExistingCart(OrderModel order, GameModel game)
        {
            var cart = await _context.Carts.Where(x => x.CartId == order.CartId && !x.IsFinished).Include(x => x.Orders).FirstOrDefaultAsync();

            ValidateOnNull<CartModel>.ValidateDataOnNull(cart);

            if (cart.Orders.Any(x => x.GameId == order.GameId))
            {
                var orderGame = cart.Orders.FirstOrDefault(x => x.GameId == order.GameId);

                if (orderGame != null)
                {
                    orderGame.AmountOfGames += order.AmountOfGames;
                    orderGame.TotalPrice += (orderGame.Price * order.AmountOfGames);
                }
            }
            else
            {
                order.Price = game.GamePrice;
                order.TotalPrice = game.GamePrice * order.AmountOfGames;
                cart.Orders.Add(order);
                _context.Orders.Add(order);
            }
            cart.TotalPrice += (game.GamePrice * order.AmountOfGames);

            await _context.SaveChangesAsync();
        }

        private async Task CreateNewCartAndAddGame(OrderModel order, GameModel game)
        {
            order.Price = game.GamePrice;
            order.TotalPrice = game.GamePrice * order.AmountOfGames;

            var cart = new CartModel()
            {
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                IsFinished = false,
                Orders = new List<OrderModel>()
            };
            cart.Orders.Add(order);

            _context.Carts.Add(cart);
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();
        }

        public async Task AddGameToCart(OrderModel order)
        {
            var game = await _context.Games.Where(x => x.GameId == order.GameId).FirstOrDefaultAsync();

            ValidateOnNull<GameModel>.ValidateDataOnNull(game);

            var user = _context.Users.Where(x => x.Id == order.UserId.ToString()).FirstOrDefaultAsync();

            ValidateOnNull<Task<UserModel>>.ValidateDataOnNull(user);

            if (order.CartId != null)
            {
                await AddGameToExistingCart(order, game);
            }
            else
            {
                await CreateNewCartAndAddGame(order, game);
            }
        }

        public async Task EditOrder(EditCartViewModel editedOrder)
        {
            ValidateOnNull<EditCartViewModel>.ValidateDataOnNull(editedOrder);
            var cart = await _context.Carts.Where(x => x.CartId == editedOrder.CartId && x.UserId == editedOrder.UserId && !x.IsFinished)
                .Include(x => x.Orders).FirstOrDefaultAsync();

            ValidateOnNull<CartModel>.ValidateDataOnNull(cart);
            var order = cart.Orders.FirstOrDefault(x => x.GameId == editedOrder.GameId);

            if (order != null)
            {
                if (order.AmountOfGames <= -editedOrder.AmountOfGames)
                {
                    cart.Orders.Remove(order);
                    _context.Orders.Remove(order);

                    if (cart.Orders.Count == 0)
                    {
                        _context.Carts.Remove(cart);
                    }
                    await _context.SaveChangesAsync();
                }
                else
                {
                    order.AmountOfGames += editedOrder.AmountOfGames;
                    order.TotalPrice += (order.Price * editedOrder.AmountOfGames);
                    cart.TotalPrice += (order.Price * editedOrder.AmountOfGames);

                    await _context.SaveChangesAsync();
                }
            }
        }
        public async Task RemoveGameFromCart(Guid gameId, Guid userId, Guid cartId)
        {
            var cart = await _context.Carts.Where(x => x.CartId == cartId && x.UserId == userId && !x.IsFinished).Include(x => x.Orders).FirstOrDefaultAsync();

            ValidateOnNull<CartModel>.ValidateDataOnNull(cart);

            var order = cart.Orders.FirstOrDefault(x => x.GameId == gameId);

            if (order != null)
            {
                cart.TotalPrice -= order.TotalPrice;
                cart.Orders.Remove(order);
                _context.Orders.Remove(order);

                if (cart.Orders.Count == 0)
                {
                    _context.Carts.Remove(cart);
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<FinishedOrdersModel> FinishOrder(FinishedOrdersModel contact)
        {
            ValidateOnNull<FinishedOrdersModel>.ValidateDataOnNull(contact);

            var cart = await _context.Carts.Where(x => x.CartId == contact.CartId && x.UserId == contact.UserId && !x.IsFinished).FirstOrDefaultAsync();

            ValidateOnNull<CartModel>.ValidateDataOnNull(cart);

            _context.FinishedOrders.Add(contact);
            cart.IsFinished = true;
            await _context.SaveChangesAsync();

            return contact;
        }
    }
}

