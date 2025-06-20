
using AutoMapper;
using CryptoAPI.Core.Interfaces;
using CryptoAPI.Core.Models;
using CryptoAPI.DAL;
using CryptoAPI.DAL.Entities;
using CryptoAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio = CryptoAPI.DAL.Entities.Portfolio;
using PortfolioItem = CryptoAPI.DAL.Entities.PortfolioItem;
using PortfolioNote = CryptoAPI.DAL.Entities.PortfolioNote;

namespace CryptoAPI.TestProject
{
    [TestClass]
    public class PortfolioRepositoryIntegrationTests
    {
        private CryptoAPIContext _context;
        private PortfolioRepository _repository;
        private ICoinGeckoService _coinGeckoService;
        private IMapper _mapper;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<CryptoAPIContext>()
                .UseInMemoryDatabase(databaseName: $"CryptoDb_{Guid.NewGuid()}")
                .Options;

            _context = new CryptoAPIContext(options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _repository = new PortfolioRepository(_context, _mapper, _coinGeckoService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }


        [TestMethod]
        public async Task GetPortfolioByUserIdAsync_ReturnsPortfolioWithItems()
        {
            _context.Portfolio.Add(new Portfolio
            {
                UserId = 1,
                PortfolioItems = new List<DAL.Entities.PortfolioItem>
                {
                    new PortfolioItem { CoinId = "btc", CoinName = "Bitcoin" }
                }
            });
            await _context.SaveChangesAsync();

            var result = await _repository.GetPortfolioByUserIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.PortfolioItems.Count);
            Assert.AreEqual("btc", result.PortfolioItems.First().CoinId);
        }


        [TestMethod]
        public async Task AddPortfolioItemAsync_AddsItem_WhenPortfolioExists()
        {
            _context.Portfolio.Add(new Portfolio
            {
                UserId = 3,
                PortfolioItems = new List<PortfolioItem>()
            });
            await _context.SaveChangesAsync();

            var item = new Core.Models.PortfolioItem { CoinId = "eth", CoinName = "Ethereum" };
            await _repository.AddPortfolioItemAsync(3, item);

            var portfolio = await _context.Portfolio.Include(p => p.PortfolioItems).FirstAsync(p => p.UserId == 3);
            Assert.AreEqual(1, portfolio.PortfolioItems.Count);
            Assert.AreEqual("eth", portfolio.PortfolioItems.First().CoinId);
        }

        [TestMethod]
        public async Task RemovePortfolioItemAsync_RemovesItem_WhenExists()
        {
            _context.Portfolio.Add(new DAL.Entities.Portfolio
            {
                UserId = 4,
                PortfolioItems = new List<DAL.Entities.PortfolioItem>
                {
                    new DAL.Entities.PortfolioItem { CoinId = "btc", CoinName = "Bitcoin" }
                }
            });
            await _context.SaveChangesAsync();

            await _repository.RemovePortfolioItemAsync(4, "btc");

            var portfolio = await _context.Portfolio.Include(p => p.PortfolioItems).FirstAsync(p => p.UserId == 4);
            Assert.AreEqual(0, portfolio.PortfolioItems.Count);
        }

        [TestMethod]
        public async Task RemovePortfolioItemAsync_Throws_WhenPortfolioNotFound()
        {
            await Assert.ThrowsExceptionAsync<NotFoundException>(() => _repository.RemovePortfolioItemAsync(5, "btc"));
        }

        [TestMethod]
        public async Task AddOrUpdateNoteAsync_AddsNote_WhenNotExists()
        {
            await _repository.AddOrUpdateNoteAsync(1, "btc", "First note", "Neutral");

            var note = await _context.PortfolioNotes.FirstOrDefaultAsync(n => n.UserId == 1 && n.CoinId == "btc");
            Assert.IsNotNull(note);
            Assert.AreEqual("First note", note.Note);
        }

        [TestMethod]
        public async Task AddOrUpdateNoteAsync_UpdatesNote_WhenExists()
        {
            _context.PortfolioNotes.Add(new PortfolioNote
            {
                UserId = 2,
                CoinId = "eth",
                Note = "Old note",
                Mood="Neutral"
            });
            await _context.SaveChangesAsync();

            await _repository.AddOrUpdateNoteAsync(2, "eth", "Updated note", "Neutral");

            var note = await _context.PortfolioNotes.FirstOrDefaultAsync(n => n.UserId == 2 && n.CoinId == "eth");
            Assert.AreEqual("Updated note", note.Note);
        }

        [TestMethod]
        public async Task GetNotesByUserAsync_ReturnsNotes()
        {
            _context.PortfolioNotes.AddRange(
                new PortfolioNote { UserId = 3, CoinId = "btc", Note = "Note A", Mood="Neutral" },
                new PortfolioNote { UserId = 3, CoinId = "eth", Note = "Note B", Mood = "Neutral" }
            );
            await _context.SaveChangesAsync();

            var notes = await _repository.GetNotesByUserAsync(3);

            Assert.AreEqual(2, notes.Count);
            Assert.IsTrue(notes.Any(n => n.Note == "Note A"));
            Assert.IsTrue(notes.Any(n => n.Note == "Note B"));
        }

        [TestMethod]
        public async Task GetNotesByUserAsync_ReturnsEmptyList_WhenNoNotes()
        {
            var notes = await _repository.GetNotesByUserAsync(100);
            Assert.IsNotNull(notes);
            Assert.AreEqual(0, notes.Count);
        }
    }
}
