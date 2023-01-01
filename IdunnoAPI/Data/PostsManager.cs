﻿using IdunnoAPI.Models;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

namespace IdunnoAPI.Data
{
    public class PostsManager
    {
        private readonly MySqlDbContext _context;

        public PostsManager(MySqlDbContext context)
        {
            _context = context;
        }

        /// <summary> postID's are signed and cannot be null -> query will be modified if this is any different
        public async Task<List<Post>> GetPostsAsync(int? postID = null) 
        {
            List<Post> posts = new List<Post>();
            Post temp = new Post();

            try
            {
                await _context.conn.OpenAsync();

                using MySqlCommand cmd = _context.conn.CreateCommand();
                cmd.CommandText = "USE idunnodb; " +
                    "SELECT PostID, UserID, date_format(PostDate, '%Y-%m-%e %H:%i') as PostDate, PostTitle, PostDescription, ImagePath " +
                    "FROM Posts";

                if(postID != null)
                {
                    cmd.CommandText += $" WHERE PostID = '{postID}'";
                }

                cmd.CommandText += ";"; 

                await using MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                while(await reader.ReadAsync())
                {
                    temp.PostID = (int)reader[0];
                    temp.UserID = (int)reader[1];
                    temp.PostDate = reader[2].ToString();
                    temp.PostTitle = reader[3].ToString();
                    temp.PostDescription = reader[4].ToString();
                    temp.ImagePath = reader[5].ToString();

                    posts.Add(temp);

                    temp = new Post();
                }

                await _context.conn.CloseAsync();
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<Post>().ToList();
            }

            return posts;
        }

        public async Task<bool> AddPostAsync(Post toBeAdded)
        {
            try
            {
                await _context.conn.OpenAsync();
                using MySqlCommand cmd = _context.conn.CreateCommand();
                cmd.CommandText = $"USE idunnodb; " +
                    $"INSERT INTO Posts " +
                    $"VALUES ({toBeAdded.PostID}, {toBeAdded.UserID}, '{toBeAdded.PostDate}', '{toBeAdded.PostTitle}', '{toBeAdded.PostDescription}', '{toBeAdded.ImagePath}');";

                if(await cmd.ExecuteNonQueryAsync() == -1)
                {
                    return false;
                }

                await _context.conn.CloseAsync();
            }
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
        public async Task<bool> DeletePostAsync(int toBeDeleted)
        {
            try
            {
                await _context.conn.OpenAsync();
                using MySqlCommand cmd = _context.conn.CreateCommand();
                cmd.CommandText = $"USE idunnodb; " +
                    $"DELETE FROM Posts " +
                    $"WHERE PostID = '{toBeDeleted}'";

                if (await cmd.ExecuteNonQueryAsync() == -1)
                {
                    return false;
                }

                await _context.conn.CloseAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdatePostAsync(int postID, Post post)
        {
            try
            {
                await _context.conn.OpenAsync();
                using MySqlCommand cmd = _context.conn.CreateCommand();


                cmd.CommandText = $"USE idunnodb; " +
                    $"UPDATE Posts " +
                    $"SET PostTitle = '{post.PostTitle}', PostDescription = '{post.PostDescription}', ImagePath = '{post.ImagePath}'" +
                    $"WHERE PostID = '{postID}'";

                if (await cmd.ExecuteNonQueryAsync() == -1)
                {
                    return false;
                }

                await _context.conn.CloseAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

    }
}
