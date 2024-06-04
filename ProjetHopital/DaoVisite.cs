﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ProjetHopital
{
    class DaoVisite
    {
        public List<Visite> SelectAll()
        {
            List<Visite> liste = new List<Visite>();
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;SELECT * FROM visites";


            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = new SqlCommand(sql, connexion);

            connexion.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                liste.Add(new Visite(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2).ToString(), reader.GetString(3),
                                 reader.GetInt32(4), reader.GetDecimal(5)));
            }

            connexion.Close();
            return liste;
        }
        public Visite SelectById(int id)
        {
            Visite v = new Visite();
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;SELECT * FROM visites WHERE id=" + id;


            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = new SqlCommand(sql, connexion);

            connexion.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {

                v = new Visite(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2).ToString(), reader.GetString(3),
                                 reader.GetInt32(4), reader.GetDecimal(5));
            }
            connexion.Close();
            return v;
        }
        public Visite SelectByIdPatient(int idPatient)
        {
            Visite v = new Visite();
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;SELECT * FROM visites WHERE idPatient=" + idPatient;

            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = new SqlCommand(sql, connexion);

            connexion.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {

                v = new Visite(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2).ToString(), reader.GetString(3),
                                 reader.GetInt32(4), reader.GetDecimal(5));
            }
            connexion.Close();
            return v;
        }
        public List<Visite> SelectByIdPatientBetween2dates(int idPatient,string date1,string date2)
        {
            List<Visite> liste = new List<Visite>();
            Visite v = new Visite();
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;SELECT * FROM visites WHERE (idPatient=" + idPatient + ") " +
                "AND (date BETWEEN '" + date1 + "' and '" + date2 + "') ";

            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = new SqlCommand(sql, connexion);

            connexion.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                v = new Visite(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2).ToString(), reader.GetString(3),
                                 reader.GetInt32(4), reader.GetDecimal(5));
                liste.Add(v);
            }
            connexion.Close();
            return liste;
        }
        public List<Visite> SelectVisitePatientByDate(string date) // Format France (ex: 04-06-2024)
        {
            List<Visite> liste = new List<Visite>();
            Visite v = new Visite();
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;SELECT * FROM visites WHERE date='" + date + "' ORDER BY date ASC";

            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = new SqlCommand(sql, connexion);

            connexion.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                v = new Visite(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2).ToString(), reader.GetString(3),
                                 reader.GetInt32(4), reader.GetDecimal(5));
                liste.Add(v);
            }
            connexion.Close();
            return liste;
        }
        public List<Visite> SelectVisitePatientByMedecin(string nomMedecin) 
        {
            List<Visite> liste = new List<Visite>();
            Visite v = new Visite();
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;SELECT * FROM visites WHERE medecin='" + nomMedecin + "'  ";

            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = new SqlCommand(sql, connexion);

            connexion.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                v = new Visite(reader.GetInt32(0), reader.GetInt32(1), reader.GetDateTime(2).ToString(), reader.GetString(3),
                                 reader.GetInt32(4), reader.GetDecimal(5));
                liste.Add(v);
            }
            connexion.Close();
            return liste;
        }
        public List<Visite> SelectByMedecin(string nomMedecin)
        {
            List<Visite> visites = new List<Visite>();
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital; SELECT * FROM visites WHERE medecin = @nomMedecin";

            using (SqlConnection connexion = new SqlConnection(connexionString))
            {
                SqlCommand command = new SqlCommand(sql, connexion);
                command.Parameters.AddWithValue("@nomMedecin", nomMedecin);

                connexion.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        visites.Add(new Visite(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetDateTime(2).ToString(),
                            reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetDecimal(5)
                        ));
                    }
                }
            }
            return visites;
        }
        public void Insert(Visite v)
        {
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;INSERT INTO visites VALUES (@idPatient,@date,@nomMedecin,@numSalle,@tarif)";

            DateTime dateTime = DateTime.Parse(v.Date);

            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = connexion.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("idPatient", SqlDbType.Int).Value = v.IdPatient;
            command.Parameters.Add("date", SqlDbType.DateTime).Value = dateTime ;
            command.Parameters.Add("nomMedecin", SqlDbType.NVarChar).Value = v.NomMedecin;
            command.Parameters.Add("numSalle", SqlDbType.Int).Value = v.NumSalle;
            command.Parameters.Add("tarif", SqlDbType.Decimal).Value = v.Tarif;


            connexion.Open();
            // Excecution de la requête
            command.ExecuteNonQuery();
            Console.WriteLine("Insertion Visite ok");
            connexion.Close();
        }

        public void Update(Visite v)
        {
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;UPDATE visites SET idPatient=@idPatient ,Medecin=@nomMedecin, date=@date, num_Salle=@numSalle, tarif=@tarif WHERE id=@idVisite";


            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = connexion.CreateCommand();
            command.CommandText = sql;
            command.Parameters.Add("idVisite", SqlDbType.Int).Value = v.IdVisite;
            command.Parameters.Add("idPatient", SqlDbType.Int).Value = v.IdPatient;
            command.Parameters.Add("nomMedecin", SqlDbType.NVarChar).Value = v.NomMedecin;
            command.Parameters.Add("date", SqlDbType.Date).Value = v.Date;
            command.Parameters.Add("numSalle", SqlDbType.Int).Value = v.NumSalle;
            command.Parameters.Add("tarif", SqlDbType.Decimal).Value = v.Tarif;

            connexion.Open();
            // Excecution de la requête
            command.ExecuteNonQuery();
            Console.WriteLine("Mise à jour Visite ok");
            //Console.WriteLine(sql);
            connexion.Close();
        }

        public void Delete(int id)
        {
            string connexionString = InfoSql.CONNEXION_INFO;

            string sql = "USE Hopital;DELETE FROM visites WHERE id=" + id;


            SqlConnection connexion = new SqlConnection(connexionString);
            SqlCommand command = new SqlCommand(sql, connexion);

            connexion.Open();

            // execution de la requete
            command.ExecuteNonQuery();
            Console.WriteLine("Suppression visite ok");
            connexion.Close();

        }
    }
}
