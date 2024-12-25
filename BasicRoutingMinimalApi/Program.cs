using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


var _people = new List<Person>//Создает список людей в качестве данных для API
{
    new("Tom", "Hanks"),
    new("Denzel", "Washington"),
    new("Leondardo", "DiCaprio"),
    new("Al", "Pacino"),
    new("Morgan", "Freeman"),
};

//app.MapPost();
app.MapGet("/person/{name}", (string name) =>//Маршрут параметризован для извлечения имени из URL - адреса
    _people.Where(p => p.FirstName.StartsWith(name, StringComparison.OrdinalIgnoreCase)));

app.Run();

record Person(string FirstName, string LastName);