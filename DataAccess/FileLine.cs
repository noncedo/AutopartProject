//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class FileLine: IEntity
    {
        public int FileLineId { get; set; }
        public int FileSourceId { get; set; }
        public Nullable<int> FileDestinationId { get; set; }
        public string Line { get; set; }
        public System.DateTime CreateDate { get; set; }
    
        public virtual FileDestination FileDestination { get; set; }
        public virtual FileSource FileSource { get; set; }
    }
}