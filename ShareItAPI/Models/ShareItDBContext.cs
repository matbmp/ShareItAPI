using Microsoft.EntityFrameworkCore;

namespace ShareItAPI.Models
{
    public class ShareItDBContext : DbContext
    {
        public ShareItDBContext(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().HasOne(p => p.Author).WithMany(a => a.CreatedPosts).HasForeignKey(p => p.AuthorId);
            modelBuilder.Entity<User>().HasMany(u => u.FeedPosts).WithMany(p => p.UserFeed);

            modelBuilder.Entity<PostLike>().HasKey(pl => new { pl.PostId, pl.UserId});
            modelBuilder.Entity<PostLike>().HasOne(pl => pl.Post).WithMany(p => p.UserLikes).HasForeignKey(pl => pl.PostId);
            modelBuilder.Entity<PostLike>().HasOne(pl => pl.User).WithMany(p => p.LikedPosts).HasForeignKey(pl => pl.UserId);

            modelBuilder.Entity<CommentLike>().HasKey(pl => new { pl.CommentId, pl.UserId });
            modelBuilder.Entity<CommentLike>().HasOne(pl => pl.Comment).WithMany(p => p.UserLikes).HasForeignKey(pl => pl.CommentId);
            modelBuilder.Entity<CommentLike>().HasOne(pl => pl.User).WithMany(p => p.LikedComments).HasForeignKey(pl => pl.UserId);

            modelBuilder.Entity<Comment>().HasOne(c => c.Author).WithMany(u => u.CreatedComments);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Section> Sections { get; set; }
    }
}
