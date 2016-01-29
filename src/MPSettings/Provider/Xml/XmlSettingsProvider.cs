using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MPSettings.Core;
using MPSettings.Utils;

namespace MPSettings.Provider.Xml
{
	public class XmlSettingsProvider : SettingsProviderBase
	{
		private XDocument XDoc;

		private XmlSettingsProvider()
		{

		}

		public override void Initialize(IReadOnlyDictionary<string, object> namevalue)
		{
			object datastream = namevalue["dataStream"];

			if (datastream == null)
				throw new Exception(); //MP: work here

			Stream xmlStream = datastream as Stream;
			xmlStream.Seek(0, SeekOrigin.Begin);

			XDoc = XDocument.Load(xmlStream);
		}


		private void ScanElements(SettingsPropertyName[] pathparts, int index, XElement searchelement, List<XElement> returnElement)
		{
			if (index == pathparts.Length - 1)
			{
				returnElement.AddRange(searchelement.Elements(pathparts[index].ToString()));
			}
			else if (index >= pathparts.Length)
			{
				return;
			}
			else
			{
				foreach (var xPathElemenet in searchelement.Elements(pathparts[index].ToString()))
				{
					ScanElements(pathparts, index + 1, xPathElemenet, returnElement);
				}
			}
			
			
			






			//for (int index = 1; index < prop.PropertyName.PathParts.Length; index++)
			//{
			//	string part = prop.PropertyName.PathParts[index].ToString();
			//	elem = elem.Element(part);

			//	if (elem == null)
			//		return null;

			//	SettingsContext context = prop.Context;

			//	if (context != null)
			//	{
			//		foreach (var keyvaluepair in context)
			//		{
			//			var attr = elem.Attribute(keyvaluepair.Key.ToString());
			//			if (attr == null)
			//				return null;

			//			if (attr.Value != keyvaluepair.Value.ToString())
			//				return null;
			//		}

			//	}

			//}


		}



		private XElement HasAllAttributes(SettingsProperty prop)
		{
			List<XElement> returnElements=new List<XElement>();

			ScanElements(prop.PropertyName.PathParts, 0, XDoc.Root, returnElements);

			XElement retVal = null;

			SettingsContext context = prop.Context;

			foreach (var elem in returnElements)
			{
				if (elem.Attributes().Count() != context.Count)
					continue;

				bool hasAllKeyValues = true;

				foreach (var keyvaluepair in context)
				{
					var attr = elem.Attribute(keyvaluepair.Key.ToString());
					if (attr == null)
					{
						hasAllKeyValues = false;
						break;
					}

					if (attr.Value != keyvaluepair.Value.ToString())
					{
						hasAllKeyValues = false;
						break;
					}
				}


				if (retVal != null && hasAllKeyValues)
					throw new Exception("Duplicate match");

				if (hasAllKeyValues)
					retVal = elem;
			}

			return retVal;
		}

		public override IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<SettingsProperty> collection)
		{
			foreach (var prop in collection)
			{
				XElement elem = HasAllAttributes(prop);

				if (elem != null)
				{
					var value = new SettingsPropertyValue(prop);

					SetValue(value, elem);

					yield return value;
				}
			}
		}


		private void SetValue(SettingsPropertyValue propval, XElement elem)
		{
			if (propval.SettingsProperty.PropertyType == typeof (IEnumerable))
			{
				if (propval.SettingsProperty.PropertyType == typeof (IDictionary<,>))
				{

				}
				else
				{
					
				}
			}


			propval.SerializedValue = elem.Value;
		}



		//public override bool HasPath(SettingsPropertyName path)
		//{
		//	return true; //MP: work here
		//	//return GetElement(path) != null;
		//}

		internal static XmlSettingsProvider CreateXmlSettingsProvider()
		{
			return new XmlSettingsProvider();
		}
	}
}
