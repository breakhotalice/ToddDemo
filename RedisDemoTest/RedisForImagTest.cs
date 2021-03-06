﻿using redisDemo;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;


namespace RedisDemoTest
{
    public class RedisForImagTest
    {
        redisDemo.RedisHelper helper = null;

        public RedisForImagTest()
        {
            helper = new redisDemo.RedisHelper();
        }

        /// <summary>
        /// 存储图片测试
        /// </summary>
        [Fact]
        public void SaveImageTest()
        {
            string[] DicFile = Directory.GetFiles(@"D:\test\1\");
            int index = 0;
            foreach (var item in DicFile)
            {
                index++;
                // Image img = Image.FromFile(item);
                // string str = ImageConvert.ToBase64String(img);
                var base64Img = Convert.ToBase64String(System.IO.File.ReadAllBytes(item));
                helper.SetStringKey("img" + index, base64Img);
            }

            string f1 = @"D:\test\2\";
            for (int i = 1; i <= index; i++)
            {
                string key = "img" + i;
                string str = helper.GetStringKey(key);

                string imgName = f1 + key + ".jpg";
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(str)))
                {
                    Bitmap img = new Bitmap(stream);
                    img.Save(imgName);
                }
                //    MemoryStream stream = new MemoryStream(Convert.FromBase64String(str));
                //Bitmap img = new Bitmap(stream);
                //img.Save(imgName);


                //  var base64Img = Convert.ToBase64String(System.IO.File.ReadAllBytes(imgName));
                // /*Image*/ img = ImageConvert.FromBase64String(str);
                // string imgName = f1 + key + ".jpg";
                // img.Save(imgName);
            }
        }

        [Fact]
        public void SaveImageTest1()
        {
            string[] DicFile = Directory.GetFiles(@"D:\test\1\");
            int index = 0;
            List<KeyValuePair<RedisKey, RedisValue>> list = new List<KeyValuePair<RedisKey, RedisValue>>();
            foreach (var item in DicFile)
            {
                index++;
                // Image img = Image.FromFile(item);
                // string str = ImageConvert.ToBase64String(img);
                var base64Img = Convert.ToBase64String(System.IO.File.ReadAllBytes(item));
                // helper.SetStringKey("img" + index, base64Img);

                KeyValuePair<RedisKey, RedisValue> keyvalue = new KeyValuePair<RedisKey, RedisValue>(key: "img" + index, value: base64Img) { };
                list.Add(keyvalue);
            }
            helper.SetStringKey(list.ToArray());


            string f1 = @"D:\test\2\";
            List<RedisKey> listKey = new List<RedisKey>();
        
            for (int i = 1; i <= index; i++)
            {
                string key = "img" + i;
                listKey.Add(key);



                string str = helper.GetStringKey(key);

                string imgName = f1 + key + ".jpg";
                using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(str)))
                {
                    Bitmap img = new Bitmap(stream);
                    img.Save(imgName);
                }
            }

            //RedisValue[] arrv = helper.GetStringKey(listKey);
        }
    }
}
