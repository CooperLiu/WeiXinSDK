using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Rabbit.WeiXin.Tests
{
    public class IgnoreRepeatMessageHandlerMiddleware_Test
    {
        private static readonly IList<KeyValuePair<string, DateTime>> MessageIdentity = new List<KeyValuePair<string, DateTime>>();

        private static Random random;

        [Fact]
        public void MessageCollection_Test()
        {
            Task t1 = Task.Factory.StartNew(TestCase);
            Task t2 = Task.Factory.StartNew(TestCase);
            Task t3 = Task.Factory.StartNew(TestCase);
            Task t4 = Task.Factory.StartNew(TestCase);

            Task.WaitAll(t1, t2, t3, t4);
        }

        private void TestCase()
        {
            random = new Random(1000);

            for (int i = 0; i < 200; i++)
            {
                var rnd = random.Next();
                var rndIndex = rnd % 2;
                if (rndIndex == 0)
                {
                    IgnoreRepeatMessageHandler($"User{rndIndex}", DateTime.Now.AddMilliseconds(-500));
                }

            }
        }

        private void IgnoreRepeatMessageHandler(string userName, DateTime messageTime)
        {
            var identity = $"{userName}{messageTime}";

            //Fixed issues: 集合已修改；可能无法执行枚举操作。
            var repeatMessageList = MessageIdentity.Where(m => m.Value.AddSeconds(5) < DateTime.Now).ToList();

            foreach (var msg in repeatMessageList)
            {
                MessageIdentity.Remove(msg);
            }

            var cacheMessages = MessageIdentity.ToList();



            //如果消息已经被标识为处理则跳过。
            if (cacheMessages.Any(i => i.Key == identity))
            {
                return;
            }

            MessageIdentity.Add(new KeyValuePair<string, DateTime>(identity, DateTime.Now));
        }
    }
}
