using Dapper;
using DotNetEnv;
using Npgsql;

namespace Capychef.E2E.Helpers;

public static class DbHelper
{
    public static string GetConnString()
    {
        try
        {
            Env.Load("../../../../.env");
            Env.Load("../../../.env");
            Env.Load("../../.env");
            Env.Load("../.env");
            Env.Load(".env");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        return connectionString;
    }

    public static async Task<int> ExecuteSqlAsync(string sql, object? parameters = null, int commandTimeout = 60)
    {
        await using var conn = new NpgsqlConnection(GetConnString());
        await conn.OpenAsync();
        return await conn.ExecuteAsync(new CommandDefinition(sql, parameters, commandTimeout: commandTimeout));
    }

    public static async Task<int> ExecuteSqlFileAsync(string filePath, int commandTimeout = 60)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"SQL file not found: {filePath}", filePath);

        var sql = await File.ReadAllTextAsync(filePath);
        return await ExecuteSqlAsync(sql, null, commandTimeout);
    }

    public static async Task<T> QuerySingleAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = new NpgsqlConnection(GetConnString());
        await conn.OpenAsync();
        return await conn.QuerySingleAsync<T>(sql, parameters);
    }

    public static async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        await using var conn = new NpgsqlConnection(GetConnString());
        await conn.OpenAsync();
        return await conn.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }
}