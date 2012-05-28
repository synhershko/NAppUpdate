using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text;
using System.Configuration;
using System.Xml.Serialization;
using System.Collections.Specialized;
using Microsoft.VisualBasic.Devices;
namespace FeedBuilder
{

	public class FeedBuilderSettingsProvider : SettingsProvider
	{

			//XML Root Node
		private const string SETTINGSROOT = "Settings";

		public void SaveAs(string filename)
		{
			try {
				var settings = Settings.Default;
				settings.Save();
				string source = Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());
				File.Copy(source, filename, true);
			} catch (Exception ex) {
				string msg = string.Format("An error occurred while saving the file: {0}{0}{1}", Environment.NewLine, ex.Message);
				MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void LoadFrom(string filename)
		{
			try {
				string dest = Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());
				if (filename == dest)
					return;
				File.Copy(filename, dest, true);
				Settings.Default.Reload();
			} catch (Exception ex) {
				string msg = string.Format("An error occurred while loading the file: {0}{0}{1}", Environment.NewLine, ex.Message);
				MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public override void Initialize(string name, NameValueCollection col)
		{
			base.Initialize(this.ApplicationName, col);
			if (!Directory.Exists(this.GetAppSettingsPath())) {
				try {
					Directory.CreateDirectory(this.GetAppSettingsPath());
				} catch (IOException) {
				}
			}
		}

		public override string ApplicationName {
			get { return "FeedBuilder"; }
				//Do nothing
			set { }
		}

		public virtual string GetAppSettingsPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), this.ApplicationName);
		}

		public virtual string GetAppSettingsFilename()
		{
			return "Settings.xml";
		}

		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
		{
			//Iterate through the settings to be stored
			//Only dirty settings are included in propvals, and only ones relevant to this provider
			foreach (SettingsPropertyValue propval in propvals) {
				SetValue(propval);
			}

			try {
				if (!Directory.Exists(this.GetAppSettingsPath()))
					Directory.CreateDirectory(this.GetAppSettingsPath());
				SettingsXML.Save(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
			} catch (Exception) {
				//Ignore if cant save, device been ejected
			}
		}

		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
		{
			//Create new collection of values
			SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

			//Iterate through the settings to be retrieved

			foreach (SettingsProperty setting in props) {
				SettingsPropertyValue value = new SettingsPropertyValue(setting);
				value.IsDirty = false;
				value.SerializedValue = GetValue(setting);
				values.Add(value);
			}
			return values;
		}


		private System.Xml.XmlDocument m_SettingsXML = null;
		private XmlDocument SettingsXML {
			get {
				//If we dont hold an xml document, try opening one.  
				//If it doesnt exist then create a new one ready.
				if (m_SettingsXML == null) {
					m_SettingsXML = new System.Xml.XmlDocument();

					try {
						m_SettingsXML.Load(System.IO.Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
					} catch (Exception) {
						//Create new document
						XmlDeclaration dec = m_SettingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
						m_SettingsXML.AppendChild(dec);

						XmlNode nodeRoot = null;

						nodeRoot = m_SettingsXML.CreateNode(XmlNodeType.Element, SETTINGSROOT, "");
						m_SettingsXML.AppendChild(nodeRoot);
					}
				}

				return m_SettingsXML;
			}
		}

		private string GetValue(SettingsProperty setting)
		{
			string ret = "";
			string path = null;

			try {
				if (IsRoaming(setting)) {
					path = string.Format("{0}/{1}", SETTINGSROOT, setting.Name);
				} else {
					path = string.Format("{0}/{1}/{2}", SETTINGSROOT, new Computer().Name, setting.Name);
				}

				if (setting.PropertyType.BaseType.Name == "CollectionBase") {
					ret = SettingsXML.SelectSingleNode(path).InnerXml;
				} else {
					ret = SettingsXML.SelectSingleNode(path).InnerText;
				}
			} catch (Exception) {
				if ((setting.DefaultValue != null)) {
					ret = setting.DefaultValue.ToString();
				} else {
					ret = string.Empty;
				}
			}

			return ret;
		}


		private void SetValue(SettingsPropertyValue propVal)
		{
			System.Xml.XmlElement MachineNode = null;
			System.Xml.XmlElement SettingNode = null;

			//Determine if the setting is roaming.
			//If roaming then the value is stored as an element under the root
			//Otherwise it is stored under a machine name node 
			try {
				if (IsRoaming(propVal.Property)) {
					SettingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + propVal.Name);
				} else {
					SettingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + new Computer().Name + "/" + propVal.Name);
				}
			} catch (Exception) {
				SettingNode = null;
			}

			//Check to see if the node exists, if so then set its new value
			if ((SettingNode != null)) {
				//SettingNode.InnerText = propVal.SerializedValue.ToString
				SetSerializedValue(SettingNode, propVal);
			} else {
				if (IsRoaming(propVal.Property)) {
					//Store the value as an element of the Settings Root Node
					SettingNode = SettingsXML.CreateElement(propVal.Name);
					//SettingNode.InnerText = propVal.SerializedValue.ToString
					SetSerializedValue(SettingNode, propVal);
					SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(SettingNode);
				} else {
					//Its machine specific, store as an element of the machine name node,
					//creating a new machine name node if one doesnt exist.
					try {
						MachineNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + new Computer().Name);
					} catch (Exception) {
						MachineNode = SettingsXML.CreateElement(new Computer().Name);
						SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
					}

					if (MachineNode == null) {
						MachineNode = SettingsXML.CreateElement(new Computer().Name);
						SettingsXML.SelectSingleNode(SETTINGSROOT).AppendChild(MachineNode);
					}

					SettingNode = SettingsXML.CreateElement(propVal.Name);
					//SettingNode.InnerText = propVal.SerializedValue.ToString
					SetSerializedValue(SettingNode, propVal);
					MachineNode.AppendChild(SettingNode);
				}
			}
		}

		private void SetSerializedValue(System.Xml.XmlElement node, SettingsPropertyValue propVal)
		{
			if (propVal.Property.PropertyType.BaseType.Name == "CollectionBase") {
				StringBuilder builder = new StringBuilder();
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				XmlWriterSettings xsettings = new XmlWriterSettings();
				XmlWriter xmlWriter = null;
				XmlSerializer s = null;

				ns.Add("", "");
				xsettings.OmitXmlDeclaration = true;
				xmlWriter = XmlWriter.Create(builder, xsettings);
				s = new XmlSerializer(propVal.Property.PropertyType);
				s.Serialize(xmlWriter, propVal.PropertyValue, ns);
				xmlWriter.Close();
				node.InnerXml = builder.ToString();
			} else {
				node.InnerText = propVal.SerializedValue.ToString();
			}
		}

		private bool IsRoaming(SettingsProperty prop)
		{
			//Determine if the setting is marked as Roaming
			foreach (DictionaryEntry d in prop.Attributes) {
				Attribute a = (Attribute)d.Value;
				if (a is System.Configuration.SettingsManageabilityAttribute) {
					return true;
				}
			}
			return false;
		}
	}
}
