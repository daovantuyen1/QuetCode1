
# RMS External API



## 1. Logon and manage account

#### 1.1 Login With Email


- URL:

  ```http
    /api/RMSAccount/LoginWithEmail
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    Use your email to login to system
  ```

- Request:
  - Header :
     ```http
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json 
  - Code view:
    ```http
      {
      "email":""
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `email` | `string` | **Required**. Your email address |

- Response:
  - Format: json 
  - Code view:
   ```http
      {
        "status": "",
        "message": "",
        "data": null
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' , if status == 'success' // it means system has checked success and  send OTP to your mail  else some error |
    | `message` | `string` | Response message   |
    | `data` | `string` |   |

 




#### 1.2 Login With Phone


- URL:

  ```http
    /api/RMSAccount/LoginWithPhone
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    Use your phone number to login to system
  ```

- Request:
  - Header :
     ```http
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json 
  - Code view:
    ```http
      {
      "phone":""
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `phone` | `string` | **Required**. Your phone number |

- Response:
  - Format: json 
  - Code view:
   ```http
      {
        "status": "",
        "message": "",
        "data": null
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' , if status == 'success' // it means system has checked success and  send OTP to your phone number  else some error |
    | `message` | `string` | Response message  |
    | `data` | `string` |   |

    ```



    
#### 1.3 Confirm Login OTP For Email


- URL:

  ```http
    /api/RMSAccount/ConfirmLoginOTPForEmail
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    After your called api "Login With Email" success , while a time , system will send OTP to your email , and then please use this api to confirm OTP valid and login
  ```

- Request:
  - Header :
     ```http
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json 
  - Code view:
    ```http
      {
      "email":"" ,
      "OTP":"" 
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `email` | `string` | **Required**. Your email address |
    | `OTP` | `string` | **Required**. OTP code has been sent to your email |
- Response:
  - Code view:
   ```http
       {
        "status": "",
        "message": "",
        "data": "jwt token"
       }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` |  Response message |
    | `data`   | `string` |  Response data: if(status=='success') then data = ' your JWT token' // example format: 'Bearer xxxxxx' ,please save this jwt token to  local storage for authen api in future                          |

    ```


 
    
#### 1.4 Check Login Valid


- URL:

  ```http
    /api/RMSAccount/CheckLoginValid
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Check you are still on logined status and return current user account information
  ```

- Request:
  - Header :
     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
- Response:
  - Code view:
   ```http
       {
        "status": "success",
        "message": "success",
        "data": {
          "mail": "abc@gmail.com",
          "name": "DAO VAN TUYEN",
          "mobile": "12345"
        }
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` |  Response message |
    | `data`   | `object` |  Current user information                         |

    ```   





    
#### 1.4 Confirm Login OTP For phone


- URL:

  ```http
    /api/RMSAccount/ConfirmLoginOTPForPhone
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    After your called api "Login With Phone" success , while a time , system will send OTP to your phone number , and then please use this api to confirm OTP valid and login
  ```

- Request:
  - Header :
     ```http
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json 
  - Code view:
    ```http
      {
      "phone":"" ,
      "OTP":"" 
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `phone` | `string` | **Required**. Your phone number |
    | `OTP` | `string` | **Required**. OTP code has been sent to your phone |
- Response:
  - Code view:
   ```http
       {
        "status": "",
        "message": "",
        "data": "jwt token"
       }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` |  Response message |
    | `data`   | `string` |  Response data: if(status=='success') then data = ' your JWT token' // example format: 'Bearer xxxxxx' ,please save this jwt token to  local storage for authen api in future                          |

    ```


 
    
#### 1.5 Get current OTP


- URL:

  ```http
    /api/RMSAccount/GetCurrentOTP
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Get current OTP code of email or phone (Only use this api for dev process)
  ```
- Request:
  - Format: Query string
  - Code view:
   ```http
       key : '',  //  your email or phone 
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `key` | `string` |  **Required** your email or phone |
   

- Response:
  - Code view:
   ```http
       {
        "status": "success",
        "message": "success",
        "data": "mzbg3j5e"
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` |  Response message |
    | `data`   | `string` |  Response data: if(status=='success') data ='your OTP code'                         |

  


    
  
#### 1.6 Logout


- URL:

  ```http
    /api/RMSAccount/Logout
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
     Logout system
  ```
- Request:
  - Header :
     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```


- Response:
  - Code view:
   ```http
       {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` |  Response message |
    | `data`   | `string` |  Response data: if(status=='success') then logout success                      |

    ```   




#### 1.7 Update Name For Account


- URL:

  ```http
    /api/RMSAccount/UpdateNameForAccount
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    Update user name for current account
  ```

- Request:
  - Header :
     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json 
  - Code view:
    ```http
     {
      "name": "DAO VAN TUYEN"
     }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `name` | `string` | **Required**. Current user name |

- Response:
  - Format: json 
  - Code view:
   ```http
      {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message : if(status=='success') then updated success  |
    | `data` | `object` |   |

    ```






#### 1.8 Update Email for your account


- URL:

  ```http
    /api/RMSAccount/UpdateEmailForAccount
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
   Update Email for your account , This method Need confirm OTP  ,Each Email only allows link only one times , please use function LoginWithEmail to get OTP code.
  ```

- Request:
  - Header :
     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json 
  - Code view:
    ```http
     {
      "mail": "" ,
       "OTP": "" 
     }
    ```

  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `mail` | `string` | **Required**. email address |
    | `OTP` | `string` | **Required**. OTP  code |



- Response:
  - Format: json 
  - Code view:
   ```http
      {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message : if(status=='success') then updated success  |
    | `data` | `object` |   |

    ```


    




#### 1.9 Update phone for your account


- URL:

  ```http
    /api/RMSAccount/UpdatePhoneForAccount
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
  Update phone for your account, This method Need confirm OTP  ,Each phone only allows link only one times , please use function LoginWithPhone to get OTP code.
  ```

- Request:
  - Header :
     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json 
  - Code view:
    ```http
     {
      "phone": "" ,
       "OTP": "" 
     }
    ```

  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `phone` | `string` | **Required**. phone number |
    | `OTP` | `string` | **Required**. OTP  code |



- Response:
  - Format: json 
  - Code view:
   ```http
      {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message : if(status=='success') then updated success  |
    | `data` | `object` |   |

    ```






## 2. Get config data


#### 2.1 Get Education Level list


- URL:

  ```http
    /api/RMSConfigData/GetEducationLevelLs
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Get education level list
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
 
- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": [
        {
          "value": "博士后",
          "textVn": "Nghiên cứu sinh sau tiến sĩ",
          "textCn": "博士后",
          "textEn": "Postdoctoral Researcher"
        },
        {
          "value": "博士",
          "textVn": "Tiến sĩ",
          "textCn": "博士",
          "textEn": "PhD"
        },
        {
          "value": "硕士",
          "textVn": "Thạc sĩ",
          "textCn": "硕士",
          "textEn": "Master"
        } ......
     ]
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `List of ConfigData` | education level list , example one element : { "value": "硕士", "textVn": "Thạc sĩ", "textCn": "硕士", "textEn": "Master" } // note : value field use for submit to api,  textVn/textCn/textEn use for show on UI  |

    ```




#### 2.2 Get Factory list


- URL:

  ```http
    /api/RMSConfigData/GetFactoryLs
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Get factory list
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
 
- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": [
        {
          "value": "桂武工業區",
          "textVn": "Khu công nghiệp Quế Võ",
          "textCn": "桂武工業區",
          "textEn": "Que Vo Industrial Zone"
        },
        {
          "value": "光州工業區",
          "textVn": "Khu công nghiệp Quang Châu",
          "textCn": "光州工業區",
          "textEn": "Quang Chau Industrial Zone"
        },
        {
          "value": "黃田工業區",
          "textVn": "Khu công nghiệp Đồng Vàng",
          "textCn": "黃田工業區",
          "textEn": "Dong Vang Industrial Zone"
        }
        .....
      ]
    } 
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `List of ConfigData` | factory list , example one element : { "value": "黃田工業區", "textVn": "Khu công nghiệp Đồng Vàng", "textCn": "黃田工業區", "textEn": "Dong Vang Industrial Zone" } // note : value field use for submit to api,  textVn/textCn/textEn use for show on UI  |

  





    

#### 2.3 Get Gender List


- URL:

  ```http
    /api/RMSConfigData/GetGenderLs
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Get gender list
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
 
- Response:
  - Format: json 
  - Code view:
   ```http
     {
  "status": "success",
  "message": "success",
  "data": [
    {
      "value": "M",
      "textVn": "Nam",
      "textCn": "男",
      "textEn": "Male"
    },
    {
      "value": "F",
      "textVn": "Nữ",
      "textCn": "女",
      "textEn": "Female"
    } 
  ]
}
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `List of ConfigData` | gender list , example one element : { "value": "F", "textVn": "Nữ", "textCn": "女", "textEn": "Female" } // note : value field use for submit to api,  textVn/textCn/textEn use for show on UI  |

    ```





## 3. Get file 


#### 3.1 Download File


- URL:

  ```http
    /api/RMSFile/DownloadFile
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Download file of External job/ School job / Profile
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```

  - Format: query string  
  - Code view:
    ```http
      "fileID": ""
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `fileID` | `string` | **Required**. id of file |

 
- Response:
  - Format: application/pdf 
  - Description: Return file data
    


## 4. Manage profile




#### 4.1  Get Profile


- URL:

  ```http
    /api/RMSProfile/GetProfile
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Get profile information of current account
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
 
- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": {
        "mail": "abc@gmail.com",
        "mobile": "12345",
        "name": "DAO VAN TUYEN",
        "gender": "M",
        "birthday": "1994/06/04 00:00:00",
        "citizenId": "122222",
        "married": "Y",
        "address": "Bac Ninh, Viet Nam",
        "monthlySalaryWish": "12000000",
        "positionWish": "IT staff",
        "positionDate": "2025/10",
        "tpStatusId": null,
        "status": null,
        "educations": [
          {
            "startTime": "2019-10",
            "endTime": "2022-10",
            "school": "UKB",
            "major": "IT",
            "eduQualify": "本科A"
          }
        ],
        "jobExperiences": [
          {
            "startTime": "2024-10",
            "endTime": "2025-11",
            "company": "Foxconn",
            "position": "IT",
            "description": "IT staff develop internal system"
          }
        ],
        "projectExperiences": [
          {
            "startTime": "2023-10",
            "endTime": "2024-10",
            "name": "Halo project",
            "description": "Main dev for halo chat project"
          }
        ],
        "createDate": "2025/11/07 15:00:15",
        "updateDate": "2025/11/07 15:00:15",
        "fileID": null,
        "fileName": null
      }
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `List of CVTemplate ` | Your profile information |




#### 4.2  Modify Profile


- URL:

  ```http
    /api/RMSProfile/ModifyProfile
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    Modify profile information of current account
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
 
  - Format: json 
  - Code view:
    ```http
     {
      "mail": "abc@mail.com",
      "mobile": "12345",
      "name": "DAO VAN TUYEN",
      "gender": "M",
      "birthday": "1994/06/04",
      "citizenId": "122222",
      "married": "Y",
      "address": "Bac Ninh, Viet Nam",
      "monthlySalaryWish": "12000000",
      "positionWish": "IT staff",
      "positionDate": "2025/10",
      "educations": [
        {
          "startTime": "2019-10",
          "endTime": "2022-10",
          "school": "UKB",
          "major": "IT",
          "eduQualify": "本科A"
        }
      ],
      "jobExperiences": [
        {
          "startTime": "2024-10",
          "endTime": "2025-11",
          "company": "Foxconn",
          "position": "IT",
          "description": "IT staff develop internal system"
        }
      ],
      "projectExperiences": [
        {
          "startTime": "2023-10",
          "endTime": "2024-10",
          "name": "Halo project",
          "description": "Main dev for halo chat project"
        }
      ]

    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `name` | `string` | **Required**. Current user name |


- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": {
        "mail": "abc@gmail.com",
        "mobile": "12345",
        "name": "DAO VAN TUYEN",
        "gender": "M",
        "birthday": "1994/06/04 00:00:00",
        "citizenId": "122222",
        "married": "Y",
        "address": "Bac Ninh, Viet Nam",
        "monthlySalaryWish": "12000000",
        "positionWish": "IT staff",
        "positionDate": "2025/10",
        "tpStatusId": null,
        "status": null,
        "educations": [
          {
            "startTime": "2019-10",
            "endTime": "2022-10",
            "school": "UKB",
            "major": "IT",
            "eduQualify": "本科A"
          }
        ],
        "jobExperiences": [
          {
            "startTime": "2024-10",
            "endTime": "2025-11",
            "company": "Foxconn",
            "position": "IT",
            "description": "IT staff develop internal system"
          }
        ],
        "projectExperiences": [
          {
            "startTime": "2023-10",
            "endTime": "2024-10",
            "name": "Halo project",
            "description": "Main dev for halo chat project"
          }
        ],
        "createDate": "2025/11/07 15:00:15",
        "updateDate": "2025/11/07 15:00:15",
        "fileID": null,
        "fileName": null
      }
    }
    ```
  - Parameter Description:

   | Parameter | Type | Description |
    | :--- | :--- | :--- |
    | **status** | string | The overall status of the request (e.g., "success"). |
    | **message** | string | Detailed message about the status (e.g., "success"). |
    | **data** | object | The object containing the main user/profile data. |
    | **data.mail** | string | The user's email address. |
    | **data.mobile** | string | The mobile phone number. |
    | **data.name** | string | The full name. |
    | **data.gender** | string | Gender (e.g., "M" - Male, "F" - Female). |
    | **data.birthday** | string | Date of birth in `YYYY/MM/DD HH:MM:SS` format. |
    | **data.citizenId** | string | National ID or Citizen ID number. |
    | **data.married** | string | Marital status (e.g., "Y" - Married, "N" - Single). |
    | **data.address** | string | Current address. |
    | **data.monthlySalaryWish** | string | Desired monthly salary. |
    | **data.positionWish** | string | Desired position to apply for. |
    | **data.positionDate** | string | Date available to start working in `YYYY/MM` format. |
    | **data.tpStatusId** | string/null | TP status ID (if available, can be `null`). |
    | **data.status** | string/null | Current status of the profile (can be `null`). |
    | **data.educations** | array | Array containing educational history. |
    | **data.jobExperiences** | array | Array containing work experience information. |
    | **data.projectExperiences** | array | Array containing project experience information. |
    | **data.createDate** | string | The date the profile was created in `YYYY/MM/DD HH:MM:SS` format. |
    | **data.updateDate** | string | The last update date of the profile in `YYYY/MM/DD HH:MM:SS` format. |
    | **data.fileID** | string/null | Attached file ID (can be `null`). |
    | **data.fileName** | string/null | Attached file name (can be `null`). |


  - Education Information (`data.educations` array)

    | Parameter | Type | Description |
    | :--- | :--- | :--- |
    | **startTime** | string | Education start time (e.g., "YYYY-MM"). |
    | **endTime** | string | Education end time (e.g., "YYYY-MM"). |
    | **school** | string | Name of the school/institution. |
    | **major** | string | Field of study/Major. |
    | **eduQualify** | string | Educational qualification/Degree. |

  - Job Experience Information (`data.jobExperiences` array)

    | Parameter | Type | Description |
    | :--- | :--- | :--- |
    | **startTime** | string | Work start time (e.g., "YYYY-MM"). |
    | **endTime** | string | Work end time (e.g., "YYYY-MM"). |
    | **company** | string | Name of the company/organization. |
    | **position** | string | Job title/Position held. |
    | **description** | string | Description of the work performed. |

   - Project Experience Information (`data.projectExperiences` array)

      | Parameter | Type | Description |
      | :--- | :--- | :--- |
      | **startTime** | string | Project start time (e.g., "YYYY-MM"). |
      | **endTime** | string | Project end time (e.g., "YYYY-MM"). |
      | **name** | string | Project name. |
      | **description** | string | Detailed description of the role and project content. |




#### 4.3  Upload File Attach To Profile


- URL:

  ```http
    /api/RMSProfile/UploadFileAttachToProfile
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    Upload CV file attach to profile
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: Form Data
  - Code view:
    ```http
        var fileName= btoa("your file name"); // fileName in base64 format 
        var form=new FormData(); 
        form.append("fileName",fileName)  ; 
        form.append("file",file[0]);
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `fileName` | `string` | **Required**. Your file name : format: base 64 string |
    | `file` | `File data` | **Required**. Your file , only allow pdf file |

- Response:
  - Format: json 
  - Code view:
   ```http
     {
        "status": "success",
        "message": "success",
        "data": "CV_2025110715241700470781.pdf"
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `string` | file id |


## 5. Manage school job 



#### 5.1  Get CV School Job List Of JobMail


- URL:

  ```http
    /api/RMSCVForSchoolJob/GetCVSchoolJobLsOfJobMail
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Get CV job for School job List of current account
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
 
- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": [
        {
          "schoolImProcesses": [],
          "tpID": "SRC202511030077",
          "createDate": "2025/11/03 10:35:10",
          "updateDate": "2025/11/03 10:36:14",
          "tpStatusId": "待安排面试",
          "jobMail": "abc@gmail.com",
          "jobName": "SV IT",
          "jobID": "122455",
          "status": "Submited and waiting",
          "isAllowEdit": true
        },
        {
          "schoolImProcesses": [],
          "tpID": "SRC202511030078",
          "createDate": "2025/11/03 10:36:31",
          "updateDate": "2025/11/03 10:36:39",
          "tpStatusId": "待安排面试",
          "jobMail": "abc@gmail.com",
          "jobName": "SV IT",
          "jobID": "1442221",
          "status": "Submited and waiting",
          "isAllowEdit": true
        },
        {
          "schoolImProcesses": [],
          "tpID": "SRC202511030079",
          "createDate": "2025/11/03 10:36:46",
          "updateDate": "2025/11/03 10:36:54",
          "tpStatusId": "待安排面试",
          "jobMail": "abc@gmail.com",
          "jobName": "SV IT",
          "jobID": "1442211",
          "status": "Submited and waiting",
          "isAllowEdit": true
        }
      ]
    }
    ```
  - Parameter Description:
    ```http
      CVSchoolJobBaseModel {
      schoolImProcesses (Array[SchoolImProcess]): List interview history result of school job ,
      tpID (string),
      createDate (string),
      updateDate (string),
      tpStatusId (string): Status of TPID ,
      jobMail (string),
      jobName (string),
      jobID (string),
      status (string): Status of doc no follow HR sign process : Deleted/Draff/Submited and waiting/ etc .. (Readonly) ,
      isAllowEdit (boolean): Allow edit your CV job ? Flag to check allow show Edit CV job (Readonly)
      }
      SchoolImProcess {
      result (string),
      createDate (string)
      }
          
      CVSchoolJobBaseModel {
      schoolImProcesses (Array[SchoolImProcess]): List interview history result of school job ,
      tpID (string),
      createDate (string),
      updateDate (string),
      tpStatusId (string): Status of TPID ,
      jobMail (string),
      jobName (string),
      jobID (string),
      status (string): Status of doc no follow HR sign process : Deleted/Draff/Submited and waiting/ etc .. ,
      isAllowEdit (boolean): Allow edit your CV job ? Flag to check allow show Edit CV job 
      }
      SchoolImProcess {
      result (string),
      createDate (string)
      }

    ```

   



#### 5.2  Get CV For School Job


- URL:

  ```http
    /api/RMSCVForSchoolJob/GetCVForSchoolJob
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Get  Cv job information detail for School job
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: query string 
  - Code view:
    ```http
     tpID:''
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `tpID` | `string` | **Required**. Doc no of Your School cv job  |

 
- Response:
  - Format: json 
  - Code view:
   ```http
     {
        "status": "success",
        "message": "success",
        "data": {
          "schoolImProcesses": [],
          "CVDetail": null,
          "TPID": "SRC202511030077",
          "jobMail": null,
          "jobID": "122455",
          "jobName": "SV IT",
          "name": "DAO VAN TUYEN",
          "gender": "F",
          "ethnic": "KINH",
          "mobile": "123213",
          "school": "UKB",
          "qualification": "DH",
          "citizenId": "123213213",
          "birthday": "1994/06/04",
          "major": "CNTT",
          "hometown": "BNVN",
          "email": "asds@dsfs.sads",
          "positionWish": "it",
          "foreignLanguage": "ENGLISH",
          "foreignLanguageLevel": 100,
          "createDate": "2025/11/03 10:35:10",
          "updateDate": "2025/11/03 10:36:14",
          "tpStatus": "待安排面试",
          "status": "Submited and waiting",
          "fileID": "2025110316375203851072",
          "isAllowEdit": true
        }
      }
    ```
  - Parameter Description:

     ```http
        CVForSchoolJob {
        schoolImProcesses (Array[SchoolImProcess]),
        CVDetail (string): No use this field ,
        TPID (string): DocNo ,
        jobMail (string): Mail of user (Readonly) ,
        jobID (string): Job id, which user applied (Required) ,
        jobName (string,): Name of job , which user applied (Required) ,
        name (string,): Your name (Required) ,
        gender (string,): M/F (Required) ,
        ethnic (string,): ethnic (Required) ,
        mobile (string,): mobile (Required) ,
        school (string,): school name (Required) ,
        qualification (string, ): qualification :University/college.. etc (Required) ,
        citizenId (string, optional): citizenID (Required) ,
        birthday (string, optional): Your birthday:yyyy/mm/dd (Required) ,
        major (string, optional): major (Required) ,
        hometown (string, optional): your address (Required) ,
        email (string, optional): email (Required) ,
        positionWish (string, optional): Detail position Wish ,
        foreignLanguage (string, optional): You know which Foreign Language ,
        foreignLanguageLevel (integer, optional): Your Foreign Language Level Score : From 0 to 100 ,
        createDate (string, optional): (Readonly) ,
        updateDate (string, optional): (Readonly) ,
        tpStatus (string, optional): Status of TPID (Readonly) ,
        status (string, optional): Status Of DocNo refer to HR sign process (Readonly) ,
        fileID (string, optional): file id of attach file ,
        isAllowEdit (boolean, optional): Allow edit your CV job ? Flag to check allow show Edit CV job (Readonly)
        }
        SchoolImProcess {
        result (string, optional),
        createDate (string, optional)
        }

    ```




#### 5.3  Delete CV For School Job


- URL:

  ```http
    /api/RMSCVForSchoolJob/DeleteCVForSchoolJob
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
    Delete CV infor for School job , only allow delete when  information of CV job is :"isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: query string 
  - Code view:
    ```http
     tpID:''
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `tpID` | `string` | **Required**. Doc no of Your School cv job  |

 
- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": null
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `object ` | |





#### 5.4 Modify CV For School Job


- URL:

  ```http
    /api/RMSCVForSchoolJob/ModifyCVForSchoolJob
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
    Add new or Update CV for School job , only allow Update when  information of CV job is :"isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: json
  - Code view:
    ```http
          {
          "TPID": "SRC202511030077",
          "jobID": "122455",
          "jobName": "SV IT",
          "name": "DAO VAN TUYEN",
          "gender": "F",
          "ethnic": "KINH",
          "mobile": "123213",
          "school": "UKB",
          "qualification": "DH",
          "citizenId": "123213213",
          "birthday": "1994/06/04",
          "major": "CNTT",
          "hometown": "BNVN",
          "email": "asds@dsfs.sads",
          "positionWish": "it",
          "foreignLanguage": "ENGLISH",
          "foreignLanguageLevel": 100
        }

    ```
  - Parameter Description:

      ```http
       CVForSchoolJob {
            TPID (string): DocNo ,
            jobID (string): Job id, which user applied (Required) ,
            jobName (string): Name of job , which user applied (Required) ,
            name (string): Your name (Required) ,
            gender (string): M/F (Required) ,
            ethnic (string): ethnic (Required) ,
            mobile (string): mobile (Required) ,
            school (string,): school name (Required) ,
            qualification (string): qualification :University/college.. etc (Required) ,
            citizenId (string): citizenID (Required) ,
            birthday (string): Your birthday:yyyy/mm/dd (Required) ,
            major (string): major (Required) ,
            hometown (string): your address (Required) ,
            email (string): email (Required) ,
            positionWish (string): Detail position Wish ,
            foreignLanguage (string): You know which Foreign Language ,
            foreignLanguageLevel (integer): Your Foreign Language Level Score : From 0 to 100 
            }

    ```
- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": null
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `object ` | |




#### 5.5  Upload File CV For School Job


- URL:

  ```http
    /api/RMSCVForSchoolJob/UploadFileCVForSchoolJob1
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
     Upload file CV pdf for School job , only allow Update when  information of CV job is :"isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: Form Data
  - Code view:
    ```http
           var form= new FormData(); 
           form.append("TPID","Your doc No") ;
           form.append("file",file[0])

    ```
  - Parameter Description:

    
    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `TPID` | `string` | **Required**. Your doc no |
    | `file` | `File data` | **Required**. Your file data |

- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": null
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `string ` |  TPID |



#### 5.6  Delete File CV For School DeleteFileCVForSchoolJobJob


- URL:

  ```http
    /api/RMSCVForSchoolJob/DeleteFileCVForSchoolJob
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
     Delete file Cv for School Job , only allow Update when  information of CV job is :"isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  - Format: query string
  - Code view:
    ```http
         TPID:''

    ```
  - Parameter Description:

    
    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `TPID` | `string` | **Required**. Your doc no |


- Response:
  - Format: json 
  - Code view:
   ```http
     {
      "status": "success",
      "message": "success",
      "data": null
    }
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error'  |
    | `message` | `string` | Response message  |
    | `data` | `object ` |   |



## 6. Manage external job (social recruitment job)




#### 6.1 Get CV External Job List Of JobMail


- URL:

  ```http
    /api/RMSCVForExternalJob/GetCVExternalJobLsOfJobMail
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
     Get CV job for External job List of current account
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  

- Response:
  - Format: json 
  - Code view:
   ```http
     {
        "status": "success",
        "message": "success",
        "data": [
          {
            "amProcesses": [],
            "resultHistories": [],
            "tpID": "ERC2025102800001",
            "createDate": "2025/10/28 10:14:18",
            "updateDate": "2025/11/03 08:32:40",
            "tpStatusId": "錄用簽單中",
            "jobMail": "abc@gmail.com",
            "jobName": "tuyen it",
            "jobID": "1223",
            "status": "Submited and waiting",
            "isAllowEdit": false
          },
          {
            "amProcesses": [],
            "resultHistories": [],
            "tpID": "ERC2025110300001",
            "createDate": "2025/11/03 08:00:41",
            "updateDate": "2025/11/03 08:06:00",
            "tpStatusId": null,
            "jobMail": "abc@gmail.com",
            "jobName": "TUYEN DUNG IT",
            "jobID": "12345",
            "status": "Submited and waiting",
            "isAllowEdit": false
          },
          {
            "amProcesses": [],
            "resultHistories": [],
            "tpID": "ERC2025110300002",
            "createDate": "2025/11/03 10:28:43",
            "updateDate": "2025/11/03 10:31:10",
            "tpStatusId": null,
            "jobMail": "abc@gmail.com",
            "jobName": "tuyen dung IT",
            "jobID": "1225455",
            "status": "Draff",
            "isAllowEdit": true
          },
          {
            "amProcesses": [],
            "resultHistories": [],
            "tpID": "ERC2025110300003",
            "createDate": "2025/11/03 10:32:03",
            "updateDate": "2025/11/03 10:32:14",
            "tpStatusId": null,
            "jobMail": "abc@gmail.com",
            "jobName": "tuyen dung IT",
            "jobID": "1552246663",
            "status": "Draff",
            "isAllowEdit": true
          },
          {
            "amProcesses": [],
            "resultHistories": [],
            "tpID": "ERC2025110300004",
            "createDate": "2025/11/03 10:32:27",
            "updateDate": "2025/11/03 10:33:34",
            "tpStatusId": null,
            "jobMail": "abc@gmail.com",
            "jobName": "tuyen dung IT",
            "jobID": "155456",
            "status": "Draff",
            "isAllowEdit": true
          },
          {
            "amProcesses": [],
            "resultHistories": [],
            "tpID": "ERC2025110400001",
            "createDate": "2025/11/04 20:03:26",
            "updateDate": "2025/11/04 20:05:49",
            "tpStatusId": null,
            "jobMail": "abc@gmail.com",
            "jobName": "TUYEN DUNG IT",
            "jobID": "2025071613415103658907",
            "status": "Draff",
            "isAllowEdit": true
          },
          {
            "amProcesses": [],
            "resultHistories": [],
            "tpID": "ERC2025110600001",
            "createDate": "2025/11/06 09:41:26",
            "updateDate": null,
            "tpStatusId": null,
            "jobMail": "abc@gmail.com",
            "jobName": "TUYEN DUNG IT",
            "jobID": "2024091216274600467168",
            "status": "Draff",
            "isAllowEdit": true
          }
        ]
      }
    ```
  - Parameter Description:
    ```http
          
        CVExternalJobBaseModel {
        amProcesses (Array[AmProcess]): List interview history result of External job ,
        resultHistories (Array[ResultHistory]): List result history of External job ,
        tpID (string),
        createDate (string),
        updateDate (string),
        tpStatusId (string): Status of TPID ,
        jobMail (string),
        jobName (string),
        jobID (string),
        status (string): Status of doc no follow HR sign process : Deleted/Draff/Submited and waiting/ etc .. (Readonly) ,
        isAllowEdit (boolean): Allow edit your CV job ? Flag to check allow show Edit CV job (Readonly)
        }
        AmProcess {
        result (string),
        createDate (string)
        }
        ResultHistory {
        result (string),
        createDate (string)
        }
   
       
    ```





#### 6.2 Get CV For External Job


- URL:

  ```http
    /api/RMSCVForExternalJob/GetCVForExternalJob
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
     Get  detail Cv job information for External job
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
  
  - Format: query string 
  - Code view:
    ```http
     tpID: ''
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `tpID` | `string` | **Required**. Doc no of your cv job of external job |


- Response:
  - Format: json 
  - Code view:
   ```http
    {
      "status": "success",
      "message": "success",
      "data": {
        "CVDetail": null,
        "TPID": "ERC2025102800001",
        "jobMail": null,
        "jobID": "1223",
        "jobName": "tuyen it",
        "isFileCVAttachedTransfered": true,
        "skills": null,
        "amProcesses": [],
        "resultHistories": [],
        "isAllowEdit": false,
        "mail": "abc@gmail.com",
        "mobile": "1233445",
        "name": "DAO VAN TUYEN",
        "gender": "M",
        "birthday": "1994/06/04",
        "citizenId": null,
        "married": "Y",
        "address": "BN",
        "monthlySalaryWish": "10000",
        "positionWish": "ITVN",
        "positionDate": "2025/10",
        "tpStatusId": "錄用簽單中",
        "status": "Submited and waiting",
        "educations": [
          {
            "startTime": "2021-10",
            "endTime": "2022-10",
            "school": "UKB",
            "major": "CNTT",
            "eduQualify": "DH"
          }
        ],
        "jobExperiences": [
          {
            "startTime": "2020-10",
            "endTime": "2021-10",
            "company": "FXN",
            "position": "IT",
            "description": "IT"
          }
        ],
        "projectExperiences": [
          {
            "startTime": "2021-10",
            "endTime": "2022-10",
            "name": "HALO",
            "description": "HALO"
          }
        ],
        "createDate": "2025/10/28 10:14:18",
        "updateDate": "2025/11/03 08:32:40",
        "fileID": "2025110315593603850997",
        "fileName": null
      }
    }
    ```
  - Parameter Description:
   ```
    CVForExternalJob {
          CVDetail (string): No use this field ,
          TPID (string): DocNo ,
          jobMail (string): Mail of user (Readonly) ,
          jobID (string): Job id, which user applied ,
          jobName (string): Name of job , which user applied ,
          isFileCVAttachedTransfered (boolean): File Cv attached was tranfered to internal server ? (Readonly) ,
          skills (string): professional skill ,
          amProcesses (Array[AmProcess]): List interview history result of external job (Readonly) ,
          resultHistories (Array[ResultHistory]): List Result history of external job (Readonly) ,
          isAllowEdit (boolean): Allow edit your CV job ? Flag to check allow show Edit CV job (Readonly) ,
          mail (string): (Readonly) ,
          mobile (string): (Required) ,
          name (string): (Required) ,
          gender (string): M: male, F:Female (Required) ,
          birthday (string): yyyy/mm/dd (Required) ,
          citizenId (string): (Required) ,
          married (string): Y/N ,
          address (string): (Required) ,
          monthlySalaryWish (string),
          positionWish (string),
          positionDate (string),
          tpStatusId (string): status of TPID (Readonly) ,
          status (string): Status Of DocNo refer to HR sign process : Deleted/Draff/Submited and waiting/ etc... (Readonly) ,
          educations (Array[Education]),
          jobExperiences (Array[JobExperience]),
          projectExperiences (Array[ProjectExperience]),
          createDate (string): (Readonly) ,
          updateDate (string): (Readonly) ,
          fileID (string): file id of attach file : 20251212445545.pdf / 20251212445546 (Readonly) ,
          fileName (string): full name of file (Readonly)
          }
          AmProcess {
          result (string),
          createDate (string)
          }
          ResultHistory {
          result (string),
          createDate (strin)
          }
          Education {
          startTime (string): (Required) yyyy-mm ,
          endTime (string): (Required) yyyy-mm ,
          school (string): (Required) Name of school ,
          major (string): Major ,
          eduQualify (string): (Required) EDUCATIONAL QUALIFICATIONS
          }
          JobExperience {
          startTime (string): (Required) yyyy-mm ,
          endTime (string): (Required) yyyy-mm ,
          company (string): (Required) Name of company ,
          position (string): (Required) position ,
          description (string): (Required) Description of job
          }
          ProjectExperience {
          startTime (string): (Required) yyyy-mm ,
          endTime (string): (Required) yyyy-mm ,
          name (string): (Required)name of company or project ,
          description (string): (Required) Description
          }
   
    ```







#### 6.3 Delete CV infor for External job


- URL:

  ```http
    /api/RMSCVForExternalJob/DeleteCVForExternalJob
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
     Delete CV infor for External job , only allow delete when information of CV job is :  "isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
  
  - Format: query string 
  - Code view:
    ```http
     tpID: ''
    ```
  - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `tpID` | `string` | **Required**. Doc no of your cv job of external job |


- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:
     | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' , if status == 'success' // it means system has checked success and  send OTP to your mail  else some error |
    | `message` | `string` | Response message   |
    | `data` | `string` |   |






#### 6.4  Modify CV for external job 


- URL:

  ```http
    /api/RMSCVForExternalJob/ModifyCVForExternalJob
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
     Add new or update CV infor for External job , only allow update when information of CV job is :  "isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
  
  - Format: json
  - Code view:
    ```http
     {
      "TPID": "",
      "jobID": "12345",
      "jobName": "IT recuirement",
      "skills": "have good job skills ",
      "mail": "abc@gmail.com",
      "mobile": "123456",
      "name": "DAO VAN TUYEN",
      "gender": "M",
      "birthday": "1994/06/04",
      "citizenId": "123455",
      "married": "Y",
      "address": "BAC NINH , VIET NAM",
      "monthlySalaryWish": "12000000",
      "positionWish": "IT staff",
      "positionDate": "2025/10",
      "educations": [
        {
          "startTime": "2023-06",
          "endTime": "2024-10",
          "school": "UKB",
          "major": "CNTT",
          "eduQualify": "本科A"
        }
      ],
      "jobExperiences": [
        {
          "startTime": "2024-12",
          "endTime": "2025-10",
          "company": "FOXCONN",
          "position": "IT",
          "description": "IT staff"
        }
      ],
      "projectExperiences": [
        {
          "startTime": "2022-10",
          "endTime": "2023-10",
          "name": "halo",
          "description": "dev halo chat project"
        }
      ]
    
    }
    ```
  - Parameter Description:
    ```http
        CVForExternalJob {
        TPID (string): DocNo ,
        jobID (string): Job id, which user applied (Required) ,
        jobName (string): Name of job , which user applied (Required) ,
        skills (string): professional skill ,
        mail (string): (Required) ,
        mobile (string): (Required) ,
        name (string): (Required) ,
        gender (string): M: male, F:Female (Required) ,
        birthday (string): yyyy/mm/dd (Required) ,
        citizenId (string): (Required) ,
        married (string): Y/N (Required)  ,
        address (string): (Required) ,
        monthlySalaryWish (string) (Required),
        positionWish (string) (Required),
        positionDate (string),
        educations (Array[Education]) (Required),
        jobExperiences (Array[JobExperience])  (Required),
        projectExperiences (Array[ProjectExperience])
        }

        Education {
        startTime (string): (Required) yyyy-mm ,
        endTime (string): (Required) yyyy-mm ,
        school (string): (Required) Name of school ,
        major (string): Major ,
        eduQualify (string): (Required) EDUCATIONAL QUALIFICATIONS
        }
        JobExperience {
        startTime (string): (Required) yyyy-mm ,
        endTime (string): (Required) yyyy-mm ,
        company (string): (Required) Name of company ,
        position (string): (Required) position ,
        description (string): (Required) Description of job
        }
        ProjectExperience {
        startTime (string): (Required) yyyy-mm ,
        endTime (string): (Required) yyyy-mm ,
        name (string): (Required)name of company or project ,
        description (string): (Required) Description
        }

    ```
 
- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:
     | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` | Response message   |
    | `data` | `object ` |   TPID : doc no  |

  







#### 6.5   Upload File CV For External Job


- URL:

  ```http
    /api/RMSCVForExternalJob/UploadFileCVForExternalJob1
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
     Upload file CV pdf for External job  , only allow update when information of CV job is :  "isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
  
  - Format: Form Data
  - Code view:
    ```http
      var form= new FormData();  
      form.append("TPID","Your doc No") ;
      form.append("file",file[0]);

    ```
  - Parameter Description:
    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `TPID` | `string` | **Required**. Your doc no |
    | `file` | `File data` | **Required**. Your file data |

 
- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:
     | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` | Response message   |
    | `data` | `object ` |   TPID : doc no  |







#### 6.6  Delete  file Cv for External Job


- URL:

  ```http
    /api/RMSCVForExternalJob/DeleteFileCVForExternalJob
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
     Delete file Cv for External Job, only allow delete when information of CV job is :  "isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
  
  - Format: query string
  - Code view:
    ```http
      TPID:''

    ```
  - Parameter Description:
    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `TPID` | `string` | **Required**. Your doc no |
   

 
- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:
     | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` | Response message   |
    | `data` | `object ` |   |









#### 6.7 Submit External job of TPID


- URL:

  ```http
    /api/RMSCVForExternalJob/SubmitExternalJob
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
     Submit External job of TPID (move status from Draft to Submit) , only allow submit when information of CV job is :  "isAllowEdit": true
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
  
  - Format: query string
  - Code view:
    ```http
      TPID:''

    ```
  - Parameter Description:
    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `TPID` | `string` | **Required**. Your doc no |
   

 
- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:
     | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` | Response message   |
    | `data` | `object ` |   |




## 7. Manage  workers register interview




#### 7.1 Employee register to interview worker


- URL:

  ```http
    /api/RMSEmployee/EmpRegisterInterviewWorker
  ```
- Method:

  ```http
    POST
  ```
- Description:

  ```http
      Employee register to interview worker
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
  
  
  - Format:  json
  - Code view:
    ```http
      {
        "name": "DAO VAN TUYEN",
        "mobile": "123456",
        "factory": "桂武工業區",
        "interviewDate": "2025/10/30"
      }

    ```
  - Parameter Description:
     ```http
        EmployeeRegister {
        name (string): your name  (Required),
        mobile (string, optional): your phone number (Required) ,
        factory (string): Factory , that you want to register to do work (Required) ,
        interviewDate (string, optional): Interview date: yyyy/mm/dd  (Required)
        }
   
      ```
    
- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": null
      }
    ```
  - Parameter Description:
     | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` | Response message   |
    | `data` | `object ` |   |





#### 7.2 Get list register to interview worker of current user


- URL:

  ```http
    /api/RMSEmployee/GetEmpRegisterInterviewLSGET
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
      Get list register to interview worker of current user
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
    
- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": [
          {
            "name": "DAO VAN TUYEN",
            "mobile": "12345",
            "factory": "Dong Vang",
            "interviewDate": "2025/11/05",
            "createDate": "2025/10/31 13:58:27",
            "updateDate": "2025/10/31 13:58:27"
          },
          {
            "name": "DAO VAN TUYEN",
            "mobile": "12345",
            "factory": "Dinh Tram",
            "interviewDate": "2025/11/05",
            "createDate": "2025/10/31 13:57:42",
            "updateDate": "2025/10/31 13:57:42"
          },
          {
            "name": "DAO VAN TUYEN",
            "mobile": "12345",
            "factory": "Que Vo",
            "interviewDate": "2025/11/05",
            "createDate": "2025/10/31 13:57:21",
            "updateDate": "2025/10/31 13:57:21"
          },
          {
            "name": "DAO VAN TUYEN",
            "mobile": "12345",
            "factory": "Quang Chau",
            "interviewDate": "2025/11/05",
            "createDate": "2025/10/31 13:57:00",
            "updateDate": "2025/10/31 13:57:00"
          }
        ]
      }
    ```
  - Parameter Description:
   ```http
     EmployeeRegister {
      name (string): your name ,
      mobile (string): your phone number ,
      factory (string): Factory , that you want to register to do work ,
      interviewDate (string): Interview date: yyyy/mm/dd ,
      createDate (string),
      updateDate (string)
      }

    ```
    


## 8. Count  job you registered



#### 8.1 count total job your registered


- URL:

  ```http
    /api/RMSCVTotal/GetCountTotalJobRegister
  ```
- Method:

  ```http
    GET
  ```
- Description:

  ```http
      count total job your registered
  ```

- Request:
  - Header :

     ```http
       Authorization: '', // jwt token string , format: 'Bearer xxxxx'
       language: "" ,//  current language of website:  VN/EN/CN
    ```
    
- Response:
  - Format: json 
  - Code view:
   ```http
        {
        "status": "success",
        "message": "success",
        "data": 13
      }
    ```
   - Parameter Description:

    | Parameter | Type     | Description                |
    | :-------- | :------- | :------------------------- |
    | `status` | `string` |  Response status: 'success' or 'error' |
    | `message` | `string` | Response message   |
    | `data` | `interger ` |  total job's quantity, which you registered    |









