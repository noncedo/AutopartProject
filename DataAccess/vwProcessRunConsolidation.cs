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
    
    public partial class vwProcessRunConsolidation: IEntity
    {
        public string Name { get; set; }
        public System.DateTime Start { get; set; }
        public Nullable<System.DateTime> End { get; set; }
        public Nullable<bool> RunStatus { get; set; }
        public Nullable<int> FileSourceCount { get; set; }
        public Nullable<int> FileDestinationCount { get; set; }
        public Nullable<int> ActiveDealerCount { get; set; }
        public int ProcessRunId { get; set; }
        public int ProcessId { get; set; }
    }
}
