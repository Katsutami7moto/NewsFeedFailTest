using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model;
using VkNet.Model.Attachments;

namespace NewsFeedFailTest
{
    public class WallPost
    {
        public string GroupOrUserImage { get; set; }
        public string GroupOrUserName { get; set; }
        public string DateText { get; set; }

        public string PostText { get; set; }
        public string PostAuthorName { get; set; } = "";

        public string LikesCountText { get; set; }
        public string RepostsCountText { get; set; }
        public string CommentsCountText { get; set; }
        public string ViewersCountText { get; set; } = "";

        public WallPost(string name, Uri photo, NewsItem newspost = null, Post wallpost = null)
        {
            if (newspost == null && wallpost == null)
            {
                throw new Exception("Отсутствие данных для создания записи стены, источник: " + name);
            }
            else if (newspost != null)
            {
                GroupOrUserName = name;
                GroupOrUserImage = photo.ToString();

                DateText = newspost.Date.Value.ToLocalTime().ToString();
                LikesCountText = newspost.Likes.Count.ToString();
                RepostsCountText = newspost.Reposts.Count.ToString();
                CommentsCountText = newspost.Comments.Count.ToString();

                // PostAuthorName = post.SignerId; - про него в схеме API забыли =)

                // ViewersCountText = post.Views.Count.ToString(); - и про него забыли

                PostText = newspost.Text;
            }
            else if (wallpost != null)
            {
                GroupOrUserName = name;
                GroupOrUserImage = photo.ToString();

                DateText = wallpost.Date.Value.ToLocalTime().ToString();
                LikesCountText = wallpost.Likes.Count.ToString();
                RepostsCountText = wallpost.Reposts.Count.ToString();
                CommentsCountText = wallpost.Comments.Count.ToString();

                if (wallpost.Views == null)
                {
                    ViewersCountText = "N/A";
                }
                else
                {
                    ViewersCountText = wallpost.Views.Count.ToString();
                }

                if (wallpost.SignerId.HasValue)
                {
                    var user = VkAuth.Api.Users.Get(new List<long>() { wallpost.SignerId.Value }).FirstOrDefault();
                    PostAuthorName = "Автор записи: " + user.FirstName + " " + user.LastName;
                }

                PostText = wallpost.Text;
            }
        }
    }
}
