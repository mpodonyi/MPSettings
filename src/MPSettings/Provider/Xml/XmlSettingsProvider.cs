using System;
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


		private void GetElements(SettingsPropertyName[] pathparts, int index, XElement searchelement, List<XElement> returnElement)
		{
			XElement elem = XDoc.Root;

			for (int index = 1; index < prop.PropertyName.PathParts.Length; index++)
			{
				string part = prop.PropertyName.PathParts[index].ToString();
				elem = elem.Element(part);

				if (elem == null)
					return null;

				SettingsContext context = prop.Context;

				if (context != null)
				{
					foreach (var keyvaluepair in context)
					{
						var attr = elem.Attribute(keyvaluepair.Key.ToString());
						if (attr == null)
							return null;

						if (attr.Value != keyvaluepair.Value.ToString())
							return null;
					}

				}

			}


		}



		private XElement GetElement(SettingsProperty prop)
		{
			List<XElement> returnElement=new List<XElement>();

			GetElements(prop.PropertyName.PathParts, 0, XDoc.Root, returnElement);

			XElement retVal = null;

			SettingsContext context = prop.Context;
			foreach (var elem in returnElement)
			{
				foreach (var keyvaluepair in context)
				{
					var attr = elem.Attribute(keyvaluepair.Key.ToString());
					if (attr == null)
						return null;

					if (attr.Value != keyvaluepair.Value.ToString())
						return null;
				}
				
			}

			

			//XElement elem = XDoc.Root;

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
			//			if(attr == null)
			//				return null;

			//			if (attr.Value != keyvaluepair.Value.ToString())
			//				return null;
			//		}

			//	}

			//}

			return elem;
		}

		public override IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<SettingsProperty> collection)
		{
			foreach (var prop in collection)
			{
				XElement elem = GetElement(prop);

				if (elem != null)
				{
					yield return new SettingsPropertyValue(prop) { SerializedValue = elem.Value };
				}
			}
		}

		public override bool HasPath(SettingsPropertyName path)
		{
			return true; //MP: work here
			//return GetElement(path) != null;
		}

		internal static XmlSettingsProvider CreateXmlSettingsProvider()
		{
			return new XmlSettingsProvider();
		}
	}
}
