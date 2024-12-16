using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorFORMS
{
    public interface ICrud<T>
    {
        List<T> GetAll();
        void Create(T entity); // Ahora retorna void
        void Update(T entity);
        void Delete(int id);
    }
    internal class CrudManager
    {
        private string _connectionString;
        private readonly Dictionary<string, object> _repositories = new Dictionary<string, object>();

        // Constructor que recibe el connectionString
        public CrudManager(string connectionString)
        {
            _connectionString = connectionString;

            // Aquí usamos siempre el mismo nombre: "Powerlifters"
            _repositories.Add("Powerlifters", new PowerlifterDataAcessLayer(_connectionString));

            _repositories.Add("Usuarios", new UserDataAccessLayer(_connectionString));
            _repositories.Add("Entrenamientos", new TrainingDataAccessLayer(_connectionString));
            _repositories.Add("Ejercicios", new TrainingDataAccessLayer(_connectionString));
            _repositories.Add("Sesiones", new TrainingSessionDataAccessLayer(_connectionString));
            _repositories.Add("Carga", new LoadDataAcessLayer(_connectionString));
            _repositories.Add("Series", new SeriesDataAccessLayer(_connectionString));
            _repositories.Add("Objetivos", new GoalDataAccessLayer(_connectionString));
            _repositories.Add("Rutinas", new RoutineDataAcessLayer(_connectionString));
            _repositories.Add("Plan Nutricional", new NutritionalPlanDataAcessLayer(_connectionString));
        }

        public ICrud<T> GetRepository<T>(string tableName)
        {
            if (_repositories.ContainsKey(tableName))
            {
                return _repositories[tableName] as ICrud<T>;
            }
            return null;
        }
    }
}

