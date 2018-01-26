using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rabbit.WeiXin.Tests
{
    public class IgnoreRepeatMessageHandlerMiddleware_Test
    {
        private static readonly IList<KeyValuePair<string, DateTime>> MessageIdentity = new List<KeyValuePair<string, DateTime>>();

        [Fact]
        public void MessageCollection_Test()
        {
            for (int i = 0; i < 8; i++)
            {
                IgnoreRepeatMessageHandler($"User{i}", DateTime.Now.AddSeconds(-i));
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
