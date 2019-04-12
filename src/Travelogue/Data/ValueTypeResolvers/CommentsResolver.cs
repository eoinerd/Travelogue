using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travelogue.Models;

namespace Travelogue.Data.ValueTypeResolvers
{
    public class CommentsResolver : ValueResolver<int, IEnumerable<Comment>>
    {
        private readonly ICommentRepository _commentRepository;
        public CommentsResolver(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        protected override IEnumerable<Comment> ResolveCore(int postId)
        {
            var test =  (IEnumerable<Comment>)_commentRepository.GetCommentsByPostId(postId);
            return test;
        }
    }
}
