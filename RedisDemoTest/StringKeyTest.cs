﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace RedisDemoTest
{
    public class StringKeyTest
    {
        IDatabase db = null;
        public StringKeyTest()
        {
            //在这里可以做测试开始前的初始化工作 
            string connstr = "127.0.0.1:6379,allowadmin=true";
            ConnectionMultiplexer conn = ConnectionMultiplexer.Connect(connstr);
            db = conn.GetDatabase(0);
        }

        /// <summary>
        /// 存储一个key
        /// </summary>
        [Fact]
        public void SetKeyString()
        {//
            //key-val存储
            //bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry = default(TimeSpan?), When when = When.Always, CommandFlags flags = CommandFlags.None);
            //expiry过期时间
            bool stringsetResult = db.StringSet("mykeyx", "myvalue11", TimeSpan.FromMilliseconds(10000), When.Always, CommandFlags.None);
            Assert.True(stringsetResult);
        }

        /// <summary>
        /// 存储多个key
        /// </summary>
        [Fact]
        public void SetKeyDic()
        {
            //key-val组存储
            //bool StringSet(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None);

            List<KeyValuePair<RedisKey, RedisValue>> listKey = new List<KeyValuePair<RedisKey, RedisValue>>();
            listKey.Add(new KeyValuePair<RedisKey, RedisValue>("mykey1", "myvalue1"));
            listKey.Add(new KeyValuePair<RedisKey, RedisValue>("mykey2", "myvalue2"));
            listKey.Add(new KeyValuePair<RedisKey, RedisValue>("mykey3", "myvalue3"));
            bool value = db.StringSet(listKey.ToArray());
            Assert.True(value);
        }

        /// <summary>
        /// 获取单个key
        /// </summary>
        [Fact]
        public void GetKey()
        {
            string Key = "test123";
            string Value = "111112222222233";

            db.StringSet(Key, Value);

            RedisValue value = db.StringGet(Key);
            Assert.Equal(Value, value.ToString());
        }

        /// <summary>
        /// 获取多个key
        /// </summary>
        [Fact]
        public void GetKeys()
        {
            List<KeyValuePair<RedisKey, RedisValue>> listKey = new List<KeyValuePair<RedisKey, RedisValue>>();
            listKey.Add(new KeyValuePair<RedisKey, RedisValue>("mykey1", "测试key1"));
            listKey.Add(new KeyValuePair<RedisKey, RedisValue>("mykey2", "测试key2"));
            listKey.Add(new KeyValuePair<RedisKey, RedisValue>("mykey3", "测试key3"));
            db.StringSet(listKey.ToArray());
            //获取一组val
            RedisValue[] rvalues = db.StringGet(new RedisKey[] { "mykey1", "mykey2", "mykey3", "mykey3x" });
            Assert.Equal(rvalues.Count(), 4);
        }

        /// <summary>
        /// 获取带有过期时间的val
        /// </summary>
        [Fact]
        public void StringGetWithExpiry()
        {
            string Key = "mykeyt";

            db.StringSet(Key, "myvalue", TimeSpan.FromHours(1), When.Always, CommandFlags.None);
            RedisValueWithExpiry rvalwithxp = db.StringGetWithExpiry(Key);

        }

        [Fact]
        public void ttt()
        {
            //bool HashSet(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None);
            db.HashSet("hashkey1", "name", "lily");
            //void HashSet(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None);
            db.HashSet("hashkey2", new HashEntry[] { new HashEntry("name", "lucy"), new HashEntry("age", 18) , new HashEntry("agex", 12) });


            //2.1.2 get
            //RedisValue HashGet(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None);
            RedisValue rhashval = db.HashGet("hashkey1", "name");
            //RedisValue[] HashGet(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None);
            RedisValue[] rhashvals = db.HashGet("hashkey2", new RedisValue[] { "name", "age" });
            RedisValue rhashva2 = db.HashGet("hashkey2", "name");
            //HashEntry[] HashGetAll(RedisKey key, CommandFlags flags = CommandFlags.None);
            HashEntry[] hashentitrys = db.HashGetAll("hashkey2");

            //long HashIncrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None);
            //double HashIncrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None);
            db.HashIncrement("hashkey2", "age", 3);

        }


        public void Dispose()
        {
            //在这里可以做测试结束后的收尾工作 
            System.Console.WriteLine("Dispose");
        }
    }
}
