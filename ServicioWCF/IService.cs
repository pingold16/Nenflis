using ServicioWCF.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServicioWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: Add your service operations here
        [OperationContract]
        bool addTemporada(DTOTemporada t);
        [OperationContract]
        IEnumerable<DTOTemporada> traerTemporadas();
        [OperationContract]
        bool addMaterial(DTOMaterial m);
        [OperationContract]
        IEnumerable<DTOPais> cargarPaises();
        [OperationContract]
        bool validarUser(DTOUser e);

        [OperationContract]
        bool addUser(DTOUser u);

        [OperationContract]
        IEnumerable<DTOMaterial> cargarMaterial();

        [OperationContract]
        object traerMaterial(string id);

        [OperationContract]
        bool deleteMaterial(string id);

        [OperationContract]
        DTOMaterial cargarMaterialxId(string id);

        [OperationContract]
        bool editMaterial(DTOMaterial m);

        [OperationContract]
        bool removeMaterial(string id);

        [OperationContract]
        IEnumerable<DTOMaterial> cargarMaterialxTipo(string type);

        [OperationContract]
        IEnumerable<DTOGenero> cargarGenero();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
