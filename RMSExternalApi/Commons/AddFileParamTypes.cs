using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace RMSExternalApi.Commons
{

    /// <summary>
    /// Them tinh nang hien thi control upload file tren swagger UI phien ban cu
    /// </summary>
    public class AddFileParamTypes : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (
                new[] { "RMSCVForExternalJob_UploadFileCVForExternalJob",
                   "RMSCVForExternalJob_UploadFileCVForExternalJob1",
                   "RMSCVForSchoolJob_UploadFileCVForSchoolJob",
                    "RMSCVForSchoolJob_UploadFileCVForSchoolJob1",
                }.Contains(operation.operationId)
              )  // controller and action name
            {
                operation.consumes.Add("multipart/form-data");
                operation.parameters = new List<Parameter>
                {
                    new Parameter
                    {
                        name = "file",
                        required = true,
                        type = "file",
                    } ,
                     new Parameter
                    {
                        name = "TPID",
                        required = true,
                        type = "text",
                    }
                };
            }

            if (
               new[] { "RMSProfile_UploadFileAttachToProfile"
                 ,
               }.Contains(operation.operationId)
             )  // controller and action name
            {
                operation.consumes.Add("multipart/form-data");
                operation.parameters = new List<Parameter>
                {
                    new Parameter
                    {
                        name = "file",
                        required = true,
                        type = "file",
                    } ,
                     new Parameter
                    {
                        name = "fileName",
                        required = true,
                        type = "text",
                    }
                };
            }

            


            //




        }
    }
}