﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Douyu.Messages
{
    /// <summary>
    /// 消息转换类
    /// </summary>
    public interface IMessageConverter
    {
        string Name { get; }
    }

    /// <summary>
    /// 斗鱼消息解码抽象类
    /// </summary>
    /// <typeparam name="TDouyuMessage"></typeparam>
    public abstract class DouyuMessageDecoder<TDouyuMessage> : IMessageConverter
        where TDouyuMessage : AbstractDouyuMessage, new()
    {
        protected Dictionary<string, PropertyInfo> propertiesMap;

        public DouyuMessageDecoder()
        {
            propertiesMap = new Dictionary<string, PropertyInfo>();
            var properties = typeof(TDouyuMessage).GetProperties();
            Array.ForEach(properties, (property) =>
             {
                 if (property.Name != "type")
                 {
                     propertiesMap[property.Name] = property;
                 }
             });
        }

        public string Name
        {
            get
            {
                return typeof(TDouyuMessage).Name;
            }
        }

        protected void SetPropertyValue(TDouyuMessage message, string propertyName, object value)
        {
            if (propertiesMap.ContainsKey(propertyName))
            {
                propertiesMap[propertyName].SetValue(message, value);
            }
        }

        /// <summary>
        /// 反转义
        /// </summary>
        /// <param name="pre"></param>
        /// <returns>反转义后的数据</returns>
        protected string Unescape(string pre)
        {
            if (pre == null) { throw new ArgumentNullException(); }
            return pre.Replace("@S", "/").Replace("@A", "@");
        }

        protected string Escape(string pre)
        {
            if (pre == null) { throw new ArgumentNullException(); }
            return pre.Replace("/", "@S").Replace("@", "@A");
        }

        /// <summary>
        /// 把字符串类型的数据转换成对应的消息实体
        /// </summary>
        /// <param name="douyuMessage"></param>
        /// <returns>斗鱼消息</returns>
        public TDouyuMessage ParseString(string douyuMessage)
        {
            TDouyuMessage m = new TDouyuMessage();
            m.raw = douyuMessage;
            var differentParts = douyuMessage.Split(new string[] { "@=", "/" }, StringSplitOptions.None);
            for (int i = 0; i < differentParts.Length / 2; i++)
            {
                var key = Unescape(differentParts[2 * i]);
                var value = Unescape(differentParts[2 * i + 1]);
                SetPropertyValue(m, key, value);
            }
            return m;
        }
    }
}