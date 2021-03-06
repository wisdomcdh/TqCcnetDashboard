﻿using Exortech.NetReflector;
using Exortech.NetReflector.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace TqLib.ccnet.Core.Util
{
    public class SiteConfigSerializer : XmlMemberSerialiser
    {
        public SiteConfigSerializer(ReflectorMember member, ReflectorPropertyAttribute attribute) : base(member, attribute)
        {
        }

        public override object Read(XmlNode node, NetReflectorTypeTable table)
        {
            Dictionary<string, string> config = GetDefaultConfig();

            if (node != null)
            {
                var str = node.InnerText;

                if (!string.IsNullOrEmpty(str))
                {
                    string[] lines = str.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    var line = lines.Select(t => t.Split('='))
                        .Where(t => t.Length >= 2)
                        .Select(t => new Tuple<string, string>(t[0].Trim(), t[1].Trim()))
                        .Where(t => !string.IsNullOrEmpty(t.Item1) && !string.IsNullOrEmpty(t.Item2));
                    line.ToList().ForEach(t => { config[t.Item1] = t.Item2; });
                }
            }
            return config;
        }

        public override void Write(XmlWriter writer, object target)
        {
            if (target == null)
            {
                return;
            }
            if (!(target is Dictionary<string, string>))
            {
                target = base.ReflectorMember.GetValue(target);
            }

            Dictionary<string, string> dic = target as Dictionary<string, string>;
            string value = string.Join("\n", dic.Select(t => t.Key + "=" + t.Value));
            writer.WriteElementString(base.Attribute.Name, value);
        }

        private Dictionary<string, string> GetDefaultConfig()
        {
            return new Dictionary<string, string>
            {
            };
        }
    }
}