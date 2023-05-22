using Riok.Mapperly.Abstractions;
using ShareItAPI.DTO;
using ShareItAPI.Models;

namespace ShareItAPI
{
    [Mapper]
    public partial class Mapper
    {
        public partial User Map(UserPostRequest request);
        public partial List<UserResponse> Map(List<User> users);
        public partial Post Map(PostPostRequest request);
        public partial List<PostResponse> Map(List<Post> posts);
        public partial PostResponse Map(Post posts);
        public partial Comment Map(PostCommentRequest request);
        public partial List<CommentResponse> Map(List<Comment> comments);
        public partial SessionResponse Map(Session session);
        public partial UserResponse Map(User user);
    }
}
