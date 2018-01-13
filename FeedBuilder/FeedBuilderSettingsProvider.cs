using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using FeedBuilder.Properties;

namespace FeedBuilder
{
	public class FeedBuilderSettingsProvider : SettingsProvider, IApplicationSettingsProvider
	{
		//XML Root Node
		private const string SETTINGSROOT = "Settings";

		public void SaveAs(string filename)
		{
			try
			{
				Settings.Default.Save();
				string source = Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());
				File.Copy(source, filename, true);
			}
			catch (Exception ex)
			{
				string msg = string.Format("An error occurred while saving the file: {0}{0}{1}", Environment.NewLine, ex.Message);
				MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public void LoadFrom(string filename)
		{
			try
			{
				string dest = Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());
				if (filename == dest) return;
				Settings.Default.Reset();
				File.Copy(filename, dest, true);
			}
			catch (Exception ex)
			{
				string msg = string.Format("An error occurred while loading the file: {0}{0}{1}", Environment.NewLine, ex.Message);
				MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public override void Initialize(string name, NameValueCollection col)
		{
			base.Initialize(ApplicationName, col);
			if (!Directory.Exists(GetAppSettingsPath()))
			{
				try
				{
					Directory.CreateDirectory(GetAppSettingsPath());
				}
				catch (IOException) { }
			}
		}

		public override string ApplicationName
		{
			get { return "FeedBuilder"; }
			//Do nothing
			set { }
		}

		public virtual string GetAppSettingsPath()
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ApplicationName);
		}

		public virtual string GetAppSettingsFilename()
		{
			return "Settings.xml";
		}

		public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection propvals)
		{
			//Iterate through the settings to be stored
			//Only dirty settings are included in propvals, and only ones relevant to this provider
			foreach (SettingsPropertyValue propval in propvals)
			{
				SetValue(propval);
			}

			try
			{
				if (!Directory.Exists(GetAppSettingsPath())) Directory.CreateDirectory(GetAppSettingsPath());
				SettingsXML.Save(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
			}
			catch (Exception)
			{
				//Ignore if cant save, device been ejected
			}
		}

		public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection props)
		{
			//Create new collection of values
			SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

			//Iterate through the settings to be retrieved

			foreach (SettingsProperty setting in props)
			{
				SettingsPropertyValue value = new SettingsPropertyValue(setting)
				{
					IsDirty = false,
					SerializedValue = GetValue(setting)
				};
				values.Add(value);
			}
			return values;
		}


		private XmlDocument m_SettingsXML;

		private XmlDocument SettingsXML
		{
			get
			{
				//If we dont hold an xml document, try opening one.  
				//If it doesnt exist then create a new one ready.
				if (m_SettingsXML == null)
				{
					m_SettingsXML = new XmlDocument();

					try
					{
						m_SettingsXML.Load(Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename()));
						XmlNode node = m_SettingsXML.SelectSingleNode(string.Format("{0}/*", SETTINGSROOT));

						// Adopt configuration if it is from another machine.
						if (node != null && node.Name != Environment.MachineName)
						{
							XmlNode machineNode = m_SettingsXML.CreateElement(Environment.MachineName);

							while (node.ChildNodes.Count > 0)
							{
								machineNode.AppendChild(node.FirstChild);
							}

							node.ParentNode.AppendChild(machineNode);
							node.ParentNode.RemoveChild(node);
						}
					}
					catch (Exception)
					{
						//Create new document
						XmlDeclaration dec = m_SettingsXML.CreateXmlDeclaration("1.0", "utf-8", string.Empty);
						m_SettingsXML.AppendChild(dec);

						XmlNode nodeRoot = m_SettingsXML.CreateNode(XmlNodeType.Element, SETTINGSROOT, "");
						m_SettingsXML.AppendChild(nodeRoot);
					}
				}

				return m_SettingsXML;
			}
		}

		private string GetValue(SettingsProperty setting)
		{
			string ret = null;

			try
			{
				string path = IsRoaming(setting) ? string.Format("{0}/{1}", SETTINGSROOT, setting.Name) : string.Format("{0}/{1}/{2}", SETTINGSROOT, Environment.MachineName, setting.Name);

				if (setting.PropertyType.BaseType != null && setting.PropertyType.BaseType.Name == "CollectionBase")
				{
					XmlNode selectSingleNode = SettingsXML.SelectSingleNode(path);
					if (selectSingleNode != null) ret = selectSingleNode.InnerXml;
				}
				else
				{
					XmlNode singleNode = SettingsXML.SelectSingleNode(path);
					if (singleNode != null) ret = singleNode.InnerText;
				}
			}
			catch (Exception)
			{
				ret = (setting.DefaultValue != null) ? setting.DefaultValue.ToString() : string.Empty;
			}

			return ret;
		}


		private void SetValue(SettingsPropertyValue propVal)
		{
			XmlElement SettingNode;

			//Determine if the setting is roaming.
			//If roaming then the value is stored as an element under the root
			//Otherwise it is stored under a machine name node 
			try
			{
				if (IsRoaming(propVal.Property))
				{
					SettingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + propVal.Name);
				}
				else
				{
					SettingNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName + "/" + propVal.Name);
				}
			}
			catch (Exception)
			{
				SettingNode = null;
			}

			//Check to see if the node exists, if so then set its new value
			if ((SettingNode != null))
			{
				//SettingNode.InnerText = propVal.SerializedValue.ToString
				SetSerializedValue(SettingNode, propVal);
			}
			else
			{
				if (IsRoaming(propVal.Property))
				{
					//Store the value as an element of the Settings Root Node
					SettingNode = SettingsXML.CreateElement(propVal.Name);
					//SettingNode.InnerText = propVal.SerializedValue.ToString
					SetSerializedValue(SettingNode, propVal);
					XmlNode selectSingleNode = SettingsXML.SelectSingleNode(SETTINGSROOT);
					if (selectSingleNode != null) selectSingleNode.AppendChild(SettingNode);
				}
				else
				{
					//Its machine specific, store as an element of the machine name node,
					//creating a new machine name node if one doesnt exist.
					XmlElement MachineNode;
					try
					{
						MachineNode = (XmlElement)SettingsXML.SelectSingleNode(SETTINGSROOT + "/" + Environment.MachineName);
					}
					catch (Exception)
					{
						MachineNode = SettingsXML.CreateElement(Environment.MachineName);
						XmlNode selectSingleNode = SettingsXML.SelectSingleNode(SETTINGSROOT);
						if (selectSingleNode != null) selectSingleNode.AppendChild(MachineNode);
					}

					if (MachineNode == null)
					{
						MachineNode = SettingsXML.CreateElement(Environment.MachineName);
						XmlNode selectSingleNode = SettingsXML.SelectSingleNode(SETTINGSROOT);
						if (selectSingleNode != null) selectSingleNode.AppendChild(MachineNode);
					}

					SettingNode = SettingsXML.CreateElement(propVal.Name);
					//SettingNode.InnerText = propVal.SerializedValue.ToString
					SetSerializedValue(SettingNode, propVal);
					MachineNode.AppendChild(SettingNode);
				}
			}
		}

		private void SetSerializedValue(XmlElement node, SettingsPropertyValue propVal)
		{
			if (propVal.Property.PropertyType.BaseType != null && propVal.Property.PropertyType.BaseType.Name == "CollectionBase")
			{
				StringBuilder builder = new StringBuilder();
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				XmlWriterSettings xsettings = new XmlWriterSettings();

				ns.Add("", "");
				xsettings.OmitXmlDeclaration = true;
				XmlWriter xmlWriter = XmlWriter.Create(builder, xsettings);
				XmlSerializer s = new XmlSerializer(propVal.Property.PropertyType);
				s.Serialize(xmlWriter, propVal.PropertyValue, ns);
				xmlWriter.Close();
				node.InnerXml = builder.ToString();
			}
			else node.InnerText = propVal.SerializedValue != null ? propVal.SerializedValue.ToString() : string.Empty;
		}

		private bool IsRoaming(SettingsProperty prop)
		{
			//Determine if the setting is marked as Roaming
			foreach (DictionaryEntry d in prop.Attributes)
			{
				Attribute a = (Attribute)d.Value;
				if (a is SettingsManageabilityAttribute) return true;
			}
			return false;
		}

		public void Reset(SettingsContext context)
		{
			string settingsFilePath = Path.Combine(GetAppSettingsPath(), GetAppSettingsFilename());
			File.Delete(settingsFilePath);
			m_SettingsXML = null;
		}

		public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty property)
		{
			return null;
		}

		public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)	{ }
	}
}
