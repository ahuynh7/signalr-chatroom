using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRChatroom.Server;
using SignalRChatroom.Server.Contracts;
using SignalRChatroom.Server.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    //cors policy to allow websocket connection
    options.AddPolicy("SignalRCorsPolicy", builder =>
    {
        builder.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:3000", "http://44.243.182.113");
    });
});
builder.Services.AddMapster();

//configure SignalR service
builder.Services.AddSignalR();

//configure db
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<SignalRChatroomDbContext>((provider, optionsBuilder) =>
    {
        var connectionString = builder.Configuration.GetConnectionString("Database");
        optionsBuilder.UseNpgsql(connectionString);
    });
else
    builder.Services.AddDbContext<SignalRChatroomDbContext>((provider, optionsBuilder) =>
    {
        var connectionString = builder.Configuration["DB_CONNECTION"];
        optionsBuilder.UseNpgsql(connectionString);
    });

//dep injections
builder.Services.AddScoped<IChatRepository, ChatRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SignalRCorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapHub<ChatHub>("/chatroom");

//minimal api
app.MapGet(
    "messages",
    async (
        IChatRepository chatRepository,
        IMapper mapper) =>
    {
        var result = await chatRepository.GetAll();

        return Results.Ok(mapper.Map<List<ChatResponse>>(result));
    }
);

app.MapPost(
    "message", 
    async (
        [FromBody] InsertChatRequest request,
        IHubContext<ChatHub, IChatHubMethods> context,
        IChatRepository chatRepository) =>
    {
        var chat = new Chat
        {
            Username = request.Username,
            Message = request.Message
        };

        await chatRepository.InsertAsync(chat);
        await context.Clients.All.InsertChat(
            chat.Timestamp, 
            chat.Username,
            chat.Message
        );

        return Results.NoContent();
    }
);

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SignalRChatroomDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();
