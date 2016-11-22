﻿using GolfBag.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GolfBag.Services
{
    public interface IScoreCardData
    {
        IEnumerable<ScoreCard> GetAll();
        ScoreCard Get(int id);
        void Add(ScoreCard newScoreCard);
        void Delete(ScoreCard scoreCard);
        int Commit();
    }

    public class SqlScoreCardData : IScoreCardData
    {
        private ScoreCardDbContext _context;

        public SqlScoreCardData(ScoreCardDbContext context)
        {
            _context = context;
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
        public void Add(ScoreCard newScoreCard)
        {
            _context.Add(newScoreCard);
            _context.SaveChanges();
        }

        public void Delete(ScoreCard scoreCard)
        {
            _context.Remove(scoreCard);
            Commit();
        }

        public ScoreCard Get(int id)
        {
            return _context.ScoreCards.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<ScoreCard> GetAll()
        {
            return _context.ScoreCards.ToList();
        }
    }
    //public class ScoreCardData : IScoreCardData
    //{
    //    List<ScoreCard> _scoreCards;
    //    public ScoreCardData()
    //    {
    //        _scoreCards = new List<ScoreCard>
    //        {
    //            new ScoreCard {Id = 1, CourseName = "Blacks", PlayerName = "Bill" },
    //            new ScoreCard {Id = 2, CourseName = "Doosie", PlayerName = "Passersbye" }
    //        };
    //    }

    //    public int Commit()
    //    {
    //        return 0;
    //    }

    //    public void Add(ScoreCard newScoreCard)
    //    {
    //        //newScoreCard.Id = _scoreCards.Max(r => r.Id) + 1;
    //        _scoreCards.Add(newScoreCard);
    //    }

    //    public ScoreCard Get(int id)
    //    {
    //        return _scoreCards.FirstOrDefault(r => r.Id == id);
    //    }
    //    public IEnumerable<ScoreCard> GetAll()
    //    {
    //        return _scoreCards;
    //    }        
    //}
}
