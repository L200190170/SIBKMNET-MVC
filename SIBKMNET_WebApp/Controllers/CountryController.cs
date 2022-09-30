using Microsoft.AspNetCore.Mvc;
using SIBKMNET_WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SIBKMNET_WebApp.Controllers
{
    public class CountryController : Controller
    {
        SqlConnection sqlConnection;

        string connectionString = "Data Source=LAPTOP-5TGG567H;Initial Catalog=SIBKMNET;User ID=sibkmnet;Password=1234567890;Connect Timeout=30;";
        public IActionResult Index()
        {
            string query = "SELECT * FROM Country";

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            List<Country> Countries = new List<Country>();
            try
            {
                sqlConnection.Open();
                Console.WriteLine("Successfully Connected");
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Country country = new Country();
                            country.Id = Convert.ToInt32(sqlDataReader[0]);
                            country.Name = sqlDataReader[1].ToString();
                            Countries.Add(country);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(Countries);
        }

        //GET BY ID
        //GET
        [HttpGet]
        public IActionResult GetById(int id)
        {
            string query = "SELECT * FROM Country WHERE Id = @id";

            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@id";
            sqlParameter.Value = id;

            sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.Add(sqlParameter);
            try
            {
                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            Console.WriteLine(sqlDataReader[0] + " - " + sqlDataReader[1]);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No Data Rows");
                    }
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return View(id);
        }

        //CREATE
        //GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Country country)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@name";
                sqlParameter.Value = country.Name;

                sqlCommand.Parameters.Add(sqlParameter);

                try
                {
                    sqlCommand.CommandText = "INSERT INTO Country " +
                        "(Name) VALUES (@name)";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return View();
        }

        //UPDATE
        //GET
        [HttpGet]
        [ActionName("Edit")]
        public IActionResult Update()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public IActionResult Update(Country country)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                SqlParameter sqlPrm = new SqlParameter();
                sqlParameter.ParameterName = "@name";
                sqlParameter.Value = country.Name;
                sqlPrm.ParameterName = "@id";
                sqlPrm.Value = country.Id;

                sqlCommand.Parameters.Add(sqlParameter);
                sqlCommand.Parameters.Add(sqlPrm);

                try
                {
                    sqlCommand.CommandText = "UPDATE Country SET Name = @name WHERE Id = @id";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return View();
        }

        //DELETE
        //GET
        [HttpGet]
        [ActionName("Delete")]
        public IActionResult Delete()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult Delete(Country country)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            
            {
                sqlConnection.Open();
                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;

                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = "@id";
                sqlParameter.Value = country.Id;
                sqlCommand.Parameters.Add(sqlParameter);

                try
                {
                    sqlCommand.CommandText = "DELETE FROM Country WHERE Id = @id";
                    sqlCommand.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
            }
            return View();
        }
    }
}
