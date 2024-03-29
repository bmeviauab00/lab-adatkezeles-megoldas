﻿using Microsoft.Data.SqlClient;

using System.Data;

namespace AdoExample;

public class Program
{
    private const string ConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=neptun;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    
    private static void Main(string[] args)
    {
        GetShippers();
        InsertShipper("Super Shipper", "+36-30-300-0000");
        GetShippers();
        UpdateProduct(1, "MyProduct", 50);
        //InsertShipper2("Super Shipper", "49-98562'); DELETE FROM Shippers;--");
        //InsertShipper("Super Shipper", "49-98562'); DELETE FROM Shippers;--");
        //InsertShipper("XXX');DELETE FROM Shippers;--", "49-98562");
        DeleteShipper(4);
    }

    private static void GetShippers()
    {
        using var conn = new SqlConnection(ConnString);
        using var command = new SqlCommand("SELECT ShipperID, CompanyName, Phone FROM Shippers", conn);

        conn.Open();

        Console.WriteLine("{0,-10}{1,-20}{2,-20}", "ShipperID", "CompanyName", "Phone");
        Console.WriteLine(new string('-', 60));

        using SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine($@"{reader["ShipperID"],-10}{reader["CompanyName"],-20}{reader["Phone"],-20}");
        }
    }

    private static void InsertShipper(string companyName, string phone)
    {
        using var conn = new SqlConnection(ConnString);
        using var command = new SqlCommand("INSERT INTO Shippers(CompanyName, Phone) VALUES(@name,@phone)", conn);
        command.Parameters.AddWithValue("@name", companyName);
        command.Parameters.AddWithValue("@phone", phone);

        conn.Open();

        int affectedRows = command.ExecuteNonQuery();

        Console.WriteLine($"{affectedRows} rows affected");
    }

    private static void UpdateProduct(int productID, string productName, decimal price)
    {
        using var conn = new SqlConnection(ConnString);
        using var command = new SqlCommand("Product_Update", conn);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@ProductID", productID);
        command.Parameters.AddWithValue("@ProductName", productName);
        command.Parameters.AddWithValue("@UnitPrice", price);

        conn.Open();

        int affectedRows = command.ExecuteNonQuery();
        
        Console.WriteLine($"{affectedRows} rows affected");
    }

    private static void InsertShipper2(string companyName, string phone)
    {
        using var conn = new SqlConnection(ConnString);
        using var command = new SqlCommand(
            $"INSERT INTO Shippers(CompanyName, Phone) VALUES('{companyName}','{phone}')",
            conn);

        conn.Open();

        int affectedRows = command.ExecuteNonQuery();
        Console.WriteLine($"{affectedRows} row(s) inserted");
    }

    private static void DeleteShipper(int shipperID)
    {
        using var conn = new SqlConnection(ConnString);
        using var command = new SqlCommand("DELETE FROM Shippers WHERE ShipperID = @ShipperID", conn);
        command.Parameters.AddWithValue("@ShipperID", shipperID);

        conn.Open();

        int affectedRows = command.ExecuteNonQuery();

        Console.WriteLine($"{affectedRows} row(s) affected");
    }
}