using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShareItAPI.Models;
using ShareItAPI.DTO;
using ShareItAPI;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;
using ShareItAPI.Migrations;

var builder = WebApplication.CreateBuilder(args);

AuthSettings authSettings = new AuthSettings();
builder.Configuration.GetSection("Auth").Bind(authSettings);
builder.Services.AddSingleton(authSettings);

builder.Services.AddTransient<CustomAuthenticationHandler>();
builder.Services.AddAuthentication(authSettings.AuthenticationScheme)
        .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>(authSettings.AuthenticationScheme, null);
builder.Services.AddAuthorization(options =>
{
    var AnyAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(authSettings.AuthenticationScheme);
    AnyAuthorizationPolicyBuilder = AnyAuthorizationPolicyBuilder.RequireAuthenticatedUser();
    options.DefaultPolicy = AnyAuthorizationPolicyBuilder.Build();
});

builder.Services.AddSingleton<Mapper>();
builder.Services.AddDbContext<ShareItDBContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("ShareItConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(setup =>
{
    setup.AddDefaultPolicy(opts =>
    {
        opts.AllowAnyHeader();
        opts.AllowAnyMethod();
        opts.AllowAnyOrigin();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/user", async ([FromServices] ShareItDBContext db, [FromServices] Mapper mapper) =>
{
    return mapper.Map(await db.Users.ToListAsync());
});

app.MapPost("/user", async ([FromBody] UserPostRequest request, [FromServices] ShareItDBContext db, [FromServices] Mapper mapper) =>
{
    var user = mapper.Map(request);
    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
    user.CreatedDate = DateTime.UtcNow;
    db.Add(user);
    await db.SaveChangesAsync();
    return Results.Created($"/user/{user.Id}", user.Id);
});
app.MapDelete("/user", [Authorize] async ([FromServices] ShareItDBContext db, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    db.Remove(user);
    await db.SaveChangesAsync();
    return Results.Ok();
});
app.MapPut("/user/modify", [Authorize] async ([FromBody] UserPutRequest request, [FromServices] ShareItDBContext db, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    if (request.Username is not null)
    {
        user.Username = request.Username;
    }
    if (request.Password is not null)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
    }
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/session", async ([FromBody]SessionPostRequest request, [FromServices] ShareItDBContext db, [FromServices]Mapper mapper) =>
{
    var user = await db.Users.ByUsername(request.Username).FirstOrDefaultAsync();
    if (user is null)
    {
        return Results.NotFound();
    }
    if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
    {
        return Results.NotFound();
    }

    string key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(256));
    var session = new Session() { Key = key, UserId = user.Id };
    db.Sessions.Add(session);
    await db.SaveChangesAsync();
    return Results.Ok(mapper.Map(session));
});
app.MapDelete("/session", [Authorize] async ([FromQuery] string key, [FromServices] ShareItDBContext db, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    if (await db.Sessions.ByKeyIdAndUserId(key, user.Id).FirstOrDefaultAsync() is not Session session) return Results.NotFound();
    db.Sessions.Remove(session);
    await db.SaveChangesAsync();
    return Results.Ok();
});
/*
app.MapGet("/session", [Authorize] async ([FromServices] ShareItDBContext db, HttpContext context, [FromServices]Mapper mapper) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    return Results.Ok(mapper.Map(user));
});
*/

app.MapGet("/post/{postId}", async (long postId,
    [FromServices] ShareItDBContext db, [FromServices] Mapper mapper, HttpContext context) =>
{
    if (await db.Posts.ById(postId).WithAuthorAndSection().FirstOrDefaultAsync() is not Post post) return Results.NotFound();
    var mapped = mapper.Map(post);
    if (await getLoggedUserAsync(db, context) is User user)
    {
        mapped.IsLiked = await db.PostLikes.AnyAsync(pl => pl.PostId == postId && pl.UserId == user.Id);
    }
    return Results.Ok(mapped);
});

app.MapGet("/post", async ([FromQuery]int take,[FromQuery] int skip, [FromQuery] bool? refresh,
    [FromServices] ShareItDBContext db, [FromServices]Mapper mapper, HttpContext context) =>
{
    var post = await db.Posts.OrderByDescending(p => p.CreatedDate).SkipTake(skip, take).WithAuthorAndSection().ToListAsync();
    var mapped = mapper.Map(post);
    if (await getLoggedUserAsync(db, context) is User user)
    {
        foreach(var m in mapped)
        {
            m.IsLiked = await db.PostLikes.AnyAsync(pl => pl.PostId == m.Id && pl.UserId == user.Id);
        }
    }
    return Results.Ok(mapped);
    /*
    if (await getLoggedUserAsync(db, context) is User user)
    {
        if (refresh.HasValue && refresh.Value)
        {
            db.Database.ExecuteSqlInterpolated(@$"
            DELETE FROM ""PostUser"" WHERE ""UserFeedId"" = {user.Id};
            INSERT INTO ""PostUser""(""FeedPostsId"", ""UserFeedId"")  (
	        SELECT ""Posts"".""Id"", {user.Id} FROM ""Sections""
	        JOIN ""Users"" ON ""Sections"".""UserId"" = ""Users"".""Id""
	        JOIN ""Posts"" ON ""Sections"".""UserId"" = ""Users"".""Id""
	        WHERE ""UserId"" = {user.Id}
	        LIMIT 400
            );
        ");
        }
        await db.SaveChangesAsync();
        if (await db.Users.ByUsername(user.Username)
        .Include(u => u.FeedPosts.Skip(skip).Take(take)).ThenInclude(p => p.Section)
        .Include(u => u.FeedPosts.Skip(skip).Take(take)).ThenInclude(p => p.Author)
        .FirstOrDefaultAsync() is not User user2) return Results.Problem();
        return Results.Ok(mapper.Map(user2.FeedPosts));
    }
    else
    {
        // Generate recent feed
        return Results.Ok(mapper.Map(await db.Posts.OrderByDescending(p => p.CreatedDate).Include(p => p.Section).Include(p => p.Author).SkipTake(skip, take).ToListAsync()));
    }
    */
});

app.MapPost("/post", [Authorize] async ([FromBody] PostPostRequest request, [FromServices] ShareItDBContext db, [FromServices] Mapper mapper, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    var post = mapper.Map(request);
    post.CreatedDate = DateTime.UtcNow;
    post.AuthorId = user.Id;
    db.Posts.Add(post);
    await db.SaveChangesAsync();
    return Results.Created($"/post/{post.Id}", post.Id);
});
app.MapDelete("/post/{id}", [Authorize] async (int id, [FromServices] ShareItDBContext db, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    if (await db.Posts.ById(id).FirstOrDefaultAsync() is not Post post) return Results.BadRequest();
    if (post.Id != id) return Results.Forbid();
    db.Posts.Remove(post);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/post/{postId}/comment", [Authorize] async (long postId, [FromBody] PostCommentRequest request,
       [FromServices] ShareItDBContext db, [FromServices] Mapper mapper, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    if(await db.Posts.ById(postId).FirstOrDefaultAsync() is not Post post) return Results.NotFound();

    Comment comment = mapper.Map(request);
    comment.AuthorId = user.Id;
    comment.PostId = post.Id;
    comment.CreatedDate = DateTime.UtcNow;
    post.Comments.Add(comment);
    await db.SaveChangesAsync();

    return Results.Created($"/post/{post.Id}/comment/{comment.Id}", comment.Id);
});

app.MapGet("/post/{postId}/comment", async (long postId, [FromQuery] int skip, [FromQuery] int take,
       [FromServices] ShareItDBContext db, [FromServices] Mapper mapper, HttpContext context) =>
{
    if (await db.Posts.ById(postId).FirstOrDefaultAsync() is not Post post) return Results.NotFound();
    var comments = mapper.Map(await db.Comments.ByPostId(post.Id).SkipTake(skip, take).WithAuthor().ToListAsync());
    if (await getLoggedUserAsync(db, context) is User user)
    {
        foreach(var comment in comments)
        {
            comment.IsLiked = await db.CommentLikes.AnyAsync(pl => pl.CommentId == comment.Id && pl.UserId == user.Id);
        }
    }
    return Results.Ok(comments);
});

app.MapDelete("/post/comment/{commentId}", [Authorize]async (long commentId,
       [FromServices] ShareItDBContext db, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    if (await db.Comments.ById(commentId).FirstOrDefaultAsync() is not Comment comment) return Results.NotFound();
    if (comment.AuthorId != user.Id) return Results.Forbid();

    db.Comments.Remove(comment);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/post/{postId}/like", [Authorize] async (long postId, [FromServices] ShareItDBContext db, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    if (await db.Posts.ById(postId).FirstOrDefaultAsync() is not Post post) return Results.NotFound();
    if(await db.PostLikes.Where(pl => pl.PostId == postId && pl.UserId == user.Id).FirstOrDefaultAsync() is PostLike postLike)
    {
        db.PostLikes.Remove(postLike);
        post.Likes--;
    }
    else
    {
        db.PostLikes.Add(new PostLike()
        {
            PostId = postId,
            UserId = user.Id,
        });
        post.Likes++;
    }
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/post/comment/{commentId}/like", [Authorize] async (long commentId, [FromServices] ShareItDBContext db, HttpContext context) =>
{
    if (await getLoggedUserAsync(db, context) is not User user) return Results.Problem();
    if (await db.Comments.ById(commentId).FirstOrDefaultAsync() is not Comment comment) return Results.NotFound();
    if (await db.CommentLikes.Where(pl => pl.CommentId == commentId && pl.UserId == user.Id).FirstOrDefaultAsync() is CommentLike commentLike)
    {
        db.CommentLikes.Remove(commentLike);
        comment.Likes--;
    }
    else
    {
        db.CommentLikes.Add(new CommentLike()
        {
            CommentId = commentId,
            UserId = user.Id,
        });
        comment.Likes++;
    }
    await db.SaveChangesAsync();
    return Results.Ok();
});


async Task<User?> getLoggedUserAsync(ShareItDBContext db, HttpContext context)
{
    if (context.User.Identity is not ClaimsIdentity identity) return null;
    if (identity.FindFirst(ClaimTypes.Name) is not Claim nameClaim) return null;
    return await db.Users.ByUsername(nameClaim.Value).FirstOrDefaultAsync();
}

app.Run();
