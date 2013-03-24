using System.Xml.Serialization;

namespace TimesheetWeb
{
    [System.SerializableAttribute]
    [XmlType]
    [XmlRoot]
    public class TimesheetConfig
    {

        [XmlElement("module", typeof(TimesheetModulePath))]
        public TimesheetModulePath Module { get; set; }

        [XmlElement("python", typeof(TimesheetPython))]
        public TimesheetPython Python { get; set; }

        [XmlElement("weeksfeed", typeof(TimesheetWeeksfeed))]
        public TimesheetWeeksfeed WeeksFeed { get; set; }

        [XmlElement("spreadsheet_generator", typeof(TimesheetWeeksfeed))]
        public TimesheetWeeksfeed SpreadsheetGenerator { get; set; }
    }

    [System.SerializableAttribute]
    [XmlType]
    public class TimesheetModulePath
    {

        [XmlAttribute("relative-path")]
        public string Relativepath { get; set; }
    }

    [System.SerializableAttribute]
    [XmlType]
    public class TimesheetPython
    {

        [XmlAttribute("path")]
        public string Path { get; set; }
    }

    [System.SerializableAttribute]
    [XmlType]
    public class TimesheetWeeksfeed
    {

        [XmlAttribute("filename")]
        public string Filename { get; set; }
    }

    [System.SerializableAttribute]
    [XmlType]
    public class SpreadsheetGenerationScript
    {

        [XmlAttribute("filename")]
        public string Filename { get; set; }
    }
}
