using IdunnoAPI.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using IdunnoAPI.DAL.Repositories.Interfaces;
using IdunnoAPI.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using IdunnoAPI.Models.Posts;

namespace IdunnoAPI.UnitTests.Repositories
{
    public class PostsRepositoryTests
    {
        private readonly IPostRepository _postRepository;
        public PostsRepositoryTests()
        {
           DbContextOptions<IdunnoDbContext> options = new DbContextOptionsBuilder<IdunnoDbContext>().
                UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            IdunnoDbContext dbContext = new IdunnoDbContext(options);
            _postRepository = new PostRepository(dbContext);
        }

        [Fact]
        public async Task AddPostAsync_GivenValidRequest_NewPostIdReturned()
        {
            // Arrange

            Post testedPost = new Post { PostTitle= "Test", PostDescription="Test", ImagePath="Test"};

            // Act

            int postIdAsResult = await _postRepository.AddPostAsync(testedPost);

            // Assert

            Assert.NotNull(_postRepository.GetPostsAsQueryable().FirstOrDefault());
            Assert.Equal(_postRepository.GetPostsAsQueryable().FirstOrDefault(), testedPost);
            Assert.Equal(postIdAsResult, _postRepository.GetPostsAsQueryable().FirstOrDefault().PostId);
        }

        [Fact]
        public async Task AddPostAsync_GivenRequestHasNoTitle_ExpectedDbUpdateException()
        {
            // Arrange

            Post testedPost = new Post {PostDescription = "Test", ImagePath = "Test" };

            await Assert.ThrowsAsync<DbUpdateException>(async () => await _postRepository.AddPostAsync(testedPost) );
        }

        [Fact]
        public async Task AddPostAsync_GivenRequestHasNoDescription_ExpectedDbUpdateException()
        {
            // Arrange

            Post testedPost = new Post {PostTitle = "Test", ImagePath = "Test" };

            await Assert.ThrowsAsync<DbUpdateException>(async () => await _postRepository.AddPostAsync(testedPost));
        }

        
    }
}
