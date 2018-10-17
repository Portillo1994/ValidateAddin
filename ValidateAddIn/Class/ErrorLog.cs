using System.Collections.Generic;
using System.Xml.Serialization;

namespace ValidateAddIn.Class
{
    [XmlRoot("RegistroErrores")]
    public class ErrorLog
    {
        [XmlElement("Error")]
        public IEnumerable<Error> ErrorList { get; set; }

    }

    public class Error
    {
        [XmlElement("ProcesoId")]
        public int ProcessId { get; set; }

        [XmlElement("RegistroId")]
        public int RecordId { get; set; }

        [XmlElement("ValidacionId")]
        public int ValidationId { get; set; }

        [XmlElement("DescripcionError")]
        public string ErrorDescription { get; set; }
    }
}
