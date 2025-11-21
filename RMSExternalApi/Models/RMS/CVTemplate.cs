using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSExternalApi.Models.RMS
{

    
    public class CVTemplate
    {

        /// <summary>
        /// (Readonly)
        /// </summary>
        public string mail { set; get; }
        /// <summary>
        ///  (Required)
        /// </summary>
        public string mobile { set; get; }
        /// <summary>
        ///  (Required)
        /// </summary>
        public string name { set; get; }
        /// <summary>
        ///  M: male, F:Female  (Required)
        /// </summary>
        public string gender { set; get; }
        /// <summary>
        ///  yyyy/mm/dd  (Required)
        /// </summary>
        public string birthday { set; get; }
        /// <summary>
        ///  (Required)
        /// </summary>
        public string citizenId { set; get; }
        /// <summary>
        /// Y/N
        /// </summary>
        public string married { set; get; }
        /// <summary>
        ///  (Required)
        /// </summary>
        public string address { set; get; }
        public string monthlySalaryWish { set; get; }
        public string positionWish { set; get; }
        public string positionDate { set; get; }

        /// <summary>
        /// status of TPID  (Readonly)
        /// </summary>
        public string tpStatusId { set; get; }

        /// <summary>
        ///  Status Of DocNo refer to HR sign process  : Deleted/Draff/Submited and waiting/ etc...  (Readonly)
        /// </summary>
        public string status { set; get; }

        public List<Education> educations { set; get; }
        public List<JobExperience> jobExperiences { set; get; }

        public List<ProjectExperience> projectExperiences { set; get; }

        /// <summary>
        /// (Readonly)
        /// </summary>
        public string createDate { set; get; }
        /// <summary>
        /// (Readonly)
        /// </summary>

        public string updateDate { set; get; }



        /// <summary> 
        /// file id of attach file  : 20251212445545.pdf / 20251212445546 (Readonly)
        /// </summary>
        public string fileID { set; get; }
        /// <summary>
        /// full name of file  (Readonly)
        /// </summary>

        public string fileName { set; get; }
    

    }


   
    public class CVForExternalJob : CVTemplate
    {

        /// <summary>
        /// No use this field
        /// </summary>
        public string CVDetail { set; get; }

        /// <summary>
        /// DocNo
        /// </summary>
        public string TPID { set; get; }

        /// <summary>
        /// Mail of user (Readonly)
        /// </summary>
        public string jobMail { set; get; }

        /// <summary>
        /// Job id, which user applied
        /// </summary>
        public string jobID { set; get; }

        /// <summary>
        /// Name of job , which user applied
        /// </summary>

        public string jobName { set; get; }

        /// <summary>
        /// File Cv attached was tranfered to internal server ? (Readonly)
        /// </summary>
        public bool isFileCVAttachedTransfered { set; get; }

        /// <summary>
        /// professional skill 
        /// </summary>
        public string skills { set; get; }


        /// <summary>
        /// List interview history result of external job (Readonly)
        /// </summary>
        public List<AmProcess> amProcesses { set; get; }

        /// <summary>
        /// List Result history  of external job (Readonly)
        /// </summary>
        public List<ResultHistory> resultHistories { set; get; }


        /// <summary>
        /// Allow edit your CV job ? Flag to check allow show Edit CV job (Readonly)
        /// </summary>
        public bool isAllowEdit { set; get; }


    }

    public class CVForSchoolJob 
    {
        /// <summary>
        /// No use this field
        /// </summary>
        public string CVDetail { set; get; }

        /// <summary>
        /// DocNo
        /// </summary>
        public string TPID { set; get; }

        /// <summary>
        /// Mail of user  (Readonly)
        /// </summary>
        public string jobMail { set; get; }

        /// <summary>
        /// Job id, which user applied (Required)
        /// </summary>
        public string jobID { set; get; }

        /// <summary>
        /// Name of job , which user applied (Required)
        /// </summary>

        public string jobName { set; get; }


        /// <summary>
        /// Your name (Required)
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// M/F (Required)
        /// </summary>
        public string gender { set; get; }

        /// <summary>
        ///ethnic (Required)
        /// </summary>
        public string ethnic { set; get; }

        /// <summary>
        ///mobile (Required)
        /// </summary>
        public string mobile { set; get; }

        /// <summary>
        ///school name (Required)
        /// </summary>
        public string school { set; get; }

        /// <summary>
        ///qualification :University/college.. etc (Required)
        /// </summary>
        public string  qualification { set; get; }

        /// <summary>
        ///citizenID (Required)
        /// </summary>
        public string citizenId { set; get; }


        /// <summary>
        ///Your age (Required)
        /// </summary>
        //public string age { set; get; }

        /// <summary>
        ///Your birthday:yyyy/mm/dd (Required)
        /// </summary>
        public string birthday { set; get; }

        
        /// <summary>
        ///major (Required)
        /// </summary>
        public string major { set; get; }

        /// <summary>
        ///your address (Required)
        /// </summary>
        public string hometown { set; get; }

        /// <summary>
        ///email (Required)
        /// </summary>
        public string email { set; get; }
        /// <summary>
        ///Detail position Wish
        /// </summary>
        public string positionWish { set; get; }


        /// <summary>
        ///You know which Foreign Language  
        /// </summary>
        public string foreignLanguage { set; get; }

        /// <summary>
        ///Your Foreign Language Level  Score : From  0 to 100  
        /// </summary>
        public int  foreignLanguageLevel { set; get; }


        /// <summary>
        /// (Readonly)
        /// </summary>
        public string createDate { set; get; }

        /// <summary>
        ///  (Readonly)
        /// </summary>
        public string updateDate { set; get; }


        /// <summary>
        /// Status of TPID (Readonly)
        /// </summary>
        public string tpStatus { set; get; }

        /// <summary>
        ///  Status Of DocNo refer to HR sign process (Readonly)
        /// </summary>
        public string status { set; get; }


        /// <summary>
        ///  file id of attach file
        /// </summary>
        public string fileID { set; get; }


        /// <summary>
        /// List interview history result of school job (Readonly)
        /// </summary>
        public List<SchoolImProcess> schoolImProcesses;


        /// <summary>
        /// Allow edit your CV job ? Flag to check allow show Edit CV job (Readonly)
        /// </summary>
        public bool isAllowEdit { set; get; }


    }


    public class ProjectExperience
    {
        /// <summary>
        /// (Required) yyyy-mm
        /// </summary>
        public string startTime { set; get; }
        /// <summary>
        ///(Required) yyyy-mm
        /// </summary>
        public string endTime { set; get; }

        /// <summary>
        /// (Required)name of company or project
        /// </summary>
        public string name { set; get; }

        /// <summary>
        /// (Required) Description 
        /// </summary>
        public string description { set; get; }


    }

    public class JobExperience
    {
        /// <summary>
        ///(Required)  yyyy-mm
        /// </summary>
        public string startTime { set; get; }
        /// <summary>
        /// (Required) yyyy-mm
        /// </summary>
        public string endTime { set; get; }
        /// <summary>
        ///(Required) Name of company
        /// </summary>
        public string company { set; get; }
        /// <summary>
        /// (Required) position
        /// </summary>
        public string position { set; get; }

        /// <summary>
        ///(Required) Description of job
        /// </summary>
        public string description { set; get; }

    }

    public class Education
    {
        /// <summary>
        ///(Required)  yyyy-mm
        /// </summary>
        public string startTime { set; get; }
        /// <summary>
        ///(Required) yyyy-mm
        /// </summary>
        public string endTime { set; get; }
        /// <summary>
        /// (Required) Name of school
        /// </summary>
        public string school { set; get; }
        /// <summary>
        /// Major
        /// </summary>
        public string major { set; get; }

        /// <summary>
        /// (Required) EDUCATIONAL QUALIFICATIONS
        /// </summary>
        public string eduQualify { set; get; }


    }
}
