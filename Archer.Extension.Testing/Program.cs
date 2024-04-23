// See https://aka.ms/new-console-template for more information
using Archer.Extension.DatabaseHelper;
using Archer.Extension.SecurityHelper;
using System.Data;

Console.WriteLine("Archer.Extension.Testing Start...");

const string keyConn = "聯邦網通加密鍵值Key     ";
const string ivConn = "聯邦網通加密鍵值IV      ";
const string keyData = "聯邦網通KeyData     ";
const string ivData = "聯邦網通IVData      ";
SecurityHelper securityHelper = new SecurityHelper(keyConn, ivConn, keyData, ivData);

DatabaseHelper databaseHelper = new DatabaseHelper(securityHelper);

IDbConnection dbConnection = databaseHelper.CreateIDbConnection("ZUtq5ndId8JRrQiiY/nci6T02LOfl8yNJOpw/AYi0anL3f6MPZwC0cfcSHcNnCdMluBFstSLsVPk5QTeA5HXCuO1Pr0HINWSb0Rn8xtC9Bqi9qWtC25KfL7AjMiUDjLxGmvHop0Jx/rrDFBcnMKcXrRWXnm2I84jpZuN0GThMC8=");

Console.ReadLine();