using Microsoft.EntityFrameworkCore;
using ShareItAPI.Models;

namespace ShareItAPI
{
    public static class IQueryableExtensions
    {
        public static IQueryable<User> ByUsername(this IQueryable<User> query, string username)
        {
            return query.Where(u => u.Username == username);
        }
        public static IQueryable<Post> ById(this IQueryable<Post> query, long id)
        {
            return query.Where(q => q.Id == id);
        }
        public static IQueryable<Comment> ById(this IQueryable<Comment> query, long id)
        {
            return query.Where(q => q.Id == id);
        }
        public static IQueryable<Post> WithAuthorAndSection(this IQueryable<Post> query)
        {
            return query.Include(p => p.Author).Include(p => p.Section);
        }

        public static IQueryable<Comment> ByPostId(this IQueryable<Comment> query, long id)
        {
            return query.Where(q => q.PostId == id);
        }
        public static IQueryable<Comment> WithAuthor(this IQueryable<Comment> query)
        {
            return query.Include(c => c.Author);
        }

        public static IQueryable<T> SkipTake<T>(this IQueryable<T> query, int skip, int take)
        {
            return query.Skip(skip).Take(take);
        }
        public static IQueryable<Session> ByKeyIdAndUserId(this IQueryable<Session> query, string key, long userId)
        {
            return query.Where(s => s.UserId == userId && s.Key == key);
        }
    }
}
