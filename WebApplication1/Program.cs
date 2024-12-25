using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace WebApplication1
{
    public class Program
    {
        static List<User> users = new List<User>
        {
            new() { Id = Guid.NewGuid().ToString(), Name = "vova", Email = "vova@mail.ru" },
            new() { Id = Guid.NewGuid().ToString(), Name = "Alice", Email = "alice@mail.ru" },
            new() { Id = Guid.NewGuid().ToString(), Name = "vova2", Email = "vova2@mail.ru" }
        }, defUs = users;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var app = builder.Build();
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }

            //app.MapGet("/", void () => throw new Exception());
            app.Run(async (context) =>
            {
                var responce = context.Response;
                var request = context.Request;
                var path = request.Path;

                string expForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
                if (path == "/api/users" && request.Method == "GET")
                {
                    GetAllPeople(responce);
                }
                else if (Regex.IsMatch(path, expForGuid) && request.Method == "GET")
                {
                    string? id = path.Value?.Split("/")[3];
                    GetUser(id, responce);
                }
                else if (path == "/api/users" && request.Method == "PUT")
                {
                    UpdateUser(responce, request);
                }
                else if (Regex.IsMatch(path, expForGuid) && request.Method == "Delete")
                {
                    string? id = path.Value?.Split("/")[3];
                    DeleteUser(id, responce);
                }
                else
                {
                    responce.ContentType = "text/html; charset=utf-8";
                    await responce.SendFileAsync("html/index.html");
                }
            });
			app.Run();

            /*
             Results.File();
            Results.Byte();
            Results.Stream();
             */
        }
		static IQueryable<User> GetAllPeople(HttpResponse httpResponse) { return users.AsQueryable(); }
		static User GetUser(string? identifier, HttpResponse httpResponse) { return users.FirstOrDefault(x => x.Id == identifier)!; }//поиск пользователя
		static bool UpdateUser(HttpResponse httpResponse, HttpRequest httpRequest) { return IsRen(); }
		static bool DeleteUser(string? identifier, HttpResponse response)
		{
			User user = GetUser(identifier, response);//поиск пользователя
            users.Remove(user);
			return IsRen();
		}
        static bool IsRen()
        {
            bool isRefr;
			if (users == defUs) isRefr = false;//если список по умолчанию совпадает с обновленным списком, то значение равно 0
            else isRefr = true;//если обновленный список отличается от списка по умолчанию, значени равно 1
            defUs = users;//обновление списка по умолчанию
            return isRefr;
        }
	}

    public class User
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public User() { }
    }
}