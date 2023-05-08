using System;
using System.Linq;
using System.Text;
using SqlSugar;

namespace GlobalProject.Repository.Model
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable("readme")]
    public partial class readme
    {
           public readme(){


           }
           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsPrimaryKey=true)]
           public int id {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Message {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           public string Bitcoin_Address {get;set;}

    }
}
