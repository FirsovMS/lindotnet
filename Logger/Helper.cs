﻿using System.IO;
using System.Xml.Serialization;

namespace LoggingAPI
{
	internal static class Helper
	{
		public static string SerializeObject<T>(this T toSerialize)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

			using (StringWriter textWriter = new StringWriter())
			{
				xmlSerializer.Serialize(textWriter, toSerialize);
				return textWriter.ToString();
			}
		}
	}
}
