using Microsoft.EntityFrameworkCore;
using ParksAPI.Data;
using ParksAPI.Models;
using ParksAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParksAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        // Repository que acessa o BD
        // dependendo do projeto pode conter regras de negócios também
        // ou as regras podem ficar em um service que acessa um DAO
        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _db.Trails.Include(t => t.NationalPark).FirstOrDefault(n => n.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(t => t.NationalPark).OrderBy(n => n.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(n => n.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsByPark(int parkId)
        {
            return _db.Trails.Include(t => t.NationalPark).Where(t => t.NationalParkId == parkId).ToList();
        }
    }
}
