using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GlobalProject.Repository.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("user")]
    public partial class user
    {
           public user(){


           }
           /// <summary>
           /// Desc:用户ID
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public string UserId {get;set;}

           /// <summary>
           /// Desc:用户名
           /// Default:
           /// Nullable:False
           /// </summary>           
           public string UserName {get;set;}

    }
}
