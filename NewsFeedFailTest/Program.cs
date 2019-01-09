using System;
using System.Linq;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using System.Threading.Tasks;
using VkNet.Exception;

namespace NewsFeedFailTest
{
    public class Program
    {

        private static void PrintWallPost(WallPost wallPost)
        {
            Console.WriteLine("Аватарка: {0}", wallPost.GroupOrUserImage);
            Console.WriteLine("Название/имя: {0}", wallPost.GroupOrUserName);
            Console.WriteLine("Дата: {0}", wallPost.DateText);

            Console.WriteLine();

            Console.WriteLine(wallPost.PostText);
            Console.WriteLine();
            Console.WriteLine("Автор поста: {0}", wallPost.PostAuthorName);

            Console.WriteLine();

            Console.WriteLine("Лайки: {0}", wallPost.LikesCountText);
            Console.WriteLine("Репосты: {0}", wallPost.RepostsCountText);
            Console.WriteLine("Комменты: {0}", wallPost.CommentsCountText);
            Console.WriteLine("Просмотры: {0}", wallPost.ViewersCountText);
            Console.WriteLine();
        }

        public static async Task ShowNewsFeed()
        {
            NewsFeed news_feed = await VkAuth.Api.NewsFeed.GetAsync(new NewsFeedGetParams() { Filters = NewsTypes.Post, Count = 20 });

            var items = news_feed.Items;
            var users = news_feed.Profiles;
            var groups = news_feed.Groups;
            string name;
            Uri photo;

            int i = 0;

            foreach (var item in items)
            {
                var source_id = item.SourceId;
                if (source_id < 0)
                {
                    var group =
                        groups
                        .Where(x => x.Id == (0 - source_id))
                        .FirstOrDefault();
                    name = group.Name;
                    photo = VkAuth.Api.Groups.GetById(null, group.Id.ToString(), null).FirstOrDefault().Photo50;
                }
                else
                {
                    var user =
                        users
                        .Where(x => x.Id == source_id)
                        .FirstOrDefault();
                    name = user.FirstName + " " + user.LastName;
                    photo = user.Photo50;
                }

                Console.WriteLine("===================");
                Console.WriteLine("Пост {0}: ", i++);
                Console.WriteLine();

                PrintWallPost(new WallPost(name, photo, newspost: item));
            }
        }

        private static async void ShowNewsFeedAsync()
        {
            await ShowNewsFeed();
        }

        private static void Authorize()
        {
            try
            {
                VkAuth.ByToken();
            }
            catch
            {
                Console.WriteLine("Access token has expired. Please sign in again.");
                Console.WriteLine("Enter login: ");
                var login = Console.ReadLine();
                Console.WriteLine("Enter password: ");
                var password = Console.ReadLine();
                try
                {
                    VkAuth.ByLogin(login, password);
                }
                catch (VkApiAuthorizationException error)
                {
                    Console.WriteLine(error.ToString());
                    Console.ReadLine();
                }
            }
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Authorize();

            Console.WriteLine("Press Enter to load newsfeed: ");
            Console.ReadLine();

            ShowNewsFeedAsync();

            Console.ReadLine();

            NLog.LogManager.Shutdown();
        }
    }
}
