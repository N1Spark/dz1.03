//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EagerLoadinf_Library_ADO_Winforms.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Books
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public int AuthorId { get; set; }
        public int PageCount { get; set; }
    
        public virtual Autors Autors { get; set; }
        public virtual Categories Categories { get; set; }
        public virtual Publishings Publishings { get; set; }
    }
}